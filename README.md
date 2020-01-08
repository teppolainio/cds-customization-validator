# cds-customization-validator
Tool which validates that CDS (i.e. Dynamics CRM / Dynamics 365) customizations are done according to defined configurable ruleset.

Validation tool is meant to run against unmanaged solution in its development environment.

# Validation capabilities
Currently following things can be validated:
* Is solution allowed to own managed entitites.
* Require entity schema name to have certain prefix.
* Require attribute schema name to have certain prefix.
* Require schema name to conform given regular expression. This functionality can be scoped to entity, all attributes or only lookup attributes.

Apart from the first rule only unmanaged custom attributes and entities are included in validation because they are owned by the solution.

# Running the tool
Currently you need to clone the repo. Build solution and execute console application. Console application has help about command line arguments. You need to create rule file for your rules. Example rule file is included in console application project.

# Future development

In future there likely will rule which checks that description is given an all custom unmanaged entities and attributes in solution.

At some point it would be nice to release NuGet of this tool and to create XrmToolBox plugin.
