using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace CdsCustomizationValidator.Domain
{
    public class SolutionEntity
    {


        public EntityMetadata Entity { get; }
        public IReadOnlyList<AttributeMetadata> Attributes { get; }
        public bool IsOwnedBySolution { get; }

        public SolutionEntity(EntityMetadata entity, List<AttributeMetadata> attributes, bool isOwned)
        {
            Entity = entity;
            Attributes = attributes;
            IsOwnedBySolution = isOwned;
        }

    }

}
