using CdsCustomizationValidator.Domain.Rule;
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
        /// <param name="validatedRule">
        /// Rule which was validated.
        /// </param>
        public AttributeValidationResult(
            EntityMetadata entity,
            IEnumerable<AttributeMetadata> failingAttributes,
            AttributePrefixRule validatedRule)
            : base(entity, !failingAttributes.Any(), validatedRule)
        {
            FailingAttributes = failingAttributes;
        }

        /// <summary>
        /// See <see cref="ValidationResult.FormatValidationResult"/>.
        /// </summary>
        /// <returns></returns>
        public override string FormatValidationResult()
        {
            var resultStr = base.FormatValidationResult();

            if (FailingAttributes.Any())
            {
                resultStr += " Failures on attributes";
                foreach (var attr in FailingAttributes)
                {
                    resultStr += $" {attr.SchemaName},";
                }
                resultStr.TrimEnd(',');
                resultStr += ".";
            }

            return resultStr;
        }


    }

}