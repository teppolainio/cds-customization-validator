﻿using CdsCustomizationValidator.Domain.Rule;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CdsCustomizationValidator.Domain {

  /// <summary>
  /// Represents a 
  /// </summary>
  public class RegexValidationResult: ValidationResult {

    /// <summary>
    /// Initializes validation result.
    /// </summary>
    /// <param name="entity">
    /// Entity which was validated.
    /// </param>
    /// <param name="passed">
    /// Result of the validation.
    /// </param>
    /// <param name="validatedRule">
    /// Rule which was validated.
    /// </param>
    public RegexValidationResult(EntityMetadata entity,
                                 bool passed,
                                 RegexRule validatedRule)
        : base(entity, passed, validatedRule) {
    }

    /// <summary>
    /// Initializes validation result.
    /// </summary>
    /// <param name="entity">
    /// Entity which was validated.
    /// </param>
    /// <param name="failingAttributes">
    /// Attributes which did not pass the validation.
    /// </param>
    /// <param name="validatedRule">
    /// Rule which was validated.
    /// </param>
    public RegexValidationResult(
        EntityMetadata entity,
        IEnumerable<AttributeMetadata> failingAttributes,
        RegexRule validatedRule)
        : this(entity,
               !failingAttributes.Any(),
               validatedRule) {

      if(validatedRule.Scope == RuleScope.Entity) {
        throw new ArgumentException(
            "Results for checking entity schema name doesn't depend " +
            "from attributes. You are likely using wrong constructor.",
            nameof(failingAttributes));
      }

      _failingAttributes = failingAttributes;
    }

    public override string FormatValidationResult() {

      // Safe because of constructor parameter.
      var rule = (RegexRule)ValidatedRule;

      if(rule.Scope == RuleScope.Attribute &&
          _failingAttributes == null) {
        throw new InvalidOperationException(
            "Results for checking attribute schema names must be " +
            "given list of failing attributes.");
      }

      if(Passed) {
        return base.FormatValidationResult();
      }

      string resultStr = $"Rule failed: {ValidatedRule.Description} ";

      if(rule.Scope == RuleScope.Entity) {

        resultStr += $"Entity schema name {Entity.SchemaName} " +
                    $"doesn't match given pattern \"{ rule.Pattern }\".";
      }
      else if(rule.Scope == RuleScope.Attribute ||
              rule.Scope == RuleScope.Lookup) {

        resultStr += $"Following ";
        resultStr += rule.Scope == RuleScope.Attribute ? "attributes" : "lookups";
        resultStr += " do not match given pattern:";

        foreach(var attr in _failingAttributes) {
          resultStr += $" {attr.SchemaName},";
        }
        resultStr = $"{resultStr.TrimEnd(',')}.";
      }
      else {
        throw new NotImplementedException();
      }

      return resultStr;
    }

    /// <summary>
    /// Attributes which have failed the rule validation.
    /// </summary>
    private readonly IEnumerable<AttributeMetadata> _failingAttributes;

  }
}
