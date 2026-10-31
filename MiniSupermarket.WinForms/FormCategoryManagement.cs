// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private Button btnPagePrevious = null!;
        private Button btnPageNext = null!;
        private Label lblPageInfo = null!;
        private Panel tablePanel = null!;
        private Panel detailPanel = null!;

        private int currentPage = 1;
        private const int pageSize = 10;
        private int totalPages = 1;
        private int totalCount = 0;
        private bool loadingRows = false;

        private readonly Color BackgroundColor = Color.FromArgb(244, 246, 249);
        private readonly Color TextColor = Color.FromArgb(35, 42, 55);

        public FormCategoryManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();
            DailyFreshTheme.Apply(this);

            Load += FormCategoryManagement_Load;
            Resize += FormCategoryManagement_Resize;
        }

        private async void FormCategoryManagement_Load(object? sender, EventArgs e)
        {
            await LoadCategoryDataAsync();
        }

        private void InitializeCustomLayout()
        {
            Text = "Quản lý Danh mục";
            BackColor = BackgroundColor;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            AutoScroll = true;
            MinimumSize = new Size(700, 450);

            Label lblTitle = new Label
            {
                Text = "📁 QUẢN LÝ DANH MỤC",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
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
                Location = new Point(22, 45)
            };

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);

            // 1. Panel Bảng Dữ Liệu
            tablePanel = new Panel
            {
                Location = new Point(20, 85),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(tablePanel);

            Label lblList = new Label
            {
                Text = "Danh sách danh mục",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 35,
                Padding = new Padding(8, 5, 0, 0)
            };

            dgvCategory = new DataGridView
            {
                Location = new Point(8, 40),
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
                Font = new Font("Segoe UI", 9.5F)
            };

            dgvCategory.RowTemplate.Height = 35;
            dgvCategory.SelectionChanged += DgvCategory_SelectionChanged;
            tablePanel.Controls.Add(dgvCategory);

            // Thanh phân trang
            var paginationPanel = new Panel { Height = 42, BackColor = Color.White, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            btnPagePrevious = CreateButton("← Trang trước", 8, 5, 110, 30, DailyFreshTheme.Sage);
            btnPageNext = CreateButton("Trang sau →", 0, 5, 110, 30, DailyFreshTheme.Sage);
            btnPagePrevious.Enabled = false;
            btnPageNext.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            lblPageInfo = new Label { Text = "Trang 1/1", TextAlign = ContentAlignment.MiddleCenter, AutoSize = false, Height = 30, Top = 5 };

            btnPagePrevious.Click += async (sender, args) => { if (currentPage > 1) { currentPage--; await LoadCategoryDataAsync(); } };
            btnPageNext.Click += async (sender, args) => { if (currentPage < totalPages) { currentPage++; await LoadCategoryDataAsync(); } };

            paginationPanel.Controls.Add(lblPageInfo);
            paginationPanel.Controls.Add(btnPagePrevious);
            paginationPanel.Controls.Add(btnPageNext);
            paginationPanel.Resize += (sender, args) =>
            {
                btnPageNext.Left = Math.Max(120, paginationPanel.ClientSize.Width - btnPageNext.Width - 8);
                lblPageInfo.Left = btnPagePrevious.Right + 4;
                lblPageInfo.Width = Math.Max(40, btnPageNext.Left - lblPageInfo.Left - 4);
                lblPageInfo.BringToFront();
                btnPagePrevious.BringToFront();
                btnPageNext.BringToFront();
            };
            tablePanel.Controls.Add(paginationPanel);
            tablePanel.Controls.Add(lblList);

            // 2. Panel Nhập Liệu Chi Tiết (Bên phải, chuẩn 2x2 nút bấm)
            detailPanel = new Panel
            {
                Location = new Point(20, 410),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(detailPanel);

            Label lblDetail = new Label
            {
                Text = "Thông tin danh mục",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(18, 12),
                AutoSize = true
            };

            Label lblName = new Label
            {
                Text = "Tên danh mục",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87),
                Location = new Point(18, 48),
                AutoSize = true
            };

            txtCategoryName = new TextBox
            {
                Location = new Point(18, 70),
                Size = new Size(280, 27),
                Font = new Font("Segoe UI", 10F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblDesc = new Label
            {
                Text = "Mô tả",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87),
                Location = new Point(18, 105),
                AutoSize = true
            };

            txtDescription = new TextBox
            {
                Location = new Point(18, 127),
                Size = new Size(280, 75),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            btnAdd = CreateButton("Thêm mới", 18, 220, 130, 36, Color.FromArgb(40, 167, 69));
            btnUpdate = CreateButton("Cập nhật", 158, 220, 130, 36, Color.FromArgb(23, 162, 184));
            btnDelete = CreateButton("Xóa", 18, 265, 130, 36, Color.FromArgb(220, 53, 69));
            btnRefresh = CreateButton("Làm mới", 158, 265, 130, 36, Color.FromArgb(108, 117, 125));

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

            FormCategoryManagement_Resize(this, EventArgs.Empty);
        }

        private void FormCategoryManagement_Resize(object? sender, EventArgs e)
        {
            if (tablePanel == null || detailPanel == null || btnPagePrevious == null || btnPageNext == null || lblPageInfo == null)
                return;

            int width = ClientSize.Width - 40;
            int height = ClientSize.Height - 100;

            if (width < 300 || height < 200)
                return;

            if (width >= 950)
            {
                int detailWidth = Math.Min(340, width / 3);
                int gap = 15;
                int tableWidth = width - detailWidth - gap;

                tablePanel.Location = new Point(20, 85);
                tablePanel.Size = new Size(tableWidth, Math.Max(300, height));

                detailPanel.Location = new Point(20 + tableWidth + gap, 85);
                detailPanel.Size = new Size(detailWidth, Math.Max(340, height));

                AutoScrollMinSize = Size.Empty;
                SetDetailControlWidths(detailWidth);
            }
            else
            {
                tablePanel.Location = new Point(20, 85);
                tablePanel.Size = new Size(width, 300);

                detailPanel.Location = new Point(20, 400);
                detailPanel.Size = new Size(width, 335);

                AutoScrollMinSize = new Size(0, 760);
                SetDetailControlWidths(width);
            }

            var paginationPanel = tablePanel.Controls.OfType<Panel>().FirstOrDefault();
            if (paginationPanel != null)
            {
                paginationPanel.SetBounds(8, tablePanel.ClientSize.Height - 46, tablePanel.ClientSize.Width - 16, 42);
            }

            dgvCategory.Width = tablePanel.ClientSize.Width - 16;
            dgvCategory.Height = Math.Max(100, tablePanel.ClientSize.Height - 90);
        }

        private void SetDetailControlWidths(int panelWidth)
        {
            int inputWidth = Math.Max(150, panelWidth - 36);
            txtCategoryName.Width = inputWidth;
            txtDescription.Width = inputWidth;

            int buttonWidth = Math.Max(100, (panelWidth - 46) / 2);
            btnAdd.Width = buttonWidth;
            btnUpdate.Width = buttonWidth;
            btnDelete.Width = buttonWidth;
            btnRefresh.Width = buttonWidth;

            btnAdd.Location = new Point(18, 220);
            btnUpdate.Location = new Point(28 + buttonWidth, 220);
            btnDelete.Location = new Point(18, 265);
            btnRefresh.Location = new Point(28 + buttonWidth, 265);
        }

        private Button CreateButton(string text, int x, int y, int width, int height, Color color)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private async Task LoadCategoryDataAsync()
        {
            try
            {
                loadingRows = true;
                string jsonResult = await ApiClientService.GetDataWithTokenAsync(
                    $"categories/paged?pageNumber={currentPage}&pageSize={pageSize}");

                var result = JsonSerializer.Deserialize<PagedListResponse<CategoryModel>>(jsonResult,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var categories = result?.Items ?? new List<CategoryModel>();
                totalCount = result?.TotalCount ?? categories.Count;
                totalPages = Math.Max(1, result?.TotalPages ?? 1);

                if (currentPage > totalPages)
                {
                    currentPage = totalPages;
                    loadingRows = false;
                    await LoadCategoryDataAsync();
                    return;
                }

                dgvCategory.DataSource = categories;
                lblPageInfo.Text = $"Trang {currentPage}/{totalPages}  •  Tổng {totalCount} danh mục";
                btnPagePrevious.Enabled = currentPage > 1;
                btnPageNext.Enabled = currentPage < totalPages;

                if (dgvCategory.Columns["CategoryId"] != null)
                    dgvCategory.Columns["CategoryId"]!.HeaderText = "Mã danh mục";

                if (dgvCategory.Columns["CategoryName"] != null)
                    dgvCategory.Columns["CategoryName"]!.HeaderText = "Tên danh mục";

                if (dgvCategory.Columns["Description"] != null)
                    dgvCategory.Columns["Description"]!.HeaderText = "Mô tả";

                // Đã xóa bỏ đoạn gán ép màu thủ công để hệ thống tự nhận diện theo chuẩn theme chung
                dgvCategory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 235, 245);
                dgvCategory.DefaultCellStyle.SelectionForeColor = Color.Black;

                dgvCategory.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh mục: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally { loadingRows = false; }
        }

        private void DgvCategory_SelectionChanged(object? sender, EventArgs e)
        {
            if (loadingRows || dgvCategory.SelectedRows.Count == 0)
                return;

            var row = dgvCategory.SelectedRows[0];

            txtCategoryName.Text =
                row.Cells["CategoryName"]?.Value?.ToString() ?? string.Empty;

            txtDescription.Text =
                row.Cells["Description"]?.Value?.ToString() ?? string.Empty;
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên danh mục!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var newCat = new
            {
                CategoryName = txtCategoryName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            try
            {
                var result = await ApiClientService.PostDataWithTokenMessageAsync(
                    "categories", newCat);

                if (result.Success)
                {
                    MessageBox.Show(
                        result.Message,
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ClearInputs();
                    currentPage = 1;
                    await LoadCategoryDataAsync();
                }
                else
                {
                    MessageBox.Show(
                        result.Message,
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvCategory.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn danh mục cần cập nhật!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var catId = dgvCategory.SelectedRows[0].Cells["CategoryId"]?.Value;
            if (catId == null) return;

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
                    MessageBox.Show(
                        "Cập nhật thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    await LoadCategoryDataAsync();
                }
                else
                {
                    MessageBox.Show(
                        "Cập nhật thất bại!",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvCategory.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn danh mục cần xóa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var catId = dgvCategory.SelectedRows[0].Cells["CategoryId"]?.Value;
            if (catId == null) return;

            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa danh mục này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                bool success = await ApiClientService.DeleteDataWithTokenAsync(
                    $"categories/{catId}");

                if (success)
                {
                    MessageBox.Show(
                        "Xóa danh mục thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ClearInputs();
                    currentPage = 1;
                    await LoadCategoryDataAsync();
                }
                else
                {
                    MessageBox.Show(
                        "Không thể xóa danh mục vì vẫn còn sản phẩm!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi xóa danh mục: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            currentPage = 1;
            await LoadCategoryDataAsync();
        }

        private void ClearInputs()
        {
            txtCategoryName.Clear();
            txtDescription.Clear();
            dgvCategory.ClearSelection();
        }
    }

    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}