using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VirtualDesktopPet.Models;

namespace VirtualDesktopPet
{
    public partial class PetForm : Form
    {
        private bool isDragging = false;
        private Point dragOffset;

        private ContextMenuStrip contextMenu;

        private int movementSpeed = 5;
        private int movementDirection = 1;

        private Timer updateTimer;
        private PetState currentState = PetState.Walking;
        private Random random = new Random();

        private int stateDurationCounter = 0;
        private int stateChangeInterval = 120; // 120 / 2 ≈ 2 seconds

        public PetForm()
        {
            InitializeComponent();
            ConfigureForm();
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

            BackColor = Color.Lime;
            TransparencyKey = Color.Lime;

            DoubleBuffered = true;

            Location = new Point(500, 300);
        }

        private void InitializeContextMenu()
        {
            contextMenu = new ContextMenuStrip();

            ToolStripMenuItem settingsItem = new ToolStripMenuItem("Settings");
            settingsItem.Click += SettingsItem_Click;

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += ExitItem_Click;

            contextMenu.Items.Add(settingsItem);
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

            if (currentState == PetState.Walking)
            {
                this.Left += movementSpeed * movementDirection;

                if (this.Left <= 0)
                {
                    movementDirection = 1;
                }

                if (this.Right >= Screen.PrimaryScreen.WorkingArea.Width)
                {
                    movementDirection = -1;
                }
            }
        }

        private void ChangePetState()
        {
            int randomValue = random.Next(1, 101);

            if (randomValue <= 50)
            {
                currentState = PetState.Walking;
            }
            else if (randomValue <= 80)
            {
                currentState = PetState.Sitting;
            }
            else
            {
                currentState = PetState.Sleeping;
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

            Color petColor;

            switch (currentState)
            {
                case PetState.Walking:
                    petColor = Color.LightGreen;
                    break;
                case PetState.Sitting:
                    petColor = Color.LightYellow;
                    break;
                case PetState.Sleeping:
                    petColor = Color.LightSkyBlue;
                    break;
                default:
                    petColor = Color.LightPink;
                    break;
            }

            using (Brush brush = new SolidBrush(petColor))
            {
                int size = 80;
                int x = (this.ClientSize.Width - size) / 2;
                int y = (this.ClientSize.Height - size) / 2;

                g.FillRectangle(
                     brush,
                     x,
                     y,  
                     size,
                     size
                 );
            }
        }

        private void PetForm_Load(object sender, EventArgs e)
        {
        }
    }
}
