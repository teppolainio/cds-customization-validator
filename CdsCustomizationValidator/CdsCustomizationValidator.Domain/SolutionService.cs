using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CdsCustomizationValidator.Domain
{

    public class SolutionService
    {

        public IList<SolutionEntity> GetSolutionEntities(string uniqueSolutionName)
        {

            Console.WriteLine($"Handling solution {uniqueSolutionName}.");

            QueryExpression entityComponentsQuery = new QueryExpression
            {
                EntityName = "solutioncomponent",
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(),
            };
            LinkEntity solutionLink = new LinkEntity("solutioncomponent", "solution", "solutionid", "solutionid", JoinOperator.Inner);
            solutionLink.LinkCriteria = new FilterExpression();
            var condition = new ConditionExpression("uniquename", ConditionOperator.In, uniqueSolutionName);
            solutionLink.LinkCriteria.AddCondition(condition);
            entityComponentsQuery.LinkEntities.Add(solutionLink);
            entityComponentsQuery.Criteria.AddCondition(new ConditionExpression("componenttype", ConditionOperator.Equal, 1));
            EntityCollection entityComponentCollection = service.RetrieveMultiple(entityComponentsQuery);

            var allEntitiesrequest = new RetrieveAllEntitiesRequest()
            {
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            };
            var allEntitiesResponse = (RetrieveAllEntitiesResponse)service.Execute(allEntitiesrequest);

            var entitiesInSolution = allEntitiesResponse.EntityMetadata
                                                        .Join(entityComponentCollection.Entities
                                                                                       .Select(x => x.Attributes["objectid"]),
                                                              x => x.MetadataId,
                                                              y => y,
                                                              (x, y) => x)
                                                        .ToList();

            var retval = new List<SolutionEntity>();

            // https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/entities/solutioncomponent#BKMK_RootComponentBehavior
            //if entityComponentCollection.RootComponentBehavior  == 0 niin pitää ottaa entiteetin kaikki attribuutit

            var bar = entityComponentCollection.Entities
                                               .Where(e => e.GetAttributeValue<OptionSetValue>("rootcomponentbehavior").Value == 0)
                                               .ToList();

            foreach (var entity in entitiesInSolution)
            {

                Console.WriteLine($"Handling entity {entity.LogicalName}.");

                if (bar.All(e => e.GetAttributeValue<Guid?>("objectid") != entity.MetadataId))
                {
                    var attributeMetadata = GetAttributesInSolution(entity, uniqueSolutionName);


                    var item = new SolutionEntity(entity, attributeMetadata, false);

                    retval.Add(item);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Entity {entity.LogicalName} has rootcomponentbehavior value {bar.Single(e => e.GetAttributeValue<Guid?>("objectid") == entity.MetadataId).GetAttributeValue<OptionSetValue>("rootcomponentbehavior").Value} meaning solution owns this entity. All fields are included in solution.");
                    Console.ResetColor();

                    var attributes = getEntityMetadata(entity.LogicalName, service, EntityFilters.Attributes);

                    var item = new SolutionEntity(entity, attributes.Attributes.ToList(), true);

                    retval.Add(item);
                }
            }

            Console.WriteLine($"All entities in solution {uniqueSolutionName} have been fetched.");

            return retval;
        }

        public SolutionService(IOrganizationService service)
        {
            this.service = service;
        }

        private readonly IOrganizationService service;

        private List<AttributeMetadata> GetAttributesInSolution(EntityMetadata entity, string uniqueSolutionName)
        {
            QueryExpression componentsQuery = new QueryExpression
            {
                EntityName = "solutioncomponent",
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(),
            };
            LinkEntity solutionLink = new LinkEntity("solutioncomponent", "solution", "solutionid", "solutionid", JoinOperator.Inner);
            solutionLink.LinkCriteria = new FilterExpression();
            var condition = new ConditionExpression("uniquename", ConditionOperator.In, uniqueSolutionName);
            solutionLink.LinkCriteria.AddCondition(condition);
            componentsQuery.LinkEntities.Add(solutionLink);
            componentsQuery.Criteria.AddCondition(new ConditionExpression("componenttype", ConditionOperator.Equal, 2));
            EntityCollection componentCollection = service.RetrieveMultiple(componentsQuery);

            var bar = componentCollection.Entities
                                         .Select(e => e.GetAttributeValue<OptionSetValue>("componenttype")
                                                       .Value)
                                         .Distinct()
                                         .OrderBy(v => v);

            var retval = new List<AttributeMetadata>();
            foreach (var foo in componentCollection.Entities)
            {

                var req = new RetrieveAttributeRequest()
                {
                    MetadataId = foo.GetAttributeValue<Guid>("objectid")
                };
                var resp = (RetrieveAttributeResponse)service.Execute(req);

                if (resp.AttributeMetadata.EntityLogicalName == entity.LogicalName)
                {
                    retval.Add(resp.AttributeMetadata);
                }
            }
            return retval;
        }

        private EntityMetadata getEntityMetadata(string entityLogicalName,
                                                 IOrganizationService service,
                                                 EntityFilters filters)
        {
            var request = new RetrieveEntityRequest
            {
                EntityFilters = filters,
                LogicalName = entityLogicalName,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveEntityResponse)service.Execute(request);

            return response.EntityMetadata;
        }
    }
}
