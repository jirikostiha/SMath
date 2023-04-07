﻿using System.Numerics;

namespace SMath.Geometry2D
{
    /// <summary>
    /// Line in two dimensional space.
    /// </summary>
    /// <remarks>
    /// Line is defined in general form by default.
    /// <a href="https://en.wikipedia.org/wiki/Line_(geometry)">wikipedia</a>
    /// <a href="https://www.123calculus.com/en/line-calculator-page-7-60-100.html">calculator 1</a>
    /// </remarks>
    public static class Line
    {
        /// <summary>
        /// X axis line in general form.
        /// </summary>
        public static (N A, N B, N C) XAxis<N>()
            where N : INumberBase<N>
            => (N.Zero, N.One, N.Zero);

        /// <summary>
        /// Y axis line in general form.
        /// </summary>
        public static (N A, N B, N C) YAxis<N>()
            where N : INumberBase<N>
            => (-N.One, N.Zero, N.Zero);

        public static string PlainTextEquation
            => "a*x + b*y + c = 0";

        /// <summary>
        /// Line in general form from two points in cartesian coordinates.
        /// </summary>
        public static (N A, N B, N C) FromTwoPoints<N>((N X, N Y) point1, (N X, N Y) point2)
            where N : IRootFunctions<N>
        {
            // direction vector = P2 - P1
            var direction = GeometricVector2.Direction(point1, point2);
            // normal vector
            var normal = GeometricVector2.Normal1(direction);
            // calculate c constant
            var c = -normal.X1 * point1.X - normal.X2 * point1.Y;

            return (normal.X1, normal.X2, c);
        }

        /// <summary>
        /// Line in general form from slope and y-intercept.
        /// </summary>
        public static (N A, N B, N C) FromSlopeAndYIntercept<N>(N slope, N yintercept)
            where N : INumberBase<N>
            => (-slope, N.One, -yintercept);

        /// <summary>
        /// Slope of a line.
        /// </summary>
        public static class Slope
        {
            /// <summary>
            /// Slope of a line from line general form of line.
            /// </summary>
            public static N FromGeneralForm<N>((N A, N B, N C) line)
                where N : IUnaryNegationOperators<N, N>, IDivisionOperators<N, N, N>
                => -line.A / line.B;

            /// <summary>
            /// Slope of a line from line angle.
            /// </summary>
            public static N FromAngle<N>(N angle)
                where N : IFloatingPointIeee754<N>, ITrigonometricFunctions<N>
                => angle != N.Pi / N.CreateChecked(2)
                ? N.Tan(angle)
                : N.PositiveInfinity;
        }

        /// <summary>
        /// The x-intercept is the point at which the lines crosses the x-axis.
        /// </summary>
        public static class XIntercept
        {
            /// <summary>
            /// The x-intercept of a line in general form.
            /// </summary>
            public static N FromGeneralForm<N>((N A, N B, N C) line)
                where N : IUnaryNegationOperators<N, N>, IDivisionOperators<N, N, N>
                => -line.C / line.A;
        }

        /// <summary>
        /// The y-intercept is the point at which the lines crosses the y-axis.
        /// </summary>
        public static class YIntercept
        {
            /// <summary>
            /// The y-intercept of a line in general form.
            /// </summary>
            public static N GeneralForm<N>((N A, N B, N C) line)
                where N : IUnaryNegationOperators<N, N>, IDivisionOperators<N, N, N>
                => -line.C / line.B;
        }

        /// <summary>
        /// Normal line to a given line.
        /// </summary>
        public static class NormalLine
        {
            /// <summary>
            /// Slope of a normal line.
            /// </summary>
            public static class Slope
            {
                /// <summary>
                /// Get a slope of a normal line to line determined in general forn.
                /// </summary>
                public static N FromGeneralForm<N>(N a, N b)
                    where N : IDivisionOperators<N, N, N>
                    => b / a;

                /// <summary>
                /// Get a slope of a normal line to line determined in general forn.
                /// </summary>
                public static N FromGeneralForm<N>((N A, N B, N C) line)
                    where N : IDivisionOperators<N, N, N>
                    => line.B / line.A;
            }

            /// <summary>
            /// Get normal line in general form from line determined by general form.
            /// </summary>
            public static (N A, N B, N C) FromGeneralForm<N>((N A, N B, N C) line)
                where N : IUnaryNegationOperators<N, N>
                => (line.B, -line.A, line.C);
        }

