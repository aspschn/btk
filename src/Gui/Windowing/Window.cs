using System.Runtime.InteropServices;

using Blusher.Foundation;
using Blusher.Drawing;
using Blusher.Gui;
using Blusher.Events;

public class Window
{
    public Window()
    {
        this._ftDesktopSurface = Foundation.ft_desktop_surface_new(
            Foundation.FT_DESKTOP_SURFACE_ROLE_TOPLEVEL);

        // Set the root view color as transparent.
        IntPtr ftSurface = Foundation.ft_desktop_surface_surface(this._ftDesktopSurface);
        IntPtr ftView = Foundation.ft_surface_root_view(ftSurface);
        ft_color_t color = ft_color_t.FromColor(new Color(0, 0, 0, 0));

        IntPtr ftColor = color.AllocCPtr();
        Foundation.ft_view_set_color(ftView, ftColor);
        Marshal.FreeHGlobal(ftColor);

        // Create a root view.
        this._rootView = new View(ftView, new Rect(0.0F, 0.0F, 100.0F, 100.0F));
        this._rootView.Color = new Color(0, 0, 0, 0);

        // Add event listeners.
        this.AddResizeEventListener();
    }

    public void Show()
    {
        Foundation.ft_desktop_surface_show(this._ftDesktopSurface);
    }

    public View RootView
    {
        get => this._rootView;
    }

    protected virtual void ResizeEvent(ResizeEvent evt)
    {
        Console.WriteLine("Window resize event.");
        Console.WriteLine(evt.Size.Width);
        Console.WriteLine(evt.Size.Height);
    }

    private void AddResizeEventListener()
    {
        IntPtr surface = Foundation.ft_desktop_surface_surface(this._ftDesktopSurface);
        var eventListener = new Foundation.EventListener(this.CallResizeEvent);
        Foundation.ft_surface_add_event_listener(surface, Enums.ft_event_type.FT_EVENT_TYPE_RESIZE, eventListener);
    }

    private void CallResizeEvent(IntPtr ftEvent)
    {
        IntPtr oldSize = Foundation.ft_event_resize_size(ftEvent);
        var oldWidth = Foundation.ft_size_width(oldSize);
        var oldHeight = Foundation.ft_size_height(oldSize);
        IntPtr size = Foundation.ft_event_resize_size(ftEvent);
        var width = Foundation.ft_size_width(size);
        var height = Foundation.ft_size_height(size);

        ResizeEvent evt = new ResizeEvent(new Size(oldWidth, oldHeight), new Size(width, height));
        this.ResizeEvent(evt);
    }

    private IntPtr _ftDesktopSurface;
    private View _shadow;
    private View _rootView;
}
