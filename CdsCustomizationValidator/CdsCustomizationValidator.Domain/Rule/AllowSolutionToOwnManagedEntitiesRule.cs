namespace CdsCustomizationValidator.Domain.Rule
{
    /// <summary>
    /// Rule to check if entity is managed and owned by solution. This can be
    /// either allowed or disallowed. You probably do not want to allow this.
    /// </summary>
    public class AllowSolutionToOwnManagedEntitiesRule : CustomizationRule
    {

        /// <summary>
        /// Creates a new rule instance.
        /// </summary>
        /// <param name="allowSolutionToOwnManagedEntities">
        /// If true solution is allowed to own managed entities.
        /// Note! You probably don't want to allow this.
        /// </param>
        public AllowSolutionToOwnManagedEntitiesRule(
            bool allowSolutionToOwnManagedEntities)
        {
            _allowSolutionToOwnManagedEntities = allowSolutionToOwnManagedEntities;
        }

        /// <summary>
        /// See <see cref="CustomizationRule.ValidateRule(SolutionEntity)"/>.
        /// </summary>
        protected override ValidationResult ValidateRule(SolutionEntity solutionEntity)
        {
            var result = true;

            if (!_allowSolutionToOwnManagedEntities &&
                solutionEntity.IsOwnedBySolution &&
                solutionEntity.Entity.IsManaged == true)
            {

                result = false;
            }

            return new ValidationResult(solutionEntity.Entity,
                                        result);
        }

        private readonly bool _allowSolutionToOwnManagedEntities;
    }

}
