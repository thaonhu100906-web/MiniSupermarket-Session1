using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormProductManagement : Form
    {
        // Khai báo các control giao diện bằng code thuần
        private DataGridView dgvProducts = null!;
        private TextBox txtId = null!;
        private TextBox txtBarcode = null!;
        private TextBox txtProductName = null!;
        private NumericUpDown nudPrice = null!;
        private NumericUpDown nudStock = null!;
        private ComboBox cboCategory = null!;
        private TextBox txtSearch = null!;
        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnLoad = null!;
        private Button btnSearch = null!;

        public FormProductManagement()
        {
            InitializeComponent();
            InitializeCustomComponents(); // Hàm tự động khởi tạo giao diện
        }

        // Hàm tự động sắp xếp các control trên form
        private void InitializeCustomComponents()
        {
            this.Text = "QUẢN LÝ SẢN PHẨM & KHO HÀNG";
            this.Size = new Size(1100, 650);
            this.BackColor = Color.FromArgb(244, 245, 247);

            // Thanh tìm kiếm phía trên bảng
            txtSearch = new TextBox { Location = new Point(20, 25), Size = new Size(510, 25) };
            this.Controls.Add(txtSearch);

            btnSearch = new Button { Text = "Tìm kiếm", Location = new Point(540, 23), Size = new Size(120, 30), BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnSearch.Click += btnSearch_Click;
            this.Controls.Add(btnSearch);

            // 1. DataGridView hiển thị danh sách sản phẩm (bên trái)
            dgvProducts = new DataGridView
            {
                Location = new Point(20, 65),
                Size = new Size(640, 520),
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            dgvProducts.CellClick += dgvProducts_CellClick;
            this.Controls.Add(dgvProducts);

            int startX = 680;
            int startY = 25;
            int spacing = 45;

            // 2. Các ô nhập liệu thông tin sản phẩm (bên phải)
            this.Controls.Add(new Label { Text = "Mã SP (ID):", Location = new Point(startX, startY), AutoSize = true });
            txtId = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(240, 25), ReadOnly = true };
            this.Controls.Add(txtId);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Mã vạch (Barcode):", Location = new Point(startX, startY), AutoSize = true });
            txtBarcode = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(240, 25) };
            this.Controls.Add(txtBarcode);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Tên sản phẩm:", Location = new Point(startX, startY), AutoSize = true });
            txtProductName = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(240, 25) };
            this.Controls.Add(txtProductName);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Đơn giá:", Location = new Point(startX, startY), AutoSize = true });
            nudPrice = new NumericUpDown { Location = new Point(startX + 120, startY), Size = new Size(240, 25), Maximum = 1000000000, DecimalPlaces = 2 };
            this.Controls.Add(nudPrice);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Số lượng tồn:", Location = new Point(startX, startY), AutoSize = true });
            nudStock = new NumericUpDown { Location = new Point(startX + 120, startY), Size = new Size(240, 25), Maximum = 100000 };
            this.Controls.Add(nudStock);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Danh mục:", Location = new Point(startX, startY), AutoSize = true });
            cboCategory = new ComboBox { Location = new Point(startX + 120, startY), Size = new Size(240, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cboCategory);

            // 3. Các nút Thêm, Sửa, Xóa, Tải lại
            startY += spacing + 25;
            btnAdd = new Button { Text = "Thêm mới", Location = new Point(startX, startY), Size = new Size(115, 38), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnAdd.Click += btnAdd_Click;
            this.Controls.Add(btnAdd);

            btnUpdate = new Button { Text = "Cập nhật", Location = new Point(startX + 125, startY), Size = new Size(115, 38), BackColor = Color.FromArgb(23, 162, 184), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnUpdate.Click += btnUpdate_Click;
            this.Controls.Add(btnUpdate);

            startY += 48;
            btnDelete = new Button { Text = "Xóa", Location = new Point(startX, startY), Size = new Size(115, 38), BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnDelete.Click += btnDelete_Click;
            this.Controls.Add(btnDelete);

            btnLoad = new Button { Text = "Tải lại", Location = new Point(startX + 125, startY), Size = new Size(115, 38), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnLoad.Click += btnLoad_Click;
            this.Controls.Add(btnLoad);
        }

        // Cấu hình HttpClient gắn kèm Token bảo mật chuẩn dự án
        private HttpClient GetAuthenticatedClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7005/api/")
            };

            if (!string.IsNullOrEmpty(SessionManager.JwtToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", SessionManager.JwtToken);
            }

            return client;
        }

        // Sự kiện khi Load Form: Tải danh mục vào ComboBox và tải danh sách sản phẩm lên bảng
        private async void FormProductManagement_Load(object? sender, EventArgs e)
        {
            await LoadCategoriesAsync();
            await LoadProductsAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                using var client = GetAuthenticatedClient();
                var categories = await client.GetFromJsonAsync<List<CategoryDto>>("categories");
                cboCategory.DataSource = categories;
                cboCategory.DisplayMember = "CategoryName";
                cboCategory.ValueMember = "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                using var client = GetAuthenticatedClient();
                var products = await client.GetFromJsonAsync<List<ProductDto>>("products");
                dgvProducts.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLoad_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            await LoadProductsAsync();
        }

        // Click chọn một dòng trên bảng để đưa dữ liệu lên các ô nhập
        private void dgvProducts_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                txtId.Text = row.Cells["ProductId"].Value?.ToString() ?? string.Empty;
                txtBarcode.Text = row.Cells["Barcode"].Value?.ToString() ?? string.Empty;
                txtProductName.Text = row.Cells["ProductName"].Value?.ToString() ?? string.Empty;
                nudPrice.Value = Convert.ToDecimal(row.Cells["Price"].Value ?? 0);
                nudStock.Value = Convert.ToInt32(row.Cells["StockQuantity"].Value ?? 0);

                if (row.Cells["CategoryId"].Value != null)
                {
                    cboCategory.SelectedValue = Convert.ToInt32(row.Cells["CategoryId"].Value);
                }
            }
        }

        // Thêm mới sản phẩm (POST)
        private async void btnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newProd = new
            {
                Barcode = txtBarcode.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Price = nudPrice.Value,
                StockQuantity = (int)nudStock.Value,
                CategoryId = cboCategory.SelectedValue != null ? (int)cboCategory.SelectedValue : 0
            };

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.PostAsJsonAsync("products", newProd);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadProductsAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Thêm mới thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật sản phẩm (PUT)
        private async void btnUpdate_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa trên bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            var updateProd = new
            {
                ProductId = id,
                Barcode = txtBarcode.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Price = nudPrice.Value,
                StockQuantity = (int)nudStock.Value,
                CategoryId = cboCategory.SelectedValue != null ? (int)cboCategory.SelectedValue : 0
            };

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.PutAsJsonAsync($"products/{id}", updateProd);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadProductsAsync();
                    ClearInputs();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xóa sản phẩm (DELETE)
        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtId.Text);
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm ID = {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using var client = GetAuthenticatedClient();
                    var response = await client.DeleteAsync($"products/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đã xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadProductsAsync();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Tìm kiếm sản phẩm theo từ khóa
        private async void btnSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                await LoadProductsAsync();
                return;
            }

            try
            {
                using var client = GetAuthenticatedClient();
                var result = await client.GetFromJsonAsync<List<ProductDto>>($"products/search?keyword={keyword}");
                dgvProducts.DataSource = result;
            }
            catch (Exception)
            {
                MessageBox.Show("Không tìm thấy sản phẩm phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Xóa trắng các ô nhập liệu
        private void ClearInputs()
        {
            txtId.Text = "";
            txtBarcode.Text = "";
            txtProductName.Text = "";
            nudPrice.Value = 0;
            nudStock.Value = 0;
            if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
        }
    }

    // Lớp DTO trung gian ánh xạ dữ liệu Sản phẩm
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}