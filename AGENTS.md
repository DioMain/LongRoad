# Long Road — Agent Guide

Text adventure / trail simulation in the spirit of *The Oregon Trail*. Unity 6 (6000.4) + URP 2D + Input System + Unity Localization.

## Assemblies (dependency direction)

```
LongRoad.Packages  (no game deps)
LongRoad.Domain    (no Unity; no Packages)
LongRoad.Core      → Domain, Packages
LongRoad           → Domain, Core, Packages
```

| Folder | Assembly | Role |
|--------|----------|------|
| `Assets/_Game/LongRoad.Domain` | `LongRoad.Domain` | Any logic that needs **no Unity** — pure C# rules, contracts, formulas |
| `Assets/_Game/LongRoad.Core` | `LongRoad.Core` | **Base** systems & implementations (including shared game systems) that use Unity or are reused by features |
| `Assets/_Game/LongRoad` | `LongRoad` | Main game pipeline, UI, scene wiring, feature systems that **compose** Core/Domain |
| `Assets/Packages` | `LongRoad.Packages` | Local/third-party scripts (not UPM) |

Game assets live under `Assets/_Game/Resources/` (ScriptableObjects, locales, scenes). Do not put game content in `Assets/Resources`.

## How to choose a layer

Ask in order:

1. **Does it need Unity** (`UnityEngine`, SO, MonoBehaviour, coroutines, sprites, localization APIs)?  
   - No → **Domain**  
   - Yes → continue
2. **Is it a reusable base building block** (shared entity/scriptable, event runner, localization, input wrapper, shared person/status model)?  
   - Yes → **Core**  
   - No → **LongRoad** (feature, screen, one-off event, turn/scene pipeline)

Core **may** contain shared game logic. LongRoad owns the **main pipeline** and feature-specific systems that call into those bases.

## Hard rules

1. **Domain never references Unity** (`noEngineReferences: true`). No `UnityEngine`, `MonoBehaviour`, ScriptableObject, or coroutines in Domain.
2. Dependencies point **inward** only. Domain knows nothing about Core/LongRoad.
3. Prefer Domain for substantial pure rules (easier EditMode tests); keep Unity-bound shared systems in Core; keep orchestration/UI in LongRoad.
4. Prefer thin MonoBehaviours: read state, send intents, render — heavy work in Core/Domain types.
5. Do not add game code under `Assets/` outside `_Game/` and `Packages/` unless configuring Unity itself.
6. **Naming — Manager vs Service**
   - **`*Manager`**: scene/global controllers that are `MonoBehaviour` (prefer subclassing `LongRoadBehaviour` / `LongRoadBehaviourCore`).
   - **`*Service`**: plain (non-MonoBehaviour) orchestration classes; implement `LongRoad.Domain.Interfaces.IService`; live under `Services/` (namespace `LongRoad.Services`). **Inject dependencies via constructor** (e.g. `new TravelService(data)`), not `Init(...)` using ctor.
   - **Exception**: `GamePipeline` — the full turn loop for the run; not a `*Service`, lives at assembly root (`LongRoad`), not under `Services/`.

## Runtime wiring

