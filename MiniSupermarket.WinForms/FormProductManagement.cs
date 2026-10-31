
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
    public partial class FormProductManagement : Form
    {
        private DataGridView dgvProduct = null!;
        private TextBox txtBarcode = null!;
        private TextBox txtName = null!;
        private TextBox txtPrice = null!;
        private TextBox txtStock = null!;
        private ComboBox cmbCategory = null!;
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

        public FormProductManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();
            DailyFreshTheme.Apply(this);

            Load += FormProductManagement_Load;
            Resize += FormProductManagement_Resize;
        }

        private async void FormProductManagement_Load(object? sender, EventArgs e)
        {
            await LoadCategoryData();
            await LoadProductData();
        }

        private void InitializeCustomLayout()
        {
            Text = "Quản lý sản phẩm";
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(244, 246, 248);
            AutoScroll = true;

            Controls.Add(new Label
            {
                Text = "Quản lý sản phẩm",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 15),
                AutoSize = true
            });

            Controls.Add(new Label
            {
                Text = "Quản lý sản phẩm, giá bán, tồn kho và danh mục",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(27, 53),
                AutoSize = true
            });

            txtSearch = new TextBox
            {
                Location = new Point(25, 78),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Nhập tên hoặc mã vạch..."
            };
            Controls.Add(txtSearch);

            btnSearch = CreateButton("Tìm kiếm", 285, 76, 105, 32,
                Color.FromArgb(52, 120, 180));
            btnSearch.Click += BtnSearch_Click;
            Controls.Add(btnSearch);

            tablePanel = new Panel
            {
                Location = new Point(25, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(tablePanel);

            dgvProduct = new DataGridView
            {
                Dock = DockStyle.None,
                Location = new Point(8, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Left |
             AnchorStyles.Right,
                Width = tablePanel.ClientSize.Width - 16,
                Height = tablePanel.ClientSize.Height - 58,
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
                RowTemplate = { Height = 32 },
                EnableHeadersVisualStyles = false
            };

            dgvProduct.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(35, 75, 95);
            dgvProduct.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProduct.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);
            dgvProduct.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvProduct.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 235, 245);
            dgvProduct.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvProduct.SelectionChanged += DgvProduct_SelectionChanged;
            dgvProduct.CellFormatting += DgvProduct_CellFormatting;
            tablePanel.Padding = new Padding(8);
            tablePanel.Controls.Add(dgvProduct);

            btnPrevious = CreateButton("← Trang trước", 8, 0, 120, 32,
                Color.FromArgb(52, 120, 180));
            btnPrevious.Click += BtnPrevious_Click;
            tablePanel.Controls.Add(btnPrevious);

            lblPagination = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87),
                Height = 32
            };
            tablePanel.Controls.Add(lblPagination);

            btnNext = CreateButton("Trang sau →", 0, 0, 120, 32,
                Color.FromArgb(52, 120, 180));
            btnNext.Click += BtnNext_Click;
            tablePanel.Controls.Add(btnNext);

            detailPanel = new Panel
            {
                Location = new Point(25, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(detailPanel);

            detailPanel.Controls.Add(new Label
            {
                Text = "Thông tin sản phẩm",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(18, 12),
                AutoSize = true
            });

            CreateLabel(detailPanel, "Mã vạch", 18, 48);
            txtBarcode = CreateTextBox(detailPanel, 18, 70);

            CreateLabel(detailPanel, "Tên sản phẩm", 18, 105);
            txtName = CreateTextBox(detailPanel, 18, 127);

            CreateLabel(detailPanel, "Đơn giá", 18, 162);
            txtPrice = CreateTextBox(detailPanel, 18, 184);

            CreateLabel(detailPanel, "Tồn kho", 18, 219);
            txtStock = CreateTextBox(detailPanel, 18, 241);

            CreateLabel(detailPanel, "Danh mục", 18, 276);
            cmbCategory = new ComboBox
            {
                Location = new Point(18, 298),
                Size = new Size(280, 28),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left |
                         AnchorStyles.Right,
                DisplayMember = "CategoryName",
                ValueMember = "CategoryId"
            };
            detailPanel.Controls.Add(cmbCategory);

            btnAdd = CreateButton("Thêm mới", 18, 340, 130, 36,
                Color.FromArgb(40, 167, 69));
            btnAdd.Click += BtnAdd_Click;
            detailPanel.Controls.Add(btnAdd);

            btnUpdate = CreateButton("Cập nhật", 158, 340, 130, 36,
                Color.FromArgb(23, 162, 184));
            btnUpdate.Click += BtnUpdate_Click;
            detailPanel.Controls.Add(btnUpdate);

            btnDelete = CreateButton("Xóa", 18, 385, 130, 36,
                Color.FromArgb(220, 53, 69));
            btnDelete.Click += BtnDelete_Click;
            detailPanel.Controls.Add(btnDelete);

            btnRefresh = CreateButton("Làm mới", 158, 385, 130, 36,
                Color.FromArgb(108, 117, 125));
            btnRefresh.Click += BtnRefresh_Click;
            detailPanel.Controls.Add(btnRefresh);

            FormProductManagement_Resize(this, EventArgs.Empty);
        }

        private void FormProductManagement_Resize(object? sender, EventArgs e)
        {
            if (tablePanel == null || detailPanel == null ||
                btnPrevious == null || btnNext == null || lblPagination == null)
                return;

            int width = ClientSize.Width - 50;
            int height = ClientSize.Height - 145;

            if (width < 300 || height < 200)
                return;

            if (width >= 1000)
            {
                int detailWidth = Math.Min(340, width / 3);
                int gap = 15;
                int tableWidth = width - detailWidth - gap;

                tablePanel.Location = new Point(25, 120);
                tablePanel.Size = new Size(tableWidth, Math.Max(250, height));
                detailPanel.Location = new Point(25 + tableWidth + gap, 120);
                detailPanel.Size = new Size(detailWidth, Math.Max(435, height));
                AutoScrollMinSize = Size.Empty;
                SetDetailControlWidths(detailWidth);
            }
            else
            {
                tablePanel.Location = new Point(25, 120);
                tablePanel.Size = new Size(width, 340);
                detailPanel.Location = new Point(25, 475);
                detailPanel.Size = new Size(width, 440);
                AutoScrollMinSize = new Size(0, 930);
                SetDetailControlWidths(width);
            }

            btnPrevious.Location = new Point(8, tablePanel.Height - 42);
            btnNext.Location = new Point(tablePanel.Width - 136,
                tablePanel.Height - 42);
            lblPagination.Location = new Point(132, tablePanel.Height - 42);
            lblPagination.Width = Math.Max(50, tablePanel.Width - 276);

            dgvProduct.Width = tablePanel.ClientSize.Width - 16;
            dgvProduct.Height = Math.Max(100, tablePanel.ClientSize.Height - 58);
        }

        private void SetDetailControlWidths(int panelWidth)
        {
            int inputWidth = Math.Max(150, panelWidth - 45);
            txtBarcode.Width = inputWidth;
            txtName.Width = inputWidth;
            txtPrice.Width = inputWidth;
            txtStock.Width = inputWidth;
            cmbCategory.Width = inputWidth;

            int buttonWidth = Math.Max(100, (panelWidth - 55) / 2);
            btnAdd.Width = buttonWidth;
            btnUpdate.Width = buttonWidth;
            btnDelete.Width = buttonWidth;
            btnRefresh.Width = buttonWidth;

            btnAdd.Location = new Point(18, 340);
            btnUpdate.Location = new Point(28 + buttonWidth, 340);
            btnDelete.Location = new Point(18, 385);
            btnRefresh.Location = new Point(28 + buttonWidth, 385);
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
                Size = new Size(280, 27),
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left |
                         AnchorStyles.Right
            };
            parent.Controls.Add(textBox);
            return textBox;
        }

        private Button CreateButton(string text, int x, int y, int width,
            int height, Color color)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
        }

        private async Task LoadCategoryData()
        {
            try
            {
                string json = await ApiClientService.GetDataWithTokenAsync("categories");
                var categories = JsonSerializer.Deserialize<List<CategoryModel>>(
                    json, jsonOptions) ?? new List<CategoryModel>();

                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh mục: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductData()
        {
            try
            {
                isLoading = true;

                string json = await ApiClientService.GetDataWithTokenAsync(
                    $"products/paged?pageNumber={currentPage}&pageSize={pageSize}" +
                    (string.IsNullOrWhiteSpace(currentKeyword) ? string.Empty : "&keyword=" + Uri.EscapeDataString(currentKeyword)));

                var result = JsonSerializer.Deserialize<PagedProductResponse>(
                    json, jsonOptions);

                var products = result?.Items ?? new List<ProductModel>();
                totalCount = result?.TotalCount ?? products.Count;
                totalPages = Math.Max(1, result?.TotalPages ?? 1);

                if (currentPage > totalPages)
                {
                    currentPage = totalPages;
                    isLoading = false;
                    await LoadProductData();
                    return;
                }

                dgvProduct.DataSource = products;
                ConfigureProductColumns();
                UpdatePaginationControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách sản phẩm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
            }
        }


        // Tiêu chí 43: cảnh báo tồn kho thấp trực tiếp trên lưới.
        private void DgvProduct_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 ||
                dgvProduct.Columns[e.ColumnIndex].Name != "StockQuantity") return;
            if (int.TryParse(Convert.ToString(e.Value), out int stock) && stock <= 10)
            {
                e.CellStyle.BackColor = Color.LightCoral;
                e.CellStyle.ForeColor = Color.DarkRed;
                e.CellStyle.SelectionBackColor = Color.IndianRed;
                e.CellStyle.SelectionForeColor = Color.White;
            }
        }

        private void ConfigureProductColumns()
        {
            SetColumnHeader("ProductId", "Mã SP");
            SetColumnHeader("Barcode", "Mã vạch");
            SetColumnHeader("ProductName", "Tên sản phẩm");
            SetColumnHeader("Price", "Đơn giá");
            SetColumnHeader("StockQuantity", "Tồn kho");
            SetColumnHeader("CategoryName", "Tên danh mục");
            SetColumnHeader("CategoryId", "Mã danh mục");

            if (dgvProduct.Columns["Price"] != null)
                dgvProduct.Columns["Price"]!.DefaultCellStyle.Format = "N0";

            if (dgvProduct.Columns["CategoryId"] != null)
                dgvProduct.Columns["CategoryId"]!.Visible = false;

            if (dgvProduct.Columns["Category"] != null)
                dgvProduct.Columns["Category"]!.Visible = false;
        }

        private void UpdatePaginationControls()
        {
            lblPagination.Text =
                $"Trang {currentPage}/{totalPages}  •  Tổng {totalCount} sản phẩm";
            btnPrevious.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
        }

        private async void BtnPrevious_Click(object? sender, EventArgs e)
        {
            if (currentPage <= 1)
                return;

            currentPage--;
            await LoadProductData();
        }

        private async void BtnNext_Click(object? sender, EventArgs e)
        {
            if (currentPage >= totalPages)
                return;

            currentPage++;
            await LoadProductData();
        }

        private void SetColumnHeader(string columnName, string header)
        {
            if (dgvProduct.Columns[columnName] != null)
                dgvProduct.Columns[columnName]!.HeaderText = header;
        }

        private void DgvProduct_SelectionChanged(object? sender, EventArgs e)
        {
            if (isLoading || dgvProduct.SelectedRows.Count == 0)
                return;

            var row = dgvProduct.SelectedRows[0];

            txtBarcode.Text = row.Cells["Barcode"]?.Value?.ToString() ?? "";
            txtName.Text = row.Cells["ProductName"]?.Value?.ToString() ?? "";
            txtPrice.Text = row.Cells["Price"]?.Value?.ToString() ?? "";
            txtStock.Text = row.Cells["StockQuantity"]?.Value?.ToString() ?? "";

            object? categoryId = row.Cells["CategoryId"]?.Value;
            if (categoryId != null &&
                int.TryParse(categoryId.ToString(), out int id))
                cmbCategory.SelectedValue = id;
            else
                cmbCategory.SelectedIndex = -1;
        }

        private bool ValidateInputs(out decimal price, out int stock,
            out int categoryId)
        {
            price = 0;
            stock = 0;
            categoryId = 0;

            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã vạch sản phẩm!");
                txtBarcode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!");
                txtName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out price) || price < 0)
            {
                MessageBox.Show("Đơn giá phải là số không âm!");
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtStock.Text.Trim(), out stock) || stock < 0)
            {
                MessageBox.Show("Tồn kho phải là số nguyên không âm!");
                txtStock.Focus();
                return false;
            }

            if (cmbCategory.SelectedValue == null ||
                !int.TryParse(cmbCategory.SelectedValue.ToString(), out categoryId))
            {
                MessageBox.Show("Vui lòng chọn danh mục sản phẩm!");
                cmbCategory.Focus();
                return false;
            }

            return true;
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(out decimal price, out int stock, out int categoryId))
                return;

            var newProduct = new
            {
                Barcode = txtBarcode.Text.Trim(),
                ProductName = txtName.Text.Trim(),
                Price = price,
                StockQuantity = stock,
                CategoryId = categoryId
            };

            try
            {
                bool success = await ApiClientService.PostDataWithTokenAsync(
                    "products", newProduct);

                if (success)
                {
                    MessageBox.Show("Thêm sản phẩm thành công!");
                    currentPage = 1;
                    ClearInputs();
                    await LoadProductData();
                }
                else
                    MessageBox.Show("Thêm thất bại. Kiểm tra dữ liệu và quyền truy cập.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần cập nhật!");
                return;
            }

            if (!ValidateInputs(out decimal price, out int stock, out int categoryId))
                return;

            object? id = dgvProduct.SelectedRows[0].Cells["ProductId"]?.Value;
            if (id == null)
            {
                MessageBox.Show("Không lấy được mã sản phẩm!");
                return;
            }

            var updatedProduct = new
            {
                Barcode = txtBarcode.Text.Trim(),
                ProductName = txtName.Text.Trim(),
                Price = price,
                StockQuantity = stock,
                CategoryId = categoryId
            };

            try
            {
                bool success = await ApiClientService.PutDataWithTokenAsync(
                    $"products/{id}", updatedProduct);

                if (success)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!");
                    await LoadProductData();
                }
                else
                    MessageBox.Show("Cập nhật thất bại. Kiểm tra dữ liệu và quyền truy cập.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
                return;
            }

            object? id = dgvProduct.SelectedRows[0].Cells["ProductId"]?.Value;
            if (id == null)
            {
                MessageBox.Show("Không lấy được mã sản phẩm!");
                return;
            }

            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sản phẩm này?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                bool success = await ApiClientService.DeleteDataWithTokenAsync(
                    $"products/{id}");

                if (success)
                {
                    MessageBox.Show("Xóa sản phẩm thành công!");
                    ClearInputs();
                    currentPage = 1;
                    await LoadProductData();
                }
                else
                    MessageBox.Show("Xóa thất bại. Sản phẩm có thể đang được tham chiếu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        private async void BtnSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            currentKeyword = keyword;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                currentPage = 1;
                await LoadProductData();
                return;
            }

            try
            {
                currentPage = 1;
                string url = "products/paged?pageNumber=" + currentPage +
                    "&pageSize=" + pageSize + "&keyword=" + Uri.EscapeDataString(keyword);
                string json = await ApiClientService.GetDataWithTokenAsync(url);
                var result = JsonSerializer.Deserialize<PagedProductResponse>(json, jsonOptions);
                var products = result?.Items ?? new List<ProductModel>();
                isLoading = true;
                dgvProduct.DataSource = products;
                ConfigureProductColumns();
                totalCount = result?.TotalCount ?? products.Count;
                totalPages = Math.Max(1, result?.TotalPages ?? 1);
                UpdatePaginationControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            currentKeyword = string.Empty;
            ClearInputs();
            currentPage = 1;
            await LoadProductData();
        }

        private void ClearInputs()
        {
            txtBarcode.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtStock.Clear();
            cmbCategory.SelectedIndex = -1;
            dgvProduct.ClearSelection();
        }
    }

    public class PagedProductResponse
    {
        public List<ProductModel> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    public class ProductModel
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public CategoryModel? Category { get; set; }
    }
}
