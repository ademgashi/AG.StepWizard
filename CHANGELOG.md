# Changelog

All notable changes to AG.StepWizard will be documented in this file.

## 1.3.0

- Added `StepWizardActionButton` for long-running actions with idle, running, success, error, and warning states.
- Added themed vector icons to `StepWizardMessageBox`.
- Added sample controls for message-box scenarios and simulated database connection testing.

## 1.2.0

- Added `StepWizardPage.Suppress` to hide optional pages from runtime Back/Next navigation and the step list while keeping them available in `Pages`.
- Updated the sample app with a runtime suppression toggle for the themed controls step.
- Documented optional page suppression and AeroWizard migration mapping.

## 1.0.0

- Initial AG.StepWizard package.
- Added `StepWizardControl`, `StepWizardPage`, `StepWizardTheme`, and `StepWizardAppearance`.
- Added built-in System, Light, Dark, OLEDBlack, BlueDark, Catppuccin, Monokai, Solarized, Linear, Notion, OpenClaw, Matrix, OneDark, Dracula, Nord, Gruvbox, Tokyo Night, GitHub, VS Code, Visual Studio, Fluent, Windows Classic, and HighContrast appearances.
- Added .NET Framework 4.7 WinForms sample app.
- Removed old AeroWizard package identity, templates, generated docs, native visual style rendering, and classic wizard surface.
