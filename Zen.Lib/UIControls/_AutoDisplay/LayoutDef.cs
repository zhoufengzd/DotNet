using System.Windows.Forms;

namespace Zen.UIControls.Layout
{
    public sealed class UIConst
    {
        public static readonly AnchorStyles AutoSize = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    }

    public sealed class UIDefault
    {
        public static readonly int Margin = 3;
        public static readonly int RowHeight = 20;
        public static readonly Padding Padding = new Padding(Margin);
    }
}
