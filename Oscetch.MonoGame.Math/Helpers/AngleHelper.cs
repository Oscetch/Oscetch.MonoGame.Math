using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Oscetch.MonoGame.Math.Helpers
{
    public static class AngleHelper
    {
        public const float MAX_DEGREES = 360.0f;

        /// <summary>
        /// Clamps the value between 0 and 2π
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ClampRadians(float value)
        {
            var clamped = value % MathHelper.TwoPi;

            if (clamped < 0)
            {
                clamped += MathHelper.TwoPi;
            }

            return clamped;
        }

        /// <summary>
        /// Clamps the value between 0 and 360
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ClampDegrees(float value)
        {
            var clamped = value % MAX_DEGREES;

            if(clamped < 0)
            {
                clamped += MAX_DEGREES;
            }

            return clamped;
        }

        /// <summary>
        /// Returns the angle between two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetAngleBetweenVectorsInRadians(Vector2 v1, Vector2 v2)
        {
            var deltaY = v2.Y - v1.Y;
            var deltaX = v2.X - v1.X;
            return (float)System.Math.Atan2(deltaY, deltaX);
        }

        /// <summary>
        /// Returns the angle between two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetAngleBetweenVectorsInDegrees(Vector2 v1, Vector2 v2)
        {
            return MathHelper.ToDegrees(GetAngleBetweenVectorsInRadians(v1, v2));
        }

        public static float ToDegrees(float roationRadians)
        {
            var degrees = MathHelper.ToDegrees(roationRadians);
            return System.Math.Abs(degrees % MAX_DEGREES);
        }

        /// <summary>
        /// Returns the closest position in the possiblePositions list
        /// </summary>
        /// <param name="positionNow"></param>
        /// <param name="possiblePositions"></param>
        /// <returns></returns>
        public static Vector2 GetClosestPositionInList(Vector2 positionNow, IList<Vector2> possiblePositions)
        {
            var closestPosition = possiblePositions[0];
            foreach (var position in possiblePositions)
            {
                var currentDistance = Vector2.Distance(closestPosition, positionNow);
                if (currentDistance > Vector2.Distance(position, positionNow))
                {
                    closestPosition = position;
                }
            }

            return closestPosition;
        }

        /// <summary>
        /// Returns the new position given the angle distance
        /// </summary>
        /// <param name="positionNow"></param>
        /// <param name="angleInRadians"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector2 MoveObjectInDirection(Vector2 positionNow, float angleInRadians, float distance)
        {
            var newX = positionNow.X + (distance * (float)System.Math.Cos(angleInRadians));
            var newY = positionNow.Y + (distance * (float)System.Math.Sin(angleInRadians));

            return new Vector2(newX, newY);
        }

        /// <summary>
        /// Returns the position of a vector that has been rotated around a rotation point.
        /// Assumes that the "vector" parameter is the position when the angleInRadians is zero
        /// </summary>
        /// <param name="rotationPoint"></param>
        /// <param name="angleInRadians"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2 RotatePoint(Vector2 rotationPoint, float angleInRadians, Vector2 vector)
        {
            var sinus = (float)System.Math.Sin(angleInRadians);
            var cosinus = (float)System.Math.Cos(angleInRadians);

            // translate point back to origin:
            var reversedPoint = vector - rotationPoint;

            // rotate point
            var newX = reversedPoint.X * cosinus - reversedPoint.Y * sinus;
            var newY = reversedPoint.X * sinus + reversedPoint.Y * cosinus;

            // translate point back:
            return new Vector2(newX, newY) + rotationPoint;
        }
    }
}
