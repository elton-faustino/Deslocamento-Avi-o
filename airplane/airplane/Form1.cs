using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace airplane
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Bitmap _bitmap = new Bitmap(Properties.Resources.airplane_icon);
        private float _angulo = 0;
        private Timer _timer = new Timer();
        private PointF _pontoSC = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, 550);
        private const float _deslocamento = 1f;

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            _timer.Interval = 10;
            _timer.Tick += (ProcessarEventosTimer);
            _timer.Start();
        }

        private void ProcessarEventosTimer(object sender, EventArgs e)
        {
            var novoPonto = ObterSenoConseno(_angulo);

            _pontoSC.X = _pontoSC.X + novoPonto.X;
            _pontoSC.Y = _pontoSC.Y + novoPonto.Y;

            Refresh();
        }

        private Bitmap RotateImage(Bitmap bmp, float angle)
        {
            var rotatedImage = new Bitmap(bmp.Width, bmp.Height);

            using (var g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);

                g.RotateTransform(angle);

                g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);

                g.DrawImage(bmp, new PointF(0, 0));
            }

            return rotatedImage;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            RotacionarAviao(e, ref _angulo);
        }

        private void RotacionarAviao(KeyEventArgs e, ref float angulo)
        {

            ValidarAngulo(ref angulo);

            ObterSenoConseno(angulo);

            if (e.KeyCode == Keys.Right)
                angulo += _deslocamento;

            if (e.KeyCode == Keys.Left)
                angulo -= _deslocamento;

            Refresh();
        }

        private PointF ObterSenoConseno(float angulo)
        {
            var seno = Math.Sin((Math.PI / 180) * angulo);
            var coseno = Math.Cos((Math.PI / 180) * angulo);

            lblAngulo.Text = angulo.ToString();
            lblSeno.Text = seno.ToString();
            lblCoseno.Text = coseno.ToString();

            return new PointF((float)coseno, (float)seno);
        }

        private void ValidarAngulo(ref float angulo)
        {
            if (angulo < 0)
                angulo = 360;

            if (angulo > 360)
                angulo = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            g.DrawImage(RotateImage(_bitmap, _angulo)
                , _pontoSC);

            g.DrawLine(new Pen(Color.Red), new Point(Screen.PrimaryScreen.WorkingArea.Width / 2,
                Screen.PrimaryScreen.WorkingArea.Height / 2),  new PointF(_pontoSC.X + 90, _pontoSC.Y + 95));

            g.DrawEllipse(new Pen(Color.Black), Screen.PrimaryScreen.WorkingArea.Width / 2 - 50,
                Screen.PrimaryScreen.WorkingArea.Height / 2 - 50, 100, 100);
        }
    }
}
