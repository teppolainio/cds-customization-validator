namespace CdsCustomizationValidator.Domain.Rule
{

    /// <summary>
    /// Rule to check that all  custom unmanaged entities
    /// owned by solution start with correct schema name prefix.
    /// </summary>
    public class EntityPrefixRule : CustomizationRule
    {

        /// <summary>
        /// Creates a new rule instance requiring given prefix.
        /// </summary>
        /// <param name="schemaPrefix">
        /// Required prefix.
        /// </param>
        public EntityPrefixRule(string schemaPrefix)
        {
            _schemaPrefix = schemaPrefix.TrimEnd('_') + "_";
        }

        /// <summary>
        /// See <see cref="CustomizationRule.ValidateRule(SolutionEntity)"/>.
        /// </summary>
        protected override ValidationResult ValidateRule(
            SolutionEntity solutionEntity)
        {
            var entityNamePassed = true;

            if (solutionEntity.IsOwnedBySolution &&
                solutionEntity.Entity.IsManaged == false &&
                solutionEntity.Entity.IsCustomEntity == true &&
                !solutionEntity.Entity.SchemaName.StartsWith(_schemaPrefix))
            {
                entityNamePassed = false;
            }

            return new ValidationResult(solutionEntity.Entity,
                                        entityNamePassed);
        }

        private readonly string _schemaPrefix;
    }
}
