using CommandLine;

namespace CdsCustomizationValidator.App {

  /// <summary>
  /// Command line arguments.
  /// </summary>
  internal class Options {

    [Option("connectionstring",
            Required = true,
            HelpText = "Connection string to CDS.")]
    public string CdsConnectionString { get; set; }

    [Option("solution",
            Required = true,
        HelpText = "Base name of the solution which is validated.")]
    public string Solution { get; set; }

    [Option("rulefile",
            Required = true,
            HelpText = "Relative path to file containing validation rules.")]
    public string RuleFile { get; set; }

  }

}
