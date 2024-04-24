namespace Blusher.Swingby;

using System;
using System.Runtime.InteropServices;

using Blusher.Drawing;

internal struct sb_point_t
{
    public float x;
    public float y;
}

internal struct sb_size_t
{
    public float width;
    public float height;

    internal static sb_size_t FromSize(Size size)
    {
        sb_size_t ret;
        ret.width = size.Width;
        ret.height = size.Height;

        return ret;
    }

    internal IntPtr AllocCPtr()
    {
        IntPtr cPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sb_size_t)));
        Marshal.StructureToPtr(this, cPtr, false);

        return cPtr;
    }
}

internal struct sb_rect_t
{
    public sb_point_t pos;
    public sb_size_t size;

    internal static sb_rect_t FromRect(Rect rect)
    {
        sb_rect_t ret;
        ret.pos.x = rect.X;
        ret.pos.y = rect.Y;
        ret.size.width = rect.Width;
        ret.size.height = rect.Height;

        return ret;
    }

    internal IntPtr AllocCPtr()
    {
        IntPtr cPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sb_rect_t)));
        Marshal.StructureToPtr(this, cPtr, false);

        return cPtr;
    }
}

internal struct sb_color_t
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    internal static sb_color_t FromColor(Color color)
    {
        sb_color_t ret;
        ret.r = color.R;
        ret.g = color.G;
        ret.b = color.B;
        ret.a = color.A;

        return ret;
    }

    internal IntPtr AllocCPtr()
    {
        IntPtr cPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sb_color_t)));
        Marshal.StructureToPtr(this, cPtr, false);

        return cPtr;
    }
}

internal class Swingby
{
#if BLUSHER_LIBSWINGBY_DEV
    private const string Libswingby = "/usr/local/lib/libswingby.so";
#else
    private const string Libswingby = "libswingby.so";
#endif

    // enum sb_event_target_type
    internal const int SB_EVENT_TARGET_TYPE_APPLICATION = 1;
    internal const int SB_EVENT_TARGET_TYPE_DESKTOP_SURFACE = 2;
    internal const int SB_EVENT_TARGET_TYPE_SURFACE = 3;
    internal const int SB_EVENT_TARGET_TYPE_VIEW = 4;

    // enum sb_event_type
    internal const int SB_EVENT_TYPE_POINTER_ENTER = 10;
    internal const int SB_EVENT_TYPE_POINTER_LEAVE = 11;
    internal const int SB_EVENT_TYPE_POINTER_MOVE = 12;
    internal const int SB_EVENT_TYPE_POINTER_PRESS = 13;
    internal const int SB_EVENT_TYPE_POINTER_RELEASE = 14;
    internal const int SB_EVENT_TYPE_POINTER_CLICK = 15;
    internal const int SB_EVENT_TYPE_POINTER_DOUBLE_CLICK = 16;
    internal const int SB_EVENT_TYPE_REQUEST_UPDATE = 70;
    internal const int SB_EVENT_TYPE_SHOW = 80;
    internal const int SB_EVENT_TYPE_HIDE = 81;
    internal const int SB_EVENT_TYPE_MOVE = 100;
    internal const int SB_EVENT_TYPE_RESIZE = 101;
    internal const int SB_EVENT_TYPE_STATE_CHANGE = 110;

    // enum sb_desktop_surface_role
    internal const int SB_DESKTOP_SURFACE_ROLE_TOPLEVEL = 0;
    internal const int SB_DESKTOP_SURFACE_ROLE_POPUP = 1;

    // enum sb_desktop_surface_toplevel_resize_edge
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_NONE = 0;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP = 1;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM = 2;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_LEFT = 4;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP_LEFT = 5;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM_LEFT = 6;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_RIGHT = 8;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_TOP_RIGHT = 9;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_RESIZE_EDGE_BOTTOM_RIGHT = 10;

    // enum sb_desktop_surface_toplevel_state
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_STATE_NORMAL = 0;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_STATE_MAXIMIZED = 1;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_STATE_FULLSCREEN = 2;

    // enum sb_view_fill_type
    // internal const int SB_VIEW_FILL_TYPE_SINGLE_COLOR = 0;
    // internal const int SB_VIEW_FILL_TYPE_IMAGE = 1;

    public delegate void EventListener(IntPtr sbEvent);

    //==================
    // Application
    //==================
    [DllImport(Libswingby)]
    internal static extern IntPtr sb_application_new(int argc, string[] argv);

    [DllImport(Libswingby)]
    internal static extern int sb_application_exec(IntPtr sbApplication);

    //====================
    // Desktop Surface
    //====================
    [DllImport(Libswingby)]
    internal static extern IntPtr sb_desktop_surface_new(int role);

    [DllImport(Libswingby)]
    internal static extern IntPtr sb_desktop_surface_surface(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_show(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_hide(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_toplevel_move(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_toplevel_resize(IntPtr desktopSurface, int edge);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_set_wm_geometry(IntPtr desktopSurface, IntPtr geometry);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_add_event_listener(IntPtr desktopSurface, int eventType,
        EventListener listener);

    //===================
    // Surface
    //===================
    [DllImport(Libswingby)]
    internal static extern IntPtr sb_surface_root_view(IntPtr surface);

    [DllImport(Libswingby)]
    internal static extern IntPtr sb_surface_size(IntPtr surface);

    [DllImport(Libswingby)]
    internal static extern void sb_surface_set_size(IntPtr surface, IntPtr size);

    [DllImport(Libswingby)]
    internal static extern void sb_surface_add_event_listener(IntPtr surface, int eventType,
        EventListener listener);

    //===================
    // View
    //===================
    [DllImport(Libswingby)]
    internal static extern IntPtr sb_view_new(IntPtr parent, IntPtr geometry);

    [DllImport(Libswingby)]
    internal static extern void sb_view_set_surface(IntPtr view, IntPtr surface);

    [DllImport(Libswingby)]
    internal static extern IntPtr sb_view_color(IntPtr view);

    [DllImport(Libswingby)]
    internal static extern void sb_view_set_color(IntPtr view, IntPtr color);

    [DllImport(Libswingby)]
    internal static extern void sb_view_set_geometry(IntPtr view, IntPtr geometry);

    [DllImport(Libswingby)]
    internal static extern void sb_view_add_event_listener(IntPtr view, int eventType, EventListener listener);

    //==================
    // Event
    //==================
    [DllImport(Libswingby)]
    internal static extern IntPtr sb_event_new(int targetType, IntPtr target, int type);

    [DllImport(Libswingby)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static extern bool sb_event_propagation(IntPtr sbEvent);

    [DllImport(Libswingby)]
    internal static extern void sb_event_set_propagation(IntPtr sbEvent, [MarshalAs(UnmanagedType.I1)]bool value);

    [DllImport(Libswingby)]
    internal static extern IntPtr sb_event_resize_old_size(IntPtr sbEvent);

    [DllImport(Libswingby)]
    internal static extern IntPtr sb_event_resize_size(IntPtr sbEvent);

    //==================
    // Structs
    //==================
    [DllImport(Libswingby)]
    internal static extern float sb_size_width(IntPtr sbSize);

    [DllImport(Libswingby)]
    internal static extern float sb_size_height(IntPtr sbSize);
}
