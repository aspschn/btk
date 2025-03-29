namespace Btk.Swingby;

using System;
using System.Runtime.InteropServices;

using Btk.Drawing;
using Btk.Gui;

internal struct sb_point_t
{
    public float x;
    public float y;

    internal IntPtr AllocCPtr()
    {
        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sb_point_t)));
        Marshal.StructureToPtr(this, ptr, false);

        return ptr;
    }
}

internal struct sb_point_i_t
{
    public Int64 x;
    public Int64 y;

    internal IntPtr AllocCPtr()
    {
        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sb_point_i_t)));
        Marshal.StructureToPtr(this, ptr, false);

        return ptr;
    }
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

internal struct sb_size_i_t
{
    public UInt64 width;
    public UInt64 height;

    internal IntPtr AllocCPtr()
    {
        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sb_size_i_t)));
        Marshal.StructureToPtr(this, ptr, false);

        return ptr;
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

internal struct sb_view_radius_t
{
    public float top_left;
    public float top_right;
    public float bottom_right;
    public float bottom_left;

    internal static sb_view_radius_t FromViewRadius(ViewRadius viewRadius)
    {
        sb_view_radius_t ret;
        ret.top_left = viewRadius.TopLeft;
        ret.top_right = viewRadius.TopRight;
        ret.bottom_right = viewRadius.BottomRight;
        ret.bottom_left = viewRadius.BottomLeft;

        return ret;
    }

    internal IntPtr AllocCPtr()
    {
        IntPtr cPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(sb_view_radius_t)));
        Marshal.StructureToPtr(this, cPtr, false);

        return cPtr;
    }
}

internal class Swingby
{
#if BTK_LIBSWINGBY_DEV
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
    internal const int SB_EVENT_TYPE_PREFERRED_SCALE = 130;

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
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_STATE_RESIZING = 4;
    internal const int SB_DESKTOP_SURFACE_TOPLEVEL_STATE_ACTIVATED = 8;

    // enum sb_view_fill_type
    internal const int SB_VIEW_FILL_TYPE_SINGLE_COLOR = 0;
    internal const int SB_VIEW_FILL_TYPE_IMAGE = 1;

    // enum sb_cursor_shape
    internal const int SB_CURSOR_SHAPE_NONE = 0;
    internal const int SB_CURSOR_SHAPE_DEFAULT = 1;
    internal const int SB_CURSOR_SHAPE_CONTEXT_MENU = 2;
    internal const int SB_CURSOR_SHAPE_HELP = 3;
    internal const int SB_CURSOR_SHAPE_POINTER = 4;
    internal const int SB_CURSOR_SHAPE_PROGRESS = 5;
    internal const int SB_CURSOR_SHAPE_WAIT = 6;
    internal const int SB_CURSOR_SHAPE_CELL = 7;
    internal const int SB_CURSOR_SHAPE_CROSSHAIR = 8;
    internal const int SB_CURSOR_SHAPE_TEXT = 9;
    internal const int SB_CURSOR_SHAPE_VERTICAL_TEXT = 10;
    internal const int SB_CURSOR_SHAPE_ALIAS = 11;
    internal const int SB_CURSOR_SHAPE_COPY = 12;
    internal const int SB_CURSOR_SHAPE_MOVE = 13;
    internal const int SB_CURSOR_SHAPE_NO_DROP = 14;
    internal const int SB_CURSOR_SHAPE_NOT_ALLOWED = 15;
    internal const int SB_CURSOR_SHAPE_GRAB = 16;
    internal const int SB_CURSOR_SHAPE_GRABBING = 17;
    internal const int SB_CURSOR_SHAPE_E_RESIZE = 18;
    internal const int SB_CURSOR_SHAPE_N_RESIZE = 19;
    internal const int SB_CURSOR_SHAPE_NE_RESIZE = 20;
    internal const int SB_CURSOR_SHAPE_NW_RESIZE = 21;
    internal const int SB_CURSOR_SHAPE_S_RESIZE = 22;
    internal const int SB_CURSOR_SHAPE_SE_RESIZE = 23;
    internal const int SB_CURSOR_SHAPE_SW_RESIZE = 24;
    internal const int SB_CURSOR_SHAPE_W_RESIZE = 25;
    internal const int SB_CURSOR_SHAPE_EW_RESIZE = 26;
    internal const int SB_CURSOR_SHAPE_NS_RESIZE = 27;
    internal const int SB_CURSOR_SHAPE_NESW_RESIZE = 28;
    internal const int SB_CURSOR_SHAPE_NWSE_RESIZE = 29;
    internal const int SB_CURSOR_SHAPE_COL_RESIZE = 30;
    internal const int SB_CURSOR_SHAPE_ROW_RESIZE = 31;
    internal const int SB_CURSOR_SHAPE_ALL_SCROLL = 32;
    internal const int SB_CURSOR_SHAPE_ZOOM_IN = 33;
    internal const int SB_CURSOR_SHAPE_ZOOM_OUT = 34;

