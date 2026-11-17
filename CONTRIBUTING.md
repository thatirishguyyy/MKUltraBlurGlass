# Contributing

Use English for fork documentation, bug reports, and pull requests. See [building](docs/BUILDING.md) and [maintenance](docs/MAINTAINING.md) before changing the application.

For bugs, include the full Windows build from `winver`, application version or commit, blur method, reproduction steps, expected and actual behavior, and other customization software. Report fork-specific problems to this fork when issues are enabled.

Keep changes focused. Describe what changed and how it was verified. Do not claim Windows compatibility based only on a successful build. Preserve copyright notices, license texts, translation credits, and attribution to upstream.

For language files, copy `Languagefiles/en-US.xml`, change the filename and `lang` element's `local` attribute to the target locale, and translate values only. Keep placeholders and literal `\n` escapes. Test the file in the application before submitting it.
