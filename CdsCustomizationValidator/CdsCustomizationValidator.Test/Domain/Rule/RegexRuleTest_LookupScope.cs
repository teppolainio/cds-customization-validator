using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using FakeXrmEasy.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using Xunit;

namespace CdsCustomizationValidator.Test.Domain.Rule {

  public class RegexRuleTest_LookupScope {


    [Fact(DisplayName = "RegexRule: Lookup rule description is correct.")]
    public void LookupRuleDescriptionIsCorrect() {
      var scope = RuleScope.Lookup;

      var rule = new RegexRule(_REGEX_PATTERN, scope);

      Assert.Equal($"Schema name of a Lookup must match to regular expression pattern {_REGEX_PATTERN}.",
                   rule.Description);

    }

    [Fact(DisplayName = "RegexRule: OOB lookup on custom unmanaged entity must be skipped.")]
    public void OobLookupOnCustomUnmanagedEntityMustBeSkipped() {
      var entity = new EntityMetadata() {
        SchemaName = "foo_MyEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      var scope = RuleScope.Lookup;

      var owningUserAttr = new LookupAttributeMetadata(LookupFormat.None) {
        SchemaName = "OwningUser"
      };
      owningUserAttr.SetSealedPropertyValue("IsCustomAttribute", false);
      owningUserAttr.SetSealedPropertyValue("IsManaged", false);

      var attributes = new List<AttributeMetadata> {
        owningUserAttr
      };
      var solutionEntity = new SolutionEntity(entity, attributes, true);

      var rule = new RegexRule(_REGEX_PATTERN, scope);
      var results = rule.Validate(solutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Custom lookup on custom unmanaged entity conforming the rule.")]
    public void CustomLookupOnCustomUnmanagedEntityConformingRule() {
      var entity = new EntityMetadata() {
        SchemaName = "foo_MyEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      var scope = RuleScope.Lookup;

      var lookupAttr = new LookupAttributeMetadata(LookupFormat.None) {
        SchemaName = "foo_MyCustomLookupId"
      };
      lookupAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      lookupAttr.SetSealedPropertyValue("IsManaged", false);

      var attributes = new List<AttributeMetadata> { 
        lookupAttr
      };
      var solutionEntity = new SolutionEntity(entity, attributes, true);

      var rule = new RegexRule(_REGEX_PATTERN, scope);
      var results = rule.Validate(solutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Custom lookup on custom unmanaged entity conforming the rule result string.")]
    public void CustomLookupOnCustomUnmanagedEntityConformingRuleResultStringIsCorrect() {
      var entity = new EntityMetadata() {
        SchemaName = "foo_MyEntity",
        DisplayName = new Label() { 
          UserLocalizedLabel = new LocalizedLabel("My Entity", 1033)
        }
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      var scope = RuleScope.Lookup;

      var lookupAttr = new LookupAttributeMetadata(LookupFormat.None) {
        SchemaName = "foo_MyCustomLookupId"
      };
      lookupAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      lookupAttr.SetSealedPropertyValue("IsManaged", false);

      var attributes = new List<AttributeMetadata> {
        lookupAttr
      };
      var solutionEntity = new SolutionEntity(entity, attributes, true);

      var rule = new RegexRule(_REGEX_PATTERN, scope);
      var results = rule.Validate(solutionEntity);

      Assert.Equal($"Rule: {rule.Description} Succeeded for entity \"My Entity\" (foo_MyEntity).",
                   results.FormatValidationResult());
    }

    [Fact(DisplayName = "RegexRule: Custom lookup on custom unmanaged entity not conforming the rule.")]
    public void CustomLookupOnCustomUnmanagedEntityNotConformingRule() {
      var entity = new EntityMetadata() {
        SchemaName = "foo_MyEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      var scope = RuleScope.Lookup;

      var lookupAttr = new LookupAttributeMetadata(LookupFormat.None) {
        SchemaName = "foo_MyCustomLookup"
      };
      lookupAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      lookupAttr.SetSealedPropertyValue("IsManaged", false);

      var attributes = new List<AttributeMetadata> {
        lookupAttr
      };
      var solutionEntity = new SolutionEntity(entity, attributes, true);

      var rule = new RegexRule(_REGEX_PATTERN, scope);
      var results = rule.Validate(solutionEntity);

      Assert.False(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Custom lookup on custom unmanaged entity not conforming the rule.")]
    public void CustomLookupOnCustomUnmanagedEntityNotConformingRuleValidationMessage() {
      var entity = new EntityMetadata() {
        SchemaName = "foo_MyEntity",
        DisplayName = new Label() {
          UserLocalizedLabel = new LocalizedLabel("My Entity", 1033)
        }
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      var scope = RuleScope.Lookup;

      var lookupAttr = new LookupAttributeMetadata(LookupFormat.None) {
        SchemaName = "foo_MyCustomLookup"
      };
      lookupAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      lookupAttr.SetSealedPropertyValue("IsManaged", false);

      var attributes = new List<AttributeMetadata> {
        lookupAttr
      };
      var solutionEntity = new SolutionEntity(entity, attributes, true);

      var rule = new RegexRule(_REGEX_PATTERN, scope);
      var results = rule.Validate(solutionEntity);

      Assert.Equal($"Rule failed: {rule.Description} Following lookups do not match given pattern: foo_MyCustomLookup.",
                   results.FormatValidationResult());
    }

    [Fact(DisplayName = "RegexRule: Custom managed lookup on custom managed entity must be skipped.")]
    public void ManagedCustomLookupOnCustomUnmanagedEntityMustConformingRule() {
      var entity = new EntityMetadata() {
        SchemaName = "foo_MyEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      var scope = RuleScope.Lookup;

      var lookupAttr = new LookupAttributeMetadata(LookupFormat.None) {
        SchemaName = "foo_MyCustomLookup"
      };
      lookupAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      lookupAttr.SetSealedPropertyValue("IsManaged", true);

      var attributes = new List<AttributeMetadata> {
        lookupAttr
      };
      var solutionEntity = new SolutionEntity(entity, attributes, true);

      var rule = new RegexRule(_REGEX_PATTERN, scope);
      var results = rule.Validate(solutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Only lookup fields are checked on custom unmanaged entity.")]
    public void OnlyLookupsAreCheckedOnCustomUnmanagedEntity() {
      var entity = new EntityMetadata() {
        SchemaName = "foo_myEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      var scope = RuleScope.Lookup;

      var stringAttr = new StringAttributeMetadata() {
        SchemaName = "foo_StrField"
      };
      stringAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      stringAttr.SetSealedPropertyValue("IsManaged", false);

      var primaryKeyAttr = new UniqueIdentifierAttributeMetadata() {
        SchemaName = "foo_myEntityId"
      };
      primaryKeyAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      primaryKeyAttr.SetSealedPropertyValue("IsManaged", false);

      var attributes = new List<AttributeMetadata> {
        stringAttr,
        primaryKeyAttr
      };
      var solutionEntity = new SolutionEntity(entity, attributes, true);

      var rule = new RegexRule(_REGEX_PATTERN, scope);
      var results = rule.Validate(solutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Exluded lookup fields aren't checked on custom unmanaged entity.")]
    public void ExcludedLookupsAreCheckedOnCustomUnmanagedEntity() {
      var entity = new EntityMetadata() {
        SchemaName = "foo_myEntity",
      };
      entity.SetSealedPropertyValue("IsManaged", false);
      entity.SetSealedPropertyValue("IsCustomEntity", true);

      var scope = RuleScope.Lookup;

      var lookupAttr = new LookupAttributeMetadata() {
        SchemaName = "foo_LookupFieldId2"
      };
      lookupAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      lookupAttr.SetSealedPropertyValue("IsManaged", false);

      var primaryKeyAttr = new LookupAttributeMetadata() {
        SchemaName = "foo_MyEntityId"
      };
      primaryKeyAttr.SetSealedPropertyValue("IsCustomAttribute", true);
      primaryKeyAttr.SetSealedPropertyValue("IsManaged", false);

      var attributes = new List<AttributeMetadata> {
        lookupAttr,
        primaryKeyAttr
      };
      var solutionEntity = new SolutionEntity(entity, attributes, true);

      var excludedSchemaNames = new string[] { "foo_myEntity.foo_LookupFieldId2" };

      var rule = new RegexRule(_REGEX_PATTERN, scope, excludedSchemaNames);
      var results = rule.Validate(solutionEntity);

      Assert.True(results.Passed);
    }

    private const string _REGEX_PATTERN = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*Id$";

  }

}
