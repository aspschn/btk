namespace Blusher.Foundation;

using System;
using System.Runtime.InteropServices;

using Blusher.Drawing;

internal struct ft_point_t
{
    public float x;
    public float y;
}

internal struct ft_size_t
{
    public float width;
    public float height;
}

internal struct ft_rect_t
{
    public ft_point_t pos;
    public ft_size_t size;

    internal static ft_rect_t FromRect(Rect rect)
    {
        ft_rect_t ret;
        ret.pos.x = rect.X;
        ret.pos.y = rect.Y;
        ret.size.width = rect.Width;
        ret.size.height = rect.Height;

        return ret;
    }

    internal IntPtr AllocCPtr()
    {
        IntPtr cPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ft_rect_t)));
        Marshal.StructureToPtr(this, cPtr, false);

        return cPtr;
    }
}

internal struct ft_color_t
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    internal static ft_color_t FromColor(Color color)
    {
        ft_color_t ret;
        ret.r = color.R;
        ret.g = color.G;
        ret.b = color.B;
        ret.a = color.A;

        return ret;
    }

    internal IntPtr AllocCPtr()
    {
        IntPtr cPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ft_color_t)));
        Marshal.StructureToPtr(this, cPtr, false);

        return cPtr;
    }
}

internal class Enums
{
    public enum ft_event_type {
        FT_EVENT_TYPE_POINTER_ENTER,
        FT_EVENT_TYPE_POINTER_LEAVE,
        FT_EVENT_TYPE_POINTER_MOVE,
        FT_EVENT_TYPE_POINTER_PRESS,
        FT_EVENT_TYPE_POINTER_RELEASE,
        FT_EVENT_TYPE_POINTER_CLICK,
        FT_EVENT_TYPE_REQUEST_UPDATE,
        FT_EVENT_TYPE_MOVE,
        FT_EVENT_TYPE_RESIZE,
    }
}

internal class Foundation
{
#if BLUSHER_LIBFOUNDATION_DEV
    private const string LibfoundationSo = "/home/hardboiled65/dev/foundation/build/libfoundation.so";
#else
    private const string LibfoundationSo = "libfoundation.so";
#endif

    // enum ft_event_target_type
    internal const int FT_EVENT_TARGET_TYPE_APPLICATION = 1;
    internal const int FT_EVENT_TARGET_TYPE_DESKTOP_SURFACE = 2;
    internal const int FT_EVENT_TARGET_TYPE_SURFACE = 3;
    internal const int FT_EVENT_TARGET_TYPE_VIEW = 4;

    // enum ft_desktop_surface_role
    internal const int FT_DESKTOP_SURFACE_ROLE_TOPLEVEL = 0;
    internal const int FT_DESKTOP_SURFACE_ROLE_POPUP = 1;

    // enum ft_event_type
    internal const int FT_EVENT_TYPE_POINTER_ENTER= 10;
    internal const int FT_EVENT_TYPE_POINTER_LEAVE = 11;
    internal const int FT_EVENT_TYPE_POINTER_MOVE = 12;
    internal const int FT_EVENT_TYPE_POINTER_PRESS = 13;
    internal const int FT_EVENT_TYPE_POINTER_RELEASE = 14;
    internal const int FT_EVENT_TYPE_POINTER_CLICK = 15;
    internal const int FT_EVENT_TYPE_REQUEST_UPDATE = 70;
    internal const int FT_EVENT_TYPE_MOVE = 100;
    internal const int FT_EVENT_TYPE_RESIZE = 101;

    public delegate void EventListener(IntPtr ftEvent);

    [DllImport(LibfoundationSo)]
    public static extern bool ft_rect_contains_point(IntPtr rect, IntPtr point);

    //====================
    // Application
    //====================
    [DllImport(LibfoundationSo)]
    public static extern IntPtr ft_application_new(int argc, in string[] argv);
    [DllImport(LibfoundationSo)]
    public static extern int ft_application_exec(IntPtr application);

    //======================
    // Desktop Surface
    //======================
    [DllImport(LibfoundationSo)]
    public static extern IntPtr ft_desktop_surface_new(int role);
    [DllImport(LibfoundationSo)]
    public static extern IntPtr ft_desktop_surface_surface(IntPtr desktopSurface);
    [DllImport(LibfoundationSo)]
    public static extern void ft_desktop_surface_show(IntPtr desktopSurface);

    //===================
    // Surface
    //===================
    [DllImport(LibfoundationSo)]
    public static extern IntPtr ft_surface_root_view(IntPtr surface);

    [DllImport(LibfoundationSo)]
    public static extern void ft_surface_add_event_listener(IntPtr surface, Enums.ft_event_type eventType,
        EventListener listener);

    //===================
    // View
    //===================
    [DllImport(LibfoundationSo)]
    public static extern IntPtr ft_view_new(IntPtr parent, IntPtr geometry);
    [DllImport(LibfoundationSo)]
    public static extern void ft_view_set_surface(IntPtr view, IntPtr surface);
    [DllImport(LibfoundationSo)]
    public static extern void ft_view_set_color(IntPtr view, IntPtr color);

    [DllImport(LibfoundationSo)]
    public static extern void ft_view_add_event_listener(IntPtr view, int eventType, EventListener listener);

    //==================
    // Event
    //==================
    [DllImport(LibfoundationSo)]
    public static extern IntPtr ft_event_new(int targetType, IntPtr target,
        Enums.ft_event_type type);

    [DllImport(LibfoundationSo)]
    public static extern bool ft_event_propagation(IntPtr ftEvent);

    [DllImport(LibfoundationSo)]
    public static extern void ft_event_set_propagation(IntPtr ftEvent, bool value);

    [DllImport(LibfoundationSo)]
    public static extern IntPtr ft_event_resize_old_size(IntPtr ftEvent);
    [DllImport(LibfoundationSo)]
    public static extern IntPtr ft_event_resize_size(IntPtr ftEvent);

    //==================
    // Structs
    //==================
    [DllImport(LibfoundationSo)]
    public static extern float ft_size_width(IntPtr ftSize);

    [DllImport(LibfoundationSo)]
    public static extern float ft_size_height(IntPtr ftSize);
}
