# Responsive UI Refactor Guide (Unity Tic-Tac-Toe)

This guide keeps your existing scripts and OnClick wiring, while making UI responsive for portrait + landscape.

## 1) Canvas setup (both PlayScene and GameScene)

1. Select **Canvas**.
2. Set **Canvas Scaler**:
   - UI Scale Mode: `Scale With Screen Size`
   - Reference Resolution: `1080 x 1920`
   - Screen Match Mode: `Match Width Or Height`
   - Match: `0.5`
3. Keep a **GraphicRaycaster** on Canvas.

## 2) Root layout hierarchy

Under each Canvas, create this structure (names can match exactly):

- `SafeAreaRoot` (RectTransform)
  - Anchors min `(0,0)` max `(1,1)`, offsets all `0`
- `OrientationLayoutSwitcher` (empty GameObject + script `OrientationLayoutSwitcher`)
  - `PortraitLayoutRoot`
  - `LandscapeLayoutRoot`

Assign script references:
- `portraitLayoutRoot` -> PortraitLayoutRoot
- `landscapeLayoutRoot` -> LandscapeLayoutRoot

> Keep your existing functional objects (MainMenuPanel, BoardPanel, HUDPanel, popups) under the active portrait/landscape roots. Do not rename button objects used by OnClick.

## 3) Main Menu responsive setup

Inside both `PortraitLayoutRoot` and `LandscapeLayoutRoot` create:

- `MainMenuPanel` (Image)
  - Add `VerticalLayoutGroup`
    - Child Alignment: `Middle Center`
    - Spacing: `18`
    - Padding: Left/Right `30`, Top/Bottom `30`
    - Child Control Width/Height: `true`
    - Child Force Expand Width: `true`
    - Child Force Expand Height: `false`
  - Add `ContentSizeFitter`
    - Horizontal Fit: `Unconstrained`
    - Vertical Fit: `Preferred Size`

For each menu button (Play, Settings, Stats, Theme, Exit):
- Add `LayoutElement`
  - Min Height: `110`
  - Preferred Height: `130`
  - Flexible Height: `0`

Panel anchors:
- Portrait: anchor center, width ~`82%`, height ~`70%`
- Landscape: anchor center-left or center, width ~`55%`, height ~`80%`

This guarantees buttons remain inside `MainMenuPanel`.

## 4) Popup responsive setup

For each popup root (ThemeSelectionPopup, SettingsPopup, StatsPopup, ExitConfirmationPopup, GameOverPopup):

1. Fullscreen blocker root:
   - Anchors stretch full screen `(0,0)` to `(1,1)`.
2. `PopupPanel` child (centered Image):
   - Portrait size: width `85%`, height `60%`
   - Landscape size: width `65%`, height `75%`
3. Add to `PopupPanel`:
   - `VerticalLayoutGroup`
     - Child Alignment: `Upper Center`
     - Spacing: `16`
     - Padding: Left/Right `24`, Top `24`, Bottom `24`
   - `ContentSizeFitter`
     - Vertical Fit: `Preferred Size`

For popup texts and buttons:
- Text objects: add `LayoutElement` with preferred heights (`Title: 90`, Body: `160`)
- Buttons row object: add `HorizontalLayoutGroup` with spacing `16`
- Each popup button: add `LayoutElement` (`Preferred Width: 220`, `Preferred Height: 96`)

This keeps popup text/buttons inside popup panel in both orientations.

## 5) Game board + HUD setup

Create `GameAreaRoot` in each orientation root.

### Board container
- `BoardContainer` (center area)
  - Add `HorizontalLayoutGroup` or `VerticalLayoutGroup` depending on your orientation layout.
- Existing `BoardPanel`:
  - Anchored center.
  - Add `AspectRatioFitter`:
    - Aspect Mode: `Fit In Parent`
    - Aspect Ratio: `1`
  - Add `LayoutElement`.
  - Add script `BoardAspectKeeper`.
    - Container: `BoardContainer`
    - Portrait Occupancy: `0.9`
    - Landscape Occupancy: `0.75`
    - Extra Padding: `24`

### Board grid
- Keep existing 3x3 `GridLayoutGroup`.
- Constraint: `Fixed Column Count = 3`.
- Child Alignment: `Middle Center`.
- If needed, set spacing to something small (`8-16`) for different resolutions.

### HUD panel
- Place `HUDPanel` in top area, separate from board container.
- HUD anchors:
  - Stretch top: min `(0,1)`, max `(1,1)` with fixed height (e.g. `220` portrait, `160` landscape).
- Add `HorizontalLayoutGroup` to arrange timer/turn/move labels.
- On HUD root add script `DisableNonInteractiveRaycasts`.

Important:
- For decorative HUD Image/TMP text that should not block clicks, uncheck **Raycast Target**.
- Keep Settings/Exit buttons raycast-enabled.

## 6) Orientation-specific arrangements

Use different roots to keep setup simple:

- `PortraitLayoutRoot`
  - Top: HUD
  - Middle: square board
  - Bottom: optional controls

- `LandscapeLayoutRoot`
  - Left: board (square)
  - Right: HUD and controls stack

`OrientationLayoutSwitcher` script automatically activates one root based on screen shape.

## 7) Existing script/onClick safety checklist

- Do **not** rename existing objects referenced by serialized fields.
- Do **not** remove existing components used by scripts (`MainMenuController`, `HUDController`, popup scripts, `BoardCell`).
- If duplicating buttons for portrait/landscape, wire OnClick to same methods as original.
- Keep EventSystem in scene.

## 8) Components added by this refactor

Add these scripts:
- `OrientationLayoutSwitcher.cs`
- `BoardAspectKeeper.cs`
- `DisableNonInteractiveRaycasts.cs`

They are intentionally small and suitable for a homework-scale project.
