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
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Verlet");
        Raylib.SetTargetFPS(60);

        float ballRadius = 15f;
        float bounce = 0.8f;
        float moveAcceleration = 3000f;
        float damping = 1f;

        Vector2 current = new Vector2(ScreenWidth / 2f, ScreenHeight / 4f);
        Vector2 startingVelocity = new Vector2(8f, -6f);
        Vector2 previous = current - startingVelocity;
        // Vector2 ballPosition = new Vector2(ScreenWidth / 2, ScreenHeight / 4);

        Vector2 gravity = new Vector2(0f, 1000f);
        
        // int ballX = ScreenWidth / 2;
        // int ballY = ScreenHeight / 2;
        
        // Vector2 linePositionStart = new Vector2(ScreenWidth / 8f, ScreenHeight * (7f / 8f));
        // Vector2 linePositionEnd = new Vector2(ScreenWidth * (7f / 8f), ScreenHeight * (7f / 8f));

        float leftEdge = ballRadius;
        float rightEdge = ScreenWidth - ballRadius;
        float topEdge = ballRadius;
        float bottomEdge = ScreenHeight - ballRadius;

        while (!Raylib.WindowShouldClose())
        {
            const float dt = 1f / 60f;

            Vector2 inputAcceleration = Vector2.Zero;

            if (Raylib.IsKeyDown(KeyboardKey.W)) inputAcceleration.Y -= moveAcceleration;
            if (Raylib.IsKeyDown(KeyboardKey.S)) inputAcceleration.Y += moveAcceleration;
            if (Raylib.IsKeyDown(KeyboardKey.A)) inputAcceleration.X -= moveAcceleration;
            if (Raylib.IsKeyDown(KeyboardKey.D)) inputAcceleration.X += moveAcceleration;

            Vector2 velocity = (current - previous) * damping;
            previous = current;
            Vector2 acceleration = gravity + inputAcceleration;
            current = current + velocity + acceleration * dt * dt;

            /*
            if(Raylib.IsKeyDown(KeyboardKey.S)) ballPosition.Y += 5;
            if (Raylib.IsKeyDown(KeyboardKey.A)) ballPosition.X -= 5;
            if(Raylib.IsKeyDown(KeyboardKey.W)) ballPosition.Y -= 5;
            if(Raylib.IsKeyDown(KeyboardKey.D)) ballPosition.X += 5;
            */

            /*
            if (!Raylib.IsKeyDown(KeyboardKey.W))
            {
                gravity += new Vector2(0, 1000);
            }
            else
            {
                gravity = new Vector2(0, 1000);
            }
            */

            velocity = current - previous;

            if (current.Y > bottomEdge)
            {
                current.Y = bottomEdge;
                previous.Y = current.Y + velocity.Y * bounce;
            }

            if (current.Y < topEdge)
            {
                current.Y = topEdge;
                previous.Y = current.Y + velocity.Y * bounce;
            }

            if (current.X > rightEdge)
            {
                current.X = rightEdge;
                previous.X = current.X + velocity.X * bounce;
            }

            if (current.X < leftEdge)
            {
                current.X = leftEdge;
                previous.X = current.X + velocity.X * bounce;
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText("Move with WASD", 10, 10, 20, Color.White);
            
            // Raylib.DrawCircle(ballX, ballY, 10, Color.White);
            // Raylib.DrawCircleV(ballPosition, 15, Color.White);
            
            Raylib.DrawCircleV(current, ballRadius, Color.White);
            // Raylib.DrawLineEx(linePositionStart, linePositionEnd, 8, Color.Red);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}