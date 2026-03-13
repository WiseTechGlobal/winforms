// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable SA1518

using System.Drawing;

namespace System.Windows.Forms;

internal static partial class Triangle
{
    private const double TriHeightRatio = 2.5;
    private const double TriWidthRatio = 0.8;

    public static void Paint(
        Graphics g,
        Rectangle bounds,
        TriangleDirection dir,
        Brush backBr,
        Pen backPen1,
        Pen backPen2,
        Pen backPen3,
        bool opaque)
    {
        Point[] points = BuildTrianglePoints(dir, bounds);

        if (opaque)
        {
            g.FillPolygon(backBr, points);
        }

        g.DrawLine(backPen1, points[0], points[1]);
        g.DrawLine(backPen2, points[1], points[2]);
        g.DrawLine(backPen3, points[2], points[0]);
    }

    private static Point[] BuildTrianglePoints(TriangleDirection dir, Rectangle bounds)
    {
        Point[] points = new Point[3];

        int upDownWidth = (int)(bounds.Width * TriWidthRatio);
        if (upDownWidth % 2 == 1)
        {
            upDownWidth++;
        }

        int upDownHeight = (int)Math.Ceiling((upDownWidth / 2D) * TriHeightRatio);

        int leftRightWidth = (int)(bounds.Height * TriWidthRatio);
        if (leftRightWidth % 2 == 0)
        {
            leftRightWidth++;
        }

        int leftRightHeight = (int)Math.Ceiling((leftRightWidth / 2D) * TriHeightRatio);

        switch (dir)
        {
            case TriangleDirection.Up:
                points[0] = new Point(0, upDownHeight);
                points[1] = new Point(upDownWidth, upDownHeight);
                points[2] = new Point(upDownWidth / 2, 0);
                break;
            case TriangleDirection.Down:
                points[0] = new Point(0, 0);
                points[1] = new Point(upDownWidth, 0);
                points[2] = new Point(upDownWidth / 2, upDownHeight);
                break;
            case TriangleDirection.Left:
                points[0] = new Point(leftRightWidth, 0);
                points[1] = new Point(leftRightWidth, leftRightHeight);
                points[2] = new Point(0, leftRightHeight / 2);
                break;
            case TriangleDirection.Right:
                points[0] = new Point(0, 0);
                points[1] = new Point(0, leftRightHeight);
                points[2] = new Point(leftRightWidth, leftRightHeight / 2);
                break;
            default:
                Debug.Fail("Unexpected triangle direction.");
                break;
        }

        switch (dir)
        {
            case TriangleDirection.Up:
            case TriangleDirection.Down:
                OffsetPoints(
                    points,
                    bounds.X + (bounds.Width - upDownHeight) / 2,
                    bounds.Y + (bounds.Height - upDownWidth) / 2);
                break;
            case TriangleDirection.Left:
            case TriangleDirection.Right:
                OffsetPoints(
                    points,
                    bounds.X + (bounds.Width - leftRightWidth) / 2,
                    bounds.Y + (bounds.Height - leftRightHeight) / 2);
                break;
        }

        return points;
    }

    private static void OffsetPoints(Point[] points, int xOffset, int yOffset)
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].X += xOffset;
            points[i].Y += yOffset;
        }
    }
}