namespace Btk.Gui.Widgets;

using Btk.Drawing;
using Btk.Drawing.Imaging;
using Btk.Gui.Rendering.Text;

public class Label : Widget
{
    private string _text = "";
    private Image _textImage;

    public Label(string text, Widget parent) : base(parent)
    {
        _textImage = new Image(new SizeI(1, 1), ImageFormat.Rgba32);
        Text = text;
        base.FillType = ViewFillType.Image;
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            UpdateTextImage();
        }
    }

    private void UpdateTextImage()
    {
        var scale = base.Surface.Scale;
        using var tr = new TextRenderer();
        float fontSize = 12.0f * scale;
        tr.Size = fontSize;
        tr.Text = Text;
        tr.UpdateLayout();
        _textImage = tr.ToImage();
        base.Image = _textImage;

        base.Geometry = new Rect(base.Geometry.X, base.Geometry.Y,
            _textImage.Size.Width / (float)scale,
            _textImage.Size.Height / (float)scale);
    }
}
