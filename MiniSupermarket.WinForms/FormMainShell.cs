
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormMainShell : Form
    {
        private Panel panelSidebar = null!;
        private Panel panelTopHeader = null!;
        private Panel panelMainContent = null!;
        private Panel panelUserFooter = null!;

        private Label lblLogo = null!;
        private Label lblTitle = null!;
        private Label lblUserInfo = null!;
        private Label lblWelcome = null!;
        private Label lblSubWelcome = null!;

        private Button btnPOS = null!;
        private Button btnCategory = null!;
        private Button btnProduct = null!;
        private Button btnCustomer = null!;
        private Button btnReports = null!;
        private Button btnAccount = null!;
        private Button btnLogout = null!;

        private Button? _selectedMenuButton;
        private Form? _activeForm = null;

        // Màu sắc đậm hơn, dễ nhìn hơn
        private readonly Color SidebarColor = Color.FromArgb(198, 222, 201);
        private readonly Color SidebarSelectedColor = Color.FromArgb(125, 170, 132);
        private readonly Color SidebarHoverColor = Color.FromArgb(179, 207, 183);
        private readonly Color MainBackground = Color.FromArgb(248, 246, 235);
        private readonly Color TextDark = Color.FromArgb(42, 65, 47);
        private readonly Color TextGray = Color.FromArgb(91, 108, 88);

        public FormMainShell()
        {
            InitializeComponent();
            BuildInterface();
            Load += FormMainShell_Load;
        }

        private void BuildInterface()
        {
            Text = "Daily Fresh - Hệ thống quản lý";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1000, 600);
            BackColor = MainBackground;
            AutoScaleMode = AutoScaleMode.Font;

            panelSidebar = new Panel
            {
                Name = "panelSidebar",
                Dock = DockStyle.Left,
                Width = 245,
                BackColor = SidebarColor
            };

            panelTopHeader = new Panel
            {
                Name = "panelTopHeader",
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = Color.White,
                Padding = new Padding(25, 0, 25, 0)
            };

            panelMainContent = new Panel
            {
                Name = "panelMainContent",
                Dock = DockStyle.Fill,
                BackColor = MainBackground,
                Padding = new Padding(0),
                Margin = new Padding(0),
                AutoScroll = false
            };

            Controls.Add(panelMainContent);
            Controls.Add(panelTopHeader);
            Controls.Add(panelSidebar);

            BuildSidebar();
            BuildHeader();
            BuildWelcomeScreen();
        }

        // ================= SIDEBAR =================

        private void BuildSidebar()
        {
            lblLogo = new Label
            {
                Text = "🥐 Daily Fresh",
                ForeColor = TextDark,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 75,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblMenu = new Label
            {
                Text = "  MENU CHỨC NĂNG",
                ForeColor = TextGray,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 0, 0)
            };

            btnPOS = CreateMenuButton("🛒  Bán hàng (POS)");
            btnCategory = CreateMenuButton("📁  Quản lý Danh mục");
            btnProduct = CreateMenuButton("📦  Quản lý Sản phẩm");
            btnCustomer = CreateMenuButton("👥  Quản lý Khách hàng");
            btnAccount = CreateMenuButton("🛡  Quản trị Tài khoản");
            btnReports = CreateMenuButton("📊  Báo cáo Doanh thu");

            btnPOS.Click += btnPOS_Click;
            btnCategory.Click += btnCategory_Click;
            btnProduct.Click += btnProduct_Click;
            btnCustomer.Click += btnCustomer_Click;
            btnAccount.Click += btnAccount_Click;
            btnReports.Click += btnReports_Click;

            // Dock Top: thêm theo thứ tự từ dưới lên.
            panelSidebar.Controls.Add(btnReports);
            panelSidebar.Controls.Add(btnAccount);
            panelSidebar.Controls.Add(btnCustomer);
            panelSidebar.Controls.Add(btnProduct);
            panelSidebar.Controls.Add(btnCategory);
            panelSidebar.Controls.Add(btnPOS);
            panelSidebar.Controls.Add(lblMenu);
            panelSidebar.Controls.Add(lblLogo);

            panelUserFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = SidebarColor,
                Padding = new Padding(12)
            };

            btnLogout = new Button
            {
                Text = "🚪  Đăng xuất",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = SidebarColor,
                ForeColor = TextDark,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false,
                Cursor = Cursors.Hand
            };

            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = SidebarHoverColor;
            btnLogout.Click += btnLogout_Click;

            panelUserFooter.Controls.Add(btnLogout);
            panelSidebar.Controls.Add(panelUserFooter);
        }

        private Button CreateMenuButton(string text)
        {
            Button button = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 48,
                FlatStyle = FlatStyle.Flat,
                BackColor = SidebarColor,
                ForeColor = TextDark,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 0, 0),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = SidebarHoverColor;

            // Chỉ đổi màu khi rê chuột vào, không ghi đè mục đang được chọn.
            button.MouseEnter += (sender, e) =>
            {
                if (button != _selectedMenuButton)
                    button.BackColor = SidebarHoverColor;
            };

            button.MouseLeave += (sender, e) =>
            {
                if (button != _selectedMenuButton)
                    button.BackColor = SidebarColor;
            };

            return button;
        }

        // ================= HEADER =================

        private void BuildHeader()
        {
            lblTitle = new Label
            {
                Text = "BÀN LÀM VIỆC HỆ THỐNG",
                ForeColor = TextDark,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, 15)
            };

            lblUserInfo = new Label
            {
                Text = "Nhân viên: Chưa đăng nhập",
                ForeColor = TextGray,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            panelTopHeader.Controls.Add(lblTitle);
            panelTopHeader.Controls.Add(lblUserInfo);

            panelTopHeader.Resize += (sender, e) =>
            {
                lblUserInfo.Location = new Point(
                    Math.Max(25, panelTopHeader.ClientSize.Width - lblUserInfo.Width - 25),
                    25
                );
            };

            panelTopHeader.PerformLayout();
        }

        // ================= MÀN HÌNH CHÀO =================

        private void BuildWelcomeScreen()
        {
            Panel welcomeCard = new Panel
            {
                Dock = DockStyle.Top,
                Height = 245,
                BackColor = Color.White,
                Padding = new Padding(35),
                Margin = new Padding(0)
            };

            lblWelcome = new Label
            {
                Text = "Chào mừng đến với Daily Fresh 👋",
                ForeColor = TextDark,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(35, 35)
            };

            lblSubWelcome = new Label
            {
                Text = "Hệ thống quản lý tiệm bánh ngọt và nước ép.",
                ForeColor = TextGray,
                Font = new Font("Segoe UI", 11F),
                AutoSize = true,
                Location = new Point(38, 85)
            };

            Label lblGuide = new Label
            {
                Text = "Chọn chức năng ở thanh menu bên trái để bắt đầu.",
                ForeColor = TextGray,
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Location = new Point(38, 125)
            };

            welcomeCard.Controls.Add(lblWelcome);
            welcomeCard.Controls.Add(lblSubWelcome);
            welcomeCard.Controls.Add(lblGuide);

            panelMainContent.Controls.Clear();
            panelMainContent.Controls.Add(welcomeCard);
        }

        // ================= LOAD FORM =================

        private void FormMainShell_Load(object? sender, EventArgs e)
        {
            string role = SessionManager.CurrentRole ?? "";
            string username = SessionManager.CurrentUsername ?? "";

            lblUserInfo.Text = $"👤  {username}  |  Vai trò: {role}";

            ApplyRolePermissions(role);

            if (!IsDisposed && !Disposing)
                OpenDefaultScreenByRole(role);
        }

        // ================= PHÂN QUYỀN =================

        private void ApplyRolePermissions(string role)
        {
            btnPOS.Visible = false;
            btnCategory.Visible = false;
            btnProduct.Visible = false;
            btnCustomer.Visible = false;
            btnAccount.Visible = false;
            btnReports.Visible = false;

            switch (role.Trim().ToUpperInvariant())
            {
                case "ADMIN":
                    btnPOS.Visible = true;
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    btnCustomer.Visible = true;
                    btnAccount.Visible = true;
                    btnReports.Visible = true;
                    break;

                case "CASHIER":
                    btnPOS.Visible = true;
                    btnCustomer.Visible = true;
                    break;

                case "WAREHOUSE":
                    btnCategory.Visible = true;
                    btnProduct.Visible = true;
                    break;

                default:
                    MessageBox.Show(
                        "Tài khoản chưa được cấp quyền hợp lệ!",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    BeginInvoke(new Action(Close));
                    break;
            }
        }

        // ================= MÀN HÌNH MẶC ĐỊNH =================

        private void OpenDefaultScreenByRole(string role)
        {
            switch (role.Trim().ToUpperInvariant())
            {
                case "ADMIN":
                    OpenChildForm(
                        new FormProductManagement(),
                        "QUẢN LÝ SẢN PHẨM & KHO HÀNG",
                        btnProduct
                    );
                    break;

                case "CASHIER":
                    OpenChildForm(
                        new FormCustomerManagement(),
                        "QUẢN LÝ KHÁCH HÀNG & TÍCH ĐIỂM",
                        btnCustomer
                    );
                    break;

                case "WAREHOUSE":
                    OpenChildForm(
                        new FormProductManagement(),
                        "QUẢN LÝ SẢN PHẨM & KHO HÀNG",
                        btnProduct
                    );
                    break;
            }
        }

        // ================= MỞ FORM CON =================

        private void OpenChildForm(
            Form childForm,
            string screenTitle,
            Button activeButton)
        {
            if (childForm == null || childForm.IsDisposed)
                return;

            if (_activeForm != null)
            {
                panelMainContent.Controls.Remove(_activeForm);
                _activeForm.Close();
                _activeForm.Dispose();
                _activeForm = null;
            }

            // Trả các nút khác về màu mặc định.
            foreach (Control control in panelSidebar.Controls)
            {
                if (control is Button button && button != btnLogout)
                {
                    button.BackColor = SidebarColor;
                    button.ForeColor = TextDark;
                }
            }

            // Chỉ đánh dấu nút đang được chọn.
            _selectedMenuButton = activeButton;
            activeButton.BackColor = SidebarSelectedColor;
            activeButton.ForeColor = Color.White;

            lblTitle.Text = screenTitle;

            childForm.SuspendLayout();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.Margin = new Padding(0);
            childForm.Padding = new Padding(0);
            childForm.StartPosition = FormStartPosition.Manual;

            panelMainContent.SuspendLayout();
            panelMainContent.Controls.Clear();

            _activeForm = childForm;
            panelMainContent.Controls.Add(childForm);
            childForm.Location = Point.Empty;
            childForm.Size = panelMainContent.ClientSize;
            childForm.ResumeLayout(true);

            panelMainContent.ResumeLayout(true);

            childForm.Show();
            childForm.BringToFront();
        }

        // ================= CÁC NÚT MENU =================

        private void btnPOS_Click(object? sender, EventArgs e)
        {
            OpenChildForm(new FormPOS(), "BÁN HÀNG POS", btnPOS);
        }

        private void btnCategory_Click(object? sender, EventArgs e)
        {
            OpenChildForm(
                new FormCategoryManagement(),
                "QUẢN LÝ DANH MỤC",
                btnCategory
            );
        }

        private void btnProduct_Click(object? sender, EventArgs e)
        {
            OpenChildForm(
                new FormProductManagement(),
                "QUẢN LÝ SẢN PHẨM & KHO HÀNG",
                btnProduct
            );
        }

        private void btnCustomer_Click(object? sender, EventArgs e)
        {
            OpenChildForm(
                new FormCustomerManagement(),
                "QUẢN LÝ KHÁCH HÀNG",
                btnCustomer
            );
        }

        private void btnAccount_Click(object? sender, EventArgs e)
        {
            OpenChildForm(
                new FormAccountManagement(),
                "QUẢN TRỊ TÀI KHOẢN",
                btnAccount
            );
        }

        private void btnReports_Click(object? sender, EventArgs e)
        {
            OpenChildForm(
                new FormQuickReport(),
                "BÁO CÁO DOANH THU",
                btnReports
            );
        }

        // ================= ĐĂNG XUẤT =================

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            SessionManager.JwtToken = string.Empty;
            SessionManager.CurrentUsername = string.Empty;
            SessionManager.CurrentRole = string.Empty;

            if (_activeForm != null)
            {
                panelMainContent.Controls.Remove(_activeForm);
                _activeForm.Close();
                _activeForm.Dispose();
                _activeForm = null;
            }

            Hide();

            using (FormLogin loginForm = new FormLogin())
            {
                loginForm.ShowDialog();
            }

            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_activeForm != null)
            {
                _activeForm.Dispose();
                _activeForm = null;
            }

            base.OnFormClosed(e);
        }
    }
}