using Raylib_cs;
using System.Numerics;

namespace VerletGame;

public class Constraint
{
    public Particle a;
    public Particle b;
    public float restLength;

    public Constraint(Particle a, Particle b)
    {
        this.a = a;
        this.b = b;
        restLength = Vector2.Distance(a.current, b.current);
    }

    public void Solve()
    {
        Vector2 difference = b.current - a.current;
        float distance = difference.Length();

        if (distance == 0f)
        {
            return;
        }

        Vector2 direction = difference / distance;
        float error = distance - restLength;
        Vector2 correction = direction * error;

        if (a.pinned && b.pinned)
        {
            return;
        }

        if (a.pinned)
        {
            b.current -= correction;
        }
        else if (b.pinned)
        {
            a.current += correction;
        }
        else
        {
            a.current += correction * 0.5f;
            b.current -= correction * 0.5f;
        }
    }

    public void Draw()
    {
        Raylib.DrawLineV(a.current, b.current, Color.DarkBlue);
    }
}