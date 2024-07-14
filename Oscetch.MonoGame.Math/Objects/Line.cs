using Microsoft.Xna.Framework;
using Oscetch.MonoGame.Math.Helpers;

namespace Oscetch.MonoGame.Math.Objects
{
    public readonly struct Line
    {
        /// <summary>
        /// X and Y are the start, Z and W are the end
        /// </summary>
        public Vector4 StartAndEnd { get; }
        /// <summary>
        /// The start of the line
        /// </summary>
        public Vector2 Start { get; }
        /// <summary>
        /// The end of the line
        /// </summary>
        public Vector2 End { get; }
        /// <summary>
        /// The middle of the line
        /// </summary>
        public Vector2 MidPoint { get; }

        public Line(Vector2 start, Vector2 end) : this(start, end, (start + end) / 2)
        {
        }

        private Line(Vector2 start, Vector2 end, Vector2 midPoint)
        {
            Start = start;
            End = end;
            StartAndEnd = new Vector4(start.X, start.Y, end.X, end.Y);
            MidPoint = midPoint;
        }

        /// <summary>
        /// Creates a 
        /// </summary>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public Rectangle ToRectangle(int thickness = 1)
        {
            var size = new Point((int)Length(), thickness);
            return new Rectangle(MidPoint.ToPoint() - (size / new Point(2)), size);
        }

        /// <summary>
        /// Returns a new line with the mid point at the provided position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Line Move(Vector2 position)
        {
            var angle = AngleHelper.GetAngleBetweenVectorsInRadians(MidPoint, position);
            var distance = Vector2.Distance(MidPoint, position);
            return Move(angle, distance);
        }

        /// <summary>
        /// Returns a new line which have been moved the distance amount at the angle
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Line Move(float angle, float distance)
        {
            var midToStartAngle = AngleHelper.GetAngleBetweenVectorsInRadians(MidPoint, Start);
            var midToStartDistance = Vector2.Distance(MidPoint, Start);
            var midToEndAngle = AngleHelper.GetAngleBetweenVectorsInRadians(MidPoint, End);
            var midToEndDistance = Vector2.Distance(MidPoint, End);

            var newMidPoint = AngleHelper.MoveObjectInDirection(MidPoint, angle, distance);

            var newStart = AngleHelper.MoveObjectInDirection(newMidPoint, midToStartAngle, midToStartDistance);
            var newEnd = AngleHelper.MoveObjectInDirection(newMidPoint, midToEndAngle, midToEndDistance);

            return new Line(newStart, newEnd, newMidPoint);
        }

        /// <summary>
        /// Returns a new line which have been rotated around the middle of the line
        /// </summary>
        /// <param name="angleInRadians"></param>
        /// <returns></returns>
        public Line Rotate(float angleInRadians)
        {
            var midToStartDistance = Vector2.Distance(MidPoint, Start);
            var midToEndDistance = Vector2.Distance(MidPoint, End);

            var newStart = AngleHelper.MoveObjectInDirection(MidPoint, angleInRadians, midToStartDistance);
            var newEnd = AngleHelper.MoveObjectInDirection(MidPoint, angleInRadians, -midToEndDistance);

            return new Line(newStart, newEnd, MidPoint);
        }

        /// <summary>
        /// Returns the two lines, one from the start of this line to the split point,
        /// the other from the split point to the end of this line
        /// </summary>
        /// <param name="splitPoint"></param>
        /// <returns></returns>
        public (Line startToSplit, Line splitToEnd) Split(Vector2 splitPoint)
        {
            var startToSplitDistance = Vector2.Distance(Start, splitPoint) - 1;
            var startToSplitAngle = AngleHelper.GetAngleBetweenVectorsInRadians(Start, splitPoint);
            var startToSplitEnd = AngleHelper.MoveObjectInDirection(Start, startToSplitAngle, startToSplitDistance);

            var endToSplitDistance = Vector2.Distance(End, splitPoint) - 1;
            var endToSplitAngle = AngleHelper.GetAngleBetweenVectorsInRadians(End, splitPoint);
            var endToSplitStart = AngleHelper.MoveObjectInDirection(End, endToSplitAngle, endToSplitDistance);

            var startToSplit = new Line(Start, startToSplitEnd);
            var splitToEnd = new Line(endToSplitStart, End);

            return (startToSplit, splitToEnd);
        }

        /// <summary>
        /// Checks if the other line intersects with this line, returns the point where the lines overlap
        /// </summary>
        /// <param name="other"></param>
        /// <param name="intersectionPoint"></param>
        /// <returns></returns>
        public bool Intersects(Line other, out Vector2 intersectionPoint)
        {
            return CollisionHelper.TryGetIntersectionPointOnLines(StartAndEnd, other.StartAndEnd, out intersectionPoint);
        }

        /// <summary>
        /// Returns the length of the line
        /// </summary>
        /// <returns></returns>
        public float Length()
        {
            return Vector2.Distance(Start, End);
        }

        /// <summary>
        /// Returns the angle between the start and the end of this line
        /// </summary>
        /// <returns></returns>
        public float GetAngleInRadians()
        {
            return AngleHelper.GetAngleBetweenVectorsInRadians(Start, End);
        }

        public float Left(Vector2 checkPoint)
        {
            return (End.X - Start.X) * (checkPoint.Y - Start.Y) - (checkPoint.X - Start.X) * (End.Y - Start.Y);
        }

        public bool IsLeftClockWiseCheck(Vector2 checkPoint)
        {
            return Left(checkPoint) > 0;
        }

        public bool IsLeftCounterClockWiseCheck(Vector2 checkPoint)
        {
            return Left(checkPoint) < 0;
        }

        /// <summary>
        /// Checks if a vector is on this line
        /// </summary>
        /// <param name="checkPoint"></param>
        /// <returns></returns>
        public bool Contains(Vector2 checkPoint)
        {
            return (checkPoint.X >= Start.X && checkPoint.X <= End.X || checkPoint.X >= End.X && checkPoint.X <= Start.X)
                && (checkPoint.Y >= Start.Y && checkPoint.Y <= End.Y || checkPoint.Y >= End.Y && checkPoint.Y <= Start.Y);
        }

        /// <summary>
        /// Creates a 90 degree line, one of these -> |
        /// </summary>
        /// <param name="midPoint"></param>
        /// <param name="lineLength"></param>
        /// <returns></returns>
        public static Line Create90DegreeLine(Vector2 midPoint, float lineLength)
        {
            return CreateLineFromMidPoint(midPoint, lineLength, MathHelper.PiOver2);
        }

        public static Line CreateLineFromMidPoint(Vector2 midPoint, float lineLength, float lineAngle)
        {
            var fromMidPointLength = lineLength / 2;
            var start = AngleHelper.MoveObjectInDirection(midPoint, lineAngle, fromMidPointLength);
            var end = AngleHelper.MoveObjectInDirection(midPoint, lineAngle, -fromMidPointLength);

            return new Line(start, end, midPoint);
        }
    }
}
