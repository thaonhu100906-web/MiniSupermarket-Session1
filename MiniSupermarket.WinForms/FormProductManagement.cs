
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
        private Panel tablePanel = null!;
        private Panel detailPanel = null!;

        private bool isLoading = false;

        public FormProductManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();

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

            var title = new Label
            {
                Text = "Quản lý sản phẩm",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 15),
                AutoSize = true
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = "Quản lý sản phẩm, giá bán, tồn kho và danh mục",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(27, 53),
                AutoSize = true
            };
            Controls.Add(subtitle);

            txtSearch = new TextBox
            {
                Location = new Point(25, 78),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Nhập tên hoặc mã vạch..."
            };
            Controls.Add(txtSearch);

            btnSearch = CreateButton(
                "Tìm kiếm", 285, 76, 105, 32,
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
                Dock = DockStyle.Fill,
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
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
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

            tablePanel.Padding = new Padding(8);
            tablePanel.Controls.Add(dgvProduct);

            detailPanel = new Panel
            {
                Location = new Point(25, 500),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(detailPanel);

            var detailTitle = new Label
            {
                Text = "Thông tin sản phẩm",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(18, 12),
                AutoSize = true
            };
            detailPanel.Controls.Add(detailTitle);

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

            btnAdd = CreateButton(
                "Thêm mới", 18, 340, 130, 36,
                Color.FromArgb(40, 167, 69));
            btnAdd.Click += BtnAdd_Click;
            detailPanel.Controls.Add(btnAdd);

            btnUpdate = CreateButton(
                "Cập nhật", 158, 340, 130, 36,
                Color.FromArgb(23, 162, 184));
            btnUpdate.Click += BtnUpdate_Click;
            detailPanel.Controls.Add(btnUpdate);

            btnDelete = CreateButton(
                "Xóa", 18, 385, 130, 36,
                Color.FromArgb(220, 53, 69));
            btnDelete.Click += BtnDelete_Click;
            detailPanel.Controls.Add(btnDelete);

            btnRefresh = CreateButton(
                "Làm mới", 158, 385, 130, 36,
                Color.FromArgb(108, 117, 125));
            btnRefresh.Click += BtnRefresh_Click;
            detailPanel.Controls.Add(btnRefresh);

            FormProductManagement_Resize(this, EventArgs.Empty);
        }

        private void FormProductManagement_Resize(object? sender, EventArgs e)
        {
            if (tablePanel == null || detailPanel == null)
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

                detailPanel.Location =
                    new Point(25 + tableWidth + gap, 120);
                detailPanel.Size =
                    new Size(detailWidth, Math.Max(435, height));

                SetDetailControlWidths(detailWidth);
                AutoScrollMinSize = Size.Empty;
            }
            else
            {
                tablePanel.Location = new Point(25, 120);
                tablePanel.Size = new Size(width, 300);

                detailPanel.Location = new Point(25, 435);
                detailPanel.Size = new Size(width, 440);

                SetDetailControlWidths(width);
                AutoScrollMinSize = new Size(0, 890);
            }
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

        private Button CreateButton(
            string text, int x, int y, int width, int height, Color color)
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
                string json = await ApiClientService.GetDataWithTokenAsync(
                    "categories");

                var categories =
                    JsonSerializer.Deserialize<List<CategoryModel>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new List<CategoryModel>();

                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không tải được danh mục: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductData()
        {
            try
            {
                isLoading = true;

                string json = await ApiClientService.GetDataWithTokenAsync(
                    "products");

                var products =
                    JsonSerializer.Deserialize<List<ProductModel>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new List<ProductModel>();

                dgvProduct.DataSource = products;

                SetColumnHeader("ProductId", "Mã SP");
                SetColumnHeader("Barcode", "Mã vạch");
                SetColumnHeader("ProductName", "Tên sản phẩm");
                SetColumnHeader("Price", "Đơn giá");
                SetColumnHeader("StockQuantity", "Tồn kho");
                SetColumnHeader("CategoryId", "Mã danh mục");
                SetColumnHeader("Category", "Danh mục");

                if (dgvProduct.Columns["Price"] != null)
                {
                    dgvProduct.Columns["Price"]!.DefaultCellStyle.Format = "N0";
                }

                if (dgvProduct.Columns["Category"] != null)
                {
                    dgvProduct.Columns["Category"]!.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách sản phẩm: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
            }
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

            txtBarcode.Text =
                row.Cells["Barcode"]?.Value?.ToString() ?? string.Empty;

            txtName.Text =
                row.Cells["ProductName"]?.Value?.ToString() ?? string.Empty;

            txtPrice.Text =
                row.Cells["Price"]?.Value?.ToString() ?? string.Empty;

            txtStock.Text =
                row.Cells["StockQuantity"]?.Value?.ToString() ?? string.Empty;

            object? categoryId = row.Cells["CategoryId"]?.Value;

            if (categoryId != null && int.TryParse(categoryId.ToString(), out int id))
                cmbCategory.SelectedValue = id;
            else
                cmbCategory.SelectedIndex = -1;
        }

        private bool ValidateInputs(
            out decimal price, out int stock, out int categoryId)
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
                    ClearInputs();
                    await LoadProductData();
                }
                else
                {
                    MessageBox.Show(
                        "Thêm thất bại. Kiểm tra mã vạch, danh mục và quyền truy cập.");
                }
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
                {
                    MessageBox.Show(
                        "Cập nhật thất bại. Kiểm tra dữ liệu và quyền truy cập.");
                }
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
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

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
                    await LoadProductData();
                }
                else
                {
                    MessageBox.Show(
                        "Xóa thất bại. Sản phẩm có thể đang được tham chiếu bởi dữ liệu khác.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        private async void BtnSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                await LoadProductData();
                return;
            }

            try
            {
                string url = "products/search?keyword=" +
                    Uri.EscapeDataString(keyword);

                string json = await ApiClientService.GetDataWithTokenAsync(url);

                var products =
                    JsonSerializer.Deserialize<List<ProductModel>>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? new List<ProductModel>();

                isLoading = true;
                dgvProduct.DataSource = products;

                SetColumnHeader("ProductId", "Mã SP");
                SetColumnHeader("Barcode", "Mã vạch");
                SetColumnHeader("ProductName", "Tên sản phẩm");
                SetColumnHeader("Price", "Đơn giá");
                SetColumnHeader("StockQuantity", "Tồn kho");
                SetColumnHeader("CategoryId", "Mã danh mục");

                if (dgvProduct.Columns["Price"] != null)
                    dgvProduct.Columns["Price"]!.DefaultCellStyle.Format = "N0";

                MessageBox.Show($"Tìm thấy {products.Count} sản phẩm.");
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
            ClearInputs();
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

    public class ProductModel
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
    }
}