# BlueLuck Event

![Development](https://img.shields.io/badge/development-AI%20generated-blue)
![Validation](https://img.shields.io/badge/validation-human%20reviewed-success)
![Framework](https://img.shields.io/badge/.NET-net6.0-purple)
![Status](https://img.shields.io/badge/status-foundation-orange)

### Ready-to-use V Rising events, built through AI-assisted engineering and live-server requirements.

BlueLuck Event is a clean-room, lightweight server-side event framework for V Rising dedicated servers. It is designed around a minimal core, a curated action catalog capped at **300 actions**, and complete event files that server owners can understand and customize.

BlueLuck is intentionally separate from the larger BattleLuck development project. BattleLuck remains the experimental laboratory. BlueLuck is the smaller public edition intended to become easy to install, configure, test, and release.

> Built with AI from the ground up. Shaped by real server owners.

## Product goals

- Keep the plugin small and readable.
- Enforce one action catalog and one action executor.
- Cap the action catalog at 300 verified actions.
- Store each event in one JSON file.
- Ship prepared event templates.
- Let an optional AI assistant suggest small, approved changes.
- Block destructive world and progression operations by default.
- Keep all live game mutations behind a native game bridge.

## Current status

This repository contains the **clean minimal foundation**, not a finished production release. The configuration, event schema, action limits, validation pipeline, session model, and AI-edit boundary are implemented. Native ProjectM gameplay adapters are deliberately isolated behind `IGameBridge` and are the next implementation phase.

That distinction matters. Software becomes much easier to market when it does not lie to people first.

## Architecture

```text
BlueLuckPlugin
└── BlueLuckCore
    ├── ActionCatalog          max 300 actions
    ├── ActionExecutor         one execution path
    ├── EventLoader            one JSON per event
    ├── EventValidator         catalog-aware startup checks
    ├── EventController        one session owner
    ├── BlueLuckAssistant      optional plan validator
    └── IGameBridge            native V Rising boundary
```

## Included event templates

- Bloodbath
- Colosseum
- Survival Waves
- Boss Hunt
- Team Arena
- AI Event Sandbox

## Build

```powershell
dotnet restore .\BlueLuck.csproj
dotnet build .\BlueLuck.csproj -c Release --no-restore
```

Target framework: `net6.0`.

## Push to GitHub

Run from the extracted project directory:

```powershell
.\PUSH_TO_GITHUB.ps1
```

## Related project

BattleLuck is the larger experimental development environment used to research advanced V Rising event systems. BlueLuck Event is a separate clean-room implementation focused on stability, simplicity, public releases, and ready-to-use event packages.

## License

MIT.
