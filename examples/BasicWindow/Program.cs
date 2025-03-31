namespace BasicWindow;

using System;
using System.Reflection;

using Btk.Drawing;
using Btk.Drawing.Imaging;
using Btk.Gui;
using Btk.Gui.Windowing;

public class Program
{
    public static int Main(string[] args)
    {
        Application app = new Application(args);

        Window window = new Window();

        window.Body.Color = new Color(255, 100, 100, 100);

        View view = new View(window.Body, new Rect(0.0f, 0.0f, 50.0f, 50.0f));
        view.Color = new Color(255, 0, 0, 255);
        view.Anchors.Bottom = window.Body.BottomAnchor;
        view.Anchors.BottomMargin = 20.0f;

        // Logo image view.
        using Stream swingbyStream = typeof(Application).Assembly.GetManifestResourceStream("Btk.Resources.swingby.png");
        using var ms = new MemoryStream();
        swingbyStream.CopyTo(ms);
        byte[] buffer = ms.ToArray();
        Image image = new Image(new SizeI(10, 10), ImageFormat.Rgba32);
        image.LoadFromData(buffer);

        View logoView = new View(window.Body, new Rect(0.0f, 0.0f, 340.0f, 150.0f));
        logoView.FillType = ViewFillType.Image;
        logoView.Image = image;
        logoView.Anchors.CenterIn = window.Body;

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
        bottomToBottom.Anchors.BottomMargin = 5.0f;

        window.Show();

        return app.Exec();
    }
}
