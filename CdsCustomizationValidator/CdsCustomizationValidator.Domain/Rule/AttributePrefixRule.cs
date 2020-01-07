using System;
using System.Linq;

namespace CdsCustomizationValidator.Domain.Rule
{
    /// <summary>
    /// Rule to check that all unmanaged custom fields in solution start with
    /// correct schema name prefix.
    /// </summary>
    public class AttributePrefixRule : CustomizationRuleBase
    {

        /// <summary>
        /// See <see cref="CustomizationRuleBase.Description"/>.
        /// </summary>
        public override string Description
        {
            get { return $"Attribute prefix must be \"{_schemaPrefix}\"."; }
        }

        /// <summary>
        /// Creates a new rule instance requiring given prefix.
        /// </summary>
        /// <param name="schemaPrefix">
        /// Required prefix.
        /// </param>
        public AttributePrefixRule(string schemaPrefix)
        {
            if (string.IsNullOrWhiteSpace(schemaPrefix))
            {
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
            SolutionEntity solutionEntity)
        {
            var customAttributes = solutionEntity.Attributes
                                                 .Where(a => a.IsManaged == false &&
                                                             a.IsCustomAttribute == true);

            var attributeFailures = customAttributes.Where(a => !a.SchemaName
                                                                  .StartsWith(_schemaPrefix + "_"));

            return new AttributeValidationResult(solutionEntity.Entity,
                                                 attributeFailures,
                                                 this);
        }

        private readonly string _schemaPrefix;

    }
}
