using System.Linq;

namespace CdsCustomizationValidator.Domain.Rule
{
    /// <summary>
    /// Rule to check that all unmanaged custom fields in solution start with
    /// correct schema name prefix.
    /// </summary>
    public class AttributePrefixRule : CustomizationRule
    {

        /// <summary>
        /// Creates a new rule instance requiring given prefix.
        /// </summary>
        /// <param name="schemaPrefix">
        /// Required prefix.
        /// </param>
        public AttributePrefixRule(string schemaPrefix)
        {
            _schemaPrefix = schemaPrefix.TrimEnd('_') + "_";
        }

        /// <summary>
        /// See <see cref="CustomizationRule.ValidateRule(SolutionEntity)"/>.
        /// </summary>
        protected override ValidationResult ValidateRule(
            SolutionEntity solutionEntity)
        {
            var customAttributes = solutionEntity.Attributes
                                                 .Where(a => a.IsManaged == false &&
                                                             a.IsCustomAttribute == true);

            var attributeFailures = customAttributes.Where(a => !a.SchemaName
                                                                  .StartsWith(_schemaPrefix));

            return new AttributeValidationResult(solutionEntity.Entity,
                                                 attributeFailures);
        }

        private readonly string _schemaPrefix;

    }
}
