// See https://aka.ms/new-console-template for more information
namespace Example;

using System;

using Blusher.Drawing;
using Blusher.Gui;
using Blusher.Gui.Windowing;

public class Program
{
    public static int Main(string[] args)
    {
        Application app = new Application(args);

        Window window = new Window();
        window.Show();

        return app.Exec();
    }
}
