using Raylib_cs;
using System.Numerics;

namespace VerletGame;

internal class Program
{
    [System.STAThread]
    public static void Main()
    {
        const int ScreenWidth = 1280;
        const int ScreenHeight = 960;
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "VerletSandbox");
        Raylib.SetTargetFPS(60);

            World world = new World(ScreenWidth, ScreenHeight);

            bool benchmarkRunning = false;
            bool benchmarkFinished = false;
            int benchmarkResultParticleCount = 0;

            const int benchmarkParticlesPerFrame = 20;
            const int benchmarkStopFps = 50;

            double vHoldStartTime = 0.0;
            double lastCircleSpawnTime = 0.0;
            const double circleHoldDelay = 0.25;
            const double circleSpawnInterval = 0.05;

            while (!Raylib.WindowShouldClose())
            {
                const float dt = 1f / 60f;

                Vector2 mousePosition = Raylib.GetMousePosition();
                double currentTime = Raylib.GetTime();

                if (Raylib.IsKeyPressed(KeyboardKey.B))
                {
                    world.CreateBox(mousePosition, 80f);
                }

                if (Raylib.IsKeyPressed(KeyboardKey.V))
                {
                    world.SpawnParticle(mousePosition);
                    vHoldStartTime = currentTime;
                    lastCircleSpawnTime = currentTime;
                }

                if (Raylib.IsKeyDown(KeyboardKey.V) && currentTime - vHoldStartTime >= circleHoldDelay)
                {
                    if (currentTime - lastCircleSpawnTime >= circleSpawnInterval)
                    {
                        world.SpawnParticle(mousePosition);
                        lastCircleSpawnTime = currentTime;
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.C))
                {
                    world.CreateCloth(mousePosition, 20, 15, 18f);
                }

                if (Raylib.IsKeyPressed(KeyboardKey.G))
                {
                    world.ToggleSpatialHashGrid();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.X))
                {
                    world.Clear();

                    benchmarkRunning = true;
                    benchmarkFinished = false;
                    benchmarkResultParticleCount = 0;
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Z))
                {
                    world.ResetToDefaultLayout();

                    benchmarkRunning = false;
                    benchmarkFinished = false;
                    benchmarkResultParticleCount = 0;
                }

                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                world.StartGrab(mousePosition, 35f);
            }

            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                world.DragGrabbed(mousePosition);
            }

            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                world.EndGrab();
            }

            if (Raylib.IsMouseButtonDown(MouseButton.Middle))
            {
                world.DragRopeEnd(mousePosition);
            }
            else
            {
                world.ReleaseRopeEnd();
            }

            if (Raylib.IsMouseButtonPressed(MouseButton.Right))
            {
                world.Tear(mousePosition, 25f);
            }

if (benchmarkRunning)
{
    world.SpawnBenchmarkParticles(benchmarkParticlesPerFrame);

    if (Raylib.GetFPS() > 0 && Raylib.GetFPS() < benchmarkStopFps)
    {
        benchmarkRunning = false;
        benchmarkFinished = true;
        benchmarkResultParticleCount = world.ParticleCount;
    }
}

world.Update(dt);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawFPS(10, 10);
            Raylib.DrawText($"Particles: {world.ParticleCount}", 10, 35, 20, Color.White);
            Raylib.DrawText("Left: grab | Right: tear | Middle: drag rope | B: box | V: circle | C: cloth | G: grid | X: benchmark | Z: reset", 10, 60, 20, Color.White);

            if (benchmarkRunning)
            {
                Raylib.DrawText($"Benchmark running... adding {benchmarkParticlesPerFrame} particles/frame", 10, 85, 20, Color.Yellow);
            }

            if (benchmarkFinished)
            {
                Raylib.DrawText($"Benchmark stopped below {benchmarkStopFps} FPS at {benchmarkResultParticleCount} particles", 10, 85, 20, Color.Orange);
            }

            world.Draw();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}