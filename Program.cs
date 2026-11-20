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

        float ballRadius = 10f;
        float bounce = 0.9f;
        float damping = 1f;
        int solverIterations = 5;

        List<Particle> particles = new List<Particle>();
        List<Constraint> constraints = new List<Constraint>();

        int ropeParticleCount = 15;
        float ropeSpacing = 20f;
        Vector2 ropeStart = new Vector2(ScreenWidth / 2f, 100f);

        for (int i = 0; i < ropeParticleCount; i++)
        {
            bool pinned = i == 0;
            Vector2 position = ropeStart + new Vector2(i * 5f, i * ropeSpacing);

            Particle particle = new Particle(position, ballRadius, pinned);
            particles.Add(particle);

            if (i > 0)
            {
                Particle previousParticle = particles[i - 1];
                constraints.Add(new Constraint(previousParticle, particle));
            }
        }

        Particle ropeEndParticle = particles[particles.Count - 1];

        Vector2 gravity = new Vector2(0f, 1000f);
        
            while (!Raylib.WindowShouldClose())
            {
                const float dt = 1f / 60f;
            
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    particles.Add(new Particle(Raylib.GetMousePosition(), ballRadius));
                }

                if (Raylib.IsMouseButtonDown(MouseButton.Right))
                {
                    Vector2 mousePosition = Raylib.GetMousePosition();

                    ropeEndParticle.pinned = true;
                    ropeEndParticle.current = mousePosition;
                    ropeEndParticle.previous = mousePosition;
                }
                else
                {
                    ropeEndParticle.pinned = false;
                }

                foreach (Particle particle in particles)
                {
                    particle.Update(dt, gravity, damping);
                }

                for (int i = 0; i < solverIterations; i++)
                {
                    foreach (Constraint constraint in constraints)
                    {
                        constraint.Solve();
                    }
                }

                foreach (Particle particle in particles)
                {
                    particle.KeepInside(ScreenWidth, ScreenHeight, bounce);
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawText("Left click: spawn ball | Right mouse: drag rope end", 10, 10, 20, Color.White);

                foreach (Constraint constraint in constraints)
                {
                    constraint.Draw();
                }
            
                foreach (Particle particle in particles)
                {
                    particle.Draw();
                }

                Raylib.EndDrawing();
            }

        Raylib.CloseWindow();
    }
}