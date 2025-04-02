namespace Btk.Pango;

using System.Runtime.InteropServices;

internal class Pango
{
    internal const int PANGO_SCALE = 1024;
    internal const int CAIRO_FORMAT_ARGB32 = 0;
    internal const int PANGO_WRAP_CHAR = 1;
    internal const int PANGO_WRAP_WORD_CHAR = 3;

    private const string Libpango = "libpango-1.0.so";
    private const string Libpangocairo = "libpangocairo-1.0.so";
    private const string Libcairo = "libcairo.so";
    private const string Libgobject = "libgobject-2.0.so";

    [DllImport(Libpangocairo)]
    internal static extern IntPtr pango_cairo_create_layout(IntPtr cr);

    [DllImport(Libpango)]
    internal static extern void pango_layout_set_text(IntPtr layout, string text, int length);

    [DllImport(Libpango)]
    internal static extern IntPtr pango_font_description_from_string(string desc);

    [DllImport(Libpango)]
    internal static extern void pango_font_description_set_absolute_size(IntPtr desc, double size);

    [DllImport(Libpango)]
    internal static extern void pango_layout_set_font_description(IntPtr layout, IntPtr desc);

    [DllImport(Libpango)]
    internal static extern void pango_font_description_free(IntPtr desc);

    [DllImport(Libpango)]
    internal static extern void pango_layout_set_width(IntPtr layout, int width);

    [DllImport(Libpango)]
    internal static extern void pango_layout_set_wrap(IntPtr layout, int wrapMode);

    [DllImport(Libpango)]
    internal static extern void pango_layout_set_ellipsize(IntPtr layout, int ellipsize);

    [DllImport(Libpango)]
    internal static extern void pango_layout_get_size(IntPtr layout, out int width, out int height);

    [DllImport(Libpangocairo)]
    internal static extern void pango_cairo_show_layout(IntPtr cr, IntPtr layout);

    [DllImport(Libcairo)]
    internal static extern IntPtr cairo_image_surface_create(int format, int width, int height);

    [DllImport(Libcairo)]
    internal static extern IntPtr cairo_create(IntPtr surface);

    [DllImport(Libcairo)]
    internal static extern IntPtr cairo_image_surface_get_data(IntPtr surface);

    [DllImport(Libcairo)]
    internal static extern int cairo_image_surface_get_stride(IntPtr surface);

    [DllImport(Libcairo)]
    internal static extern void cairo_destroy(IntPtr cr);

    [DllImport(Libcairo)]
    internal static extern void cairo_surface_destroy(IntPtr surface);

    [DllImport(Libgobject)]
    internal static extern void g_object_unref(IntPtr obj);
}
