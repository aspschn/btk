namespace Btk.Gui;

public enum CursorShape
{
    None,
    /// Default arrow cursor.
    Default,
    /// Context menu arrow. Menu icon next to the arrow.
    ContextMenu,
    /// Question mark in a circle next to the arrow.
    Help,
    /// Finger pointing hand.
    Pointer,
    /// Arrow cursor with animated waiting indicator.
    Progress        = 5,
    /// Animated waiting indicator.
    Wait            = 6,
    /// Thick cross.
    Cell            = 7,
    /// Thin cross.
    Crosshair       = 8,
    /// I-Beam shape.
    Text            = 9,
    /// Vertical I-Beam shape.
    VerticalText   = 10,
    /// Curved arrow sign next to the arrow.
    Alias           = 11,
    /// Copy arrow. Plus sign next to the arrow.
    Copy            = 12,
    /// Top, bottom, left and right arrow or same as grabbing.
    Move            = 13,
    /// Arrow with crossed circle.
    NoDrop         = 14,
    /// Red circle with diagonal in the circle.
    NotAllowed     = 15,
    /// Open hand.
    Grab            = 16,
    /// Closed hand.
    Grabbing        = 17,
    /// Resize arrow pointing right.
    EResize        = 18,
    /// Risize arrow pointing top.
    NResize        = 19,
    /// Resize arrow pointing top right.
    NEResize       = 20,
    /// Resize arrow pointing top left.
    NWResize       = 21,
    /// Resize arrow pointing bottom.
    SResize        = 22,
    /// Resize arrow pointing bottom right.
    SEResize       = 23,
    /// Resize arrow pointing bottom left,
    SWResize       = 24,
    /// Resize arrow pointing left.
    WResize        = 25,
    /// Resize arrow pointing both left and right.
    EWResize       = 26,
    /// Resize arrow pointing both top and bottom.
    NSResize       = 27,
    /// Resize arrow pointing both top right and bottom left.
    NeswResize     = 28,
    /// Resize arrow pointing both top left and bottom right.
    NwseResize     = 29,
    /// Two arrows for left and right with a vertical bar.
    ColResize      = 30,
    /// Tow arrows for top and bottom with a horizontal bar.
    RowResize      = 31,
    /// Four arrows for top, left, right and bottom with a dot in center.
    AllScroll      = 32,
    /// Magnifier with plus sign.
    ZoomIn         = 33,
    /// Magnifier with minus sign.
    ZoomOut        = 34,
}
