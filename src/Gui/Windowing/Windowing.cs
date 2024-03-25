namespace Blusher.Gui.Windowing;

using System.Runtime.InteropServices;

using Blusher.Foundation;
using Blusher.Drawing;
using Blusher.Gui;
using Blusher.Events;

public class WindowShadow : View
{
    public WindowShadow(Window window) : base(window.RootView, new Rect(0.0F, 0.0F, 0.0F, 0.0F))
    {
        //
    }
}

public enum WindowState
{
    Normal,
    Maximized,
    Fullscreen,
}
