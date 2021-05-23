using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Zen.UIControls
{
    #region abstract base
    public abstract class LineCtrlbase : Control
    {
        protected const int DoubleLineSpacing = 4;

        public LineCtrlbase()
        {
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw, true);

            Paint += new PaintEventHandler(OnPaint);
            SizeChanged += new System.EventHandler(this.OnSizeChanged);
            BackColor = Color.Transparent;

            Margin = new Padding(0);
            Padding = new Padding(0);
        }

        public int LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }
        public Color Color
        {
            // Default color: Color.DarkGray or ForeColor?
            get { return (_color.IsEmpty) ? Color.DarkGray : _color; }
            set { _color = value; }
        }
        public DashStyle DashStyle
        {
            get { return _dashStyle; }
            set { _dashStyle = value; }
        }
        public bool DoubleLine
        {
            get { return _doubleLine; }
            set { _doubleLine = value; }
        }

        /// <summary>
        /// Disabled properties
        /// </summary>
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = string.Empty; }
        }

        #region protected functions
        protected Pen BuildPen()
        {
            Pen pen = null;
            pen = new Pen(this.Color, _lineWidth);
            pen.DashStyle = _dashStyle;

            return pen;
        }
        protected abstract void OnPaint(object sender, PaintEventArgs e);
        protected abstract void OnSizeChanged(object sender, EventArgs e);
        #endregion

        #region protected data
        protected int _lineWidth = 1;
        protected Color _color = new Color();
        protected DashStyle _dashStyle = DashStyle.Solid;
        protected bool _doubleLine = false;
        #endregion
    }
    #endregion

    public sealed class HorizontalLineCtrl : LineCtrlbase
    {
        static readonly Size DefaultCtrlSize = new Size(20, 10);

        public HorizontalLineCtrl()
        {
            base.Size = new Size(DefaultCtrlSize.Width, DefaultCtrlSize.Height);
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            using (Pen pen = BuildPen())
            {
                graphics.DrawLine(pen, 0, 0, Width, 0);
                graphics.DrawLine(Pens.White, 0, 1, Width, 1);

                if (_doubleLine)
                {
                    graphics.DrawLine(pen, 0, DoubleLineSpacing, Width, DoubleLineSpacing);
                    graphics.DrawLine(Pens.White, 0, DoubleLineSpacing + 1, Width, DoubleLineSpacing + 1);
                }
            }
        }
        protected override void OnSizeChanged(object sender, EventArgs e)
        {
            base.Size = new Size(base.Size.Width, DefaultCtrlSize.Height);
        }
    }

    public sealed class VerticalLineCtrl : LineCtrlbase
    {
        static readonly Size DefaultCtrlSize = new Size(10, 20);

        public VerticalLineCtrl()
        {
            base.Size = DefaultCtrlSize;
        }

        protected override void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            using (Pen pen = BuildPen())
            {
                graphics.DrawLine(pen, 0, 0, 0, Height);
                graphics.DrawLine(Pens.White, 1, 0, 1, Height);

                if (_doubleLine)
                {
                    graphics.DrawLine(pen, DoubleLineSpacing, 0, DoubleLineSpacing, Height);
                    graphics.DrawLine(pen, DoubleLineSpacing + 1, 0, DoubleLineSpacing + 1, Height);
                }
            }
        }

        protected override void OnSizeChanged(object sender, EventArgs e)
        {
            base.Size = new Size(DefaultCtrlSize.Width, base.Size.Height);
        }
    }
}
