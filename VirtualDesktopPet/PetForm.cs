using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VirtualDesktopPet.Core;
using VirtualDesktopPet.Models;
using VirtualDesktopPet.Services;

namespace VirtualDesktopPet
{
    public partial class PetForm : Form
    {
        private bool isDragging = false;
        private Point dragOffset;

        private ContextMenuStrip contextMenu;

        private Timer updateTimer;
        private PetState currentState = PetState.Walking;
        private Random random = new Random();

        private int movementSpeed;
        private int movementDirection = 1;

        private int stateDurationCounter = 0;
        private int stateChangeInterval;

        private PetConfig config;
        private ConfigService configService;

        private PetBehaviorManager behaviorManager;
        private PetMovementManager movementManager;
        private SpriteManager spriteManager;

        public PetForm()
        {
            InitializeComponent();
            ConfigureForm();

            InitializeServices();
            InitializeContextMenu();
            InitializeGameLoop();

            this.Paint += PetForm_Paint;
        }
        private void ConfigureForm()
        {
            this.Text = "Virtual Desktop Pet";

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;

            Width = 120;
            Height = 120;

            TopMost = true;
            ShowInTaskbar = true;

            BackColor = Color.Black;
            TransparencyKey = Color.Black;

            DoubleBuffered = true;

            Location = new Point(500, 300);
        }

        private void InitializeServices()
        {
            configService = new ConfigService();
            config = configService.LoadConfig();

            if (config == null)
                throw new Exception("Config could not be loaded!");

            movementSpeed = config.MovementSpeed;
            stateChangeInterval = config.StateChangeInterval;

            behaviorManager = new PetBehaviorManager(
                config.WalkChance,
                config.SitChance,
                config.SleepChance
            );

            movementManager = new PetMovementManager();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            PetSpriteSet set = new PetSpriteSet
            {
                WalkRight = LoadSpritesFromFolder(@"Assets\Cat\Walk\Right"),
                WalkLeft = LoadSpritesFromFolder(@"Assets\Cat\Walk\Left"),
                WalkUp = LoadSpritesFromFolder(@"Assets\Cat\Walk\Up"),
                WalkDown = LoadSpritesFromFolder(@"Assets\Cat\Walk\Down"),

                Sit = LoadSpritesFromFolder(@"Assets\Cat\Sit"),
                Sleep = LoadSpritesFromFolder(@"Assets\Cat\Sleep")
            };

            spriteManager = new SpriteManager(set);
        }

        private Image[] LoadSpritesFromFolder(string folderPath)
        {

            string fullPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                folderPath
            );

            if (!Directory.Exists(fullPath))
                return new Image[0];

            var files = Directory.GetFiles(fullPath, "*.png")
                                 .OrderBy(f => f)
                                 .ToArray();

            return files.Select(f => Image.FromFile(f)).ToArray();
        }

        private Image MakeTransparent(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            bmp.MakeTransparent(Color.Lime);
            return bmp;
        }

        private void InitializeContextMenu()
        {
            contextMenu = new ContextMenuStrip();

            contextMenu.ShowImageMargin = false;
            contextMenu.BackColor = Color.FromArgb(30, 30, 30);
            contextMenu.ForeColor = Color.White;

            ToolStripMenuItem settingsItem = new ToolStripMenuItem("Settings");
            settingsItem.Click += SettingsItem_Click;

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += ExitItem_Click;

            contextMenu.Items.Add(settingsItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(exitItem);

            this.ContextMenuStrip = contextMenu;
        }

        private void InitializeGameLoop()
        {
            updateTimer = new Timer();
            updateTimer.Interval = 16; // 1000 / 16 ≈ 60 FPS

            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdatePet();
            Invalidate();
        }

        private void UpdatePet()
        {
            stateDurationCounter++;

            if (stateDurationCounter >= stateChangeInterval)
            {
                ChangePetState();
                stateDurationCounter = 0;
            }

            movementManager.Update();

            if (currentState == PetState.Walking)
            {
                int speed = movementManager.GetFinalSpeed(movementSpeed);

                switch (movementManager.CurrentDirection)
                {
                    case Direction.Left:
                        this.Left -= speed;
                        break;

                    case Direction.Right:
                        this.Left += speed;
                        break;

                    case Direction.Up:
                        this.Top -= speed;
                        break;

                    case Direction.Down:
                        this.Top += speed;
                        break;
                }

                KeepInsideScreen();
            }
        }

        private void ChangePetState()
        {
            currentState = behaviorManager.GetNextState();
        }

        private void KeepInsideScreen()
        {
            Rectangle area = Screen.PrimaryScreen.WorkingArea;

            bool bounced = false;

            if (this.Left < area.Left)
            {
                this.Left = area.Left;
                movementManager.SetDirection(Direction.Right);
                bounced = true;
            }

            if (this.Right > area.Right)
            {
                this.Left = area.Right - this.Width;
                movementManager.SetDirection(Direction.Left);
                bounced = true;
            }

            if (this.Top < area.Top)
            {
                this.Top = area.Top;
                movementManager.SetDirection(Direction.Down);
                bounced = true;
            }

            if (this.Bottom > area.Bottom)
            {
                this.Top = area.Bottom - this.Height;
                movementManager.SetDirection(Direction.Up);
                bounced = true;
            }

            // extra safety: jitter prevention
            if (bounced)
            {
                stateDurationCounter += 5;
            }
        }

        private void SettingsItem_Click(object sender, EventArgs e)
        {
            Form popup = new Form()
            {
                Width = 300,
                Height = 120,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                Text = "Virtual Desktop Pet"
            };

            Label label = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Settings will be implemented later <3",
                TextAlign = ContentAlignment.MiddleCenter
            };

            popup.Controls.Add(label);
            popup.Show();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragOffset = new Point(
                    e.X,
                    e.Y
                );
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDragging)
            {
                Point screenPos = MousePosition;
                this.Location = new Point(
                    screenPos.X - dragOffset.X,
                    screenPos.Y - dragOffset.Y
                );
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void PetForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Image frame = spriteManager.GetCurrentFrame(currentState, movementManager.CurrentDirection);

            g.DrawImage(frame, new Rectangle(0, 0, this.Width, this.Height));
        }

        private void PetForm_Load(object sender, EventArgs e)
        {
        }
    }
}
