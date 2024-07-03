using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ScreenRecorder
{
    public partial class OverlayForm : Form
    {
        private bool _dragging = false;
        private Point _startPoint;
        private Rectangle _selectionRectangle;
        private Screen _currentScreen;
        private int _currentScreenIndex;

        public Rectangle SelectionRectangle => _selectionRectangle;
        public int SelectedMonitorIndex => _currentScreenIndex;

        public OverlayForm()
        {
            InitializeForm();
            ShowAcrossBothScreens();
        }

        private void InitializeForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.Opacity = 0.25;
            this.TopMost = true;
            this.ShowInTaskbar = false;

            this.MouseDown += OverlayForm_MouseDown!;
            this.MouseMove += OverlayForm_MouseMove!;
            this.MouseUp += OverlayForm_MouseUp!;
            this.Paint += OverlayForm_Paint!;
        }

        private void ShowAcrossBothScreens()
        {
            Screen[] screens = Screen.AllScreens;
            if (screens.Length > 1)
            {
                Screen primaryScreen = Screen.PrimaryScreen;
                Screen secondaryScreen = screens.First(screen => screen != primaryScreen);

                int width = primaryScreen.Bounds.Width + secondaryScreen.Bounds.Width;
                int height = Math.Max(primaryScreen.Bounds.Height, secondaryScreen.Bounds.Height);

                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(
                    Math.Min(primaryScreen.Bounds.Left, secondaryScreen.Bounds.Left),
                    Math.Min(primaryScreen.Bounds.Top, secondaryScreen.Bounds.Top));
                this.Size = new Size(width, height);
            }
            else
            {
                Screen primaryScreen = screens[0];
                this.StartPosition = FormStartPosition.Manual;
                this.Location = primaryScreen.Bounds.Location;
                this.Size = primaryScreen.Bounds.Size;
            }
        }

        private void OverlayForm_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _startPoint = new Point(e.X + this.Left, e.Y + this.Top);
            _currentScreen = Screen.FromPoint(_startPoint);
            _currentScreenIndex = Array.IndexOf(Screen.AllScreens, _currentScreen);

        }

        private void OverlayForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point currentPoint = new Point(e.X + this.Left, e.Y + this.Top);
                var screenBounds = _currentScreen.Bounds;

                int x1 = Math.Max(screenBounds.Left, Math.Min(_startPoint.X, currentPoint.X));
                int y1 = Math.Max(screenBounds.Top, Math.Min(_startPoint.Y, currentPoint.Y));
                int x2 = Math.Min(screenBounds.Right, Math.Max(_startPoint.X, currentPoint.X));
                int y2 = Math.Min(screenBounds.Bottom, Math.Max(_startPoint.Y, currentPoint.Y));

                _selectionRectangle = new Rectangle(
                    x1,
                    y1,
                    x2 - x1,
                    y2 - y1);

                if (currentPoint.X < screenBounds.Left || currentPoint.X > screenBounds.Right ||
                    currentPoint.Y < screenBounds.Top || currentPoint.Y > screenBounds.Bottom)
                {
                    this.Opacity = 0.5;
                }
                else
                {
                    this.Opacity = 0.25;
                }

                Invalidate();
            }
        }

        private void OverlayForm_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OverlayForm_Paint(object sender, PaintEventArgs e)
        {
            if (_selectionRectangle.Width > 0 && _selectionRectangle.Height > 0)
            {
                e.Graphics.DrawRectangle(Pens.Red, _selectionRectangle);
                Region region = new Region(new Rectangle(0, 0, this.Width, this.Height));
                region.Exclude(new Rectangle(_selectionRectangle.X - this.Left, _selectionRectangle.Y - this.Top, _selectionRectangle.Width, _selectionRectangle.Height));
                this.Region = region;
            }
            else
            {
                this.Region = new Region(new Rectangle(0, 0, this.Width, this.Height));
            }
        }
    }
}
