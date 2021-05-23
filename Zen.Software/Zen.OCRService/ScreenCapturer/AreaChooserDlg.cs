using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ModiLib;

namespace ScreenCapturer
{
    /// <summary>
    /// Opaque form to help use decides capture size.
    /// </summary>
    public partial class AreaChooserDlg : Form
    {
        public AreaChooserDlg()
        {
            InitializeComponent();

            _graphics = this.CreateGraphics();
            this.MouseDown += new MouseEventHandler(mouse_Click);
            this.MouseUp += new MouseEventHandler(mouse_Up);
            this.MouseMove += new MouseEventHandler(mouse_Move);
            _drawPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        }

        public Point TopLeft
        {
            get { return _topLeft; }
        }
        public Point BottomRight
        {
            get { return _bottomRight; }
        }

        #region private functions
        private void mouse_Click(object sender, MouseEventArgs e)
        {
            _graphics.Clear(Color.FromArgb(255, 255, 192));
            _leftButtonDown = true;
            _clickPoint = new Point(Control.MousePosition.X, Control.MousePosition.Y);
        }

        private void mouse_Up(object sender, MouseEventArgs e)
        {
            _leftButtonDown = false;
            this.Hide();
        }

        private void mouse_Move(object sender, MouseEventArgs e)
        {
            //Resize (actually delete then re-draw) the rectangle if the left mouse button is held down
            if (!_leftButtonDown)
                return;

            //Erase the previous rectangle
            _graphics.DrawRectangle(_eraserPen, _topLeft.X, _topLeft.Y, _bottomRight.X - _topLeft.X, _bottomRight.Y - _topLeft.Y);

            //Calculate X Coordinates
            if (Cursor.Position.X < _clickPoint.X)
            {
                _topLeft.X = Cursor.Position.X;
                _bottomRight.X = _clickPoint.X;
            }
            else
            {
                _topLeft.X = _clickPoint.X;
                _bottomRight.X = Cursor.Position.X;
            }

            //Calculate Y Coordinates
            if (Cursor.Position.Y < _clickPoint.Y)
            {
                _topLeft.Y = Cursor.Position.Y;
                _bottomRight.Y = _clickPoint.Y;
            }
            else
            {
                _topLeft.Y = _clickPoint.Y;
                _bottomRight.Y = Cursor.Position.Y;
            }

            //Draw a new rectangle
            _graphics.DrawRectangle(_drawPen, _topLeft.X, _topLeft.Y, _bottomRight.X - _topLeft.X, _bottomRight.Y - _topLeft.Y);

        }
        #endregion

        #region private data
        private bool _leftButtonDown = false;
        private Point _clickPoint = new Point();
        private Point _topLeft = new Point();
        private Point _bottomRight = new Point();

        private Graphics _graphics;
        private Pen _drawPen = new Pen(Color.Blue, 1);
        private Pen _eraserPen = new Pen(Color.FromArgb(255, 255, 192), 1);
        #endregion
    }
}