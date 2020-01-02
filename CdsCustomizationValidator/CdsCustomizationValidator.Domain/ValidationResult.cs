using Microsoft.Xrm.Sdk.Metadata;

namespace CdsCustomizationValidator.Domain
{
    /// <summary>
    /// Represents a customization validation results.
    /// </summary>
    public class ValidationResult
    {
        public EntityMetadata Entity { get; internal set; }
        public bool Passed { get; internal set; }
    }

}