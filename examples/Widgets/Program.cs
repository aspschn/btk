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
        ProgressBar progressBar = new ProgressBar(mainWidget);
        progressBar.Value = 0.5f;

        window.Show();

        return app.Exec();
    }
}
