using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Oscetch.MonoGame.Math.Objects
{
    public class OscetchRectangle : ShapeBase
    {
        public override IReadOnlyList<Vector2> Corners { get; }
        public override IReadOnlyList<Line> Lines { get; }
        public Rectangle RawRectangle { get; }
        public override Vector2 RotationPoint { get; }
        public Vector2 X { get; }
        public Vector2 Y { get; }
        public Vector2 Z { get; }
        public Vector2 W { get; }
        public override float Rotation { get; }

        public OscetchRectangle(Rectangle rawRectangle, float rotation)
        {
            RawRectangle = rawRectangle;
            Rotation = rotation;

            RotationPoint = rawRectangle.Center.ToVector2();
            var v1 = new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));
            var v2 = new Vector2(-v1.Y, v1.X);

            v1 *= rawRectangle.Width / 2;
            v2 *= rawRectangle.Height / 2;

            X = RotationPoint - v1 - v2;// RotationPoint + v1 + v2;
            Y = RotationPoint + v1 - v2;
            Z = RotationPoint + v1 + v2;
            W = RotationPoint - v1 + v2;

            Corners = new List<Vector2>
            {
                X,
                Y,
                Z,
                W
            };

            Lines = new List<Line>
            {
                new(X, Y),
                new(Y, Z),
                new(Z, W),
                new(W, X)
            };
        }

        public OscetchRectangle(Vector2 center, Vector2 size, float rotation)
        {
            RawRectangle = new Rectangle((center - size / 2).ToPoint(), size.ToPoint());
            Rotation = rotation;
            RotationPoint = center;

            var v1 = new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));
            var v2 = new Vector2(-v1.Y, v1.X);

            v1 *= size.X / 2;
            v2 *= size.Y / 2;

            X = RotationPoint - v1 - v2;// RotationPoint + v1 + v2;
            Y = RotationPoint + v1 - v2;
            Z = RotationPoint + v1 + v2;
            W = RotationPoint - v1 + v2;

            Corners = new List<Vector2>
            {
                X,
                Y,
                Z,
                W
            };

            Lines = new List<Line>
            {
                new(X, Y),
                new(Y, Z),
                new(Z, W),
                new(W, X)
            };
        }
    }
}
