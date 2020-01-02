using Microsoft.Xrm.Sdk.Metadata;

namespace CdsCustomizationValidator.Domain
{
    /// <summary>
    /// Represents a customization validation results.
    /// </summary>
    public class ValidationResult
    {

        /// <summary>
        /// Entity which was validated.
        /// </summary>
        public EntityMetadata Entity { get; }

        /// <summary>
        /// Result of the validation.
        /// </summary>
        public bool Passed { get;}

        /// <summary>
        /// Initializes validation result.
        /// </summary>
        /// <param name="entity">
        /// Entity which was validated.
        /// </param>
        /// <param name="passed">
        /// Result of the validation.
        /// </param>
        public ValidationResult(EntityMetadata entity,
                                bool passed)
        {
            Entity = entity;
            Passed = passed;
        }

    }

}