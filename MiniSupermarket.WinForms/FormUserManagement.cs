// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormUserManagement : Form
    {
        private DataGridView dgvUsers = null!;
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private TextBox txtFullName = null!;
        private ComboBox cboRole = null!;
        private ComboBox cboStatus = null!;
        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;

        public FormUserManagement()
        {
            InitializeComponent();
            Controls.Clear();
            BuildLayout();
        }

        private void BuildLayout()
        {
            Text = "Quản lý tài khoản";
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);
            Dock = DockStyle.Fill;
            AutoScroll = true;
            MinimumSize = new Size(700, 500);

            var title = new Label
            {
                Text = "Quản lý tài khoản",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 75),
                Location = new Point(24, 18),
                AutoSize = true
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = "Quản lý tên đăng nhập, thông tin và quyền truy cập",
                ForeColor = Color.DimGray,
                Location = new Point(27, 58),
                AutoSize = true
            };
            Controls.Add(subtitle);

            var editor = new Panel
            {
                Location = new Point(24, 95),
                Size = new Size(650, 190),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                Padding = new Padding(15)
            };
            Controls.Add(editor);

            AddLabel(editor, "Tên đăng nhập", 15, 15);
            txtUsername = AddTextBox(editor, 15, 40, 270);

            AddLabel(editor, "Mật khẩu", 315, 15);
            txtPassword = AddTextBox(editor, 315, 40, 280);
            txtPassword.UseSystemPasswordChar = true;

            AddLabel(editor, "Họ tên", 15, 85);
            txtFullName = AddTextBox(editor, 15, 110, 270);

            AddLabel(editor, "Vai trò", 315, 85);
            cboRole = new ComboBox
            {
                Location = new Point(315, 110),
                Size = new Size(130, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboRole.Items.AddRange(new object[] { "Admin", "Cashier", "Warehouse" });
            cboRole.SelectedIndex = 1;
            editor.Controls.Add(cboRole);

            cboStatus = new ComboBox
            {
                Location = new Point(460, 110),
                Size = new Size(135, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new object[] { "Hoạt động", "Ngừng hoạt động" });
            cboStatus.SelectedIndex = 0;
            editor.Controls.Add(cboStatus);

            btnAdd = MakeButton("Thêm", 24, 300, Color.FromArgb(25, 135, 84));
            btnUpdate = MakeButton("Cập nhật", 130, 300, Color.FromArgb(13, 110, 253));
            btnDelete = MakeButton("Xóa", 250, 300, Color.FromArgb(220, 53, 69));
            btnRefresh = MakeButton("Làm mới", 345, 300, Color.FromArgb(108, 117, 125));

            Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete, btnRefresh });

            dgvUsers = new DataGridView
            {
                Location = new Point(24, 355),
                Size = new Size(650, 260),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false
            };
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 75, 95);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 235, 245);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;
            Controls.Add(dgvUsers);

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;

            LoadDemoRows();
        }

        private static void AddLabel(Control parent, string text, int x, int y)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.FromArgb(55, 65, 75)
            });
        }

        private static TextBox AddTextBox(Control parent, int x, int y, int width)
        {
            var box = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 30),
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(box);
            return box;
        }

        private Button MakeButton(string text, int x, int y, Color color)
        {
            var button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(text == "Cập nhật" ? 110 : 90, 38),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LoadDemoRows()
        {
            dgvUsers.DataSource = null;
            dgvUsers.Columns.Clear();
            dgvUsers.Rows.Clear();
            dgvUsers.Columns.Add("Username", "Tên đăng nhập");
            dgvUsers.Columns.Add("FullName", "Họ tên");
            dgvUsers.Columns.Add("Role", "Vai trò");
            dgvUsers.Columns.Add("Status", "Trạng thái");

            dgvUsers.Rows.Add("admin01", "Nguyễn Minh Anh", "Admin", "Hoạt động");
            dgvUsers.Rows.Add("cashier01", "Phạm Ngọc Lan", "Cashier", "Hoạt động");
            dgvUsers.Rows.Add("warehouse01", "Nguyễn Thành Đạt", "Warehouse", "Hoạt động");
        }

        private void DgvUsers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.IsNewRow)
                return;

            txtUsername.Text = Convert.ToString(dgvUsers.CurrentRow.Cells["Username"].Value) ?? "";
            txtFullName.Text = Convert.ToString(dgvUsers.CurrentRow.Cells["FullName"].Value) ?? "";

            var role = Convert.ToString(dgvUsers.CurrentRow.Cells["Role"].Value);
            if (role != null && cboRole.Items.Contains(role))
                cboRole.SelectedItem = role;

            var status = Convert.ToString(dgvUsers.CurrentRow.Cells["Status"].Value);
            if (status != null && cboStatus.Items.Contains(status))
                cboStatus.SelectedItem = status;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập, mật khẩu và họ tên.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                if (string.Equals(Convert.ToString(row.Cells["Username"].Value),
                    txtUsername.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại trong danh sách.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            dgvUsers.Rows.Add(
                txtUsername.Text.Trim(),
                txtFullName.Text.Trim(),
                cboRole.Text,
                cboStatus.Text);

            ClearInputs();
            MessageBox.Show("Đã thêm vào danh sách hiện tại. Dữ liệu chưa được lưu vào database/API.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần cập nhật.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Tên đăng nhập và họ tên không được để trống.");
                return;
            }

            var row = dgvUsers.CurrentRow;
            row.Cells["Username"].Value = txtUsername.Text.Trim();
            row.Cells["FullName"].Value = txtFullName.Text.Trim();
            row.Cells["Role"].Value = cboRole.Text;
            row.Cells["Status"].Value = cboStatus.Text;

            MessageBox.Show("Đã cập nhật trong danh sách hiện tại; chưa lưu vào database/API.");
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null || dgvUsers.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            dgvUsers.Rows.Remove(dgvUsers.CurrentRow);
            ClearInputs();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadDemoRows();
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cboRole.SelectedIndex = 1;
            cboStatus.SelectedIndex = 0;
        }

        private void FormUserManagement_Load(object sender, EventArgs e)
        {
            // Đã khởi tạo giao diện trong BuildLayout().
        }
    }
}