    internal static int FromCursorShape(CursorShape shape)
    {
        return shape switch
        {
            CursorShape.Default => SB_CURSOR_SHAPE_DEFAULT,
            CursorShape.ContextMenu => SB_CURSOR_SHAPE_CONTEXT_MENU,
            CursorShape.Help => SB_CURSOR_SHAPE_HELP,
            CursorShape.Pointer => SB_CURSOR_SHAPE_POINTER,
            CursorShape.Progress => SB_CURSOR_SHAPE_PROGRESS,
            CursorShape.Wait => SB_CURSOR_SHAPE_WAIT,
            CursorShape.Cell => SB_CURSOR_SHAPE_CELL,
            CursorShape.Crosshair => SB_CURSOR_SHAPE_CROSSHAIR,
            CursorShape.Text => SB_CURSOR_SHAPE_TEXT,
            CursorShape.VerticalText => SB_CURSOR_SHAPE_VERTICAL_TEXT,
            CursorShape.Alias => SB_CURSOR_SHAPE_ALIAS,
            CursorShape.Copy => SB_CURSOR_SHAPE_COPY,
            CursorShape.Move => SB_CURSOR_SHAPE_MOVE,
            CursorShape.NoDrop => SB_CURSOR_SHAPE_NO_DROP,
            CursorShape.NotAllowed => SB_CURSOR_SHAPE_NOT_ALLOWED,
            CursorShape.Grab => SB_CURSOR_SHAPE_GRAB,
            CursorShape.Grabbing => SB_CURSOR_SHAPE_GRABBING,
            CursorShape.EResize => SB_CURSOR_SHAPE_E_RESIZE,
            CursorShape.NResize => SB_CURSOR_SHAPE_N_RESIZE,
            CursorShape.NWResize => SB_CURSOR_SHAPE_NW_RESIZE,
            CursorShape.NEResize => SB_CURSOR_SHAPE_NE_RESIZE,
            CursorShape.SResize => SB_CURSOR_SHAPE_S_RESIZE,
            CursorShape.SEResize => SB_CURSOR_SHAPE_SE_RESIZE,
            CursorShape.SWResize => SB_CURSOR_SHAPE_SW_RESIZE,
            CursorShape.WResize => SB_CURSOR_SHAPE_W_RESIZE,
            CursorShape.EWResize => SB_CURSOR_SHAPE_EW_RESIZE,
            CursorShape.NSResize => SB_CURSOR_SHAPE_NS_RESIZE,
            CursorShape.NeswResize => SB_CURSOR_SHAPE_NESW_RESIZE,
            CursorShape.NwseResize => SB_CURSOR_SHAPE_NWSE_RESIZE,
            CursorShape.ColResize => SB_CURSOR_SHAPE_COL_RESIZE,
            CursorShape.RowResize => SB_CURSOR_SHAPE_ROW_RESIZE,
            CursorShape.AllScroll => SB_CURSOR_SHAPE_ALL_SCROLL,
            CursorShape.ZoomIn => SB_CURSOR_SHAPE_ZOOM_IN,
            CursorShape.ZoomOut => SB_CURSOR_SHAPE_ZOOM_OUT,
            _ => SB_CURSOR_SHAPE_NONE
        };
    }

