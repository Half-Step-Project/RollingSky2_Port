using UnityEngine;

namespace _RTPort
{
    public static class PortUtils
    {
        public static Vector2 SetX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }
        
        public static Vector2 SetY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }
    }
}