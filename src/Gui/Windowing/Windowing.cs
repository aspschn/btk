namespace Blusher.Gui.Windowing;

using System.Runtime.InteropServices;

using Blusher.Drawing;
using Blusher.Gui;
using Blusher.Events;
using Blusher.Swingby;

public interface IWindowDecoration
{
    public uint Thickness { get; }
}

public class WindowShadow : View, IWindowDecoration
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

public enum ResizeEdge
{
    None,
    Top,
    Bottom,
    Left,
    TopLeft,
    BottomLeft,
    Right,
    TopRight,
    BottomRight,
}
