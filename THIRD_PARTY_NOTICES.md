# Third-Party Notices

BlueLuck is licensed under the MIT License. See `LICENSE`.

## NuGet & runtime dependencies

This file documents the primary third-party components referenced by BlueLuck at build time and runtime.

| Component | Version | Role | License | Source |
| --- | --- | --- | --- | --- |
| `VampireReferenceAssemblies` | `1.1.11-r96495-b8` | V Rising reference assemblies for compilation | (see upstream) | https://github.com/mfoltz/VampireReferenceAssemblies
| `BepInEx.Core` | `6.0.0-be.733` | Core mod loader/runtime APIs | LGPL-2.1-only | https://github.com/BepInEx/BepInEx
| `BepInEx.Unity.IL2CPP` | `6.0.0-be.733` | Unity IL2CPP integration (BepInEx) | LGPL-2.1-only | https://github.com/BepInEx/BepInEx

## Game-provided assemblies

BlueLuck references game assemblies (Stunlock / ProjectM) at build-time but does not include them in the repository or release artifact. Server owners must obtain these from a legitimate V Rising dedicated-server or game installation.

- `Stunlock.Core.dll` (game assemblies are not redistributed)

## Notes

- If you redistribute any third-party binaries (LGPL/GPL), include the corresponding license text and comply with that license's obligations.
- Verify and update this file whenever package versions change.
