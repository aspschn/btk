namespace Btk.Gui.Layouts;

using Btk.Drawing;
using Btk.Events;

public enum Anchor
{
    Top,
    Bottom,
    Left,
    Right,
    HorizontalCenter,
    VerticalCenter,
    Fill,
    CenterIn,
}

public class AnchorLine
{
    public AnchorLine(View view, Anchor anchor)
    {
        View = view;
        Anchor = anchor;
    }

    internal void OnAnchorMove(View view, MoveEvent evt)
    {
        foreach (AnchorLayout layout in Subscribed.Keys)
        {
            switch (Subscribed[layout])
            {
                case Anchor.Top:
                    layout.OnTopAnchorMove(Anchor);
                    break;
                case Anchor.Bottom:
                    layout.OnBottomAnchorMove(Anchor);
                    break;
            }
        }
    }

    internal void OnAnchorResize(View view, ResizeEvent evt)
    {
        foreach (AnchorLayout layout in Subscribed.Keys)
        {
            switch (Subscribed[layout])
            {
                case Anchor.Top:
                    layout.OnTopAnchorResize(Anchor);
                    break;
                case Anchor.Bottom:
                    layout.OnBottomAnchorResize(Anchor);
                    break;
            }
        }
    }

    public View View { get; private set; }

    public Anchor Anchor { get; private set; }

    internal Dictionary<AnchorLayout, Anchor> Subscribed { get; } = [];
}

public class AnchorLayout
{
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
                TopAnchorLine = value;
                value.Subscribed.Add(this, Anchor.Top);
            }
            else
            {
                if (TopAnchorLine == null)
                {
                    return;
                }
                TopAnchorLine.Subscribed.Remove(this);
                TopAnchorLine = null;
            }
        }
    }

    public AnchorLine? Bottom
    {
        set
        {
            if (value != null)
            {
                BottomAnchorLine = value;
                Console.WriteLine("Anchor.Bottom added. " + this.GetHashCode());
                Console.WriteLine(" |- Target: " + value.GetHashCode());
                value.Subscribed.Add(this, Anchor.Bottom);
            }
            else
            {
                if (BottomAnchorLine == null)
                {
                    return;
                }
                BottomAnchorLine.Subscribed.Remove(this);
                BottomAnchorLine = null;
            }
        }
    }

    public View? Fill
    {
        set
        {
            if (value != null)
            {
                FillAnchorView = value;
                value.FillViews.Add(View);
            }
            else
            {
                if (FillAnchorView == null)
                {
                    return;
                }
                FillAnchorView.FillViews.Remove(View);
                FillAnchorView = null;
            }
        }
    }

    internal AnchorLine? TopAnchorLine { get; set; } = null;
    internal AnchorLine? BottomAnchorLine { get; set; } = null;
    internal View? LeftAnchorView { get; set; } = null;
    internal View? RightAnchorView { get; set; } = null;
    internal View? FillAnchorView { get; set; } = null;

    public float TopMargin { get; set; } = 0.0f;
    public float BottomMargin { get; set; } = 0.0f;
    public float LeftMargin { get; set; } = 0.0f;
    public float RightMargin { get; set; } = 0.0f;

    //=====================
    // Anchor View Moved
    //=====================

    /// <summary>
    /// Top anchor view moved.
    /// </summary>
    internal void OnTopAnchorMove(Anchor destAnchor)
    {
        if (TopAnchorLine == null)
        {
            return;
        }
        Rect anchorGeo = TopAnchorLine.View.Geometry;
        if (destAnchor == Anchor.Top)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y + TopMargin,
                View.Geometry.Width,
                View.Geometry.Height);
        } else if (destAnchor == Anchor.Bottom)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y + anchorGeo.Height + TopMargin,
                View.Geometry.Width,
                View.Geometry.Height);
        }
    }

    /// <summary>
    /// Bottom anchor view moved.
    /// </summary>
    internal void OnBottomAnchorMove(Anchor destAnchor)
    {
        if (BottomAnchorLine == null)
        {
            return;
        }
        Rect anchorGeo = BottomAnchorLine.View.Geometry;
        if (destAnchor == Anchor.Top)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y - View.Geometry.Height + BottomMargin,
                View.Geometry.Width,
                View.Geometry.Height);
        } else if (destAnchor == Anchor.Bottom)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y + anchorGeo.Height - View.Geometry.Height - BottomMargin,
                View.Geometry.Width,
                View.Geometry.Height);
        }
    }

    //=======================
    // Anchor View Resized
    //=======================
    internal void OnTopAnchorResize(Anchor destAnchor)
    {
        if (TopAnchorLine == null)
        {
            return;
        }
        Rect anchorGeo = TopAnchorLine.View.Geometry;
        if (destAnchor == Anchor.Bottom)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Height,
                View.Geometry.Width,
                View.Geometry.Height);
        } else if (destAnchor == Anchor.Top)
        {
            // TODO.
        }
    }

    internal void OnBottomAnchorResize(Anchor destAnchor)
    {
        if (BottomAnchorLine == null)
        {
            return;
        }
        Rect anchorGeo = BottomAnchorLine.View.Geometry;
        if (destAnchor == Anchor.Bottom)
        {
            View.Geometry = new Rect(View.Geometry.X,
                anchorGeo.Y + anchorGeo.Height - View.Geometry.Height - BottomMargin,
                View.Geometry.Width,
                View.Geometry.Height);
        } else if (destAnchor == Anchor.Top)
        {
            //
        }
    }
}
