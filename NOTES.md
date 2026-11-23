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

## Phase 5: Soft bodies and cloth
- Created `World.cs`, which owns the particle list and constraint list
- Moved the per-frame logic out of `Program.cs` into `World.Update()` and `World.Draw()`
- Each frame: update particles, solve constraints several times, keep particles inside the box, draw
- Soft box: 4 corner particles joined by 4 edge constraints plus 2 diagonals
- Without the diagonals the square collapses into a flat diamond, because edges alone don't hold the angles
- Added a `CreateBox(position, size)` helper to spawn boxes
- Cloth: a grid of particles, each connected to its right and bottom neighbor, so every pair is created once
- Pinned the top row so the cloth hangs like a curtain
- More solver iterations make the cloth stiffer and less stretchy
- Drew the cloth as lines only, which looks more like fabric than circles
- Mouse grab: find the nearest particle and drag it while the button is held
- Tearing: right click removes the constraints near the mouse
- Removing from a list while looping throws an error, so loop backwards or use `RemoveAll`

## Phase 6: Collisions
- Two particles overlap when the distance between centers is less than the sum of their radii
- Compared squared distances first, so a square root is only taken when there is a real collision
- On overlap: overlap = (r1 + r2) - distance, then move each particle half of it in opposite directions along the line between them
- Same pattern as `Constraint.Solve()`, but it only acts when the distance is too small
- Skip the push when the distance is 0, to avoid dividing by zero
- Pinned particles don't move, so the other particle takes the full correction
- Checked every pair once with a nested loop (inner index starts at `i + 1`)
- Collisions run inside the same iteration loop as constraints, so they settle together
- Particles joined by a constraint sit closer than their radii, so they fight the constraint and jitter; skipped collisions between them
- Resting piles jitter a little, because position-based contact is never perfectly still

## Phase 7: Spatial hashing
- Divided the window into a grid of square cells; each frame every particle goes into the cell containing its position
- Cell size is at least 2 × the largest radius, so a particle can only touch its own cell and the 8 around it
- Cell index = `MathF.Floor(position / cellSize)`, because casting a negative float to `int` rounds toward zero
- Each frame: clear the grid, insert all particles, then solve collisions using only nearby cells
- Avoided checking each pair twice by only handling pairs where one index is smaller
- Reused the cell lists and called `Clear()` instead of allocating new ones, to avoid garbage collection pauses
- Built the grid once per frame, which was good enough
- Cell size too small misses collisions; too large goes back toward O(n²)
- Normal Benchmark: 60 fps to 59 fps with particle count 800
- Actual Benchmark: 60 fps to 50 fps with particle count 1640
- A spatial hash is faster because each particle is only compared with nearby ones instead of all of them, so the cost grows roughly with the particle count