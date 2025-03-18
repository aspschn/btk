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

        View bottomToTop = new View(window.Body, new Rect(0.0f, 0.0f, 10.0f, 10.0f));
        bottomToTop.Color = new Color(0, 0, 255, 255);
        bottomToTop.Anchors.Bottom = view.TopAnchor;

        View topToTop = new View(window.Body, new Rect(20.0f, 0.0f, 10.0f, 10.0f));
        topToTop.Color = new Color(0, 255, 255, 255);
        topToTop.Anchors.Top = view.TopAnchor;

        View topToBottom = new View(window.Body, new Rect(30.0f, 0.0f, 10.0f, 10.0f));
        topToBottom.Color = new Color(255, 255, 0, 255);
        topToBottom.Anchors.Top = view.BottomAnchor;

        View bottomToBottom = new View(window.Body, new Rect(40.0f, 0.0f, 10.0f, 10.0f));
        bottomToBottom.Color = new Color(255, 255, 0, 255);
        bottomToBottom.Anchors.Bottom = view.BottomAnchor;

        window.Show();

        return app.Exec();
    }
}
