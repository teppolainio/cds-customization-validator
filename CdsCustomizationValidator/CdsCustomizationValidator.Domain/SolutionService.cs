using CdsCustomizationValidator.Domain.Rule;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using DAO = CdsCustomizationValidator.Infrastructure.DAO;

namespace CdsCustomizationValidator.Domain {

  public class SolutionService {

    public SolutionService(IOrganizationService service) {
      this.service = service;
    }

    public IReadOnlyList<SolutionEntity> GetSolutionEntities(string uniqueSolutionName) {

      Console.WriteLine($"Handling solution {uniqueSolutionName}.");

      var entityComponentsQuery = new QueryExpression {
        EntityName = DAO.SolutionComponent.EntityLogicalName,
        ColumnSet = new ColumnSet(true),
        Criteria = new FilterExpression(),
      };
      var solutionLink = new LinkEntity(DAO.SolutionComponent.EntityLogicalName,
                                        DAO.Solution.EntityLogicalName,
                                        "solutionid",
                                        "solutionid",
                                        JoinOperator.Inner);
      solutionLink.LinkCriteria = new FilterExpression();
      var condition = new ConditionExpression("uniquename", ConditionOperator.In, uniqueSolutionName);
      solutionLink.LinkCriteria.AddCondition(condition);
      entityComponentsQuery.LinkEntities.Add(solutionLink);
      entityComponentsQuery.Criteria.AddCondition(new ConditionExpression("componenttype", ConditionOperator.Equal, 1));
      EntityCollection entityComponentCollection = service.RetrieveMultiple(entityComponentsQuery);

      var allEntitiesrequest = new RetrieveAllEntitiesRequest() {
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

      var ownedEntities = entityComponentCollection.Entities
                                                   .Where(e => e.GetAttributeValue<OptionSetValue>("rootcomponentbehavior").Value == 0)
                                                   .ToList();

      foreach(var entity in entitiesInSolution) {

        Console.WriteLine($"Handling entity {entity.LogicalName}.");

        if(ownedEntities.All(e => e.GetAttributeValue<Guid?>("objectid") != entity.MetadataId)) {
          var attributeMetadata = GetAttributesInSolution(entity, uniqueSolutionName);


          var item = new SolutionEntity(entity, attributeMetadata, false);

          retval.Add(item);
        }
        else {
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine($"Entity {entity.LogicalName} has rootcomponentbehavior value {ownedEntities.Single(e => e.GetAttributeValue<Guid?>("objectid") == entity.MetadataId).GetAttributeValue<OptionSetValue>("rootcomponentbehavior").Value} meaning solution owns this entity. All fields are included in solution.");
          Console.ResetColor();

          var attributes = getEntityMetadata(entity.LogicalName, service, EntityFilters.Attributes);

          var item = new SolutionEntity(entity, attributes.Attributes.ToList(), true);

          retval.Add(item);
        }
      }

      Console.WriteLine($"All entities in solution {uniqueSolutionName} have been fetched.");

      return retval;
    }

    public Dictionary<EntityMetadata, List<ValidationResult>> Validate(
        IReadOnlyList<SolutionEntity> solutionEntitities,
        List<CustomizationRuleBase> rules) {
      var results = new Dictionary<EntityMetadata, List<ValidationResult>>();

      foreach(var solutionEntity in solutionEntitities) {
        results.Add(solutionEntity.Entity, new List<ValidationResult>());

        foreach(var rule in rules) {
          var validationResult = rule.Validate(solutionEntity);
          results[solutionEntity.Entity].Add(validationResult);
        }

      }

      return results;
    }

    private readonly IOrganizationService service;

    private List<AttributeMetadata> GetAttributesInSolution(EntityMetadata entity, string uniqueSolutionName) {
      QueryExpression componentsQuery = new QueryExpression {
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
      foreach(var foo in componentCollection.Entities) {

        var req = new RetrieveAttributeRequest() {
          MetadataId = foo.GetAttributeValue<Guid>("objectid")
        };
        var resp = (RetrieveAttributeResponse)service.Execute(req);

        if(resp.AttributeMetadata.EntityLogicalName == entity.LogicalName) {
          retval.Add(resp.AttributeMetadata);
        }
      }
      return retval;
    }

    private EntityMetadata getEntityMetadata(string entityLogicalName,
                                             IOrganizationService service,
                                             EntityFilters filters) {
      var request = new RetrieveEntityRequest {
        EntityFilters = filters,
        LogicalName = entityLogicalName,
        RetrieveAsIfPublished = true
      };

      var response = (RetrieveEntityResponse)service.Execute(request);

      return response.EntityMetadata;
    }
  }
}
