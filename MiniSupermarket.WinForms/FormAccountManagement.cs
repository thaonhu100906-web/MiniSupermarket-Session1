// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;
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
        private TextBox txtSearch = null!;
        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Button btnSearch = null!;
        private Button btnPrevious = null!;
        private Button btnNext = null!;
        private Label lblPagination = null!;
        private Panel tablePanel = null!;
        private Panel detailPanel = null!;

        private bool isLoading;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalPages = 1;
        private int totalCount;
        private string currentKeyword = string.Empty;

        private readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public FormAccountManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();
            DailyFreshTheme.Apply(this);

            Load += FormAccountManagement_Load;
            Resize += FormAccountManagement_Resize;
        }

        private async void FormAccountManagement_Load(object? sender, EventArgs e)
        {
            await LoadUserData();
        }

        private void InitializeCustomLayout()
        {
            Text = "Quản trị tài khoản";
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(244, 246, 248);
            AutoScroll = true;

            Controls.Add(new Label
            {
                Text = "QUẢN TRỊ TÀI KHOẢN",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            });

            Controls.Add(new Label
            {
                Text = "Quản lý tài khoản, thông tin và phân quyền hệ thống",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(22, 45),
                AutoSize = true
            });

            txtSearch = new TextBox
            {
                Location = new Point(20, 75),
                Size = new Size(240, 28),
                Font = new Font("Segoe UI", 9.5f),
                PlaceholderText = "Nhập tên hoặc tài khoản..."
            };
            Controls.Add(txtSearch);

            btnSearch = CreateButton("Tìm kiếm", 268, 73, 95, 30, Color.FromArgb(52, 120, 180));
            btnSearch.Click += BtnSearch_Click;
            Controls.Add(btnSearch);

            // 1. Panel Bảng Dữ Liệu
            tablePanel = new Panel
            {
                Location = new Point(20, 112),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(tablePanel);

            dgvAccounts = new DataGridView
            {
                Location = new Point(8, 8),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoGenerateColumns = true,
                EnableHeadersVisualStyles = false
            };

            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 75, 95);
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvAccounts.ColumnHeadersHeight = 36;
            dgvAccounts.RowTemplate.Height = 30;
            dgvAccounts.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgvAccounts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 235, 245);
            dgvAccounts.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged;

            tablePanel.Controls.Add(dgvAccounts);

            // Thanh phân trang
            btnPrevious = CreateButton("← Trang trước", 8, 0, 110, 30, Color.FromArgb(52, 120, 180));
            btnPrevious.Click += BtnPrevious_Click;
            tablePanel.Controls.Add(btnPrevious);

            lblPagination = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87),
                Height = 30
            };
            tablePanel.Controls.Add(lblPagination);

            btnNext = CreateButton("Trang sau →", 0, 0, 110, 30, Color.FromArgb(52, 120, 180));
            btnNext.Click += BtnNext_Click;
            tablePanel.Controls.Add(btnNext);

            // 2. Panel Nhập Liệu Chi Tiết (Giống form Khách hàng)
            detailPanel = new Panel
            {
                Location = new Point(20, 470),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(detailPanel);

            detailPanel.Controls.Add(new Label
            {
                Text = "Thông tin tài khoản",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(18, 12),
                AutoSize = true
            });

            CreateLabel(detailPanel, "Tài khoản", 18, 48);
            txtUsername = CreateTextBox(detailPanel, 18, 70);

            CreateLabel(detailPanel, "Mật khẩu (thêm mới)", 18, 105);
            txtPassword = CreateTextBox(detailPanel, 18, 127);
            txtPassword.UseSystemPasswordChar = true;

            CreateLabel(detailPanel, "Họ tên", 18, 162);
            txtFullName = CreateTextBox(detailPanel, 18, 184);

            CreateLabel(detailPanel, "Vai trò hệ thống", 18, 219);
            cmbRole = new ComboBox
            {
                Location = new Point(18, 241),
                Size = new Size(280, 28),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cmbRole.Items.AddRange(new object[] { "Admin", "Warehouse", "Cashier" });
            cmbRole.SelectedIndex = 2;
            detailPanel.Controls.Add(cmbRole);

            btnAdd = CreateButton("Thêm mới", 18, 285, 130, 36, Color.FromArgb(40, 167, 69));
            btnAdd.Click += BtnAdd_Click;
            detailPanel.Controls.Add(btnAdd);

            btnUpdate = CreateButton("Cập nhật", 158, 285, 130, 36, Color.FromArgb(23, 162, 184));
            btnUpdate.Click += BtnUpdate_Click;
            detailPanel.Controls.Add(btnUpdate);

            btnDelete = CreateButton("Xóa", 18, 330, 130, 36, Color.FromArgb(220, 53, 69));
            btnDelete.Click += BtnDelete_Click;
            detailPanel.Controls.Add(btnDelete);

            btnRefresh = CreateButton("Làm mới", 158, 330, 130, 36, Color.FromArgb(108, 117, 125));
            btnRefresh.Click += BtnRefresh_Click;
            detailPanel.Controls.Add(btnRefresh);

            FormAccountManagement_Resize(this, EventArgs.Empty);
        }

        private void FormAccountManagement_Resize(object? sender, EventArgs e)
        {
            if (tablePanel == null || detailPanel == null || btnPrevious == null || btnNext == null || lblPagination == null)
                return;

            int width = ClientSize.Width - 40;
            int height = ClientSize.Height - 125;

            if (width < 300 || height < 200)
                return;

            if (width >= 950)
            {
                int detailWidth = Math.Min(340, width / 3);
                int gap = 15;
                int tableWidth = width - detailWidth - gap;

                tablePanel.Location = new Point(20, 112);
                tablePanel.Size = new Size(tableWidth, Math.Max(250, height));
                detailPanel.Location = new Point(20 + tableWidth + gap, 112);
                detailPanel.Size = new Size(detailWidth, Math.Max(390, height));
                AutoScrollMinSize = Size.Empty;
                SetDetailControlWidths(detailWidth);
            }
            else
            {
                tablePanel.Location = new Point(20, 112);
                tablePanel.Size = new Size(width, 280);
                detailPanel.Location = new Point(20, 405);
                detailPanel.Size = new Size(width, 390);
                AutoScrollMinSize = new Size(0, 810);
                SetDetailControlWidths(width);
            }

            btnPrevious.Location = new Point(8, tablePanel.Height - 38);
            btnNext.Location = new Point(tablePanel.Width - 118, tablePanel.Height - 38);
            lblPagination.Location = new Point(122, tablePanel.Height - 38);
            lblPagination.Width = Math.Max(50, tablePanel.Width - 244);

            dgvAccounts.Width = tablePanel.ClientSize.Width - 16;
            dgvAccounts.Height = Math.Max(100, tablePanel.ClientSize.Height - 50);
        }

        private void SetDetailControlWidths(int panelWidth)
        {
            int inputWidth = Math.Max(150, panelWidth - 36);
            txtUsername.Width = inputWidth;
            txtPassword.Width = inputWidth;
            txtFullName.Width = inputWidth;
            cmbRole.Width = inputWidth;

            int buttonWidth = Math.Max(100, (panelWidth - 46) / 2);
            btnAdd.Width = buttonWidth;
            btnUpdate.Width = buttonWidth;
            btnDelete.Width = buttonWidth;
            btnRefresh.Width = buttonWidth;

            btnAdd.Location = new Point(18, 285);
            btnUpdate.Location = new Point(28 + buttonWidth, 285);
            btnDelete.Location = new Point(18, 330);
            btnRefresh.Location = new Point(28 + buttonWidth, 330);
        }

        private Label CreateLabel(Control parent, string text, int x, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
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
                Size = new Size(280, 27),
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            parent.Controls.Add(textBox);
            return textBox;
        }

        private Button CreateButton(string text, int x, int y, int width, int height, Color color)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private async Task LoadUserData()
        {
            try
            {
                isLoading = true;

                string url = $"users/paged?pageNumber={currentPage}&pageSize={pageSize}" +
                             (string.IsNullOrWhiteSpace(currentKeyword) ? string.Empty : "&keyword=" + Uri.EscapeDataString(currentKeyword));

                string jsonResult = await ApiClientService.GetDataWithTokenAsync(url);
                var result = JsonSerializer.Deserialize<PagedListResponse<UserModel>>(jsonResult, jsonOptions);

                var users = result?.Items ?? new List<UserModel>();
                totalCount = result?.TotalCount ?? users.Count;
                totalPages = Math.Max(1, result?.TotalPages ?? 1);

                if (currentPage > totalPages)
                {
                    currentPage = totalPages;
                    isLoading = false;
                    await LoadUserData();
                    return;
                }

                dgvAccounts.DataSource = users;
                ConfigureUserColumns();
                UpdatePaginationControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void ConfigureUserColumns()
        {
            SetColumnHeader("UserId", "Mã tài khoản");
            SetColumnHeader("Username", "Tài khoản");
            SetColumnHeader("FullName", "Họ tên");
            SetColumnHeader("Role", "Vai trò");
        }

        private void UpdatePaginationControls()
        {
            lblPagination.Text = $"Trang {currentPage}/{totalPages}  •  Tổng {totalCount} TK";
            btnPrevious.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
        }

        private async void BtnPrevious_Click(object? sender, EventArgs e)
        {
            if (currentPage <= 1) return;
            currentPage--;
            await LoadUserData();
        }

        private async void BtnNext_Click(object? sender, EventArgs e)
        {
            if (currentPage >= totalPages) return;
            currentPage++;
            await LoadUserData();
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvAccounts.Columns[columnName] != null)
                dgvAccounts.Columns[columnName]!.HeaderText = headerText;
        }

        private void DgvAccounts_SelectionChanged(object? sender, EventArgs e)
        {
            if (isLoading || dgvAccounts.SelectedRows.Count == 0)
                return;

            var row = dgvAccounts.SelectedRows[0];

            txtUsername.Text = row.Cells["Username"]?.Value?.ToString() ?? string.Empty;
            txtFullName.Text = row.Cells["FullName"]?.Value?.ToString() ?? string.Empty;

            string role = row.Cells["Role"]?.Value?.ToString() ?? "Cashier";
            cmbRole.SelectedItem = role;

            if (cmbRole.SelectedIndex < 0 && cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 0;

            txtPassword.Clear();
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                bool success = await ApiClientService.PostDataWithTokenAsync("users", newUser);

                if (success)
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    currentPage = 1;
                    await LoadUserData();
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại. Kiểm tra thông tin nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                cmbRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng nhập tài khoản, họ tên và vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            object? id = dgvAccounts.SelectedRows[0].Cells["UserId"]?.Value;
            if (id == null) return;

            var updatedUser = new
            {
                Username = txtUsername.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Role = cmbRole.SelectedItem!.ToString()
            };

            try
            {
                bool success = await ApiClientService.PutDataWithTokenAsync($"users/{id}", updatedUser);

                if (success)
                {
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadUserData();
                }
                else
                {
                    MessageBox.Show("Cập nhật tài khoản thất bại. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            object? id = dgvAccounts.SelectedRows[0].Cells["UserId"]?.Value;
            if (id == null) return;

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                bool success = await ApiClientService.DeleteDataWithTokenAsync($"users/{id}");

                if (success)
                {
                    MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    currentPage = 1;
                    await LoadUserData();
                }
                else
                {
                    MessageBox.Show("Xóa tài khoản thất bại. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnSearch_Click(object? sender, EventArgs e)
        {
            currentKeyword = txtSearch.Text.Trim();
            currentPage = 1;
            await LoadUserData();
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            currentKeyword = string.Empty;
            ClearInputs();
            currentPage = 1;
            await LoadUserData();
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cmbRole.SelectedIndex = 2;
            dgvAccounts.ClearSelection();
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