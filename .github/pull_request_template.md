## Summary

## Validation

- [ ] `nuget restore src/AG.StepWizard.sln`
- [ ] `msbuild src/AG.StepWizard.sln /p:Configuration=Release`
- [ ] `dotnet pack src/AG.StepWizard/AG.StepWizard.csproj -c Release -o ./artifacts`
- [ ] Screenshots updated for appearance/layout changes

## Checklist

- [ ] Public API changes are documented
- [ ] Old AeroWizard package identity was not reintroduced
- [ ] Theme changes cover light, dark, blue dark, system, and high contrast
