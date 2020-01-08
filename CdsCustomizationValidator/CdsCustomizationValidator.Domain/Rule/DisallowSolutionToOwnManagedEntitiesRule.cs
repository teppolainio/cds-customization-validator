namespace CdsCustomizationValidator.Domain.Rule {
  /// <summary>
  /// Rule to check if entity is managed and owned by solution. This can be
  /// either allowed or disallowed. You probably do not want to allow this.
  /// </summary>
  public class DisallowSolutionToOwnManagedEntitiesRule: CustomizationRuleBase {

    /// <summary>
    /// See <see cref="CustomizationRuleBase.Description"/>.
    /// </summary>
    public override string Description {
      get {
        var allowance = "Disallow";
        if(_allowSolutionToOwnManagedEntities) {
          allowance = "Allow";
        }
        return $"{allowance} solution to own managed entities.";
      }
    }

    /// <summary>
    /// Creates a new rule instance.
    /// </summary>
    /// <param name="allowSolutionToOwnManagedEntities">
    /// If true solution is allowed to own managed entities.
    /// Note! You probably don't want to allow this.
    /// </param>
    public DisallowSolutionToOwnManagedEntitiesRule(
        bool allowSolutionToOwnManagedEntities) {
      _allowSolutionToOwnManagedEntities = allowSolutionToOwnManagedEntities;
    }

    /// <summary>
    /// See <see cref="CustomizationRuleBase.ValidateRule(SolutionEntity)"/>.
    /// </summary>
    protected override ValidationResult ValidateRule(SolutionEntity solutionEntity) {
      var result = true;

      if(!_allowSolutionToOwnManagedEntities &&
          solutionEntity.IsOwnedBySolution &&
          solutionEntity.Entity.IsManaged == true) {

        result = false;
      }

      return new ValidationResult(solutionEntity.Entity,
                                  result,
                                  this);
    }

    private readonly bool _allowSolutionToOwnManagedEntities;

  }

}
