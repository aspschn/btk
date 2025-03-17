namespace Btk.Gui.Layouts;

using Btk.Events;

public enum Anchor
{
    Top,
    Bottom,
    Left,
    Right,
    HorizontalCenter,
    VerticalCenter,
}

public class AnchorLine
{
    public AnchorLine(View view, Anchor anchor)
    {
        View = view;
        Anchor = anchor;
    }

    internal void Subscribe(View view)
    {
        SubscribedViews.Add(view);
    }

    internal void Unsubscribe(View view)
    {
        SubscribedViews.Remove(view);
    }

    public View View { get; private set; }

    public Anchor Anchor { get; private set; }

    internal List<View> SubscribedViews { get; } = [];
}

public class AnchorLayout
{
    private AnchorLine? _topTarget = null;
    private AnchorLine? _bottomTarget = null;

    public AnchorLayout(View view)
    {
        View = view;
    }

    public View View { get; private set; }

    public AnchorLine? Top
    {
        set
        {
            if (value != null)
            {
                _topTarget = value;
                value.Subscribe(View);
            }
            else
            {
                if (_topTarget == null)
                {
                    return;
                }
                _topTarget.Unsubscribe(View);
                _topTarget = null;
            }
        }
    }

    public AnchorLine? Bottom
    {
        set
        {
            if (value != null)
            {
                Console.WriteLine("Bottom set. value: " + value.ToString());
                _bottomTarget = value;
                value.Subscribe(View);
            }
            else
            {
                if (_bottomTarget == null)
                {
                    return;
                }
                _bottomTarget.Unsubscribe(View);
                _bottomTarget = null;
            }
        }
    }
}
