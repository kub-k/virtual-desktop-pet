using System;
using System.Drawing;
using System.Windows.Forms;
using VirtualDesktopPet.Models;
using VirtualDesktopPet.Services;

namespace VirtualDesktopPet.UI
{
    public partial class SettingsForm : Form
    {
        private PetConfig config;
        private ConfigService configService;

        private Label labelMovementSpeed;
        private TrackBar trackMovementSpeed;
        private Label valueMovementSpeed;

        private Label labelWalkChance;
        private TrackBar trackWalkChance;
        private Label valueWalkChance;

        private Label labelSitChance;
        private TrackBar trackSitChance;
        private Label valueSitChance;

        private Label labelSleepChance;
        private TrackBar trackSleepChance;
        private Label valueSleepChance;

        private Button buttonSave;
        private Button buttonCancel;
        private Button buttonReset;

        public event Action<PetConfig> OnSettingsSaved;

        public SettingsForm(PetConfig currentConfig)
        {
            config = currentConfig;
            configService = new ConfigService();

            InitializeComponent();
            LoadCurrentSettings();
        }

        private void InitializeComponent()
        {
            // Form settings
            this.Text = "Virtual Pet - Settings";
            this.Width = 400;
            this.Height = 340;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Color darkBg = Color.FromArgb(76, 56, 45);       
            Color textColor = Color.FromArgb(245, 235, 220); 

            this.BackColor = darkBg;
            this.ForeColor = textColor;
            this.Icon = new Icon("Assets/app-icon.ico");

            // Create panel
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 5,
                Padding = new Padding(15),
                AutoSize = false,
                BackColor = darkBg
            };

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            // Movement Speed
            labelMovementSpeed = CreateLabel("Movement Speed:", darkBg, textColor);
            trackMovementSpeed = CreateTrackBar(1, 10, 3);
            trackMovementSpeed.ValueChanged += TrackMovementSpeed_ValueChanged;
            valueMovementSpeed = CreateValueLabel("3");

            mainPanel.Controls.Add(labelMovementSpeed, 0, 0);
            mainPanel.Controls.Add(trackMovementSpeed, 1, 0);
            mainPanel.Controls.Add(valueMovementSpeed, 2, 0);

            // Walk Chance
            labelWalkChance = CreateLabel("Walk Chance (%):", darkBg, textColor);
            trackWalkChance = CreateTrackBar(0, 10, 3);
            trackWalkChance.ValueChanged += TrackWalkChance_ValueChanged;
            valueWalkChance = CreateValueLabel("30%");

            mainPanel.Controls.Add(labelWalkChance, 0, 1);
            mainPanel.Controls.Add(trackWalkChance, 1, 1);
            mainPanel.Controls.Add(valueWalkChance, 2, 1);

            // Sit Chance
            labelSitChance = CreateLabel("Sit Chance (%):", darkBg, textColor);
            trackSitChance = CreateTrackBar(0, 10, 2);
            trackSitChance.ValueChanged += TrackSitChance_ValueChanged;
            valueSitChance = CreateValueLabel("20%");

            mainPanel.Controls.Add(labelSitChance, 0, 2);
            mainPanel.Controls.Add(trackSitChance, 1, 2);
            mainPanel.Controls.Add(valueSitChance, 2, 2);

            // Sleep Chance 
            labelSleepChance = CreateLabel("Sleep Chance (%):", darkBg, textColor);
            trackSleepChance = CreateTrackBar(0, 10, 5);
            trackSleepChance.ValueChanged += TrackSleepChance_ValueChanged;
            valueSleepChance = CreateValueLabel("50%");

            mainPanel.Controls.Add(labelSleepChance, 0, 3);
            mainPanel.Controls.Add(trackSleepChance, 1, 3);
            mainPanel.Controls.Add(valueSleepChance, 2, 3);

