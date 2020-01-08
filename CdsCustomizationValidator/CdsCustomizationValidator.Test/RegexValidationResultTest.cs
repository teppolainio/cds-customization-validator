using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using Xunit;

namespace CdsCustomizationValidator.Test {

  public class RegexValidationResultTest {

    [Fact(DisplayName = "RegexValidationResult: Entity scope must not take attributes on result object.")]
    public void EntityValidationResultMustNotTakeAttributes() {

      var metadata = new EntityMetadata();
      var rule = new RegexRule("[A-Z]", RuleScope.Entity);

      var failingAttributes = new List<AttributeMetadata>();

      var ex = Assert.Throws<ArgumentException>(
          () => new RegexValidationResult(metadata,
                                          failingAttributes,
                                          rule));

      Assert.StartsWith("Results for checking entity schema name doesn't depend from attributes. You are likely using wrong constructor.",
                        ex.Message);
    }

    [Fact(DisplayName = "RegexValidationResult: Attribute scope must have failing attributes on result object.")]
    public void AttributeValidationResultMustHaveFailingAttributes() {

      var metadata = new EntityMetadata();
      var rule = new RegexRule("[A-Z]", RuleScope.Attribute);

      var results = new RegexValidationResult(metadata,
                                              false,
                                              rule);

      var ex = Assert.Throws<InvalidOperationException>(
          () => results.FormatValidationResult());

      Assert.Equal("Results for checking attribute schema names must be given list of failing attributes.",
                   ex.Message);
    }

    [Fact(DisplayName = "RegexValidationResult: Attribute scope must have attributes on result object even when there isn't any failures.")]
    public void AttributeValidationResultMustTakeFailingAttributesAlsoWhenNotFailing() {

      var metadata = new EntityMetadata();
      var rule = new RegexRule("[A-Z]", RuleScope.Attribute);

      var failingAttributes = new List<AttributeMetadata>();

      var ex = Record.Exception(
          () => new RegexValidationResult(metadata,
                                          failingAttributes,
                                          rule));

      Assert.Null(ex);
    }

  }

}
