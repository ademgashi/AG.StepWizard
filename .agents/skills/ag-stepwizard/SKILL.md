---
name: ag-stepwizard
description: Repository guidance for maintaining AG.StepWizard, a .NET Framework 4.7 WinForms NuGet package.
---

# AG.StepWizard Repository Skill

Use this skill when working in this repository.

## Identity Rules

- Package ID: `AG.StepWizard`
- Assembly name: `AG.StepWizard`
- Root namespace: `AG.StepWizard`
- Product: `Modern Step Wizard`
- Author: `Adem Gashi`
- Repository URL: `https://github.com/ademgashi/AG.StepWizard`
- Do not reintroduce old AeroWizard package identity.
- Keep AeroWizard references only in license, credits, and migration notes.

## Build And Pack

```bash
nuget restore src/AG.StepWizard.sln
msbuild src/AG.StepWizard.sln /p:Configuration=Release
dotnet pack src/AG.StepWizard/AG.StepWizard.csproj -c Release -o ./artifacts
```

## Theme Requirements

Every appearance must cover header, footer, step list, selected step, completed indicator, buttons, hover, pressed, disabled, focus, borders, wizard background, and page background.

Themes are token-first. Do not hardcode rendered colors in controls; route every visual surface through `WizardTheme`/`StepWizardTheme` tokens and `ThemeCatalog` built-ins.

Required theme tokens: `Name`, `IsDark`, `WindowBack`, `ContentBack`, `HeaderBack`, `SidebarBack`, `CardBack`, `Border`, `Text`, `MutedText`, `Accent`, `AccentText`, `HoverBack`, `SelectedBack`, `DisabledText`, `Success`, `Warning`, and `Error`.

Wizard page child controls should inherit the active theme through `ThemePageControls`. Keep propagation recursive, support controls added at runtime, and provide an opt-out for applications with custom page styling.

## Branching

- `main`: protected release branch
- `develop`: integration branch
- `feature/*`: feature work
- `fix/*`: bug fixes
- `release/*`: stabilization
- `hotfix/*`: urgent fixes from `main`
