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
        private const int SlideDistance = 40;
        private const int TimerIntervalFast = 15;
        private const int ProgressBarHeight = 3;
        private const int CornerRadius = 10;
        private const int AccentBarWidth = 4;

        private Color _accentColor = AcrColors.Info;
        private int _duration = 5000;
        private DateTime _waitStarted;
        private int _elapsedBeforePause;
        private bool _paused;
        private string? _title;

        public Frm_Notification()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            BackColor = AcrColors.Surface;
            lbl_Message.Font = AcrFonts.Get(9.5F);
            lbl_Message.ForeColor = AcrColors.Text;
            btn_Close.FlatAppearance.MouseOverBackColor = AcrColors.SurfaceHover;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Pausa o tempo de exibição enquanto o mouse estiver sobre a notificação.
            foreach (Control c in Controls)
            {
                c.MouseEnter += (_, _) => PauseIfHovered();
                c.MouseLeave += (_, _) => ResumeIfNotHovered();
            }
            MouseEnter += (_, _) => PauseIfHovered();
            MouseLeave += (_, _) => ResumeIfNotHovered();
            lbl_Message.Click += (_, _) => btn_Close_Click(this, EventArgs.Empty);
        }

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_TOPMOST = 0x00000008;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOPMOST | WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        public void ShowNotification(string msg, NotificationType nType = NotificationType.Info) =>
            ShowNotification(msg, nType, null, 5000, true);

        public void ShowNotification(string msg, NotificationType nType, string? title, int durationMs, bool playSound) =>
            ShowNotification(msg, nType, title, durationMs, playSound, null, null);

        /// <summary>
        /// Exibe a notificação. Se <paramref name="actionText"/> for informado, mostra um botão
        /// (ex.: "Desfazer") que executa <paramref name="onAction"/> e fecha a notificação.
        /// </summary>
        public void ShowNotification(string msg, NotificationType nType, string? title, int durationMs, bool playSound, string? actionText, Action? onAction)
        {
            if (!string.IsNullOrWhiteSpace(actionText) && onAction != null)
                AddActionButton(actionText, onAction);

            _duration = Math.Max(1000, durationMs);
            _title = string.IsNullOrWhiteSpace(title) ? null : title;
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
                    var area = (Form.ActiveForm != null ? Screen.FromControl(Form.ActiveForm) : Screen.PrimaryScreen!).WorkingArea;

                    this.x = area.Right - this.Width - 10; // Distancia da parte da direita
                    this.y = area.Bottom - (this.Height + 8) * (i + 1); // Distancia da parte inferior (com espaço entre notificações)
                    this.Location = new Point(x + SlideDistance, y); // começa deslocada e desliza para a esquerda
                    break;
                }
            }
            if (_title != null)
            {
                lbl_Message.Text = _title + Environment.NewLine + msg;
            }
            else
            {
                this.lbl_Message.Text = msg;
            }
            if (playSound) PlaySound(nType);
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

                    if (this.Left > x) this.Left = Math.Max(x, this.Left - SlideSpeed * 2);
                    else if (this.Opacity >= 1.0)
                    {
                        _action = NotificationAction.Waiting;
                        _waitStarted = DateTime.Now;
                        _elapsedBeforePause = 0;
                    }
                    break;

                case NotificationAction.Waiting:
                    timer1.Interval = 50;
                    if (!_paused && Elapsed >= _duration) _action = NotificationAction.Close;
                    Invalidate(new Rectangle(0, Height - ProgressBarHeight - 1, Width, ProgressBarHeight + 1));
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

        private void AddActionButton(string text, Action onAction)
        {
            var button = new Acr.WindowsForms.Controls.Controls.CustomButton.AcrButton
            {
                Text = text,
                Variant = AcrButtonVariant.Ghost,
                AutoSize = false,
                Height = LogicalToDeviceUnits(26),
                Width = TextRenderer.MeasureText(text, AcrFonts.Get(9F, FontStyle.Bold)).Width + LogicalToDeviceUnits(20),
                Font = AcrFonts.Get(9F, FontStyle.Bold),
            };
            button.Location = new Point(lbl_Message.Left - LogicalToDeviceUnits(6), Height - button.Height - LogicalToDeviceUnits(10));
            lbl_Message.Height = button.Top - lbl_Message.Top;
            button.Click += (_, _) =>
            {
                try { onAction(); }
                finally { btn_Close_Click(this, EventArgs.Empty); }
            };
            button.MouseEnter += (_, _) => PauseIfHovered();
            button.MouseLeave += (_, _) => ResumeIfNotHovered();
            Controls.Add(button);
            button.BringToFront();
        }

        private int Elapsed => _paused ? _elapsedBeforePause : _elapsedBeforePause + (int)(DateTime.Now - _waitStarted).TotalMilliseconds;

        private void PauseIfHovered()
        {
            if (_paused || _action != NotificationAction.Waiting) return;
            _elapsedBeforePause = Elapsed;
            _paused = true;
        }

        private void ResumeIfNotHovered()
        {
            if (!_paused) return;
            if (ClientRectangle.Contains(PointToClient(Cursor.Position))) return;
            _paused = false;
            _waitStarted = DateTime.Now;
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

            if (_action == NotificationAction.Waiting)
            {
                float remaining = 1f - Math.Clamp(Elapsed / (float)_duration, 0f, 1f);
                using var trackBrush = new SolidBrush(Color.FromArgb(40, _accentColor));
                e.Graphics.FillRectangle(trackBrush, AccentBarWidth, Height - ProgressBarHeight, Width - AccentBarWidth, ProgressBarHeight);
                e.Graphics.FillRectangle(accentBrush, AccentBarWidth, Height - ProgressBarHeight, (Width - AccentBarWidth) * remaining, ProgressBarHeight);
            }

            using var borderPen = new Pen(AcrColors.BorderSubtle, 1);
            using var borderPath = AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, Width - 1, Height - 1), CornerRadius);
            e.Graphics.DrawPath(borderPen, borderPath);
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0) return;

            using var path = AcrGraphics.CreateRoundedRectPath(new Rectangle(0, 0, Width, Height), CornerRadius);
            var old = Region;
            Region = new Region(path);
            old?.Dispose();
        }
    }
}
