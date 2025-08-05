namespace wpf_Console;

public class IntRect
{
    public Vector2Int TopLeft { get; set; }
    public Vector2Int BottomRight { get; set; }

    public int Width => BottomRight.x - TopLeft.x;
    public int Height => BottomRight.y - TopLeft.y;

    public List<Vector2Int> Points
    {
        get
        {
            List<Vector2Int> points = [];
            for (int y = TopLeft.y; y < BottomRight.y; y++)
            {
                for (int x = TopLeft.x; x < BottomRight.x; x++)
                {
                    points.Add(new Vector2Int(x, y));
                }
            }

            return points;
        }
    }
    

    public IntRect(Vector2Int topLeft, Vector2Int bottomRight)
    {
        TopLeft = topLeft;
        BottomRight = bottomRight;
    }

    public bool IsPointInside(Vector2Int point)
    {
        return point >= TopLeft && point <= BottomRight;
    }

    public override bool Equals(object? obj)
    {
        if (obj is IntRect other) return Equals(other);
        return base.Equals(obj);
    }
    public bool Equals(IntRect other)
    {
        return other.TopLeft == TopLeft && other.BottomRight == BottomRight;
    }
}