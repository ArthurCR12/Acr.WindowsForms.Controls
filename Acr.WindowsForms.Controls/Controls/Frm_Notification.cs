using Acr.WindowsForms.Controls.Class;
using Acr.WindowsForms.Controls.Enums;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace Acr.WindowsForms.Controls.Controls
{
    public partial class Frm_Notification : Form
    {
        private NotificationAction _action;
        private int x, y;

        private const int MaxNotifications = 10;
        private const int SlideSpeed = 3;
        private const double FadeStep = 0.1;
        private const int WaitTime = 5000;
        private const int TimerIntervalFast = 1;
        private const int CornerRadius = 10;
        private const int AccentBarWidth = 4;

        private Color _accentColor = AcrColors.Info;

        public Frm_Notification()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
            lbl_Message.Font = new Font("Segoe UI", 9.5F);
            lbl_Message.ForeColor = AcrColors.Text;
            btn_Close.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        }

        public void ShowNotification(string msg, NotificationType nType = NotificationType.Info)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;

            if (nType == NotificationType.Sucess)
            {
                picB_Icon.Image = Properties.Resources._80_OkGreen;
                _accentColor = AcrColors.Success;
            }
            else if (nType == NotificationType.Warning)
            {
                picB_Icon.Image = Properties.Resources._80_warning;
                _accentColor = AcrColors.Warning;
            }
            else if (nType == NotificationType.Error)
            {
                picB_Icon.Image = Properties.Resources._80_Error;
                _accentColor = AcrColors.Error;
            }
            else
            {
                picB_Icon.Image = Properties.Resources._80_info;
                _accentColor = AcrColors.Info;
            }

            Invalidate();


            for (int i = 0; i < MaxNotifications; i++)
            {
                string fName = "alert" + i;

                var existingForm = Application.OpenForms[fName];

                if (existingForm == null)
                {
                    this.Name = fName;
                    int screenWidth = Screen.PrimaryScreen!.WorkingArea.Width;
                    int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                    this.x = screenWidth - this.Width - 10; // Distancia da parte da esquerda
                    this.y = screenHeight - this.Height * (i + 1) - 5; // Distancia da parte inferior
                    this.Location = new Point(x, y);
                    break;
                }
            }
            this.lbl_Message.Text = msg;
            PlaySound(nType);
            this.Show();

            _action = NotificationAction.Start;
            timer1.Interval = TimerIntervalFast;
            timer1.Start();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            timer1.Interval = TimerIntervalFast;
            _action = NotificationAction.Close;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this._action)
            {
                case NotificationAction.Start:
                    timer1.Interval = TimerIntervalFast;
                    this.Opacity += FadeStep;

                    if (this.Left > x) this.Left += SlideSpeed;
                    else if (this.Opacity >= 1.0) _action = NotificationAction.Waiting;
                    break;

                case NotificationAction.Waiting:
                    timer1.Interval = WaitTime;
                    _action = NotificationAction.Close;
                    break;


                case NotificationAction.Close:
                    timer1.Interval = TimerIntervalFast;
                    this.Opacity -= FadeStep;
                    this.Left += SlideSpeed;
                    if (this.Opacity <= 0.0)
                    {
                        timer1.Stop();
                        this.Close();
                        this.Dispose();
                    }
                    break;
            }
        }

        private void PlaySound(NotificationType type)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                if (type == NotificationType.Error)
                {
                    using Stream? stream = assembly.GetManifestResourceStream("Acr.WindowsForms.Controls.Resources.error.wav");
                    if (stream != null)
                    {
                        using var soundPlayer = new System.Media.SoundPlayer(stream);
                        soundPlayer.Play();
                    }
                }
                else
                {
                    using Stream? stream = assembly.GetManifestResourceStream("Acr.WindowsForms.Controls.Resources.pop.wav");
                    if (stream != null)
                    {
                        using var soundPlayer = new System.Media.SoundPlayer(stream);
                        soundPlayer.Play();
                    }

                }

            }
            catch (Exception)
            {
            }
        }

        private void Frm_Notification_Load(object sender, EventArgs e)
        {
            lbl_Message.Focus();
            UpdateRegion();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var accentBrush = new SolidBrush(_accentColor);
            e.Graphics.FillRectangle(accentBrush, 0, 0, AccentBarWidth, Height);

            using var borderPen = new Pen(Color.FromArgb(225, 225, 225), 1);
            using var borderPath = AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, Width - 1, Height - 1), CornerRadius);
            e.Graphics.DrawPath(borderPen, borderPath);
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0) return;

            using var path = AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, Width, Height), CornerRadius);
            Region = new Region(path);
        }
    }
}
