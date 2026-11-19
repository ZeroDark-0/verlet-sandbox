using Raylib_cs;
using System.Numerics;

namespace VerletGame;

public class Particle
{
    public Vector2 current;
    public Vector2 previous;
    public float radius;
    
    public Particle(Vector2 startPosition, float radius)
    {
        current = startPosition;
        previous = startPosition;
        this.radius = radius;
    }

    public void Update(float dt, Vector2 acceleration, float damping)
    {
        Vector2 velocity = (current - previous) * damping;
        previous = current;
        current = current + velocity + acceleration * dt * dt;
    }

    public void KeepInside(int width, int height, float bounce)
    {
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

}