    internal static CursorShape ToCursorShape(int sbCursorShape)
    {
        return sbCursorShape switch
        {
            SB_CURSOR_SHAPE_DEFAULT => CursorShape.Default,
            SB_CURSOR_SHAPE_CONTEXT_MENU => CursorShape.ContextMenu,
            SB_CURSOR_SHAPE_HELP => CursorShape.Help,
            SB_CURSOR_SHAPE_POINTER => CursorShape.Pointer,
            SB_CURSOR_SHAPE_PROGRESS => CursorShape.Progress,
            SB_CURSOR_SHAPE_WAIT => CursorShape.Wait,
            SB_CURSOR_SHAPE_CELL => CursorShape.Cell,
            SB_CURSOR_SHAPE_CROSSHAIR => CursorShape.Crosshair,
            SB_CURSOR_SHAPE_TEXT => CursorShape.Text,
            SB_CURSOR_SHAPE_VERTICAL_TEXT => CursorShape.VerticalText,
            SB_CURSOR_SHAPE_ALIAS => CursorShape.Alias,
            SB_CURSOR_SHAPE_COPY => CursorShape.Copy,
            SB_CURSOR_SHAPE_MOVE => CursorShape.Move,
            SB_CURSOR_SHAPE_NO_DROP => CursorShape.NoDrop,
            SB_CURSOR_SHAPE_NOT_ALLOWED => CursorShape.NotAllowed,
            SB_CURSOR_SHAPE_GRAB => CursorShape.Grab,
            SB_CURSOR_SHAPE_GRABBING => CursorShape.Grabbing,
            SB_CURSOR_SHAPE_E_RESIZE => CursorShape.EResize,
            SB_CURSOR_SHAPE_N_RESIZE => CursorShape.NResize,
            SB_CURSOR_SHAPE_NE_RESIZE => CursorShape.NEResize,
            SB_CURSOR_SHAPE_NW_RESIZE => CursorShape.NWResize,
            SB_CURSOR_SHAPE_S_RESIZE => CursorShape.SResize,
            SB_CURSOR_SHAPE_SE_RESIZE => CursorShape.SEResize,
            SB_CURSOR_SHAPE_SW_RESIZE => CursorShape.SWResize,
            SB_CURSOR_SHAPE_W_RESIZE => CursorShape.WResize,
            SB_CURSOR_SHAPE_EW_RESIZE => CursorShape.EWResize,
            SB_CURSOR_SHAPE_NS_RESIZE => CursorShape.NSResize,
            SB_CURSOR_SHAPE_NESW_RESIZE => CursorShape.NeswResize,
            SB_CURSOR_SHAPE_NWSE_RESIZE => CursorShape.NwseResize,
            SB_CURSOR_SHAPE_COL_RESIZE => CursorShape.ColResize,
            SB_CURSOR_SHAPE_ROW_RESIZE => CursorShape.RowResize,
            SB_CURSOR_SHAPE_ALL_SCROLL => CursorShape.AllScroll,
            SB_CURSOR_SHAPE_ZOOM_IN => CursorShape.ZoomIn,
            SB_CURSOR_SHAPE_ZOOM_OUT => CursorShape.ZoomOut,
            _ => CursorShape.None
        };
    }

    // enum sb_filter_type
    internal static int SB_FILTER_TYPE_BLUR = 0;
    internal static int SB_FILTER_TYPE_DROP_SHADOW = 1;

    // enum sb_image_format
    internal static int SB_IMAGE_FORMAT_RGBA32 = 0;
    internal static int SB_IMAGE_FORMAT_ARGB32 = 1;

