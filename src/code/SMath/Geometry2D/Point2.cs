﻿using System.Numerics;
using System.Runtime.CompilerServices;
using static SMath.Geometry2D.GeometricVector2;

namespace SMath.GeometryD2;

/// <summary>
/// Point in two dimensions.
/// </summary>
public static class Point2
{
    /// <summary>
    /// Euclidean distance of the point and origin.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Euclidean_distance">Wikipedia</a>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static N Distance<N>((N X, N Y) point)
        where N : IRootFunctions<N>
        => PT.Hypotenuse(point);

    /// <summary>
    /// Euclidean distance of two points.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Euclidean_distance">Wikipedia</a>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static N Distance<N>((N X, N Y) point1, (N X, N Y) point2)
        where N : IRootFunctions<N>
        => PT.Hypotenuse(point2.X - point1.X, point2.Y - point1.Y);

    /// <summary>
    /// Manhattan or taxicab distance of point and origin.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia</a>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static N ManhattanDistance<N>((N X, N Y) point)
        where N : INumberBase<N>
        => N.Abs(point.X + point.Y);

    /// <summary>
    /// Manhattan or taxicab distance of two points.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia</a>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static N ManhattanDistance<N>((N X, N Y) point1, (N X, N Y) point2)
        where N : INumberBase<N>
        => N.Abs(point1.X - point2.X) + N.Abs(point1.Y - point2.Y);

    /// <summary>
    /// Chebyshev distance of point and origin
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Chebyshev_distance">Wikipedia</a>
    /// </remarks>
    public static N ChebyshevDistance<N>((N X, N Y) point)
        where N : INumber<N>
        => N.Max(N.Abs(point.X), N.Abs(point.Y));

    /// <summary>
    /// Chebyshev distance of two points.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Chebyshev_distance">Wikipedia</a>
    /// </remarks>
    public static N ChebyshevDistance<N>((N X, N Y) point1, (N X, N Y) point2)
        where N : INumber<N>
        => N.Max(N.Abs(point1.X - point2.X), N.Abs(point1.Y - point2.Y));

    /// <summary>
    /// Minkowski distance of point and origin.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Minkowski_distance">Wikipedia</a>
    /// </remarks>
    public static N MinkowskiDistance<N>((N X, N Y) point, N r)
        where N : IPowerFunctions<N>
        => N.Pow(N.Pow(N.Abs(point.X), r) + N.Pow(N.Abs(point.Y), r), N.One / r);

    /// <summary>
    /// Minkowski distance of two points.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Minkowski_distance">Wikipedia</a>
    /// </remarks>
    public static N MinkowskiDistance<N>((N X, N Y) point1, (N X, N Y) point2, N r)
        where N : IPowerFunctions<N>
        => N.Pow(N.Pow(N.Abs(point1.X - point2.X), r) + N.Pow(N.Abs(point1.Y - point2.Y), r), N.One / r);

    /// <summary>
    /// Get all coordinates at exact Manhattan or taxicab distance from the center point.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia</a>
    /// </remarks>
    public static IEnumerable<(NInt X, NInt Y)> CoordinatesAtManhattanDistance<NInt>((NInt X, NInt Y) center, NInt distance)
        where NInt : IBinaryInteger<NInt>
    {
        if (distance < NInt.One)
            yield break;

        var minX = center.X - distance;
        var maxX = center.X + distance;
        var minY = center.Y - distance;
        var maxY = center.Y + distance;

        for (var diff = NInt.Zero; diff <= distance; diff++)
            yield return (center.X + diff, minY + diff);

        for (var diff = NInt.One; diff < distance; diff++)
            yield return (maxX - diff, center.Y + diff);

        for (var diff = NInt.Zero; diff <= distance; diff++)
            yield return (center.X - diff, maxY - diff);

        for (var diff = NInt.One; diff < distance; diff++)
            yield return (minX + diff, center.Y - diff);
    }

