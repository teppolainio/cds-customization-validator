using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace CdsCustomizationValidator.Domain
{
    /// <summary>
    /// Validation results for attributes of a specific entity.
    /// </summary>
    public class AttributeValidationResult : ValidationResult
    {

        /// <summary>
        /// Attributes which have failed the rule validation.
        /// </summary>
        public IEnumerable<AttributeMetadata> FailingAttributes { get; }

        /// <summary>
        /// Initializes validation result.
        /// </summary>
        /// <param name="entity">
        /// Entity which was validated.
        /// </param>
        /// <param name="failingAttributes">
        /// Attributes which did not pass the validation.
        /// </param>
        public AttributeValidationResult(
            EntityMetadata entity,
            IEnumerable<AttributeMetadata> failingAttributes)
            : base(entity, !failingAttributes.Any())
        {
            FailingAttributes = failingAttributes;
        }


    }

}