    // enum sb_image_file_format
    internal static int SB_IMAGE_FILE_FORMAT_PNG = 1;
    internal static int SB_IMAGE_FILE_FORMAT_JPEG = 2;
    internal static int SB_IMAGE_FILE_FORMAT_AUTO = 255;

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
    internal static extern int sb_desktop_surface_set_parent(IntPtr sbDesktopSurface, IntPtr parent);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_show(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_hide(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_toplevel_close(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_toplevel_set_minimized(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_toplevel_move(IntPtr desktopSurface);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_toplevel_resize(IntPtr desktopSurface, int edge);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_set_wm_geometry(IntPtr desktopSurface, IntPtr geometry);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_popup_set_position(IntPtr desktopSurface, IntPtr position);

    [DllImport(Libswingby)]
    internal static extern void sb_desktop_surface_free(IntPtr desktopSurface);

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
    internal static extern void sb_surface_set_input_geometry(IntPtr surface, IntPtr geometry);

    [DllImport(Libswingby)]
    internal static extern UInt32 sb_surface_scale(IntPtr surface);

    [DllImport(Libswingby)]
    internal static extern void sb_surface_set_scale(IntPtr surface, UInt32 scale);

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
    internal static extern void sb_view_set_image(IntPtr view, IntPtr image);

    [DllImport(Libswingby)]
    internal static extern void sb_view_set_fill_type(IntPtr view, int fillType);

    [DllImport(Libswingby)]
    internal static extern void sb_view_set_geometry(IntPtr view, IntPtr geometry);

    [DllImport(Libswingby)]
    internal static extern void sb_view_set_radius(IntPtr view, IntPtr radius);

    [DllImport(Libswingby)]
    internal static extern int sb_view_cursor_shape(IntPtr view);

    [DllImport(Libswingby)]
    internal static extern void sb_view_set_cursor_shape(IntPtr view, int shape);

    [DllImport(Libswingby)]
    internal static extern void sb_view_add_filter(IntPtr view, IntPtr filter);

    [DllImport(Libswingby)]
    internal static extern void sb_view_set_clip(IntPtr view, [MarshalAs(UnmanagedType.I1)]bool clip);

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

    [DllImport(Libswingby)]
    internal static extern IntPtr sb_event_move_old_position(IntPtr sbEvent);

    [DllImport(Libswingby)]
    internal static extern IntPtr sb_event_move_position(IntPtr sbEvent);

    [DllImport(Libswingby)]
    internal static extern int sb_event_state_change_state(IntPtr sbEvent);

    [DllImport(Libswingby)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static extern bool sb_event_state_change_value(IntPtr sbEvent);

    [DllImport(Libswingby)]
    internal static extern UInt32 sb_event_scale_scale(IntPtr sbEvent);

    //==================
    // Structs
    //==================
    [DllImport(Libswingby)]
    internal static extern float sb_size_width(IntPtr sbSize);

    [DllImport(Libswingby)]
    internal static extern float sb_size_height(IntPtr sbSize);

    [DllImport(Libswingby)]
    internal static extern float sb_point_x(IntPtr sbPoint);

    [DllImport(Libswingby)]
    internal static extern float sb_point_y(IntPtr sbPoint);

    //==================
    // Filter
    //==================
    [DllImport(Libswingby)]
    internal static extern IntPtr sb_filter_new(int filterType);

    [DllImport(Libswingby)]
    internal static extern void sb_filter_blur_set_radius(IntPtr sbFilter, float radius);

    [DllImport(Libswingby)]
    internal static extern void sb_filter_drop_shadow_set_offset(IntPtr sbFilter, IntPtr offset);

    [DllImport(Libswingby)]
    internal static extern void sb_filter_drop_shadow_set_radius(IntPtr sbFilter, float radius);

    [DllImport(Libswingby)]
    internal static extern void sb_filter_drop_shadow_set_color(IntPtr sbFilter, IntPtr color);

    //=================
    // Image
    //=================
    [DllImport(Libswingby)]
    internal static extern IntPtr sb_image_new(IntPtr sbSize, int format);

    [DllImport(Libswingby)]
    internal static extern IntPtr sb_image_size(IntPtr sbImage);

    [DllImport(Libswingby)]
    internal static extern void sb_image_set_data(IntPtr sbImage, IntPtr data, IntPtr size);

    [DllImport(Libswingby)]
    internal static extern void sb_image_fill(IntPtr sbImage, IntPtr sbColor);

    [DllImport(Libswingby)]
    internal static extern bool sb_image_load_from_data(IntPtr sbImage, IntPtr data, UInt64 dataLen, int format);
}
