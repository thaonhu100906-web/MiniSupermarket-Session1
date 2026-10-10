
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
    public partial class FormEmployeeManagement : Form
    {
        private DataGridView dgvEmployee = null!;
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

        private readonly Color backgroundColor = Color.FromArgb(244, 246, 249);
        private readonly Color primaryColor = Color.FromArgb(35, 75, 95);
        private readonly Color textColor = Color.FromArgb(33, 37, 41);

        public FormEmployeeManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();
            LoadEmployeeData();
        }

        private void InitializeCustomLayout()
        {
            Text = "Quản lý nhân viên";
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BackColor = backgroundColor;
            AutoScroll = true;
            MinimumSize = new Size(650, 450);

            var title = new Label
            {
                Text = "QUẢN LÝ NHÂN VIÊN",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(20, 15),
                AutoSize = true
            };

            var subtitle = new Label
            {
                Text = "Quản lý tài khoản, thông tin và vai trò nhân viên",
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
                Text = "Danh sách nhân viên",
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = textColor
            };

            dgvEmployee = new DataGridView
            {
                Dock = DockStyle.Fill,
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

            dgvEmployee.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvEmployee.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmployee.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);
            dgvEmployee.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 235, 245);
            dgvEmployee.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvEmployee.SelectionChanged += DgvEmployee_SelectionChanged;

            tablePanel.Controls.Add(dgvEmployee);
            tablePanel.Controls.Add(lblList);
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
                Text = "Thông tin nhân viên",
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

            Resize += FormEmployeeManagement_Resize;
            FormEmployeeManagement_Resize(this, EventArgs.Empty);
        }

        private void FormEmployeeManagement_Resize(object? sender, EventArgs e)
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

        private async void LoadEmployeeData()
        {
            try
            {
                string jsonResult =
                    await ApiClientService.GetDataWithTokenAsync("employees");

                var employees = JsonSerializer.Deserialize<List<EmployeeModel>>(
                    jsonResult,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                dgvEmployee.DataSource = employees ?? new List<EmployeeModel>();

                SetColumnHeader("EmployeeId", "Mã NV");
                SetColumnHeader("Username", "Tài khoản");
                SetColumnHeader("FullName", "Họ tên");
                SetColumnHeader("Role", "Vai trò");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách nhân viên: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dgvEmployee.Columns[columnName] != null)
                dgvEmployee.Columns[columnName]!.HeaderText = headerText;
        }

        private void DgvEmployee_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvEmployee.SelectedRows.Count == 0)
                return;

            var row = dgvEmployee.SelectedRows[0];

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
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhân viên!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn vai trò!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newEmployee = new
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text,
                FullName = txtFullName.Text.Trim(),
                Role = cmbRole.SelectedItem!.ToString()
            };

            try
            {
                bool success = await ApiClientService.PostDataWithTokenAsync(
                    "employees", newEmployee);

                if (success)
                {
                    MessageBox.Show("Thêm nhân viên thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại. Kiểm tra thông tin nhập!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message);
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvEmployee.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                cmbRole.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng nhập tài khoản, họ tên và vai trò!");
                return;
            }

            object? id = dgvEmployee.SelectedRows[0].Cells["EmployeeId"].Value;
            if (id == null)
                return;

            var updatedEmployee = new
            {
                Username = txtUsername.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Role = cmbRole.SelectedItem!.ToString()
            };

            try
            {
                bool success = await ApiClientService.PutDataWithTokenAsync(
                    $"employees/{id}", updatedEmployee);

                if (success)
                {
                    MessageBox.Show("Cập nhật nhân viên thành công!");
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show("Cập nhật nhân viên thất bại. Vui lòng kiểm tra lại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật nhân viên: " + ex.Message);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvEmployee.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!");
                return;
            }

            object? id = dgvEmployee.SelectedRows[0].Cells["EmployeeId"].Value;
            if (id == null)
                return;

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhân viên này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool success = await ApiClientService.DeleteDataWithTokenAsync(
                    $"employees/{id}");

                if (success)
                {
                    MessageBox.Show("Xóa nhân viên thành công!");
                    ClearInputs();
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show("Xóa nhân viên thất bại. Vui lòng kiểm tra lại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa nhân viên: " + ex.Message);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            LoadEmployeeData();
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cmbRole.SelectedIndex = -1;
            dgvEmployee.ClearSelection();
        }

        private void FormEmployeeManagement_Load(object sender, EventArgs e)
        {
        }
    }

    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}