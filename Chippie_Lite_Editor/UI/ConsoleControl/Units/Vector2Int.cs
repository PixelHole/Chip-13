using System.Numerics;

namespace wpf_Console
{
    public class Vector2Int
    {
        public int x { get; set; }
        public int y { get; set; }

        public Vector2Int()
        {
            x = 0;
            y = 0;
        }

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetX(int val) => x = val; 
        public void SetY(int val) => y = val; 
        
        // -- Units --
        /// <summary>
        /// Short-Hand for writing new Vector2Int(0, 0)
        /// </summary>
        public static Vector2Int Zero => new Vector2Int(0, 0);
        /// <summary>
        /// Short-Hand for writing new Vector2Int(1, 1)
        /// </summary>
        public static Vector2Int One => new Vector2Int(1, 1);
        /// <summary>
        /// Short-Hand for writing new Vector2Int(-1, -1)
        /// </summary>
        public static Vector2Int MinusOne => new Vector2Int(-1, -1);
        /// <summary>
        /// Short-Hand for writing new Vector2Int(0, 1)
        /// </summary>
        public static Vector2Int Up => new Vector2Int(0, 1);
        /// <summary>
        /// Short-Hand for writing new Vector2Int(0, -1)
        /// </summary>
        public static Vector2Int Down => new Vector2Int(0, -1);
        /// <summary>
        /// Short-Hand for writing new Vector2Int(-1, 0)
        /// </summary>
        public static Vector2Int Left => new Vector2Int(-1, 0);
        /// <summary>
        /// Short-Hand for writing new Vector2Int(1, 0)
        /// </summary>
        public static Vector2Int Right => new Vector2Int(1, 0);
        /// <summary>
        /// Short-Hand for writing new Vector2Int(-1, 1)
        /// </summary>
        public static Vector2Int UpLeft => new Vector2Int(-1, 1);
        /// <summary>
        /// Short-Hand for writing new Vector2Int(1, -1)
        /// </summary>
        public static Vector2Int DownRight => new Vector2Int(1, -1);

        public Vector2 ToVector2() => new Vector2(x, y);
        
        // -- Operators --
        // Sum operator
        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.x + b.x, a.y + b.y);
        
        // Reduction operator
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.x - b.x, a.y - b.y);
        
        // Multiply operators
        public static Vector2Int operator *(int a, Vector2Int b) => new Vector2Int(b.x * a, b.y * a);
        
        public static Vector2Int operator *(Vector2Int b, int a) => new Vector2Int(b.x * a, b.y * a);
        
        // division operator
        public static Vector2Int operator /(Vector2Int b, int a) => new Vector2Int(b.x / a, b.y / a);
        
        public static Vector2Int operator /(Vector2Int b, Vector2Int a) => new Vector2Int(b.x / a.x, b.y / a.y);
        
        
        // -- Comparers -- 
        public static bool operator <(Vector2Int a, Vector2Int b) => a.x < b.x && a.y < b.y;
        public static bool operator >(Vector2Int a, Vector2Int b) => a.x > b.x && a.y > b.y;
        public static bool operator <=(Vector2Int a, Vector2Int b) => a.x <= b.x && a.y <= b.y;
        public static bool operator >=(Vector2Int a, Vector2Int b) => a.x >= b.x && a.y >= b.y;
        public static bool operator ==(Vector2Int a, Vector2Int b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Vector2Int a, Vector2Int b) => a.x != b.x || a.y != b.y;

        public override bool Equals(object? obj)
        {
            if (obj is Vector2Int other) return Equals(other);
            return base.Equals(obj);
        }
        public bool Equals(Vector2Int other)
        {
            return other.x == x && other.y == y;
        }
        public override string ToString()
        {
            return $"{x}, {y}";
        }
    }
}