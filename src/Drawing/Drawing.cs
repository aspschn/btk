namespace Btk.Drawing;

using System;

using RectangleF = System.Drawing.RectangleF;

public struct Point
{
    public Point(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public float X { get; set; }
    public float Y { get; set; }
}

public struct PointI
{
    public PointI(Int64 x, Int64 y)
    {
        X = x;
        Y = y;
    }

    public Int64 X { get; set; }
    public Int64 Y { get; set; }
}

public struct Size
{
    public Size(float width, float height)
    {
        this.Width = width;
        this.Height = height;
    }

    public float Width { get; set; }
    public float Height { get; set; }
}

public struct SizeI
{
    public SizeI(UInt64 width, UInt64 height)
    {
        Width = width;
        Height = height;
    }

    public UInt64 Width { get; set; }
    public UInt64 Height { get; set; }
}

public struct Rect
{
    public Rect(float x, float y, float width, float height)
    {
        this.Pos = new Point(x, y);
        this.Size = new Size(width, height);
    }

    public Point Pos { get; set; }
    public Size Size { get; set; }

    public float X
    {
        get
        {
            return this.Pos.X;
        }
        set
        {
            this.Pos = new Point(value, this.Pos.Y);
        }
    }

    public float Y
    {
        get
        {
            return this.Pos.Y;
        }
        set
        {
            this.Pos = new Point(this.Pos.X, value);
        }
    }

    public float Width
    {
        get
        {
            return this.Size.Width;
        }
        set
        {
            this.Size = new Size(value, this.Size.Height);
        }
    }

    public float Height
    {
        get
        {
            return this.Size.Height;
        }
        set
        {
            this.Size = new Size(this.Size.Height, value);
        }
    }
}

public struct Color
{
    public Color(byte r, byte g, byte b, byte a)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;
    }

    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; }

}

public struct ViewRadius
{
    public ViewRadius(float topLeft, float topRight, float bottomRight, float bottomLeft)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomRight = bottomRight;
        BottomLeft = bottomLeft;
    }

    public float TopLeft { get; set; }
    public float TopRight { get; set; }
    public float BottomRight { get; set; }
    public float BottomLeft { get; set; }
}
