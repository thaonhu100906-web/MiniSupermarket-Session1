
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormCategoryManagement : Form
    {
        private DataGridView dgvCategory = null!;
        private TextBox txtCategoryName = null!;
        private TextBox txtDescription = null!;
        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;

        private readonly Color BackgroundColor = Color.FromArgb(244, 246, 249);
        private readonly Color PrimaryColor = Color.FromArgb(41, 100, 180);
        private readonly Color TextColor = Color.FromArgb(35, 42, 55);

        public FormCategoryManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();
            LoadCategoryData();
        }

        private void InitializeCustomLayout()
        {
            Text = "Quản lý Danh mục";
            BackColor = BackgroundColor;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            MinimumSize = new Size(700, 450);

            Label lblTitle = new Label
            {
                Text = "📁  QUẢN LÝ DANH MỤC",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(20, 15)
            };

            Label lblSubtitle = new Label
            {
                Text = "Quản lý nhóm hàng và danh mục sản phẩm trong hệ thống",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(110, 118, 130),
                AutoSize = true,
                Location = new Point(23, 50)
            };

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);

            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Location = new Point(20, 85),
                Size = new Size(ClientSize.Width - 40, ClientSize.Height - 105),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = BackgroundColor,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel tablePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 10, 0)
            };

            Label lblList = new Label
            {
                Text = "Danh sách danh mục",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 35
            };

            dgvCategory = new DataGridView
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
                Font = new Font("Segoe UI", 9.5F),
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            };

            dgvCategory.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };

            dgvCategory.EnableHeadersVisualStyles = false;
            dgvCategory.RowTemplate.Height = 35;
            dgvCategory.SelectionChanged += DgvCategory_SelectionChanged;

            tablePanel.Controls.Add(dgvCategory);
            tablePanel.Controls.Add(lblList);

            Panel detailPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(18),
                Margin = new Padding(0)
            };

            Label lblDetail = new Label
            {
                Text = "Thông tin danh mục",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(18, 18),
                AutoSize = true
            };

            Label lblName = new Label
            {
                Text = "Tên danh mục",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(18, 65),
                AutoSize = true
            };

            txtCategoryName = new TextBox
            {
                Location = new Point(18, 90),
                Width = 250,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 10F)
            };

            Label lblDesc = new Label
            {
                Text = "Mô tả",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(18, 135),
                AutoSize = true
            };

            txtDescription = new TextBox
            {
                Location = new Point(18, 160),
                Width = 250,
                Height = 85,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 10F)
            };

            btnAdd = CreateActionButton("＋  Thêm mới", Color.FromArgb(40, 167, 69));
            btnUpdate = CreateActionButton("✎  Cập nhật", Color.FromArgb(41, 100, 180));
            btnDelete = CreateActionButton("✕  Xóa", Color.FromArgb(220, 53, 69));
            btnRefresh = CreateActionButton("↻  Làm mới", Color.FromArgb(108, 117, 125));

            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnAdd.Location = new Point(18, 275);
            btnUpdate.Location = new Point(18, 325);
            btnDelete.Location = new Point(18, 375);
            btnRefresh.Location = new Point(18, 425);

            detailPanel.SizeChanged += (s, e) =>
            {
                int buttonWidth = Math.Max(100, detailPanel.ClientSize.Width - 36);
                btnAdd.Width = buttonWidth;
                btnUpdate.Width = buttonWidth;
                btnDelete.Width = buttonWidth;
                btnRefresh.Width = buttonWidth;
                txtCategoryName.Width = buttonWidth;
                txtDescription.Width = buttonWidth;
            };

            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;

            detailPanel.Controls.Add(lblDetail);
            detailPanel.Controls.Add(lblName);
            detailPanel.Controls.Add(txtCategoryName);
            detailPanel.Controls.Add(lblDesc);
            detailPanel.Controls.Add(txtDescription);
            detailPanel.Controls.Add(btnAdd);
            detailPanel.Controls.Add(btnUpdate);
            detailPanel.Controls.Add(btnDelete);
            detailPanel.Controls.Add(btnRefresh);

            contentLayout.Controls.Add(tablePanel, 0, 0);
            contentLayout.Controls.Add(detailPanel, 1, 0);
            Controls.Add(contentLayout);
        }

        private Button CreateActionButton(string text, Color color)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(145, 42),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private async void LoadCategoryData()
        {
            try
            {
                string jsonResult = await ApiClientService.GetDataWithTokenAsync("categories");

                var categories = JsonSerializer.Deserialize<List<CategoryModel>>(
                    jsonResult,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                dgvCategory.DataSource = categories ?? new List<CategoryModel>();

                if (dgvCategory.Columns["CategoryId"] != null)
                    dgvCategory.Columns["CategoryId"]!.HeaderText = "Mã danh mục";

                if (dgvCategory.Columns["CategoryName"] != null)
                    dgvCategory.Columns["CategoryName"]!.HeaderText = "Tên danh mục";

                if (dgvCategory.Columns["Description"] != null)
                    dgvCategory.Columns["Description"]!.HeaderText = "Mô tả";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh mục: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DgvCategory_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvCategory.SelectedRows.Count == 0)
                return;

            var row = dgvCategory.SelectedRows[0];

            txtCategoryName.Text = row.Cells["CategoryName"].Value?.ToString() ?? string.Empty;
            txtDescription.Text = row.Cells["Description"].Value?.ToString() ?? string.Empty;
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newCat = new
            {
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            try
            {
                bool success = await ApiClientService.PostDataWithTokenAsync("categories", newCat);

                if (success)
                {
                    MessageBox.Show("Thêm danh mục thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadCategoryData();
                }
                else
                {
                    MessageBox.Show("Thêm danh mục thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm danh mục: " + ex.Message);
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvCategory.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần cập nhật!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!");
                return;
            }

            var catId = dgvCategory.SelectedRows[0].Cells["CategoryId"].Value;

            var updatedCat = new
            {
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            try
            {
                bool success = await ApiClientService.PutDataWithTokenAsync(
                    $"categories/{catId}", updatedCat);

                if (success)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategoryData();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật danh mục: " + ex.Message);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvCategory.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var catId = dgvCategory.SelectedRows[0].Cells["CategoryId"].Value;

            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa danh mục này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                // Dùng hàm hiện có của ApiClientService để tránh phụ thuộc kiểu trả về.
                bool success = await ApiClientService.DeleteDataWithTokenAsync(
                    $"categories/{catId}");

                if (success)
                {
                    MessageBox.Show("Xóa danh mục thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadCategoryData();
                }
                else
                {
                    // ApiClientService hiện chỉ trả về true/false,
                    // nên không thể biết chính xác API trả về mã 400 hay lỗi khác.
                    MessageBox.Show(
                        "Không thể xóa danh mục vì vẫn còn sản phẩm!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa danh mục: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            LoadCategoryData();
        }

        private void ClearInputs()
        {
            txtCategoryName.Clear();
            txtDescription.Clear();
        }

        private void FormCategoryManagement_Load(object sender, EventArgs e)
        {
        }
    }

    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}