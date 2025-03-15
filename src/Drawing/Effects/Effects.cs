namespace Btk.Drawing.Effects;

using System.Runtime.InteropServices;
using Btk.Swingby;

public enum FilterType
{
    Blur,
    DropShadow,
}

public interface IFilter
{
    public FilterType Type { get; }

    internal IntPtr GetCPtr();
}

public class BlurFilter : IFilter
{
    private IntPtr _sbFilter;
    private float _radius;

    public BlurFilter(float radius)
    {
        _sbFilter = Swingby.sb_filter_new(Swingby.SB_FILTER_TYPE_BLUR);
        Radius = radius;
    }

    public FilterType Type => FilterType.Blur;

    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            Swingby.sb_filter_blur_set_radius(_sbFilter, value);
        }
    }

    IntPtr IFilter.GetCPtr()
    {
        return _sbFilter;
    }
}

public class DropShadowFilter : IFilter
{
    private IntPtr _sbFilter;
    private Point _offset = new Point(0.0f, 0.0f);
    private float _radius = 0.0f;
    private Color _color = new Color(0, 0, 0, 255);

    public DropShadowFilter()
    {
        _sbFilter = Swingby.sb_filter_new(Swingby.SB_FILTER_TYPE_DROP_SHADOW);
    }

    public FilterType Type => FilterType.DropShadow;

    public Point Offset
    {
        get => _offset;
        set
        {
            _offset = value;
            var sbPoint = new sb_point_t();
            sbPoint.x = _offset.X;
            sbPoint.y = _offset.Y;
            var sbRectPtr = sbPoint.AllocCPtr();
            Swingby.sb_filter_drop_shadow_set_offset(_sbFilter, sbRectPtr);
            Marshal.FreeHGlobal(sbRectPtr);
        }
    }

    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            Swingby.sb_filter_drop_shadow_set_radius(_sbFilter, _radius);
        }
    }

    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            var sbColor = sb_color_t.FromColor(_color);
            var sbColorPtr = sbColor.AllocCPtr();
            Swingby.sb_filter_drop_shadow_set_color(_sbFilter, sbColorPtr);
            Marshal.FreeHGlobal(sbColorPtr);
        }
    }

    IntPtr IFilter.GetCPtr()
    {
        return _sbFilter;
    }
}
