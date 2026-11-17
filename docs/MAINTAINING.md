# Maintaining this fork

## Scope and provenance

Maintain English documentation and changes needed for the owner's Windows environment. Track original work at https://github.com/Maplespe/DWMBlurGlass. A period without commits does not establish that upstream is abandoned.

The runtime baseline is official 2.3.1 commit 828f09503193aaadf2a00edcbcbadbf21b39e057. Record renamed-runtime testing separately from the earlier DWMBlurGlass-named test.

## Git workflow

```powershell
git clone https://github.com/thatirishguyyy/MKUltraBlurGlass.git
cd MKUltraBlurGlass
git remote add upstream https://github.com/Maplespe/DWMBlurGlass.git
git fetch upstream
git switch -c fix/describe-the-change
```

Review upstream commits before merging or cherry-picking them. Keep functional fixes separate from translations, and submit fork changes to this fork rather than accidentally targeting upstream.

## Release checklist

- Establish a successful x64 build and record toolchain, SDK, NuGet versions, and the MiaoUI commit.
- Test on the exact target Windows build and record it in release notes. Start with the owner's configuration; expand support claims only with evidence.
- Check install, uninstall, reboot persistence, symbol download, active/inactive windows, light/dark mode, and each claimed blur method. Check mixed DPI or multiple monitors when relevant.
- Package matching binaries, resources, translations, license texts, and corresponding source. Distinguish fork releases clearly from upstream releases.
- Keep a previous working package and document rollback steps.

## Documentation and translations

`README.md` and `docs/` are the maintained English entry points. Other `README_*.md` files are preserved upstream translations. Translate new user and maintainer instructions into English; do not rename XML localization keys or change code identifiers as part of documentation work.

Issue templates are in `.github/ISSUE_TEMPLATE`. GitHub currently reports issues disabled for this fork; enable them in repository settings if you want to accept reports. The existing issue workflow closes empty/template-only reports and is not a build or release pipeline.

Original donation configuration points to upstream's `maplespe` account. It is not a donation destination for the fork owner.

## Upstream updates

Keep the GitHub fork connection and original attribution. Do not sync upstream changes directly into mkultra/main. Review candidate changes on a separate branch, run the relevant regression checks and builds, and complete appropriate live testing before merging. The upstream commits-behind count is not a release requirement.
