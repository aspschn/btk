namespace Btk.Drawing.Imaging;

using System.Runtime.InteropServices;
using Btk.Swingby;

public enum ImageFormat
{
    Rgba32,
    Argb32,
}

public class Image
{
    internal IntPtr _sbImage;

    public Image(SizeI size, ImageFormat format)
    {
        Size = size;
        Format = format;

        sb_size_i_t sbSizeI = new sb_size_i_t();
        sbSizeI.width = size.Width;
        sbSizeI.height = size.Height;
        IntPtr sbSizeIPtr = sbSizeI.AllocCPtr();

        int sbFormat = format switch
        {
            ImageFormat.Argb32 => Swingby.SB_IMAGE_FORMAT_ARGB32,
            ImageFormat.Rgba32 => Swingby.SB_IMAGE_FORMAT_RGBA32,
            _ => Swingby.SB_IMAGE_FORMAT_RGBA32
        };

        _sbImage = Swingby.sb_image_new(sbSizeIPtr, sbFormat);

        Marshal.FreeHGlobal(sbSizeIPtr);
    }

    public SizeI Size { get; private set; }

    public ImageFormat Format { get; private set; }

    public void Fill(Color color)
    {
        sb_color_t sbColor = sb_color_t.FromColor(color);
        IntPtr sbColorPtr = sbColor.AllocCPtr();

        Swingby.sb_image_fill(_sbImage, sbColorPtr);

        Marshal.FreeHGlobal(sbColorPtr);
    }
}
