# Contributing

Thanks for helping improve AG.StepWizard.

## Branching

Use `main` as the protected release branch. Use GitFlow-style branch names:

- `develop` for integration work
- `feature/<short-name>` for features
- `fix/<short-name>` for bug fixes
- `release/<version>` for release stabilization
- `hotfix/<short-name>` for urgent fixes from `main`

Pull requests should target `develop` unless they are release or hotfix work.

## Local Validation

Run these before opening a pull request:

```bash
nuget restore src/AG.StepWizard.sln
msbuild src/AG.StepWizard.sln /p:Configuration=Release
dotnet pack src/AG.StepWizard/AG.StepWizard.csproj -c Release -o ./artifacts
```

## Pull Requests

- Keep changes focused.
- Include screenshots when changing appearance or layout.
- Update README or docs when public API behavior changes.
- Do not reintroduce old AeroWizard package identity.
- Keep the package ID, namespace, assembly, and sample identity as `AG.StepWizard`.

## Theme Requirements

Every built-in appearance must cover:

- wizard background
- page background
- header
- footer
- step list
- selected step
- completed step indicator
- buttons
- hover, pressed, disabled, and focus states
- borders