    /// <summary>
    /// Get all coordinates at exact Manhattan or taxicab distance from the center point limited by bounds.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia</a>
    /// </remarks>
    public static IEnumerable<(NInt X, NInt Y)> CoordinatesAtManhattanDistance<NInt>((NInt X, NInt Y) center, NInt distance,
        (NInt X, NInt Y) bottomLimit, (NInt X, NInt Y) topLimit)
        where NInt : IBinaryInteger<NInt>
    {
        if (distance < NInt.One)
            yield break;

        var minX = center.X - distance;
        var maxX = center.X + distance;
        var minY = center.Y - distance;
        var maxY = center.Y + distance;

        for (var diff = distance - NInt.Min(distance, center.Y - bottomLimit.Y); diff <= NInt.Min(distance, topLimit.X - center.X); diff++)
            yield return (center.X + diff, minY + diff);

        {
            var from = NInt.Max(distance - NInt.Min(distance, topLimit.X - center.X), NInt.One);
            var to = NInt.Min(distance - NInt.One, topLimit.Y - center.Y);
            for (var diff = from; diff <= to; diff++)
                yield return (maxX - diff, center.Y + diff);
        }

        for (var diff = distance - NInt.Min(distance, topLimit.Y - center.Y); diff <= NInt.Min(distance, center.X - bottomLimit.X); diff++)
            yield return (center.X - diff, maxY - diff);

        {
            var from = NInt.Max(distance - NInt.Min(distance, center.X - bottomLimit.X), NInt.One);
            var to = NInt.Min(distance - NInt.One, center.Y - bottomLimit.Y);
            for (var diff = from; diff <= to; diff++)
                yield return (minX + diff, center.Y - diff);
        }
    }

    /// <summary>
    /// Get all coordinates up to the Manhattan or taxicab distance from the center point.
    /// Center is included.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Taxicab_geometry">Wikipedia</a>
    /// </remarks>
    public static IEnumerable<(NInt X, NInt Y)> CoordinatesUpToManhattanDistance<NInt>((NInt X, NInt Y) center, NInt distance)
       where NInt : IBinaryInteger<NInt>
    {
        if (distance < NInt.One)
            yield break;

        var minX = center.X - distance;
        var maxX = center.X + distance;
        var minY = center.Y - distance;
        var maxY = center.Y + distance;

        for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
                yield return (x, y);
    }

    public static IEnumerable<(NInt X, NInt Y)> CoordinatesUpToManhattanDistance<NInt>((NInt X, NInt Y) center, NInt distance,
        (NInt X, NInt Y) bottomLimit, (NInt X, NInt Y) topLimit)
        where NInt : IBinaryInteger<NInt>
    {
        if (distance < NInt.One)
            yield break;

        var minX = NInt.Max(center.X - distance, bottomLimit.X);
        var maxX = NInt.Min(center.X + distance, topLimit.X);
        var minY = NInt.Max(center.Y - distance, bottomLimit.Y);
        var maxY = NInt.Min(center.Y + distance, topLimit.X);

        for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
                yield return (x, y);
    }

    /// <summary>
    /// Get all coordinates in determined Chebyshev distance from the center point.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Chebyshev_distance">Wikipedia</a>
    /// </remarks>
    public static IEnumerable<(NInt X, NInt Y)> CoordinatesInChebyshevDistance<NInt>((NInt X, NInt Y) center, NInt distance)
        where NInt : IBinaryInteger<NInt>
    {
        var minX = center.X - distance;
        var maxX = center.X + distance;
        var minY = center.Y - distance;
        var maxY = center.Y + distance;

        for (var x = minX; x <= maxX; x++)
            yield return (x, minY);

        for (var y = minY + NInt.One; y < maxY; y++)
            yield return (maxX, y);

        for (var x = maxX; x >= minX; x--)
            yield return (x, maxY);

        for (var y = maxY - NInt.One; y > minY; y--)
            yield return (minX, y);
    }

    /// <summary>
    /// Get all coordinates in determined Chebyshev distance from the center point limited by bounds.
    /// </summary>
    /// <remarks>
    /// <a href="https://en.wikipedia.org/wiki/Chebyshev_distance">Wikipedia</a>
    /// </remarks>
    public static IEnumerable<(NInt X, NInt Y)> CoordinatesInChebyshevDistance<NInt>((NInt X, NInt Y) center, NInt distance,
        (NInt X, NInt Y) bottomLimit, (NInt X, NInt Y) topLimit)
        where NInt : IBinaryInteger<NInt>
    {
        var minX = center.X - distance;
        var maxX = center.X + distance;
        var minY = center.Y - distance;
        var maxY = center.Y + distance;

        if (minY >= bottomLimit.Y)
        {
            for (var x = NInt.Max(minX, bottomLimit.X); x <= NInt.Min(maxX, topLimit.X); x++)
                yield return (x, minY);
        }

        if (maxX <= topLimit.X)
        {
            for (var y = NInt.Max(minY + NInt.One, bottomLimit.Y); y < NInt.Min(maxY, topLimit.Y); y++)
                yield return (maxX, y);
        }

        if (maxY <= topLimit.Y)
        {
            for (var x = NInt.Min(maxX, topLimit.X); x >= NInt.Max(minX, bottomLimit.X); x--)
                yield return (x, maxY);
        }

        if (minX >= bottomLimit.X)
        {
            for (var y = NInt.Min(maxY - NInt.One, topLimit.Y); y > NInt.Max(minY, bottomLimit.Y); y--)
                yield return (minX, y);
        }
    }
}
