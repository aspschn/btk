namespace Btk.Gui.Windowing;

using System.Runtime.InteropServices;

using Btk.Drawing;
using Btk.Drawing.Effects;
using Btk.Gui;
using Btk.Events;
using Btk.Swingby;

public interface IWindowDecoration
{
    public uint Thickness { get; }

    public bool Activated { get; set; }
}

public class WindowShadow : View, IWindowDecoration
{
    private uint _thickness;
    private View _dummy;
    private bool _activated;

    public WindowShadow(Window window, IntPtr rootView) : base(rootView, window, new Rect(0.0F, 0.0F, 10.0F, 10.0F))
    {
        this._thickness = 40;
        _dummy = new View(this, new Rect(0.0f, 0.0f, 10.0f, 10.0f));
        _dummy.Color = new Color(0, 0, 0, 255);
        var filter = new BlurFilter(15.0f);
        _dummy.Filters.Add(filter);

        this.Color = new Color(0, 0, 0, 0);
    }

    public uint Thickness
    {
        get => this._thickness;
    }

    public bool Activated
    {
        get => _activated;
        set
        {
            _activated = value;
            if (value)
            {
                // TODO. Change shadow.
            }
        }
    }

    public void UpdateShadow()
    {
        _dummy.Geometry = new Rect(
            Thickness,
            Thickness,
            base.Geometry.Width - Thickness * 2,
            base.Geometry.Height - Thickness * 2);
    }
}

public enum WindowState
{
    Normal,
    Maximized,
    Fullscreen,
}
