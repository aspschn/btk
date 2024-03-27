namespace Blusher.Gui.Windowing;

using System.Runtime.InteropServices;

using Blusher.Foundation;
using Blusher.Drawing;
using Blusher.Gui;
using Blusher.Events;

public class WindowShadow : View
{
    public WindowShadow(Window window) : base(window.RootView, new Rect(0.0F, 0.0F, 10.0F, 10.0F))
    {
        this._thickness = 40;

        this.Color = new Color(255, 0, 0, 100);
    }

    public uint Thickness
    {
        get => this._thickness;
    }

    private uint _thickness;
}

public enum WindowState
{
    Normal,
    Maximized,
    Fullscreen,
}
