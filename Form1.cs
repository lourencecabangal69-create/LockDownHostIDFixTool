using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace LockDownHostIDFixTool
{
    public partial class Form1 : Form
    {
        private const string LockdownPath = @"C:\ProgramData\Apple\Lockdown";
        private const string ContactUrl = "https://www.facebook.com/RenzCabangal18/";
        private const string DonateNumber = "09368005389";

        private Label? lblTitle;
        private Label? lblInfo;
        private Label? lblWarning;
        private Label? lblContact;
        private Button? btnApplyFix;
        private Button? btnRestartPc;
        private Button? btnDonate;
        private LinkLabel? lnkContact;
        private TableLayoutPanel? pnlMain;
        private TableLayoutPanel? pnlFooter;

        public Form1()
        {
            InitializeComponent();
            LoadEmbeddedIcon();
            SetupCustomUI();
            CheckAdminStatus();
        }

        private void LoadEmbeddedIcon()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                // The icon is embedded as a resource with the name matching the file
                using var stream = assembly.GetManifestResourceStream("LockDownHostIDFixTool.Apple Lockdown ID Fix Icon.ico");
                if (stream != null)
                {
                    Icon = new Icon(stream);
                }
            }
            catch
            {
                // Fallback: use default icon if embedded resource fails
            }
        }

        private void SetupCustomUI()
        {
            Text = "LockDownHostID Error Fix Tool";
            Size = new Size(440, 400);
            MinimumSize = new Size(440, 400);
            MaximumSize = new Size(440, 400);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(245, 245, 250);
            Font = new Font("Segoe UI", 9f);

            // Main layout - TableLayoutPanel for responsive structure
            pnlMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 5,
                AutoSize = true
            };
            pnlMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            pnlMain.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Title
            pnlMain.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Info
            pnlMain.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Warning
            pnlMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // Buttons (expands)
            pnlMain.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Footer spacer
            Controls.Add(pnlMain);

            // Title
            lblTitle = new Label
            {
                Text = "🔧 LockDownHostID Error Fix Tool",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 60),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 0, 0, 8)
            };
            pnlMain.Controls.Add(lblTitle, 0, 0);

            // Info
            lblInfo = new Label
            {
                Text = "Resolves Apple Device Trust (0xE) Errors\nby removing corrupted pairing certificates.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(80, 80, 90),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 0, 0, 12)
            };
            pnlMain.Controls.Add(lblInfo, 0, 1);

            // Warning
            lblWarning = new Label
            {
                Text = "⚠️ Please close and run as Administrator",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(200, 60, 60),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 0, 0, 16),
                Visible = false
            };
            pnlMain.Controls.Add(lblWarning, 0, 2);

            // Buttons panel - uniform sizing with TableLayoutPanel
            var pnlButtons = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0)
            };
            pnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            pnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            pnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            pnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            pnlMain.Controls.Add(pnlButtons, 0, 3);

            const int buttonHeight = 44;
            const int buttonSpacing = 8;

            btnApplyFix = CreateUniformButton("✨ Apply Fix", Color.FromArgb(0, 120, 215), Color.White, FontStyle.Bold);
            btnApplyFix.Height = buttonHeight;
            btnApplyFix.Dock = DockStyle.Fill;
            btnApplyFix.Margin = new Padding(0, 0, 0, buttonSpacing);
            btnApplyFix.Click += BtnApplyFix_Click;
            pnlButtons.Controls.Add(btnApplyFix, 0, 0);

            btnRestartPc = CreateUniformButton("🔄 Restart PC", Color.FromArgb(220, 220, 230), Color.FromArgb(60, 60, 70));
            btnRestartPc.Height = buttonHeight;
            btnRestartPc.Dock = DockStyle.Fill;
            btnRestartPc.Margin = new Padding(0, 0, 0, buttonSpacing);
            btnRestartPc.Click += BtnRestartPc_Click;
            pnlButtons.Controls.Add(btnRestartPc, 0, 1);

            btnDonate = CreateUniformButton($"💝 Donate ({DonateNumber})", Color.FromArgb(255, 193, 7), Color.FromArgb(60, 40, 0), FontStyle.Bold);
            btnDonate.Height = buttonHeight;
            btnDonate.Dock = DockStyle.Fill;
            btnDonate.Margin = new Padding(0);
            btnDonate.Click += BtnDonate_Click;
            pnlButtons.Controls.Add(btnDonate, 0, 2);

            // Footer - TableLayoutPanel for proper alignment
            pnlFooter = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.FromArgb(235, 235, 245),
                Padding = new Padding(16, 6, 16, 6),
                ColumnCount = 3,
                RowCount = 1
            };
            pnlFooter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Contact label
            pnlFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // Link (expands)
            pnlFooter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Version
            Controls.Add(pnlFooter);

            lblContact = new Label
            {
                Text = "Contact: ",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(100, 100, 110),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 0, 4, 0)
            };
            pnlFooter.Controls.Add(lblContact, 0, 0);

            lnkContact = new LinkLabel
            {
                Text = "Renz Cabangal (Facebook)",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Underline),
                LinkColor = Color.FromArgb(0, 100, 200),
                ActiveLinkColor = Color.FromArgb(0, 80, 180),
                VisitedLinkColor = Color.FromArgb(100, 60, 160),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0)
            };
            lnkContact.LinkClicked += (s, e) => Process.Start(new ProcessStartInfo(ContactUrl) { UseShellExecute = true });
            pnlFooter.Controls.Add(lnkContact, 1, 0);

            var lblVersion = new Label
            {
                Text = $"v{Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0"}",
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(140, 140, 150),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Right,
                Margin = new Padding(0)
            };
            pnlFooter.Controls.Add(lblVersion, 2, 0);
        }

        private Button CreateUniformButton(string text, Color backColor, Color foreColor, FontStyle style = FontStyle.Regular)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10, style),
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.15f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.1f);
            return btn;
        }

        private void CheckAdminStatus()
        {
            bool isAdmin = IsRunningAsAdmin();
            lblWarning!.Visible = !isAdmin;
            btnApplyFix!.Enabled = isAdmin;

            if (!isAdmin)
            {
                btnApplyFix.BackColor = Color.FromArgb(180, 180, 190);
                btnApplyFix.ForeColor = Color.FromArgb(120, 120, 130);
                btnApplyFix.Cursor = Cursors.No;
                toolTip1.SetToolTip(btnApplyFix, "Run as Administrator to enable this button");
            }
        }

        private bool IsRunningAsAdmin()
        {
            try
            {
                using var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private void BtnApplyFix_Click(object? sender, EventArgs e)
        {
            if (!IsRunningAsAdmin())
            {
                MessageBox.Show("You must run this tool as an Administrator to delete system files.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (Directory.Exists(LockdownPath))
                {
                    Directory.Delete(LockdownPath, true);
                    MessageBox.Show(
                        "The fix has been applied successfully.\n\n" +
                        "Please reconnect your Apple device or restart your PC " +
                        "to generate a new pairing certificate.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "The specified path was not found.\n" +
                        "It may have already been deleted or never existed.",
                        "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied. Please ensure you are running as Administrator.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"An I/O error occurred:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRestartPc_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to restart your computer now?\n\nPlease save any open work first.",
                "Restart Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                try
                {
                    Process.Start("shutdown", "/r /t 0");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to initiate restart:\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDonate_Click(object? sender, EventArgs e)
        {
            var msg = $"Support the developer!\n\n" +
                      $"GCash / PayMaya: {DonateNumber}\n\n" +
                      $"Your donations help maintain and improve this tool.";
            MessageBox.Show(msg, "Donate", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private ToolTip toolTip1 = new ToolTip();
    }
}