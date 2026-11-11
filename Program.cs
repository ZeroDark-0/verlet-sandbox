using Raylib_cs;

namespace VerletGame;

internal class Program
{
    [System.STAThread]
    public static void Main()
    {
        const int ScreenWidth = 800;
        const int ScreenHeight = 450;
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Verlet");
        Raylib.SetTargetFPS(60);

        int ballX = ScreenWidth / 2;
        int ballY = ScreenHeight / 2;
        
        while (!Raylib.WindowShouldClose())
        {
            if(Raylib.IsKeyDown(KeyboardKey.S)) ballY += 5;
            if (Raylib.IsKeyDown(KeyboardKey.A)) ballX -= 5;
            if(Raylib.IsKeyDown(KeyboardKey.W)) ballY -= 5;
            if(Raylib.IsKeyDown(KeyboardKey.D)) ballX += 5;
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Raylib.DrawText("Move with WASD", 10, 10, 20, Color.White);
            Raylib.DrawCircle(ballX, ballY, 10, Color.White);
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}