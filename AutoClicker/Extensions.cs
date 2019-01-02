using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoClicker
{
    public static class Extensions
    {
        public static IEnumerable<T> AllControls<T>(this Control startingPoint) where T : Control
        {
            var hit = startingPoint is T;

            if (hit)
                yield return (T) startingPoint;

            foreach (var child in startingPoint.Controls.Cast<Control>())
            {
                foreach (var item in AllControls<T>(child))
                {
                    yield return item;
                }
            }
        }
    }
}
