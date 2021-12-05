using Microsoft.Xna.Framework;

namespace Oscetch.MonoGame.Math.Helpers
{
    public static class CollisionHelper
    {
        public static bool TryGetIntersectionPointOnLines(Vector4 line1, Vector4 line2, out Vector2 closestPoint)
        {
            const double tolerance = .001d;

            closestPoint = Vector2.Zero;

            double x1 = line1.X, y1 = line1.Y;
            double x2 = line1.Z, y2 = line1.W;

            double x3 = line2.X, y3 = line2.Y;
            double x4 = line2.Z, y4 = line2.W;

            var isFirstLineVertical = System.Math.Abs(x1 - x2) < tolerance;
            var isSecondLineVertical = System.Math.Abs(x3 - x4) < tolerance;
            var isFirstLineHorizontal = System.Math.Abs(y1 - y2) < tolerance;
            var isSecondLineHorizontal = System.Math.Abs(y3 - y4) < tolerance;

            if (isFirstLineVertical && isSecondLineVertical && System.Math.Abs(x1 - x3) < tolerance
                || isFirstLineHorizontal && isSecondLineHorizontal && System.Math.Abs(y1 - y3) < tolerance)
            {
                return false;
            }

            if (isFirstLineVertical && isSecondLineVertical)
            {
                return false;
            }

            if (isFirstLineHorizontal && System.Math.Abs(y3 - y4) < tolerance)
            {
                return false;
            }

            double x, y;

            if (isFirstLineVertical)
            {
                var m2 = (y4 - y3) / (x4 - x3);
                var c2 = -m2 * x3 + y3;

                x = x1;
                y = c2 + m2 * x1;
            }
            else if (isSecondLineVertical)
            {
                var m1 = (y2 - y1) / (x2 - x1);
                var c1 = -m1 * x1 + y1;

                x = x3;
                y = c1 + m1 * x3;
            }
            else
            {
                var m1 = (y2 - y1) / (x2 - x1);
                var c1 = -m1 * x1 + y1;

                var m2 = (y4 - y3) / (x4 - x3);
                var c2 = -m2 * x3 + y3;

                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                if (!(System.Math.Abs(-m1 * x + y - c1) < tolerance)
                    && System.Math.Abs(-m2 * x + y - c2) < tolerance)
                {
                    return false;
                }
            }

            closestPoint = new Vector2((float)x, (float)y);
            return IsPointInsideLine(line1, closestPoint) && IsPointInsideLine(line2, closestPoint);
        }

        private static bool IsPointInsideLine(Vector4 line, Vector2 point)
        {
            return (point.X >= line.X && point.X <= line.Z || point.X >= line.Z && point.X <= line.X)
                && (point.Y >= line.Y && point.Y <= line.W || point.Y >= line.W && point.Y <= line.Y);
        }
    }
}
