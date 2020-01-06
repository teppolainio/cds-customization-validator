using CdsCustomizationValidator.Domain.Rule;
using DTO = CdsCustomizationValidator.Infrastructure.DTO;

namespace CdsCustomizationValidator.Domain
{

    /// <summary>
    /// Factory for creating new rule instances.
    /// </summary>
    internal class RuleFactory
    {

        internal DisallowSolutionToOwnManagedEntitiesRule CreateFrom(
            DTO.DisallowSolutionToOwnManagedEntitiesRule deserializedRule)
        {
            var allow = deserializedRule.allow;
            return new DisallowSolutionToOwnManagedEntitiesRule(allow);
        }
    }
}