        /// <summary>
        /// Ray.
        /// Determined by point and angle.
        /// </summary>
        public static class Ray
        {
            /// <summary>
            /// Points on a ray.
            /// </summary>
            public static class Points
            {
                /// <summary>
                /// Get points on a ray determined by origin point and angle.
                /// </summary>
                public static IEnumerable<(N X, N Y)> Get<N>(N angle, N step, int count)
                    where N : ITrigonometricFunctions<N>
                {
                    for (int i = 1; i <= count; i++)
                        yield return (
                            Circle.Perimeter.Point.XFromAngle(N.CreateChecked(i) * step, angle),
                            Circle.Perimeter.Point.YFromAngle(N.CreateChecked(i) * step, angle));
                }
            }
        }

        /// <summary>
        /// Line Segment
        /// Determined by two points.
        /// </summary>
        public static class Segment
        {
            /// <summary>
            /// Length of a line segment.
            /// </summary>
            public static class Length
            {
                /// <summary>
                /// Length of a line segment determined by two points.
                /// </summary>
                public static N FromTwoPints<N>((N X, N Y) p1, (N X, N Y) p2)
                    where N : IRootFunctions<N>
                    => PT.Hypotenuse(p2.X - p1.X, p2.Y - p1.Y);
            }

            /// <summary>
            /// Slope of a line segment.
            /// </summary>
            public static class Slope
            {
                /// <summary>
                /// Get slope value from two points in 2D.
                /// </summary>
                /// <remarks>
                /// <a href="https://en.wikipedia.org/wiki/Slope">wikipedia</a>
                /// </remarks>
                public static N FromTwoPoints<N>((N X, N Y) point1, (N X, N Y) point2)
                    where N : ISubtractionOperators<N, N, N>, IDivisionOperators<N, N, N>
                    => (point2.Y - point1.Y) / (point2.X - point1.X);
            }

            /// <summary>
            /// Points on a segment line.
            /// </summary>
            public static class Points
            {
                /// <summary>
                /// Get n points on a line segment determined by two points.
                /// </summary>
                public static IEnumerable<(N X, N Y)> Get<N>((N X, N Y) point1, (N X, N Y) point2, int count)
                    where N : INumberBase<N>
                {
                    var xstep = (point2.X - point1.X) / N.CreateChecked(count + 1);
                    var ystep = (point2.Y - point1.Y) / N.CreateChecked(count + 1);
                    for (int i = 1; i <= count; i++)
                        yield return (
                            point1.X + N.CreateChecked(i) * xstep,
                            point1.Y + N.CreateChecked(i) * ystep);
                }
            }

            /// <summary>
            /// Parallel line segment to original segment.
            /// </summary>
            public static class Parallel
            {
                /// <summary>
                /// Get a point which is with s2p1 line segment parallel to (s1p1, s1p2).
                /// </summary>
                public static (N X, N Y) FromThreePoints<N>((N X, N Y) point1, (N X, N Y) point2, (N X, N Y) seedPoint)
                    where N : INumberBase<N>
                    => (seedPoint.X + point2.X - point1.X, seedPoint.Y + point2.Y - point1.Y);
            }

            /// <summary>
            /// Parallel line segments to original segment.
            /// </summary>
            public static class Parallels
            {
                /// <summary>
                /// Get line segment parallels to given line segment in seed direction.
                /// </summary>
                public static IEnumerable<((N X, N Y) Point1, (N X, N Y) Point2)> Get<N>((N X, N Y) basePoint,
                    (N X, N Y) direction, (N X, N Y) seedDirection, params (N Distance, N Length)[] @params)
                    where N : INumberBase<N>, IRootFunctions<N>
                    => Get(basePoint, direction, seedDirection, @params);

                /// <summary>
                /// Get line segment parallels to given line segment in seed direction.
                /// </summary>
                public static IEnumerable<((N X, N Y) Point1, (N X, N Y) Point2)> Get<N>((N X, N Y) basePoint,
                    (N X, N Y) direction, (N X, N Y) seedDirection, IList<(N Distance, N Length)> @params)
                    where N : INumberBase<N>, IRootFunctions<N>
                {
                    direction = GeometricVector2.Normalized(direction);
                    seedDirection = GeometricVector2.Normalized(seedDirection);

                    foreach (var param in @params)
                    {
                        yield return (
                            (basePoint.X + seedDirection.X * param.Distance,
                             basePoint.Y + seedDirection.Y * param.Distance),
                            (basePoint.X + seedDirection.X * param.Distance + direction.X * param.Length,
                             basePoint.Y + seedDirection.Y * param.Distance + direction.Y * param.Length));
                    }
                }
            }
        }
    }
}
