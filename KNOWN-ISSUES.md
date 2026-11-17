# Known issues

## Color picker interaction

Owner report, November 17, 2026: choosing or entering a color does not appear to commit reliably until the selector is moved on the main color palette. Exact reproduction steps and affected input controls are not yet confirmed. Investigate input-change notifications and commit behavior in a future change. This report is separate from the fixed per-window dark-mode detection bug; no color-picker changes are included in 0.1.1.
