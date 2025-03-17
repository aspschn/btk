// See https://aka.ms/new-console-template for more information
namespace Example;

using System;

using Btk.Drawing;
using Btk.Gui;
using Btk.Gui.Windowing;

public class Program
{
    public static int Main(string[] args)
    {
        Application app = new Application(args);

        Window window = new Window();

        View view = new View(window.Body, new Rect(0.0f, 0.0f, 50.0f, 50.0f));
        view.Color = new Color(255, 0, 0, 255);
        view.Anchors.Bottom = window.Body.BottomAnchor;

        window.Show();

        return app.Exec();
    }
}
