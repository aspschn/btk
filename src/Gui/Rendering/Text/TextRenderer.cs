using System.Runtime.InteropServices;
using Btk.Drawing;

namespace Btk.Gui.Rendering.Text;

using Btk.Drawing.Imaging;
using Btk.Pango;

public class TextRenderer : IDisposable
{
    private IntPtr _surface = IntPtr.Zero;
    private IntPtr _context = IntPtr.Zero;
    private IntPtr _layout = IntPtr.Zero;
    private int _width;
    private int _height;

    public void UpdateLayout()
    {
        _surface = Pango.cairo_image_surface_create(Pango.CAIRO_FORMAT_ARGB32, 1, 1);
        _context = Pango.cairo_create(_surface);
        _layout = Pango.pango_cairo_create_layout(_context);

        Pango.pango_layout_set_text(_layout, Text, -1);
        IntPtr desc = Pango.pango_font_description_from_string("Sans " + Size.ToString());
        Pango.pango_layout_set_font_description(_layout, desc);
        Pango.pango_font_description_free(desc);

        Pango.pango_layout_set_width(_layout, -1);

        Pango.pango_layout_get_size(_layout, out int pw, out int ph);
        _width = pw / Pango.PANGO_SCALE;
        _height = ph / Pango.PANGO_SCALE;

        DisposeResources();
    }

    public string Text { get; set; } = "";

    public float Size { get; set; } = 16.0f;

    public Image ToImage()
    {
        _surface = Pango.cairo_image_surface_create(Pango.CAIRO_FORMAT_ARGB32, _width, _height);
        _context = Pango.cairo_create(_surface);
        _layout = Pango.pango_cairo_create_layout(_context);

        Pango.pango_layout_set_text(_layout, Text, -1);
        IntPtr desc = Pango.pango_font_description_from_string("Sans " + Size.ToString());
        Pango.pango_layout_set_font_description(_layout, desc);
        Pango.pango_font_description_free(desc);

        Pango.pango_layout_set_width(_layout, _width * Pango.PANGO_SCALE);
        Pango.pango_layout_set_wrap(_layout, Pango.PANGO_WRAP_CHAR);
        Pango.pango_layout_set_ellipsize(_layout, 2);

        Image image = new Image(new SizeI(10, 10), ImageFormat.Rgba32);

        Pango.pango_cairo_show_layout(_context, _layout);

        IntPtr ptr = Pango.cairo_image_surface_get_data(_surface);
        int stride = Pango.cairo_image_surface_get_stride(_surface);

        int length = stride * _height;
        byte[] buffer = new byte[length];

        Marshal.Copy(ptr, buffer, 0, length);

        SizeI size = new SizeI((UInt64)_width, (UInt64)_height);

        image.SetData(buffer, size);

        return image;
    }

    public void Dispose()
    {
        DisposeResources();
    }

    private void DisposeResources()
    {
        if (_layout != IntPtr.Zero)
        {
            Pango.g_object_unref(_layout);
            _layout = IntPtr.Zero;
        }

        if (_context != IntPtr.Zero)
        {
            Pango.cairo_destroy(_context);
            _context = IntPtr.Zero;
        }

        if (_surface != IntPtr.Zero)
        {
            Pango.cairo_surface_destroy(_surface);
            _surface = IntPtr.Zero;
        }
    }
}
