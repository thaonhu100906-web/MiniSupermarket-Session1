// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormPOS : Form
    {
        private sealed class ProductItem
        {
            public int ProductId { get; set; }
            public string Barcode { get; set; } = "";
            public string ProductName { get; set; } = "";
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
        }

        private sealed class CartItem
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; } = "";
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal Subtotal => UnitPrice * Quantity;
        }

        private readonly List<ProductItem> products = new();
        private readonly List<CartItem> cart = new();

        private TextBox txtSearch = null!;
        private TextBox txtBarcode = null!;
        private DataGridView dgvProducts = null!;
        private DataGridView dgvCart = null!;
        private Label lblTotal = null!;
        private Label lblItemCount = null!;
        private Button btnPay = null!;

        public FormPOS()
        {
            InitializeComponent();
            Controls.Clear();
            BuildLayout();
            Load += FormPOS_Load;
        }

        private async void FormPOS_Load(object? sender, EventArgs e)
        {
            await LoadProductsAsync();
        }

        private void BuildLayout()
        {
            Text = "Bán hàng POS";
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);
            Dock = DockStyle.Fill;
            Padding = new Padding(12);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 66));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(root);

            var header = new Panel { Dock = DockStyle.Fill };
            header.Controls.Add(new Label
            {
                Text = "QUẦY BÁN HÀNG",
                Font = new Font("Segoe UI", 19, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 75),
                Location = new Point(0, 0),
                AutoSize = true
            });
            header.Controls.Add(new Label
            {
                Text = "Chọn sản phẩm, thêm vào giỏ và thanh toán",
                Location = new Point(2, 37),
                AutoSize = true,
                ForeColor = Color.DimGray
            });
            root.Controls.Add(header, 0, 0);

            var content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0)
            };
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
            root.Controls.Add(content, 0, 1);

            var productPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 6, 0)
            };
            content.Controls.Add(productPanel, 0, 0);

            var productLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            productLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            productLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            productLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            productPanel.Controls.Add(productLayout);

            productLayout.Controls.Add(new Label
            {
                Text = "Danh sách sản phẩm",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);

            txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                PlaceholderText = "Tìm theo tên hoặc mã sản phẩm..."
            };
            txtSearch.TextChanged += (s, e) => RefreshProducts();
            productLayout.Controls.Add(txtSearch, 0, 1);

            var productArea = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            productArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            productArea.RowStyles.Add(new RowStyle(SizeType.Absolute, 43));
            productLayout.Controls.Add(productArea, 0, 2);

            dgvProducts = CreateGrid();
            dgvProducts.Columns.Add("ProductId", "Mã");
            dgvProducts.Columns.Add("Barcode", "Barcode");
            dgvProducts.Columns.Add("ProductName", "Tên sản phẩm");
            dgvProducts.Columns.Add("Price", "Đơn giá");
            dgvProducts.Columns.Add("StockQuantity", "Tồn kho");
            dgvProducts.Columns["Price"]!.DefaultCellStyle.Format = "N0";
            productArea.Controls.Add(dgvProducts, 0, 0);

            var btnAdd = MakeButton("Thêm vào giỏ", Color.FromArgb(25, 135, 84));
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.Click += BtnAdd_Click;
            productArea.Controls.Add(btnAdd, 0, 1);

            var cartPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10),
                Margin = new Padding(6, 0, 0, 0)
            };
            content.Controls.Add(cartPanel, 1, 0);

            var cartLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6
            };
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43));
            cartPanel.Controls.Add(cartLayout);

            cartLayout.Controls.Add(new Label
            {
                Text = "Giỏ hàng",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);

            txtBarcode = new TextBox
            {
                Dock = DockStyle.Fill,
                PlaceholderText = "Nhập mã barcode rồi nhấn Enter..."
            };
            txtBarcode.KeyDown += TxtBarcode_KeyDown;
            cartLayout.Controls.Add(txtBarcode, 0, 1);

            dgvCart = CreateGrid();
            dgvCart.Columns.Add("ProductId", "Mã");
            dgvCart.Columns.Add("ProductName", "Sản phẩm");
            dgvCart.Columns.Add("UnitPrice", "Đơn giá");
            dgvCart.Columns.Add("Quantity", "SL");
            dgvCart.Columns.Add("Subtotal", "Thành tiền");
            dgvCart.Columns["UnitPrice"]!.DefaultCellStyle.Format = "N0";
            dgvCart.Columns["Subtotal"]!.DefaultCellStyle.Format = "N0";
            cartLayout.Controls.Add(dgvCart, 0, 2);

            var summary = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            summary.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            summary.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            cartLayout.Controls.Add(summary, 0, 3);

            lblTotal = new Label
            {
                Text = "0 đ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 135, 84),
                Dock = DockStyle.Fill
            };
            summary.Controls.Add(lblTotal, 0, 0);

            lblItemCount = new Label
            {
                Text = "Số lượng: 0",
                Dock = DockStyle.Fill,
                ForeColor = Color.DimGray
            };
            summary.Controls.Add(lblItemCount, 0, 1);

            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
                Padding = new Padding(0)
            };

            var btnIncrease = MakeButton("+ SL", Color.FromArgb(13, 110, 253));
            btnIncrease.Size = new Size(68, 32);
            btnIncrease.Click += (s, e) => ChangeSelectedQuantity(1);

            var btnDecrease = MakeButton("- SL", Color.Gray);
            btnDecrease.Size = new Size(68, 32);
            btnDecrease.Click += (s, e) => ChangeSelectedQuantity(-1);

            var btnRemove = MakeButton("Xóa món", Color.Crimson);
            btnRemove.Size = new Size(90, 32);
            btnRemove.Click += BtnRemove_Click;

            actions.Controls.AddRange(new Control[]
            {
                btnIncrease, btnDecrease, btnRemove
            });
            cartLayout.Controls.Add(actions, 0, 4);

            var bottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var btnClear = MakeButton("Làm trống giỏ", Color.Gray);
            btnClear.Dock = DockStyle.Fill;
            btnClear.Click += (s, e) =>
            {
                cart.Clear();
                RefreshCart();
            };

            btnPay = MakeButton("Thanh toán", Color.FromArgb(25, 135, 84));
            btnPay.Dock = DockStyle.Fill;
            btnPay.Click += BtnPay_Click;

            bottom.Controls.Add(btnClear, 0, 0);
            bottom.Controls.Add(btnPay, 1, 0);
            cartLayout.Controls.Add(bottom, 0, 5);
        }

        private static DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 34,
                AutoGenerateColumns = false
            ,
                ColumnHeadersDefaultCellStyle =
                {
                    BackColor = Color.FromArgb(35, 75, 95),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                },
                DefaultCellStyle =
                {
                    SelectionBackColor = Color.LightBlue,
                    SelectionForeColor = Color.Black,
                    Font = new Font("Segoe UI", 9)
                }
            };
        }

        private static Button MakeButton(string text, Color color)
        {
            var button = new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var result = await ApiClientService.Client
                    .GetFromJsonAsync<List<ProductItem>>("products");

                products.Clear();

                if (result != null)
                    products.AddRange(result);

                EnsureProductColumns();
                RefreshProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không tải được sản phẩm từ API. Hãy kiểm tra API và SQL Server.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void EnsureProductColumns()
        {
            if (dgvProducts.Columns.Count > 0)
                return;

            dgvProducts.Columns.Add("ProductId", "Mã");
            dgvProducts.Columns.Add("Barcode", "Barcode");
            dgvProducts.Columns.Add("ProductName", "Tên sản phẩm");
            dgvProducts.Columns.Add("Price", "Đơn giá");
            dgvProducts.Columns.Add("StockQuantity", "Tồn kho");
            dgvProducts.Columns["Price"]!.DefaultCellStyle.Format = "N0";
        }

        private void EnsureCartColumns()
        {
            if (dgvCart.Columns.Count > 0)
                return;

            dgvCart.Columns.Add("ProductId", "Mã");
            dgvCart.Columns.Add("ProductName", "Sản phẩm");
            dgvCart.Columns.Add("UnitPrice", "Đơn giá");
            dgvCart.Columns.Add("Quantity", "SL");
            dgvCart.Columns.Add("Subtotal", "Thành tiền");
            dgvCart.Columns["UnitPrice"]!.DefaultCellStyle.Format = "N0";
            dgvCart.Columns["Subtotal"]!.DefaultCellStyle.Format = "N0";
        }

        private void RefreshProducts()
        {
            if (dgvProducts == null)
                return;

            EnsureProductColumns();
            dgvProducts.Rows.Clear();

            string keyword = txtSearch?.Text.Trim() ?? "";

            foreach (var p in products.Where(p =>
                (p.ProductName ?? "").Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (p.Barcode ?? "").Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            {
                dgvProducts.Rows.Add(
                    p.ProductId,
                    p.Barcode,
                    p.ProductName,
                    p.Price,
                    p.StockQuantity);
            }
        }

        private void RefreshCart()
        {
            if (dgvCart == null)
                return;

            EnsureCartColumns();
            dgvCart.Rows.Clear();

            foreach (var item in cart)
            {
                dgvCart.Rows.Add(
                    item.ProductId,
                    item.ProductName,
                    item.UnitPrice,
                    item.Quantity,
                    item.Subtotal);
            }

            lblTotal.Text = cart.Sum(x => x.Subtotal).ToString("N0") + " đ";
            lblItemCount.Text = "Số lượng: " + cart.Sum(x => x.Quantity);
        }

        private void AddProductToCart(ProductItem? product)
        {
            if (product == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm.");
                return;
            }

            var item = cart.FirstOrDefault(x => x.ProductId == product.ProductId);

            if ((item?.Quantity ?? 0) >= product.StockQuantity)
            {
                MessageBox.Show("Số lượng trong giỏ đã đạt tồn kho hiện tại.");
                return;
            }

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.Price,
                    Quantity = 1
                });
            }
            else
            {
                item.Quantity++;
            }

            RefreshCart();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm.");
                return;
            }

            int id = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductId"].Value);
            AddProductToCart(products.FirstOrDefault(p => p.ProductId == id));
        }

        private void TxtBarcode_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;

            string barcode = txtBarcode.Text.Trim();

            AddProductToCart(products.FirstOrDefault(p =>
                string.Equals(p.Barcode, barcode, StringComparison.OrdinalIgnoreCase)));

            txtBarcode.Clear();
        }

        private void ChangeSelectedQuantity(int amount)
        {
            if (dgvCart.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm trong giỏ.");
                return;
            }

            int id = Convert.ToInt32(dgvCart.CurrentRow.Cells["ProductId"].Value);
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            var product = products.FirstOrDefault(x => x.ProductId == id);

            if (item == null || product == null)
                return;

            int quantity = item.Quantity + amount;

            if (quantity <= 0)
                cart.Remove(item);
            else if (quantity <= product.StockQuantity)
                item.Quantity = quantity;
            else
                MessageBox.Show("Số lượng vượt quá tồn kho.");

            RefreshCart();
        }

        private void BtnRemove_Click(object? sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.");
                return;
            }

            int id = Convert.ToInt32(dgvCart.CurrentRow.Cells["ProductId"].Value);
            cart.RemoveAll(x => x.ProductId == id);
            RefreshCart();
        }

        private async void BtnPay_Click(object? sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.");
                return;
            }

            decimal total = cart.Sum(x => x.Subtotal);

            if (MessageBox.Show(
                $"Tổng thanh toán: {total:N0} đ\n\nXác nhận thanh toán và lưu hóa đơn?",
                "Xác nhận thanh toán",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            btnPay.Enabled = false;

            try
            {
                var request = new
                {
                    CashierUsername = SessionManager.CurrentUsername,
                    Items = cart.Select(x => new
                    {
                        x.ProductId,
                        x.Quantity,
                        x.UnitPrice
                    }).ToList()
                };

                var response = await ApiClientService.Client
                    .PostAsJsonAsync("orders/checkout", request);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();

                    MessageBox.Show(
                        "Thanh toán thất bại. Hóa đơn chưa được lưu.\n" + error,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                string result = await response.Content.ReadAsStringAsync();

                cart.Clear();
                RefreshCart();
                await LoadProductsAsync();

                MessageBox.Show(
                    "Thanh toán thành công! Hóa đơn đã được lưu vào SQL Server.\n" + result,
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không gọi được API thanh toán. Kiểm tra API đang chạy.\n" + ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnPay.Enabled = true;
            }
        }
    }
}