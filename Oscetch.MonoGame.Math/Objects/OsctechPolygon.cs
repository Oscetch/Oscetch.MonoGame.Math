using Microsoft.Xna.Framework;
using Oscetch.MonoGame.Math.Helpers;
using System;
using System.Collections.Generic;

namespace Oscetch.MonoGame.Math.Objects
{
    public class OsctechPolygon : ShapeBase
    {
        public override IReadOnlyList<Vector2> Corners { get; }
        public override IReadOnlyList<Line> Lines { get; }
        public override float Rotation { get; }
        public override Vector2 RotationPoint { get; }

        public float Radius { get; }

        public OsctechPolygon(float rotation, Vector2 rotationPoint, params Vector2[] corners)
        {
            if (corners.Length <= 2)
            {
                throw new Exception("Polygon needs atleast 3 corners");
            }
            RotationPoint = rotationPoint;
            Rotation = rotation;
            var lines = new List<Line>();
            if (Rotation == 0)
            {
                Corners = corners;
                for (var i = 1; i < Corners.Count; i++)
                {
                    lines.Add(new Line(Corners[i - 1], Corners[i]));
                }
            }
            else
            {
                var rotatedCorners = new List<Vector2>();
                for (var i = 0; i < corners.Length; i++)
                {
                    var rotatedCorner = AngleHelper.RotatePoint(rotationPoint, rotation, corners[i]);
                    rotatedCorners.Add(rotatedCorner);
                    if (i == 0)
                    {
                        continue;
                    }

                    lines.Add(new Line(rotatedCorner, rotatedCorners[i - 1]));
                }

                Corners = rotatedCorners;
            }
            lines.Add(new Line(Corners[Corners.Count - 1], Corners[0]));
            Lines = lines;
        }
    }
}
