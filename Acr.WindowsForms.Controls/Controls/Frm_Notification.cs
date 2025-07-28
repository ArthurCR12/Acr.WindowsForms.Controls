using Acr.WindowsForms.Controls.Enums;
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


        public Frm_Notification()
        {
            InitializeComponent();
        }

        public void ShowNotification(string msg, NotificationType nType = NotificationType.Info)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;

            if (nType == NotificationType.Sucess)
                picB_Icon.Image = Properties.Resources._80_OkGreen;
            else if (nType == NotificationType.Warning)
            {
                picB_Icon.Image = Properties.Resources._80_warning;
            }
            else if (nType == NotificationType.Error)
            {
                picB_Icon.Image = Properties.Resources._80_Error;
            }
            else
            {
                picB_Icon.Image = Properties.Resources._80_info;
            }


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
        }
    }
}
