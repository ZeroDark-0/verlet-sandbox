# Notes

## Phase 1: Window and game loop
- Opened a window with `InitWindow`, 60 FPS
- Game loop: read input, update, draw
- Drew a circle and moved it with WASD
- Next: replace `int` position with `Vector2` (`current` and `previous`)

## Phase 2: One falling ball
- Changed the ball's position from `int` to `Vector2` (`current` and `previous`)
- Added a floor so the ball falls and stops on it
- Added a bounce on the floor
- Removed the separate floor and made the whole window a box (floor, walls, ceiling)
- Changed `WASD` to move the ball in all directions

## Phase 3: Many particles
- Created `Particle.cs` with `current`, `previous` and `radius`
- Moved the Verlet step into `Update(dt, gravity)`
- Moved the box bounce into `KeepInside(width, height)`
- Added `Draw()` so a particle draws itself
- Each frame: update, keep inside, draw for every particle
- Left click spawns a new particle at the mouse position

## Phase 4: constraints and rope
...