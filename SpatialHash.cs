using Raylib_cs;
using System.Numerics;

namespace VerletGame;

public class SpatialHash
{
    private readonly Dictionary<(int Column, int Row), List<Particle>> cells = new Dictionary<(int Column, int Row), List<Particle>>();

    private readonly float cellSize;

    public int NonEmptyCellCount => cells.Count;

    public SpatialHash(float cellSize)
    {
        this.cellSize = cellSize;
    }

    public void Clear()
    {
        cells.Clear();
    }

    public void Insert(Particle particle)
    {
        (int column, int row) = GetCell(particle.current);

        if (!cells.TryGetValue((column, row), out List<Particle> cellParticles))
        {
            cellParticles = new List<Particle>();
            cells[(column, row)] = cellParticles;
        }

        cellParticles.Add(particle);
    }

    public IEnumerable<Particle> GetNearbyParticles(Particle particle)
    {
        (int centerColumn, int centerRow) = GetCell(particle.current);

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                int column = centerColumn + columnOffset;
                int row = centerRow + rowOffset;

                if (!cells.TryGetValue((column, row), out List<Particle> cellParticles))
                {
                    continue;
                }

                foreach (Particle nearbyParticle in cellParticles)
                {
                    yield return nearbyParticle;
                }
            }
        }
    }

    public void DrawDebugGrid(int screenWidth, int screenHeight)
    {
        Color gridColor = new Color(50, 50, 50, 255);

        for (float x = 0f; x <= screenWidth; x += cellSize)
        {
            Raylib.DrawLineV(new Vector2(x, 0f), new Vector2(x, screenHeight), gridColor);
        }

        for (float y = 0f; y <= screenHeight; y += cellSize)
        {
            Raylib.DrawLineV(new Vector2(0f, y), new Vector2(screenWidth, y), gridColor);
        }
    }

    private (int Column, int Row) GetCell(Vector2 position)
    {
        int column = (int)MathF.Floor(position.X / cellSize);
        int row = (int)MathF.Floor(position.Y / cellSize);

        return (column, row);
    }
}