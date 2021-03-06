﻿using System;

namespace CdsCustomizationValidator.Domain.Rule {

  /// <summary>
  /// Rule to check that all  custom unmanaged entities
  /// owned by solution start with correct schema name prefix.
  /// </summary>
  public class EntityPrefixRule: CustomizationRuleBase {

    /// <summary>
    /// See <see cref="CustomizationRuleBase.Description"/>.
    /// </summary>
    public override string Description {
      get { return $"Entity prefix must be \"{_schemaPrefix}\"."; }
    }

    /// <summary>
    /// Creates a new rule instance requiring given prefix.
    /// </summary>
    /// <param name="schemaPrefix">
    /// Required prefix.
    /// </param>
    public EntityPrefixRule(string schemaPrefix) {
      if(string.IsNullOrWhiteSpace(schemaPrefix)) {
        throw new ArgumentException(
            "Schema prefix isn't allowed to be null, empty or whitespace.",
            nameof(schemaPrefix));
      }

      _schemaPrefix = schemaPrefix.TrimEnd('_');
    }

    /// <summary>
    /// See <see cref="CustomizationRuleBase.ValidateRule(SolutionEntity)"/>.
    /// </summary>
    protected override ValidationResult ValidateRule(
        SolutionEntity solutionEntity) {
      var entityNamePassed = true;

      if(solutionEntity.IsOwnedBySolution &&
          solutionEntity.Entity.IsManaged == false &&
          solutionEntity.Entity.IsCustomEntity == true &&
          !solutionEntity.Entity.SchemaName.StartsWith(_schemaPrefix + "_")) {
        entityNamePassed = false;
      }

      return new ValidationResult(solutionEntity.Entity,
                                  entityNamePassed,
                                  this);
    }

    private readonly string _schemaPrefix;
  }
}
