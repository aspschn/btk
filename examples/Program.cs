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

        View mainView = new View(window.Body, new Rect(0.0f, 0.0f, 0.0f, 0.0f));
        mainView.Color = new Color(220, 220, 220, 255);
        mainView.Anchors.Fill = window.Body;

        View view = new View(mainView, new Rect(0.0f, 0.0f, 50.0f, 50.0f));
        view.Color = new Color(255, 0, 0, 255);
        view.Anchors.Bottom = mainView.BottomAnchor;
        view.Anchors.BottomMargin = 20.0f;

        View bottomToTop = new View(mainView, new Rect(0.0f, 0.0f, 10.0f, 10.0f));
        bottomToTop.Color = new Color(0, 0, 255, 255);
        bottomToTop.Anchors.Bottom = view.TopAnchor;

        View topToTop = new View(mainView, new Rect(20.0f, 0.0f, 10.0f, 10.0f));
        topToTop.Color = new Color(0, 255, 255, 255);
        topToTop.Anchors.Top = view.TopAnchor;

        View topToBottom = new View(mainView, new Rect(30.0f, 0.0f, 10.0f, 10.0f));
        topToBottom.Color = new Color(255, 255, 0, 255);
        topToBottom.Anchors.Top = view.BottomAnchor;

        View bottomToBottom = new View(mainView, new Rect(40.0f, 0.0f, 10.0f, 10.0f));
        bottomToBottom.Color = new Color(255, 255, 0, 255);
        bottomToBottom.Anchors.Bottom = view.BottomAnchor;
        bottomToBottom.Anchors.BottomMargin = 5.0f;

        window.Show();

        return app.Exec();
    }
}
