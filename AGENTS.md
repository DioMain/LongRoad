# Long Road — Agent Guide

Text adventure / trail simulation in the spirit of *The Oregon Trail*. Unity 6 + URP 2D + Input System.

## Assemblies (dependency direction)

```
LongRoad.Packages  (no game deps)
LongRoad.Domain    (no Unity; no Packages)
LongRoad.Core      → Domain, Packages
LongRoad           → Domain, Core, Packages
```

| Folder | Assembly | Role |
|--------|----------|------|
| `Assets/_Game/LongRoad.Domain` | `LongRoad.Domain` | Pure C# game rules: party, journey, resources, events |
| `Assets/_Game/LongRoad.Core` | `LongRoad.Core` | Unity infrastructure: adapters, services, shared Unity utils |
| `Assets/_Game/LongRoad` | `LongRoad` | Presentation: MonoBehaviours, UI, scene wiring |
| `Assets/Packages` | `LongRoad.Packages` | Local/third-party scripts (not UPM) |

`Resources/` stays outside these assemblies (scenes, assets only).

## Hard rules

1. **Domain never references Unity** (`noEngineReferences: true`). No `UnityEngine`, `MonoBehaviour`, ScriptableObject, or coroutines in Domain.
2. Dependencies point **inward** only. Domain knows nothing about Core/LongRoad.
3. Put game rules and state transitions in Domain; put view/input/audio in LongRoad; put bridges in Core.
4. Prefer small, testable Domain types over logic inside MonoBehaviours.
5. Do not add code under `Assets/` root outside `_Game/` and `Packages/` unless configuring Unity itself.

## Game domain (Oregon Trail–style)

Expect concepts like: wagon party, trail progress, supplies (food, ammo, etc.), health/morale, weather, random encounters, choices with consequences, win/lose conditions. Keep these as Domain models and use-cases; Unity only presents and persists them.
