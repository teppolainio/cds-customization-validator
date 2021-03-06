﻿namespace CdsCustomizationValidator.Domain.Rule {

  /// <summary>
  /// Base class for customization rule.
  /// </summary>
  public abstract class CustomizationRuleBase {

    /// <summary>
    /// Descriptive name for this rule. This can be displayed on various
    /// UI representations.
    /// </summary>
    /// <remarks>
    /// This should be pretty short (under 60 characters).
    /// Description should form a sentence ending with dot.
    /// </remarks>
    public abstract string Description { get; }

    /// <summary>
    /// Checks whether rule is adhered or not.
    /// </summary>
    /// <param name="solutionEntity">
    /// Solution entity which is validated by rule.
    /// </param>
    public ValidationResult Validate(
        SolutionEntity solutionEntity) {
      return ValidateRule(solutionEntity);
    }

    /// <summary>
    /// Validation implementation which must be implemented by rules
    /// inherited from this base class.
    /// </summary>
    /// <param name="solutionEntity">
    /// Solution entity which is validated by rule.
    /// </param>
    /// <returns>
    /// True when rule validation has passed. False otherwise.
    /// </returns>
    protected abstract ValidationResult ValidateRule(
        SolutionEntity solutionEntity);
  }



}
