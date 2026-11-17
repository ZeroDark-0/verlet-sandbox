# Verlet Sandbox: Build Order

## Phase 1: Window

Get a window open with a circle in it before anything else. Finish each step and check it works before moving on.

1. **Open a window.** Look up `InitWindow`, `WindowShouldClose` and `CloseWindow` in the Raylib-cs docs.
2. **Add the game loop.** Inside `while (!WindowShouldClose)`, call `BeginDrawing`, `ClearBackground` and `EndDrawing`.
3. **Draw one circle** with `DrawCircleV`. Set the frame rate with `SetTargetFPS(60)`.

Done when a circle sits in a window and Esc closes it.

## Phase 2: One falling ball

One point that falls, bounces and settles, using only its current and previous position.

1. **Store two positions:** `current` and `previous`, both `Vector2`.
2. **Apply the formula each frame:** work out the velocity, save the old position, then move by velocity plus gravity.
3. **Use a fixed `dt`** of 1/60 second instead of a changing frame time.
4. **Stop it at the floor.** If it goes below the bottom edge, put it back on the edge.
5. **Make it bounce.** Move `previous` instead of flipping a velocity, since there isn't one.
6. **Handle the left and right walls** the same way.

Done when the ball falls, bounces and gradually settles.

## Phase 3: Many particles

Turn the single ball into a reusable particle that you can spawn by the hundred.

1. **Make a `Particle` class or struct** holding `current`, `previous` and a radius.
2. **Move the update logic into a method** like `Update(dt)`.
3. **Create a `List<Particle>`** and update and draw every particle each frame.
4. **Spawn a particle on mouse click** so you can fill the screen.

## Phase 4: Constraints and rope

Connect particles with fixed-length sticks and build the first rope.

1. **Make a `Constraint` class** holding two particles and a rest length.
2. **Solve it.** Measure the distance between the two points, find how far off it is from the rest length, and move each point half the error toward or away from the other.
3. **Run the solver several times per frame**, around 5-10 iterations, for a stiffer result.
4. **Build a rope:** a chain of particles joined by constraints, with the first one pinned in place.


