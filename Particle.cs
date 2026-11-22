using Raylib_cs;
using System.Numerics;

namespace VerletGame;

public class Particle
{
    public Vector2 current;
    public Vector2 previous;
    public float radius;
    public bool pinned;

    public Particle(Vector2 startPosition, float radius, bool pinned = false)
    {
        current = startPosition;
        previous = startPosition;
        this.radius = radius;
        this.pinned = pinned;
    }

    public void Update(float dt, Vector2 acceleration, float damping)
    {
        if (pinned)
        {
            return;
        }

        Vector2 velocity = (current - previous) * damping;
        previous = current;
        current = current + velocity + acceleration * dt * dt;
    }

    public void KeepInside(int width, int height, float bounce)
    {
        if (pinned)
        {
            return;
        }

        float leftEdge = radius;
        float rightEdge = width - radius;
        float topEdge = radius;
        float bottomEdge = height - radius;

        Vector2 velocity = current - previous;

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
    } 
    
    public void Draw()
    {
        Raylib.DrawCircleV(current, radius, Color.White);
    }

    public void Draw(Color color)
    {
        Raylib.DrawCircleV(current, radius, color);
    }

}