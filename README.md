# Dungeon Scene Management Lab
### Unity Scene Management & Async Transitions

---

## Overview

This project is a Unity lab focused on **Scene Management** and **asynchronous transitions**. The goal is to demonstrate proper scene handling, persistent managers, and smooth loading transitions across multiple playable levels set in a low-poly dungeon environment.

---

## Scenes

| Scene | Description |
|---|---|
| `Bootstrap` | Entry point. Initializes persistent managers and loads the Main Menu. |
| `MainMenu` | Main menu with play button, level carousel selector, and quit option. |
| `LoadingScreen` | Animated loading screen shown between level transitions. |
| `Level01` | Dungeon room — Pull a lever to lower a door and reach the exit. |
| `Level02` | Dungeon room — Push a cauldron onto a pressure point to open the path. |
| `Level03` | Dungeon room — Collect 2 hidden items to remove a bookshelf blocking the exit. |
| `WinScreen` | Additive scene loaded on top of Level 3 upon completion. Returns to Main Menu. |

---

## Architecture

### Bootstrap & Singleton Pattern
The `Bootstrap` scene is the first scene loaded (index 0 in Build Profiles). It initializes the `LoadingManager` singleton, which persists across all scenes using `DontDestroyOnLoad`.

## Additive Scene Usage

The project uses **additive scene loading** in two cases:

1. **LoadingScreen** — Loaded additively on top of any scene during transitions, then unloaded once the destination scene is ready.
2. **WinScreen** — Loaded additively on top of Level 3 when the player reaches the exit, pausing the game and displaying a completion message.

---

## Level Puzzles

### Level 1 — Lever
- Approach the lever and press **E** to activate it.
- The lever rotates and the door sinks into the floor.
- Reach the exit trigger to advance to Level 2.

### Level 2 — Push Puzzle
- Push the cauldron by walking against it toward the glowing target point.
- When the cauldron reaches the target, the door opens.
- Reach the exit trigger to advance to Level 3.

### Level 3 — Collect Items
- Find and collect 2 hidden items by pressing **E** near them.
- Once both are collected, the bookshelf blocking the exit is removed.
- Reach the exit trigger to load the Win Screen.

---

## Author

*Universidad del Valle de Guatemala*  
*Laboratorio — Scene Management*