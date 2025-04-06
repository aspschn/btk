namespace BasicWindow;

using System;
using System.Reflection;

using Btk.Drawing;
using Btk.Drawing.Imaging;
using Btk.Gui;
using Btk.Gui.Windowing;
using Btk.Gui.Widgets;

public class Program
{
    public static int Main(string[] args)
    {
        Application app = new Application(args);

        Window window = new Window();

        Widget mainWidget = new Widget(window.Body);
        mainWidget.Anchors.Fill = window.Body;
        ProgressBar progressBar = new ProgressBar(mainWidget);
        progressBar.Value = 0.5f;

        Label label = new Label("Hello, BTK!", mainWidget);
        label.Anchors.Top = progressBar.BottomAnchor;

        PushButton button = new PushButton("Click", mainWidget);
        button.Anchors.Top = label.BottomAnchor;

        window.Show();

        return app.Exec();
    }
}
