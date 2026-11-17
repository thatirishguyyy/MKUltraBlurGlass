# Security policy

## Supported versions

Security fixes target the latest published MKUltraBlurGlass release. Older releases and development snapshots do not receive separate security backports. Support for this fork does not imply support for the original DWMBlurGlass project or other forks.

## Reporting a vulnerability

Please report suspected security vulnerabilities privately using [GitHub's Report a vulnerability form](https://github.com/thatirishguyyy/MKUltraBlurGlass/security/advisories/new). You can also find it under **Security → Advisories → Report a vulnerability** in this repository. A GitHub account is required.

Do not open a public issue containing exploit details, credentials, or sensitive system information. Ordinary crashes, symbol-download problems, visual defects and feature requests belong in normal project support channels unless they have a security impact.

Include, where available:

- The affected release or commit and exact Windows version/build.
- A description of the security impact and any required privileges or configuration.
- Reproduction steps or a minimal proof of concept.
- Relevant logs or screenshots with personal information and secrets removed.
- Any suggested mitigation or fix.

Do not include passwords, access tokens, unrelated personal files or full memory dumps in an initial report. If more information is needed, coordinate through the private advisory.

## Handling reports

This is a personal maintenance fork maintained by **thatirishguyyy**, based on the work of **Maplespe** and upstream contributors. Reports are reviewed as maintainer availability permits; there is no guaranteed response or remediation timeline and no bug-bounty program.

When a report is confirmed, the maintainer will work with the reporter on a fix or mitigation and coordinate disclosure through GitHub security advisories where appropriate. If the issue also affects upstream or a dependency, coordinate sharing relevant details privately before public disclosure.

## Security context

MKUltraBlurGlass runs with elevated privileges and modifies behavior in the Windows Desktop Window Manager. A security flaw can therefore have consequences beyond the GUI. Changes to task registration, process handling, DLL loading, downloaded symbols, file paths and uninstall cleanup require particular care during review and testing.

Download releases from [this repository's Releases page](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases). SHA-256 checksums are provided to check file integrity; they are not a code-signing certificate or a guarantee that software is free of vulnerabilities. Preserve original licenses and attribution when distributing modified builds.