- **`GameManager`** — global singleton (`DontDestroyOnLoad`): shared services (e.g. `PlayerInput`, `LocalizationManager`).
- **`LocalManager`** — manager for the active **game scene**; owns `GameData`, `GamePipeline`, `PersonService`, `InventoryService`, `GameTimeService`, `TravelService`, `LocationService`, `MoneyService`, and kicks `CarModelManager.Init` / `GameUIManager.Init` after services are ready.
- **`GameUIManager`** — scene UI host (`UIDocument` + `MainUI.uxml`); caches named slots (`Hud`, `Content`, `Party`, `Inventory`, `Location`, `Overlay`); stores and initializes child `GameUIElement` scripts. Access via `Local.UI` / `GameUIManager.Instance`. Child panels use slots from the manager — do not add separate `UIDocument`s for in-game panels.
- **`CarModelManager`** — spawns `CarModel`; exposes `SetState` / `RefreshState`. Subscribes to `Pipeline.OnPhaseChanged`, `Travel.OnArrived` / `OnDeparted`, `Car.OnFuelChanged` (services never reference the manager). States: `Off` (no fuel), `Idle` (Player phase or at location), `Drive` (Modifiers/Event on the road).
- **`HudStatusUI`** (`GameUIElement`) — HUD turn / day / day-night labels + «Поехали» (`Local.Continue`); enabled only in `GamePhase.Player`.
- **`GameData`** — session **data store only** (Car, Turn, Day, IsDaytime, Money, TravelledKm, CurrentLocation, Route). No events/logic; services read/write fields.
- **`GameTimeService`** (`IService`) — advances turn after pipeline phase 3; every 3 turns flips day/night; after night→day increments `Day`. UI: `OnTurnChanged`, `OnDayNightChanged`, `OnDayChanged`.
- **`TravelService`** (`IService`) — after turn advance, adds `Car.DistancePerTurn` km while on the road (`CurrentLocation == null`); arrives when `TravelledKm` hits absolute route stop distance. UI: `OnTravelProgress`, `OnArrived`, `OnDeparted`.
- **`LocationService`** (`IService`) — interact at `CurrentLocation` (gas, shop, hospital, entertainment, leave). UI: `OnChanged`.
- **`MoneyService`** (`IService`) — spend/add session money on `GameData.Money`. UI: `OnMoneyChanged`.
- **`GamePipeline`** — turn loop: (1) player (`Continue`), (2) modifiers + bound Status/Trait events, (3) random `[UseGameEvent]`, then `AdvanceTurn` + `AdvanceTravel`. UI: `OnPhaseChanged`, `OnEnded`.
- **`PersonService`** (`IService`) — party roster; phase-2 hunger/heal tick. UI: `OnPersonAdded` / `OnPersonRemoved` / `OnModifiersApplied`.
- **`InventoryService`** (`IService`) — session cargo; `TryAdd` respects `Car.MaxWeight`; `UseItem` runs bound Item events. UI: `OnChanged` / `OnItemAdded` / `OnItemRemoved` / `OnItemUsed`.
- Scene MonoBehaviours should subclass `LongRoadBehaviour` for access to `Game` / `Local` / `UI`.

## UI events

Subscribe to **services / `GamePipeline` / entities**, not `GameData`. Resolve VisualElement slots only through **`GameUIManager`** (`Local.UI` / `UI.Hud`, `UI.Content`, …):

- **Time**: `Local.Time` — turn / day-night / day
- **Travel / Location / Money**: `Local.Travel`, `Local.Locations`, `Local.Money`
- **Pipeline**: `Local.Pipeline` — phase, win/lose
- **People**: `Local.People` + `PersonEntity.OnStatsChanged` (heal/hunger/mood) / status events
- **Inventory**: `Local.Inventory`
- **Car**: `Data.Car.OnFuelChanged` / `OnDurabilityChanged` (via `SetFuel` / `SetDurability`)

## Game events

- Base runner: `LongRoad.Core.GameEvent.GameEventBase` (coroutine on a `MonoBehaviour`).
- Concrete events in LongRoad (e.g. `LongRoad.GameEvents`).
- **`[UseGameEvent(probability)]`** — random pool for pipeline phase 3 (catalog cached once).
- **`[BoundGameEvent(kind, tag)]`** — bind event class to a Scriptable `Tag` (`Status` / `Trait` / `Item`). Catalog cached in `BoundEventCatalog`.
  - Prefer subclassing `ContextualGameEventBase` for bound events: `Source` (initiator / status-trait owner), optional `Target` (Item used on a person; null = party-wide / no target).
  - `Status` / `Trait`: run in pipeline phase 2 after `PersonService.ApplyPhaseModifiers` (`Source` = that person).
  - `Item`: run via `InventoryService.UseItem(item, host, target?, source?)` (1 unit removed **before** invoke).
- Shared helpers: `BoundEventRunner` in Core.

## Content & localization

- Scriptable content: `Assets/_Game/Resources/` (Character, Cars, Items, Locations, Routes, Specialities, Traits, …). `Car` SO references a `CarModel` prefab (`MonoBehaviour` spawned on the scene); cargo capacity is `Car.MaxWeight`. `Route` stops use absolute `DistanceFromStartKm`.
- Locale assets/tables: `Assets/_Game/Resources/Locales/` — **Main** (UI), **Entities** (named content).
- Entity strings: `{Tag}_name`, `{Tag}_desc` via `LongRoadScriptable` + `LocalizationManager`.
- App locales: `en-US`, `ru-RU` (`LongRoad.Core.Localization.Locale`).

## Motion

- Prefer **DOTween** for simple UI/object animations. Avoid custom tween systems for basic cases.

## Vendored & UPM stack

- **Local (`Assets/Packages`)**: NaughtyAttributes, Demigiant DOTween (via `LongRoad.Packages`).
- **UPM**: Input System, Unity Localization, URP / 2D packages, uGUI, Test Framework.
