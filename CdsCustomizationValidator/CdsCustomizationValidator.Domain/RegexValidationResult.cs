using CdsCustomizationValidator.Domain.Rule;
using Microsoft.Xrm.Sdk.Metadata;

namespace CdsCustomizationValidator.Domain
{

    /// <summary>
    /// Represents a 
    /// </summary>
    public class RegexValidationResult : ValidationResult
    {

        /// <summary>
        /// See <see cref="ValidationResult.ValidationResult(EntityMetadata, bool, CustomizationRuleBase)"/>.
        /// </summary>
        public RegexValidationResult(EntityMetadata entity,
                                     bool passed,
                                     RegexRule validatedRule)
            : base(entity, passed, validatedRule)
        {
        }

        public override string FormatValidationResult()
        {
            var resultStr = base.FormatValidationResult();

            if (!Passed)
            {
                // Safe because of constructor parameter.
                var rule = (RegexRule)ValidatedRule;

                resultStr += $" Entity schema name {Entity.SchemaName} " +
                            $"doesn't match given pattern \"{ rule.Pattern }\".";
            }

            return resultStr;
        }

    }
}
