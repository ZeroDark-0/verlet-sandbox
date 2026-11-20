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

## Phase 4: Constraints and rope
- Added a `pinned` flag to `Particle`; pinned particles skip `Update` and `KeepInside`
- Created `Constraint.cs` with two particles (`a` and `b`) and a `restLength`
- `restLength` is set from the distance between the particles when the constraint is created
- `Solve()` measures the current distance, finds the error against `restLength`, and moves both particles half the error along the line between them
- If one particle is pinned, the other takes the full correction; if both are pinned, nothing moves
- Skip the solve when the distance is 0, to avoid dividing by zero
- Each frame: update particles, solve constraints several times, keep inside the box, draw
- Solving several times per frame (5-10 iterations) makes the rope stiffer, because one pass leaves some error that the next pass reduces
- Built a rope as a chain of particles joined by constraints, with the first particle pinned
- Rope stretches like rubber with too few iterations