            // Buttons Panel
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                AutoSize = false,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10, 10, 10, 10),
                BackColor = darkBg
            };

            buttonSave = CreateButton("Save", Color.FromArgb(198, 139, 96), Color.White);
            buttonSave.Click += ButtonSave_Click;

            buttonCancel = CreateButton("Cancel", Color.FromArgb(100, 70, 50), Color.White);
            buttonCancel.Click += ButtonCancel_Click;

            buttonReset = CreateButton("Reset", Color.FromArgb(180, 140, 100), Color.White);
            buttonReset.Click += ButtonReset_Click;

            buttonPanel.Controls.Add(buttonSave);
            buttonPanel.Controls.Add(buttonCancel);
            buttonPanel.Controls.Add(buttonReset);

            this.Controls.Add(mainPanel);
            this.Controls.Add(buttonPanel);
        }

        private Label CreateLabel(string text, Color bgColor, Color textColor)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = false,
                Font = new System.Drawing.Font("Segoe UI", 10),
                ForeColor = textColor,
                BackColor = bgColor
            };
        }

        private TrackBar CreateTrackBar(int min, int max, int value)
        {
            return new TrackBar
            {
                Minimum = min,
                Maximum = max,
                Value = value,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Height = 35,
            };
        }

        private Label CreateValueLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                AutoSize = false,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = Color.FromArgb(198, 139, 96),
                ForeColor = Color.FromArgb(255, 255, 240),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private Button CreateButton(string text, Color bgColor, Color textColor)
        {
            return new Button
            {
                Text = text,
                Width = 80,
                Height = 35,
                BackColor = bgColor,
                ForeColor = textColor,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private void LoadCurrentSettings()
        {
            trackMovementSpeed.Value = config.MovementSpeed;
            trackWalkChance.Value = config.WalkChance / 10;
            trackSitChance.Value = config.SitChance / 10;
            trackSleepChance.Value = config.SleepChance / 10;
        }

        private void TrackMovementSpeed_ValueChanged(object sender, EventArgs e)
        {
            valueMovementSpeed.Text = trackMovementSpeed.Value.ToString();
        }

        private void TrackWalkChance_ValueChanged(object sender, EventArgs e)
        {
            valueWalkChance.Text = (trackWalkChance.Value * 10) + "%";
        }

        private void TrackSitChance_ValueChanged(object sender, EventArgs e)
        {
            valueSitChance.Text = (trackSitChance.Value * 10) + "%";
        }

        private void TrackSleepChance_ValueChanged(object sender, EventArgs e)
        {
            valueSleepChance.Text = (trackSleepChance.Value * 10) + "%";
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            // Validate percentage values sum to 100
            int walkPercent = trackWalkChance.Value * 10;
            int sitPercent = trackSitChance.Value * 10;
            int sleepPercent = trackSleepChance.Value * 10;
            int total = walkPercent + sitPercent + sleepPercent;

            if (total != 100)
            {
                Form warningPopup = new Form()
                {
                    Width = 300,
                    Height = 120,
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    Text = "Warning!"
                };

                Label label = new Label()
                {
                    Dock = DockStyle.Fill,
                    Text = $"Walk + Sit + Sleep chances must equal 100%!\n" +
                    $"Current total: {total}%",
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(198, 139, 96),
                    ForeColor = Color.FromArgb(255, 255, 240),
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                };

                warningPopup.Controls.Add(label);
                warningPopup.ShowDialog();
                return;
            }

            // Update config
            config.MovementSpeed = trackMovementSpeed.Value;
            config.WalkChance = walkPercent;
            config.SitChance = sitPercent;
            config.SleepChance = sleepPercent;

            // Save to file
            configService.SaveConfig(config);

            // Trigger event
            OnSettingsSaved?.Invoke(config);

            Form successPopup = new Form()
            {
                Width = 300,
                Height = 120,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                Text = "Success!"
            };

            Label nlabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Settings saved successfully!",
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(198, 139, 96),
                ForeColor = Color.FromArgb(255, 255, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
            };

            successPopup.Controls.Add(nlabel);
            successPopup.ShowDialog();
            
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            trackMovementSpeed.Value = 3;
            trackWalkChance.Value = 3;
            trackSitChance.Value = 2;
            trackSleepChance.Value = 5;
        }
    }
}