using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oscetch.MonoGame.Math.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oscetch.MonoGame.Math.Objects
{
    public abstract class ShapeBase
    {
        private float? _top;
        private float? _bottom;
        private float? _left;
        private float? _right;

        public float Top
        {
            get
            {
                if (_top.HasValue)
                {
                    return _top.Value;
                }

                _top = Corners.Min(x => x.Y);
                return _top.Value;
            }
        }

        public float Bottom
        {
            get
            {
                if (_bottom.HasValue)
                {
                    return _bottom.Value;
                }

                _bottom = Corners.Max(x => x.Y);
                return _bottom.Value;
            }
        }

        public float Left
        {
            get
            {
                if (_left.HasValue)
                {
                    return _left.Value;
                }

                _left = Corners.Min(x => x.X);
                return _left.Value;
            }
        }

        public float Right
        {
            get
            {
                if (_right.HasValue)
                {
                    return _right.Value;
                }

                _right = Corners.Max(x => x.X);
                return _right.Value;
            }
        }

        public abstract IReadOnlyList<Vector2> Corners { get; }
        public abstract IReadOnlyList<Line> Lines { get; }
        public abstract float Rotation { get; }
        public abstract Vector2 RotationPoint { get; }

        public RelativeWidthDirection GetXLookAtPointDirection(ShapeBase other)
        {
            // this polygon is to the right of the other
            if (RotationPoint.X > other.RotationPoint.X)
            {
                return RelativeWidthDirection.Right;
            }
            // this polygon is to the left of the other
            if (RotationPoint.X < other.RotationPoint.X)
            {
                return RelativeWidthDirection.Left;
            }

            // this polygon is on the same width as the other
            return RelativeWidthDirection.Same;
        }

        public RelativeHeightDirection GetYLookAtPointDirection(ShapeBase other)
        {
            // this polygon is below of the other
            if (RotationPoint.Y > other.RotationPoint.Y)
            {
                return RelativeHeightDirection.Below;
            }
            // this polygon is above the other
            if (RotationPoint.Y < other.RotationPoint.Y)
            {
                return RelativeHeightDirection.Above;
            }

            // this polygon is on the same width as the other
            return RelativeHeightDirection.Same;
        }

        public bool Contains(Vector2 vector)
        {
            var contained = false;

            for (int i = 0, j = Corners.Count - 1; i < Corners.Count; j = i++)
            {
                var c1 = Corners[i];
                var c2 = Corners[j];

                if (((c1.Y > vector.Y) != (c2.Y > vector.Y))
                    && (vector.X < (c2.X - c1.X) * (vector.Y - c1.Y) / (c2.Y - c1.Y) + c1.X))
                {
                    contained = !contained;
                }
            }

            return contained;
        }

        public bool Contains(ShapeBase other)
        {
            return other.Corners.All(x => Contains(x));
        }

        public bool Intersects(Rectangle rectangle)
        {
            return Intersects(new OsctechRectangle(rectangle, 0));
        }

        public bool Intersects(ShapeBase other)
        {
            foreach (var corner in other.Corners)
            {
                if (Contains(corner))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Vector2> GetIntersectionPoints(ShapeBase other)
        {
            foreach (var line in Lines)
            {
                foreach (var otherLine in other.Lines)
                {
                    if (line.Intersects(otherLine, out var intersection))
                    {
                        yield return intersection;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, float scale = 1f)
        {
            var origin = texture.Bounds.Size.ToVector2() / 2f;
            foreach (var line in Lines)
            {
                spriteBatch.Draw(texture, line.MidPoint, null, Color.White, line.GetAngleInRadians(), origin,
                    new Vector2(line.Length(), scale),
                    SpriteEffects.None, 0);
            }

            foreach (var corner in Corners)
            {
                spriteBatch.Draw(texture, corner, Color.White);
            }
        }

        public Vector2? GetFirstIntersectionPoint(ShapeBase other, Predicate<Vector2> selector = null)
        {
            foreach (var line in Lines)
            {
                foreach (var otherLine in other.Lines)
                {
                    if (!line.Intersects(otherLine, out var intersection))
                    {
                        continue;
                    }

                    if (selector == null)
                    {
                        return intersection;
                    }
                    if (selector.Invoke(intersection))
                    {
                        return intersection;
                    }
                }
            }

            return null;
        }
    }
}
