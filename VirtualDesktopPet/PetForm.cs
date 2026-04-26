using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualDesktopPet
{
    public partial class PetForm : Form
    {
        private bool isDragging = false;
        private Point dragOffset;
        private ContextMenuStrip contextMenu;

        public PetForm()
        {
            InitializeComponent();
            ConfigureForm();
            InitializeContextMenu();
            this.Paint += PetForm_Paint;
        }
        private void ConfigureForm()
        {
            this.Text = "Virtual Desktop Pet";

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;

            Width = 300;
            Height = 300;

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
            using (Brush brush = new SolidBrush(Color.LightPink))
            {
                int size = 80;
                int x = (this.ClientSize.Width - size) / 2;
                int y = (this.ClientSize.Height - size) / 2;
                g.FillRectangle(brush, x, y, size, size);
            }
        }

        private void PetForm_Load(object sender, EventArgs e)
        {
        }
    }
}
