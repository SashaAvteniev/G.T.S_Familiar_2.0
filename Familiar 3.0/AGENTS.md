# AGENTS.md

## Project Snapshot
- Unity project on **Unity 6** (`ProjectSettings/ProjectVersion.txt`: `6000.3.7f1`).
- Built-in gameplay code is currently small and scene-driven (3 runtime scripts in `Assets/Scripts` + `Assets/Shaders/CutoutObject.cs`).
- Build settings include only `Assets/Scenes/SampleScene.unity`; active gameplay prototype logic lives in `Assets/Scenes/CameraTest.unity`.
- No prior AI-agent convention files were found via glob search (`.github/copilot-instructions.md`, `AGENTS.md`, `CLAUDE.md`, `.cursorrules`, etc.).

## Architecture (What Talks to What)
- `PlayerInput` in `Assets/Scenes/CameraTest.unity` (object `InputSystem`) dispatches UnityEvents directly to `PlayerMovement.Move` and `PlayerMovement.Jump`.
- `PlayerMovement` (`Assets/Scripts/Movement/PlayerMovement.cs`) applies movement by mutating `player.transform.position` in `FixedUpdate`; it does **not** drive `Rigidbody` velocity.
- `CameraFollow` (`Assets/Scripts/Camera/CameraFollow.cs`) follows the player by lerping the camera transform toward `target + followDistance`.
- `CutoutObject` (`Assets/Shaders/CutoutObject.cs`) raycasts from camera to target, then writes `_CutoutPos/_CutoutSize/_FalloffSize/_ShowCutout` on hit wall materials.
- Scene wiring is the source of truth for gameplay references (`target`, `wallMask`, movement speed, etc.), so inspect `CameraTest.unity` before changing script defaults.

## High-Value Project Conventions
- Layer names are semantically important and case-sensitive in code:
  - Ground check: `LayerMask.NameToLayer("floor")` in `PlayerMovement`.
  - Wall cutout mask uses layer `Walls` (layer index 6 in `ProjectSettings/TagManager.asset`).
- Input assets are split:
  - Project-wide Input System setting points to `Assets/InputSystem_Actions.inputactions`.
  - `CameraTest.unity` `PlayerInput` currently references `Assets/Familiar 3.0.inputactions`.
  - If input behavior seems inconsistent, reconcile these two assets first.
- Script organization is partial and pragmatic (`Assets/Scripts/Camera`, `Assets/Scripts/Movement`, plus runtime `CutoutObject` under `Assets/Shaders`). Keep edits near existing subsystem folders unless intentionally refactoring.
- `Assets/Prefabs/Player.prefab` is visual/collider-only; movement behavior is attached in the scene instance, not the prefab.

## Developer Workflows (Practical)
- Open/run with Unity Editor `6000.3.7f1`; scene iteration is mainly in `Assets/Scenes/CameraTest.unity`.
- Check build scene list before release work in `ProjectSettings/EditorBuildSettings.asset`.
- Useful local compile sanity check (requires Unity-generated csproj references on your machine):

```bash
set -gx PROJECT_ROOT "/home/nmbdude/Documents/Familiar/Familiar 3.0"
dotnet build "$PROJECT_ROOT/Assembly-CSharp.csproj"
```

- Use Unity Console for runtime validation of camera/material behavior (`CutoutObject` logs each material update).

## Integration Points
- Rendering pipeline: URP (`com.unity.render-pipelines.universal`) with assets under `Assets/Settings/` (`PC_RPAsset.asset`, `Mobile_RPAsset.asset`, renderers, volume profiles).
- Input backend: new Input System (`com.unity.inputsystem`) with event-driven callbacks from `PlayerInput`.
- Navigation and test framework packages are present in `Packages/manifest.json`, but there are no gameplay tests in `Assets/` yet.

