using UnityEngine;

namespace ClockEngine
{
    public static class RectExtensions
    {
        public static Rect AlignRight(this Rect rect, float width)
        {
            var deltaWidth = rect.width - width;
            rect.width = width;
            rect.x += deltaWidth;
            return rect;
        }

        public static Rect SetWidth(this Rect rect, float width)
        {
            rect.width = width;
            return rect;
        }
    }
}