using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using FakeXrmEasy.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using Xunit;

namespace CdsCustomizationValidator.Test.Domain.Rule {
  public class EntityPrefixRuleTest {

    [Theory(DisplayName = "EntityPrefixRule: Valid prefix for custom unmanaged entity owned by solution.")]
    [InlineData("foo")]
    [InlineData("foo_")]
    [InlineData("abitlonger_")]
    public void ValidPrefixTestForCustomUnmanagedEntity(string requiredPrefix) {

      EntityMetadata entity = new EntityMetadata() {
        SchemaName = requiredPrefix.TrimEnd('_') + "_MyMagnificientEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      List<AttributeMetadata> attributes = null;

      var isOwnedBySolution = true;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var ruleToTest = new EntityPrefixRule(requiredPrefix);

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.True(results.Passed);
    }

    [Theory(DisplayName = "EntityPrefixRule: Invalid prefix for custom unmanaged entity owned by solution.")]
    [InlineData("foo")]
    [InlineData("foo_")]
    [InlineData("abitlonger_")]
    public void InvalidPrefixTestForCustomUnmanagedEntity(string requiredPrefix) {

      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "bar_MyMagnificientEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      List<AttributeMetadata> attributes = null;

      var isOwnedBySolution = true;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var ruleToTest = new EntityPrefixRule(requiredPrefix);

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.False(results.Passed);
    }

    // This test is a bit questionable because this is a scenario
    // Microsoft doesn't really encourage.
    [Fact(DisplayName = "EntityPrefixRule: Skip for custom unmanaged entity not owned by solution.")]
    public void SkipPrefixTestForCustomUnmanagedEntityNotOwnedBySolution() {

      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "bar_MyMagnificientEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      List<AttributeMetadata> attributes = null;

      var isOwnedBySolution = false;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var ruleToTest = new EntityPrefixRule("bar");

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "EntityPrefixRule: Skip for custom managed entities.")]
    public void SkipPrefixTestForCustomManagedEntity() {

      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "bar_MyMagnificientEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      List<AttributeMetadata> attributes = null;

      var isOwnedBySolution = true;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var ruleToTest = new EntityPrefixRule("foo");

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "EntityPrefixRule: Skip for OOB managed entities.")]
    public void SkipPrefixTestForOobManagedEntity() {

      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "Account",
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", false);

      List<AttributeMetadata> attributes = null;

      var isOwnedBySolution = true;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var ruleToTest = new EntityPrefixRule("foo");

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.True(results.Passed);
    }

  }
}
