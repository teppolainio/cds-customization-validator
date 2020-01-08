using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CdsCustomizationValidator.Domain.Rule {


  /// <summary>
  /// Scope of the rule.
  /// </summary>
  public enum RuleScope {
    /// <summary>
    /// Entities.
    /// </summary>
    Entity,
    /// <summary>
    /// Attributes of an entity.
    /// </summary>
    Attribute,
    /// <summary>
    /// Lookup (i.e. EntityReference) type of attribute of an entity.
    /// </summary>
    Lookup
  };

  /// <summary>
  /// Rule which allows to define rules using regular expression.
  /// Expressions need to be in .NET regular expression format.
  /// </summary>
  public class RegexRule: CustomizationRuleBase {

    /// <summary>
    /// See <see cref="CustomizationRuleBase.Description"/>.
    /// </summary>
    public override string Description {
      get {
        var sb = new StringBuilder("Schema name of an ");
        sb.Append(Scope)
          .Append(" must match to regular expression pattern ")
          .Append(Pattern)
          .Append(".");

        if(_excludedSchemaNames != null &&
            _excludedSchemaNames.Any()) {
          sb.Append(" ")
            .Append(Scope)
            .Append(" ");

          for(int i = 0; i < _excludedSchemaNames.Count; i++) {
            if(i > 0) {
              if(i < (_excludedSchemaNames.Count - 1)) {
                sb.Append(", ");
              }
              else {
                sb.Append(" and ");
              }
            }
            sb.Append(_excludedSchemaNames.ElementAt(i));
          }

          sb.Append(" are excluded.");
        }

        return sb.ToString();
      }
    }

    /// <summary>
    /// Creates a new rule instance requiring naming to match given
    /// pattern on certain scope.
    /// </summary>
    /// <param name="regexPattern">
    /// Regular expression pattern which must be met on scope given by
    /// parameter <paramref name="scope"/>.
    /// </param>
    /// <param name="scope">
    /// Scope of the rule.
    /// </param>
    public RegexRule(string regexPattern, RuleScope scope)
        : this(regexPattern, scope, new List<string>()) {
    }

    /// <summary>
    /// Creates a new rule instance requiring naming to match given
    /// pattern on certain scope.
    /// </summary>
    /// <param name="regexPattern">
    /// Regular expression pattern which must be met on scope given by
    /// parameter <paramref name="scope"/>.
    /// </param>
    /// <param name="scope">
    /// Scope of the rule.
    /// </param>
    /// <param name="excludedSchemaNames">
    /// List of schema names which are exluded from the rule.
    /// </param>
    public RegexRule(string regexPattern,
                     RuleScope scope,
                     ICollection<string> excludedSchemaNames) {
      if(excludedSchemaNames == null) {
        throw new ArgumentNullException(nameof(excludedSchemaNames));
      }

      try {
        Pattern = new Regex(regexPattern);
      }
      catch(ArgumentException ex) {
        throw new ArgumentException(
            $"Given pattern \"{regexPattern}\" was invalid regular expression.",
            ex);
      }

      Scope = scope;

      _excludedSchemaNames = excludedSchemaNames;
    }

    /// <summary>
    /// See <see cref="CustomizationRuleBase.ValidateRule(SolutionEntity)"/>.
    /// </summary>
    protected override ValidationResult ValidateRule(
        SolutionEntity solutionEntity) {

      RegexValidationResult validationResult = null;
      switch(Scope) {
        case RuleScope.Entity:
          validationResult = ValidateEntityScope(solutionEntity);
          break;
        case RuleScope.Attribute:
          validationResult = ValidateAttributeScope(solutionEntity);
          break;
        case RuleScope.Lookup:
          validationResult = ValidateLookupScope(solutionEntity);
          break;
        default:
          throw new NotImplementedException(
              $"Implementation is missing for scope {Scope}.");
      }

      return validationResult as ValidationResult;
    }

    
    /// <summary>
    /// Regular expression pattern being applied to rule scope.
    /// </summary>
    internal Regex Pattern { get; }

    /// <summary>
    /// Scope of the rule.
    /// </summary>
    internal RuleScope Scope { get; }

    private readonly ICollection<string> _excludedSchemaNames;

    private RegexValidationResult ValidateEntityScope(
        SolutionEntity solutionEntity) {
      bool validationPassed;

      var entity = solutionEntity.Entity;

      if(solutionEntity.IsOwnedBySolution == false ||
         entity.IsManaged == true ||
         _excludedSchemaNames.Contains(entity.SchemaName)) {
        validationPassed = true;
      }
      else {
        validationPassed = Pattern.IsMatch(entity.SchemaName);
      }

      return new RegexValidationResult(solutionEntity.Entity,
                                       validationPassed,
                                       this);
    }

    private RegexValidationResult ValidateAttributeScope(
        SolutionEntity solutionEntity) {
      var attributesToCheck = solutionEntity.Attributes
                                            .Where(a => a.IsManaged != true &&
                                                        a.IsCustomAttribute == true);

      var failingAttributes = new List<AttributeMetadata>();

      foreach(var attribute in attributesToCheck) {

        var attrFullname = $"{solutionEntity.Entity.SchemaName}.{attribute.SchemaName}";
        if(_excludedSchemaNames.Contains(attrFullname)) {
          continue;
        }

        var pass = Pattern.IsMatch(attribute.SchemaName);

        if(!pass) {
          failingAttributes.Add(attribute);
        }
      }

      return new RegexValidationResult(solutionEntity.Entity,
                                       failingAttributes,
                                       this);
    }

    private RegexValidationResult ValidateLookupScope(
      SolutionEntity solutionEntity) {
      throw new NotImplementedException();
    }


  }
}
