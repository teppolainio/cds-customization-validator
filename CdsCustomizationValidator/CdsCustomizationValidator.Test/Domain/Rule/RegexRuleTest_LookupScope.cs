using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeXrmEasy.Extensions;
using CdsCustomizationValidator.Domain.Rule;
using CdsCustomizationValidator.Domain;
using Xunit;
using Microsoft.Xrm.Sdk;

namespace CdsCustomizationValidator.Test.Domain.Rule {
  
  public class RegexRuleTest_LookupScope {

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

      Assert.Equal($"Rule: {rule.Description} Failed for entity \"My Entity\" (foo_MyEntity). Following lookups do not match given pattern: foo_MyCustomLookup.",
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

    private const string _REGEX_PATTERN = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]Id*$";

  }

}
