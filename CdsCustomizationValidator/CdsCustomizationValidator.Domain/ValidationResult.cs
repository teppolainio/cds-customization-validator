using Microsoft.Xrm.Sdk.Metadata;

namespace CdsCustomizationValidator.Domain {
  /// <summary>
  /// Represents a customization validation results.
  /// </summary>
  public class ValidationResult {

    /// <summary>
    /// Entity which was validated.
    /// </summary>
    public EntityMetadata Entity { get; }

    /// <summary>
    /// Result of the validation.
    /// </summary>
    public bool Passed { get; }

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
    public ValidationResult(EntityMetadata entity,
                            bool passed,
                            Rule.CustomizationRuleBase validatedRule) {
      Entity = entity;
      Passed = passed;
      ValidatedRule = validatedRule;
    }

    /// <summary>
    /// Formats a userfriendly string representing validation status of
    /// this result object.
    /// </summary>
    /// <returns>
    /// User-friendly validation status representation.
    /// </returns>
    public virtual string FormatValidationResult() {
      var retVal = $"Rule: {ValidatedRule.Description} " +
                   (Passed ? "Succeeded" : "Failed") + " for entity " +
                   $"\"{Entity.DisplayName.UserLocalizedLabel.Label}\" " +
                   $"({Entity.SchemaName}).";
      return retVal;
    }

    protected Rule.CustomizationRuleBase ValidatedRule { get; }

  }

}