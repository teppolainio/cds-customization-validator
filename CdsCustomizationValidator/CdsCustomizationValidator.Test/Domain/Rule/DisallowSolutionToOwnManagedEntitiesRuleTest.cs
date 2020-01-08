using CdsCustomizationValidator.Domain.Rule;
using Xunit;

namespace CdsCustomizationValidator.Test.Domain.Rule {

  /// <summary>
  /// Unit tests for <see cref="DisallowSolutionToOwnManagedEntitiesRule"/>
  /// </summary>
  public class DisallowSolutionToOwnManagedEntitiesRuleTest {

    [Fact(DisplayName = "DisallowSolutionToOwnManagedEntitiesRule: Description when allowed.")]
    public void DescriptionWhenAllowd() {
      var rule = new DisallowSolutionToOwnManagedEntitiesRule(true);

      Assert.Equal("Allow solution to own managed entities.",
                   rule.Description);
    }

    [Fact(DisplayName = "DisallowSolutionToOwnManagedEntitiesRule: Description when disallowed.")]
    public void DescriptionWhenDisallowd() {
      var rule = new DisallowSolutionToOwnManagedEntitiesRule(false);

      Assert.Equal("Disallow solution to own managed entities.",
                   rule.Description);
    }

  }

}
