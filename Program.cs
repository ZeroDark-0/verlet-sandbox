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

        float ballRadius = 15f;
        float bounce = 0.9f;
        float damping = 1f;
        
        List<Particle> particles = new List<Particle>();
        Vector2 gravity = new Vector2(0f, 1000f);
        
        while (!Raylib.WindowShouldClose())
        {
            const float dt = 1f / 60f;
            
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                particles.Add(new Particle(Raylib.GetMousePosition(), ballRadius));
            }

            foreach (Particle particle in particles)
            {
                particle.Update(dt, gravity, damping);
                particle.KeepInside(ScreenWidth, ScreenHeight, bounce);
            };

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText("Left click to spawn a ball", 10, 10, 20, Color.White);
            
            foreach (Particle particle in particles)
            {
                particle.Draw();
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}