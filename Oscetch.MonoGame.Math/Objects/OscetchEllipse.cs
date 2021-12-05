using Microsoft.Xna.Framework;
using Oscetch.MonoGame.Math.Helpers;
using System.Collections.Generic;

namespace Oscetch.MonoGame.Math.Objects
{
    public class OscetchEllipse : ShapeBase
    {
        public override IReadOnlyList<Vector2> Corners { get; }

        public override IReadOnlyList<Line> Lines { get; }

        public override float Rotation { get; }

        public override Vector2 RotationPoint { get; }

        public OscetchEllipse(Vector2 position, Vector2 size, float rotation, int cornersPerDegree = 20)
        {
            RotationPoint = position;
            Rotation = rotation;

            var corners = new List<Vector2>();
            var lines = new List<Line>();
            var iterations = 360 / cornersPerDegree;
            for (var i = 0; i < iterations; i++)
            {
                var angle = MathHelper.ToRadians(i * cornersPerDegree);
                var x = (int)(size.X * System.Math.Cos(angle));
                var y = (int)(size.Y * System.Math.Sin(angle));
                var rotatedCorner = AngleHelper.RotatePoint(RotationPoint, rotation, new Vector2(x, y));
                corners.Add(rotatedCorner);

                if (i == 0)
                {
                    continue;
                }

                lines.Add(new Line(rotatedCorner, corners[i - 1]));
            }
            Corners = corners;
            lines.Add(new Line(Corners[Corners.Count - 1], Corners[0]));
            Lines = lines;
        }
    }
}
