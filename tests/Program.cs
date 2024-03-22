// See https://aka.ms/new-console-template for more information
using System;

using Blusher.Core;

Console.WriteLine("Hello, World!");

void RectContains()
{
    Rect rect = new Rect(10, 10, 50, 50);
    Point point = new Point(5, 5);
    if (!rect.Contains(point)) {
        Environment.Exit(1);
    }
}

RectContains();
