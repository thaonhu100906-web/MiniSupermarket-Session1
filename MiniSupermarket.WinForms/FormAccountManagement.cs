
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormAccountManagement : Form
    {
        private DataGridView dgvAccounts = null!;
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private TextBox txtFullName = null!;
        private ComboBox cmbRole = null!;
        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Panel tablePanel = null!;
        private Panel detailPanel = null!;
        private Button btnPagePrevious = null!;
        private Button btnPageNext = null!;
        private Label lblPageInfo = null!;
        private int currentPage = 1;
        private const int pageSize = 10;
        private int totalPages = 1;
        private int totalCount = 0;
        private bool loadingRows = false;

        private readonly Color backgroundColor = Color.FromArgb(250, 248, 239);
        private readonly Color primaryColor = Color.FromArgb(103, 155, 112);
        private readonly Color textColor = Color.FromArgb(54, 75, 58);

        public FormAccountManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();
            DailyFreshTheme.Apply(this);
            LoadUserData();
        }

        private void InitializeCustomLayout()
        {
            Text = "Quản trị tài khoản";
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BackColor = backgroundColor;
            AutoScroll = true;
            MinimumSize = new Size(650, 450);

            var title = new Label
            {
                Text = "QUẢN TRỊ TÀI KHOẢN",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(20, 15),
                AutoSize = true
            };

            var subtitle = new Label
            {
                Text = "Quản lý tài khoản, thông tin và vai trò tài khoản",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(22, 52),
                AutoSize = true
            };

            Controls.Add(title);
            Controls.Add(subtitle);

            tablePanel = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblList = new Label
            {
                Text = "Danh sách tài khoản",
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = textColor
            };

            dgvAccounts = new DataGridView
            {
                Dock = DockStyle.None,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = true,
                EnableHeadersVisualStyles = false,
                RowTemplate = { Height = 32 },
                Font = new Font("Segoe UI", 9)
            };

            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);
            dgvAccounts.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 235, 245);
            dgvAccounts.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged;

            var paginationPanel = new Panel { Height = 40, BackColor = Color.White, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            btnPagePrevious = new Button { Text = "← Trang trước", Width = 115, Height = 30, Left = 8, Top = 5, Enabled = false };
            btnPageNext = new Button { Text = "Trang sau →", Width = 115, Height = 30, Top = 5, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            lblPageInfo = new Label { Text = "Trang 1/1", TextAlign = ContentAlignment.MiddleCenter, AutoSize = false, Height = 30, Top = 5 };
            btnPagePrevious.Click += async (sender, args) => { if (currentPage > 1) { currentPage--; await LoadUserDataAsync(); } };
            btnPageNext.Click += async (sender, args) => { if (currentPage < totalPages) { currentPage++; await LoadUserDataAsync(); } };
            paginationPanel.Controls.Add(lblPageInfo);
            paginationPanel.Controls.Add(btnPagePrevious);
            paginationPanel.Controls.Add(btnPageNext);
            paginationPanel.Resize += (sender, args) =>
            {
                btnPageNext.Left = Math.Max(125, paginationPanel.ClientSize.Width - btnPageNext.Width - 8);
                lblPageInfo.Left = btnPagePrevious.Right + 4;
                lblPageInfo.Width = Math.Max(40, btnPageNext.Left - lblPageInfo.Left - 4);
                lblPageInfo.BringToFront();
                btnPagePrevious.BringToFront();
                btnPageNext.BringToFront();
            };
            tablePanel.Controls.Add(dgvAccounts);
            tablePanel.Controls.Add(paginationPanel);
            tablePanel.Controls.Add(lblList);

            void LayoutAccountTable()
            {
                int innerWidth = Math.Max(100, tablePanel.ClientSize.Width - tablePanel.Padding.Horizontal);
                int innerHeight = Math.Max(150, tablePanel.ClientSize.Height - tablePanel.Padding.Vertical);
                lblList.Location = new Point(tablePanel.Padding.Left, tablePanel.Padding.Top);
                lblList.Width = innerWidth;
                paginationPanel.SetBounds(tablePanel.Padding.Left, tablePanel.ClientSize.Height - tablePanel.Padding.Bottom - paginationPanel.Height, innerWidth, paginationPanel.Height);
                dgvAccounts.SetBounds(tablePanel.Padding.Left, tablePanel.Padding.Top + lblList.Height, innerWidth, Math.Max(80, innerHeight - lblList.Height - paginationPanel.Height));
                dgvAccounts.BringToFront();
                paginationPanel.BringToFront();
                lblList.BringToFront();
            }
            tablePanel.Resize += (sender, args) => LayoutAccountTable();
            LayoutAccountTable();
            Controls.Add(tablePanel);

            detailPanel = new Panel
            {
                BackColor = Color.White,
                Padding = new Padding(16),
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(detailPanel);

            var detailTitle = new Label
            {
                Text = "Thông tin tài khoản",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(16, 15),
                AutoSize = true
            };
            detailPanel.Controls.Add(detailTitle);

            CreateLabel(detailPanel, "Tài khoản", 16, 52);
            txtUsername = CreateTextBox(detailPanel, 16, 74);

            CreateLabel(detailPanel, "Mật khẩu (khi thêm mới)", 16, 108);
            txtPassword = CreateTextBox(detailPanel, 16, 130);
            txtPassword.UseSystemPasswordChar = true;

            CreateLabel(detailPanel, "Họ tên", 16, 164);
            txtFullName = CreateTextBox(detailPanel, 16, 186);

            CreateLabel(detailPanel, "Vai trò", 16, 220);
            cmbRole = new ComboBox
            {
                Location = new Point(16, 242),
                Height = 30,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            cmbRole.Items.AddRange(new object[]
            {
                "Admin",
                "Warehouse",
                "Cashier"
            });
            detailPanel.Controls.Add(cmbRole);

            btnAdd = CreateButton("Thêm mới", Color.FromArgb(40, 167, 69));
            btnUpdate = CreateButton("Cập nhật", Color.FromArgb(41, 100, 180));
            btnDelete = CreateButton("Xóa", Color.FromArgb(220, 53, 69));
            btnRefresh = CreateButton("Làm mới", Color.FromArgb(108, 117, 125));

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;

            detailPanel.Controls.Add(btnAdd);
            detailPanel.Controls.Add(btnUpdate);
            detailPanel.Controls.Add(btnDelete);
            detailPanel.Controls.Add(btnRefresh);

            Resize += FormAccountManagement_Resize;
            FormAccountManagement_Resize(this, EventArgs.Empty);
        }

        private void FormAccountManagement_Resize(object? sender, EventArgs e)
        {
            if (tablePanel == null || detailPanel == null)
                return;

            int width = ClientSize.Width - 40;
            int height = ClientSize.Height - 95;

            if (width <= 0 || height <= 0)
                return;

            int gap = 12;

            if (width >= 850)
            {
                int detailWidth = Math.Min(330, (int)(width * 0.32));
                int tableWidth = width - detailWidth - gap;

                tablePanel.Location = new Point(20, 85);
                tablePanel.Size = new Size(tableWidth, height);

                detailPanel.Location = new Point(
                    20 + tableWidth + gap, 85);
                detailPanel.Size = new Size(detailWidth, height);

                AutoScrollMinSize = Size.Empty;
            }
            else
            {
                int panelWidth = Math.Max(300, width);

                tablePanel.Location = new Point(20, 85);
                tablePanel.Size = new Size(panelWidth, 300);

                detailPanel.Location = new Point(20, 397);
                detailPanel.Size = new Size(panelWidth, 350);

                AutoScrollMinSize = new Size(0, 765);
            }

            LayoutDetailControls();
        }

        private void LayoutDetailControls()
        {
            int inputWidth = Math.Max(180, detailPanel.ClientSize.Width - 48);
            txtUsername.Width = inputWidth;
            txtPassword.Width = inputWidth;
            txtFullName.Width = inputWidth;
            cmbRole.Width = inputWidth;

            int buttonWidth = Math.Max(
                100, (detailPanel.ClientSize.Width - 44) / 2);

            int buttonY = 286;

            btnAdd.Location = new Point(16, buttonY);
            btnAdd.Size = new Size(buttonWidth, 34);

            btnUpdate.Location = new Point(24 + buttonWidth, buttonY);
            btnUpdate.Size = new Size(buttonWidth, 34);

            btnDelete.Location = new Point(16, buttonY + 42);
            btnDelete.Size = new Size(buttonWidth, 34);

            btnRefresh.Location = new Point(24 + buttonWidth, buttonY + 42);
            btnRefresh.Size = new Size(buttonWidth, 34);
        }

        private Label CreateLabel(Control parent, string text, int x, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87)
            };

            parent.Controls.Add(label);
            return label;
        }

        private TextBox CreateTextBox(Control parent, int x, int y)
        {
            var textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(250, 27),
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left |
                         AnchorStyles.Right
            };

            parent.Controls.Add(textBox);
            return textBox;
        }

        private Button CreateButton(string text, Color color)
        {
            var button = new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private async void LoadUserData() => await LoadUserDataAsync();

        private async System.Threading.Tasks.Task LoadUserDataAsync()
        {
            try
            {
                loadingRows = true;
                string jsonResult = await ApiClientService.GetDataWithTokenAsync(
                    $"users/paged?pageNumber={currentPage}&pageSize={pageSize}");
                var result = JsonSerializer.Deserialize<PagedListResponse<UserModel>>(jsonResult,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var users = result?.Items ?? new List<UserModel>();
                totalCount = result?.TotalCount ?? users.Count;
                totalPages = Math.Max(1, result?.TotalPages ?? 1);
                if (currentPage > totalPages) { currentPage = totalPages; loadingRows = false; await LoadUserDataAsync(); return; }
                dgvAccounts.DataSource = users;
                lblPageInfo.Text = $"Trang {currentPage}/{totalPages} • Tổng {totalCount} tài khoản";
                btnPagePrevious.Enabled = currentPage > 1;
                btnPageNext.Enabled = currentPage < totalPages;

                SetColumnHeader("UserId", "Mã tài khoản");
                SetColumnHeader("Username", "Tài khoản");
                SetColumnHeader("FullName", "Họ tên");
                SetColumnHeader("Role", "Vai trò");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách tài khoản: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally { loadingRows = false; }
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvAccounts.Columns[columnName] != null)
                dgvAccounts.Columns[columnName]!.HeaderText = headerText;
        }

        private void DgvAccounts_SelectionChanged(object? sender, EventArgs e)
        {
            if (loadingRows || dgvAccounts.SelectedRows.Count == 0)
                return;

            var row = dgvAccounts.SelectedRows[0];

            txtUsername.Text =
                row.Cells["Username"].Value?.ToString() ?? string.Empty;

            txtFullName.Text =
                row.Cells["FullName"].Value?.ToString() ?? string.Empty;

            string role = row.Cells["Role"].Value?.ToString() ?? "Cashier";
            cmbRole.SelectedItem = role;
            txtPassword.Clear();
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn vai trò!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newUser = new
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text,
                FullName = txtFullName.Text.Trim(),
                Role = cmbRole.SelectedItem!.ToString()
            };

            try
            {
                bool success = await ApiClientService.PostDataWithTokenAsync(
                    "users", newUser);

                if (success)
                {
                    MessageBox.Show("Thêm tài khoản thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    currentPage = 1;
                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại. Kiểm tra thông tin nhập!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm tài khoản: " + ex.Message);
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần cập nhật!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                cmbRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng nhập tài khoản, họ tên và vai trò!");
                return;
            }

            object? id = dgvAccounts.SelectedRows[0].Cells["UserId"].Value;
            if (id == null)
                return;

            var updatedUser = new
            {
                Username = txtUsername.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Role = cmbRole.SelectedItem!.ToString()
            };

            try
            {
                bool success = await ApiClientService.PutDataWithTokenAsync(
                    $"users/{id}", updatedUser);

                if (success)
                {
                    MessageBox.Show("Cập nhật tài khoản thành công!");
                    currentPage = 1;
                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Cập nhật tài khoản thất bại. Vui lòng kiểm tra lại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tài khoản: " + ex.Message);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!");
                return;
            }

            object? id = dgvAccounts.SelectedRows[0].Cells["UserId"].Value;
            if (id == null)
                return;

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tài khoản này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool success = await ApiClientService.DeleteDataWithTokenAsync(
                    $"users/{id}");

                if (success)
                {
                    MessageBox.Show("Xóa tài khoản thành công!");
                    ClearInputs();
                    currentPage = 1;
                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Xóa tài khoản thất bại. Vui lòng kiểm tra lại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            currentPage = 1;
            LoadUserData();
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cmbRole.SelectedIndex = -1;
            dgvAccounts.ClearSelection();
        }

        private void FormAccountManagement_Load(object sender, EventArgs e)
        {
        }
    }

    public class UserModel
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}