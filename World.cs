using Raylib_cs;
using System.Numerics;

namespace VerletGame;

public class World
{
    private readonly List<Particle> particles = new List<Particle>();
    private readonly List<Constraint> constraints = new List<Constraint>();
    private readonly HashSet<Particle> hiddenParticles = new HashSet<Particle>();

    private readonly int screenWidth;
    private readonly int screenHeight;

    private readonly float ballRadius = 10f;
    private readonly float bounce = 0.9f;
    private readonly float damping = 1f;
    private readonly int solverIterations = 12;

    private readonly Vector2 gravity = new Vector2(0f, 1000f);

    private Particle ropeEndParticle;
    private Particle grabbedParticle;

    public World(int screenWidth, int screenHeight)
    {
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;

        CreateRope();
        CreateBox(new Vector2(500f, 200f), 120f);
        CreateCloth(new Vector2(200f, 100f), 20, 15, 18f);
    }

    private void CreateRope()
    {
        int ropeParticleCount = 15;
        float ropeSpacing = 20f;
        Vector2 ropeStart = new Vector2(screenWidth / 2f, 300f);

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

        ropeEndParticle = particles[particles.Count - 1];
    }

    public void CreateBox(Vector2 position, float size)
    {
        float halfSize = size / 2f;

        Particle topLeft = new Particle(position + new Vector2(-halfSize, -halfSize), ballRadius);
        Particle topRight = new Particle(position + new Vector2(halfSize, -halfSize), ballRadius);
        Particle bottomRight = new Particle(position + new Vector2(halfSize, halfSize), ballRadius);
        Particle bottomLeft = new Particle(position + new Vector2(-halfSize, halfSize), ballRadius);

        particles.Add(topLeft);
        particles.Add(topRight);
        particles.Add(bottomRight);
        particles.Add(bottomLeft);

        constraints.Add(new Constraint(topLeft, topRight));
        constraints.Add(new Constraint(topRight, bottomRight));
        constraints.Add(new Constraint(bottomRight, bottomLeft));
        constraints.Add(new Constraint(bottomLeft, topLeft));

        constraints.Add(new Constraint(topLeft, bottomRight));
        constraints.Add(new Constraint(topRight, bottomLeft));
    }

    public void SpawnParticle(Vector2 position)
    {
        particles.Add(new Particle(position, ballRadius));
    }

    public void CreateCloth(Vector2 startPosition, int columns, int rows, float spacing)
    {
        Particle[,] clothParticles = new Particle[columns, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 position = startPosition + new Vector2(x * spacing, y * spacing);

                bool pinned = y == 0;

                Particle particle = new Particle(position, ballRadius, pinned);
                particles.Add(particle);
                hiddenParticles.Add(particle);
                clothParticles[x, y] = particle;
            }
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Particle current = clothParticles[x, y];

                if (x < columns - 1)
                {
                    Particle right = clothParticles[x + 1, y];
                    constraints.Add(new Constraint(current, right));
                }

                if (y < rows - 1)
                {
                    Particle bottom = clothParticles[x, y + 1];
                    constraints.Add(new Constraint(current, bottom));
                }
            }
        }
    }

    public void DragRopeEnd(Vector2 position)
    {
        ropeEndParticle.pinned = true;
        ropeEndParticle.current = position;
        ropeEndParticle.previous = position;
    }

    public void ReleaseRopeEnd()
    {
        ropeEndParticle.pinned = false;
    }

    public void StartGrab(Vector2 mousePosition, float grabRadius)
    {
        Particle nearestParticle = null;
        float nearestDistanceSquared = grabRadius * grabRadius;

        foreach (Particle particle in particles)
        {
            float distanceSquared = Vector2.DistanceSquared(mousePosition, particle.current);

            if (distanceSquared < nearestDistanceSquared)
            {
                nearestDistanceSquared = distanceSquared;
                nearestParticle = particle;
            }
        }

        grabbedParticle = nearestParticle;

        if (grabbedParticle != null)
        {
            grabbedParticle.pinned = true;
            grabbedParticle.current = mousePosition;
            grabbedParticle.previous = mousePosition;
        }
    }

    public void DragGrabbed(Vector2 mousePosition)
    {
        if (grabbedParticle == null)
        {
            return;
        }

        grabbedParticle.current = mousePosition;
        grabbedParticle.previous = mousePosition;
    }

    public void EndGrab()
    {
        if (grabbedParticle == null)
        {
            return;
        }

        grabbedParticle.pinned = false;
        grabbedParticle = null;
    }

    public void Update(float dt)
    {
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
            particle.KeepInside(screenWidth, screenHeight, bounce);
        }
    }

    public void Draw()
    {
        foreach (Constraint constraint in constraints)
        {
            constraint.Draw();
        }

        foreach (Particle particle in particles)
        {
            if (!hiddenParticles.Contains(particle))
            {
                particle.Draw();
            }
        }
    }

    public void Tear(Vector2 mousePosition, float tearRadius)
    {
        for (int i = constraints.Count - 1; i >= 0; i--)
        {
            if (constraints[i].IsNear(mousePosition, tearRadius))
            {
                constraints.RemoveAt(i);
            }
        }
    }

}