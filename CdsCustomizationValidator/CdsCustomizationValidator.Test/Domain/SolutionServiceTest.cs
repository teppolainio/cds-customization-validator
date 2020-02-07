using CdsCustomizationValidator.Domain;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using DAO = CdsCustomizationValidator.Infrastructure.DAO;

namespace CdsCustomizationValidator.Test.Domain {

  /// <summary>
  /// Unit tests for <see cref="SolutionService"/>.
  /// </summary>
  public class SolutionServiceTest {

    [Fact(DisplayName = "SolutionService: Include entity which is not owned.",
          Skip = "Waiting for improved FakeXrmEasy functionality.")]
    public void Foo() {
      var solutionName = "MySolution";

      var solution = new DAO.Solution() {
        Id = Guid.NewGuid(),
        UniqueName = solutionName,
      };

      var meta = new EntityMetadata() { 
        MetadataId = Guid.NewGuid(),
        LogicalName = "my_entity"
      };


      var dbContent = new List<Entity>() {
        solution,
        new DAO.SolutionComponent() { 
          Id = Guid.NewGuid(),
          ["componenttype"] = new OptionSetValue((int)DAO.ComponentType.Entity),
          ["objectid"] = meta.MetadataId,
          ["rootcomponentbehavior"] = new OptionSetValue((int)DAO.SolutionComponent_RootComponentBehavior.Donotincludesubcomponents),
          ["solutionid"] = solution.ToEntityReference(),
        }
      };

      var metaDbContent = new List<EntityMetadata> { 
        meta
      };

      var fakedContext = new XrmFakedContext();
      fakedContext.Initialize(dbContent);
      fakedContext.InitializeMetadata(metaDbContent);

      var service = fakedContext.GetOrganizationService();

      var solutionService = new SolutionService(service);

      var entities = solutionService.GetSolutionEntities(solutionName);

      Assert.Single(entities);
      var result = entities.First();

      Assert.False(result.IsOwnedBySolution);
      Assert.Equal("my_entity", result.Entity.LogicalName);
    }

  }

}
