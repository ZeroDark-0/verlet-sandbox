# Verlet Sandbox

A 2D physics sandbox built from nothing in C# using Verlet integration, with ropes, soft bodies, and cloth.

> **Status:** work in progress. Currently at Phase 1 (window and game loop).

<!--
![demo](docs/media/demo.gif)
-->

## About

Verlet integration stores each point's current position and its position one frame ago. Velocity is not stored; it is implied by the difference:

```
velocity     = current - previous
new position = current + velocity + gravity * dt * dt
```

Because there is no explicit velocity, constraints such as "these two points must stay a fixed distance apart" can be enforced by moving the points directly. That keeps the simulation stable and makes ropes, cloth, and soft bodies simple to build.

## Planned features

- Verlet integration for particles
- Distance constraints (sticks) between particles
- Ropes and chains
- Soft bodies that keep their shape
- Cloth with pinned points and mouse tearing
- Particle and obstacle collisions
- Spatial hash grid for thousands of particles at 60 FPS

## Roadmap

- [x] Phase 1: window, game loop, drawing, keyboard input
- [ ] Phase 2: one falling ball (Verlet step, floor, bounce, walls)
- [ ] Phase 3: many particles
- [ ] Phase 4: constraints and rope
- [ ] Phase 5: soft bodies and cloth
- [ ] Phase 6: collisions
- [ ] Phase 7: spatial hashing
- [ ] Phase 8: demo scene and documentation

## How it works

_to be written later in future._

## Tech stack

- C# on .NET 9+
- [Raylib-cs](https://www.raylib.com/cheatsheet/cheatsheet.html) for windowing and drawing
- JetBrains Rider

## Build and run

Requires the [.NET SDK](https://dotnet.microsoft.com/download) (version 8 or newer).

```bash
git clone <repo-url>
cd VerletGame
dotnet run --project VerletGame
```
<!--
## Project structure

```

```
-->
## References

- Thomas Jakobsen, "Advanced Character Physics"
- [Raylib-cs](https://github.com/ChrisDill/Raylib-cs)
