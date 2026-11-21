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

        while (!Raylib.WindowShouldClose())
        {
            const float dt = 1f / 60f;

            Vector2 mousePosition = Raylib.GetMousePosition();

            if (Raylib.IsKeyPressed(KeyboardKey.B))
            {
                world.CreateBox(mousePosition, 80f);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.C))
            {
                world.CreateCloth(mousePosition, 20, 15, 18f);
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

            world.Update(dt);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText("Left: grab | Right: tear | Middle: drag rope | B: box | C: cloth", 10, 10, 20, Color.White);

            world.Draw();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}