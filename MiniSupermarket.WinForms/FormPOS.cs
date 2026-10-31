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
using Microsoft.VisualBasic;

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
        private TextBox txtCashReceived = null!;
        private Label lblChange = null!;
        private TextBox txtCustomerPhone = null!;
        private TextBox txtRedeemPoints = null!;
        private Label lblCustomerInfo = null!;
        private Label lblCustomerPoints = null!;
        private Button btnLookupCustomer = null!;
        private int? selectedCustomerId;
        private int availableRewardPoints;

        private TableLayoutPanel root = null!;
        private TableLayoutPanel content = null!;
        private Panel productPanel = null!;
        private Panel cartPanel = null!;

        public FormPOS()
        {
            InitializeComponent();
            Controls.Clear();
            BuildLayout();
            DailyFreshTheme.Apply(this);
            Load += FormPOS_Load;
            Resize += FormPOS_Resize; // Bắt sự kiện thu nhỏ/phóng to để rớt dòng
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
            AutoScroll = true; // Cho phép cuộn trang khi thu nhỏ
            Padding = new Padding(12);

            root = new TableLayoutPanel
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

            content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0)
            };
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
            root.Controls.Add(content, 0, 1);

            // 1. Panel Sản phẩm (Bên trái khi mở rộng)
            productPanel = new Panel
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

            // 2. Panel Giỏ hàng & Thanh toán (Bên phải khi mở rộng, rớt xuống dưới khi thu nhỏ)
            cartPanel = new Panel
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
                RowCount = 8
            };

            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 88));
            cartLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
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

            var customerPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Margin = new Padding(0, 2, 0, 2)
            };
            customerPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            customerPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 27));
            customerPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 27));

            var customerSearchRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            customerSearchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72));
            customerSearchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            txtCustomerPhone = new TextBox
            {
                Dock = DockStyle.Fill,
                PlaceholderText = "SĐT khách hàng (không bắt buộc)"
            };
            txtCustomerPhone.TextChanged += (s, e) =>
            {
                selectedCustomerId = null;
                availableRewardPoints = 0;
                lblCustomerInfo.Text = "Khách hàng: Chưa tra cứu";
                lblCustomerPoints.Text = "Điểm có thể đổi: 0";
                txtRedeemPoints.Text = "0";
                CalculateChange();
            };
            btnLookupCustomer = MakeButton("Tra cứu", Color.FromArgb(13, 110, 253));
            btnLookupCustomer.Dock = DockStyle.Fill;
            btnLookupCustomer.Click += BtnLookupCustomer_Click;
            customerSearchRow.Controls.Add(txtCustomerPhone, 0, 0);
            customerSearchRow.Controls.Add(btnLookupCustomer, 1, 0);
            customerPanel.Controls.Add(customerSearchRow, 0, 0);

            lblCustomerInfo = new Label
            {
                Text = "Khách hàng: Chưa tra cứu",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(45, 65, 75)
            };
            customerPanel.Controls.Add(lblCustomerInfo, 0, 1);

            var redeemRow = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            redeemRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            redeemRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            lblCustomerPoints = new Label
            {
                Text = "Điểm có thể đổi: 0",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.DarkGreen
            };
            txtRedeemPoints = new TextBox { Dock = DockStyle.Fill, Text = "0" };
            txtRedeemPoints.TextChanged += (s, e) => CalculateChange();
            redeemRow.Controls.Add(lblCustomerPoints, 0, 0);
            redeemRow.Controls.Add(txtRedeemPoints, 1, 0);
            customerPanel.Controls.Add(redeemRow, 0, 2);
            cartLayout.Controls.Add(customerPanel, 0, 4);

            var paymentPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };

            paymentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            paymentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            paymentPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            paymentPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            paymentPanel.Controls.Add(new Label
            {
                Text = "Tiền khách đưa:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);

            txtCashReceived = new TextBox
            {
                Dock = DockStyle.Fill,
                Text = "0"
            };
            txtCashReceived.TextChanged += (s, e) => CalculateChange();

            paymentPanel.Controls.Add(txtCashReceived, 1, 0);

            paymentPanel.Controls.Add(new Label
            {
                Text = "Tiền thối lại:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 1);

            lblChange = new Label
            {
                Text = "0 đ",
                Dock = DockStyle.Fill,
                ForeColor = Color.DarkGreen,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            paymentPanel.Controls.Add(lblChange, 1, 1);

            cartLayout.Controls.Add(paymentPanel, 0, 5);

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
            cartLayout.Controls.Add(actions, 0, 6);

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
            cartLayout.Controls.Add(bottom, 0, 7);
        }

        private void FormPOS_Resize(object? sender, EventArgs e)
        {
            if (content == null || productPanel == null || cartPanel == null)
                return;

            int clientWidth = ClientSize.Width;

            // Ngưỡng 1100px: Màn hình rộng -> Chia 2 cột ngang. Màn hình nhỏ -> Rớt xuống thành 2 hàng dọc
            if (clientWidth >= 1100)
            {
                if (content.ColumnCount != 2)
                {
                    content.RowCount = 1;
                    content.ColumnCount = 2;
                    content.ColumnStyles.Clear();
                    content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
                    content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
                    content.RowStyles.Clear();
                    content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                    content.Controls.Add(productPanel, 0, 0);
                    content.Controls.Add(cartPanel, 1, 0);

                    productPanel.Margin = new Padding(0, 0, 6, 0);
                    cartPanel.Margin = new Padding(6, 0, 0, 0);
                }
                root.Dock = DockStyle.Fill;
            }
            else
            {
                if (content.ColumnCount != 1)
                {
                    content.RowCount = 2;
                    content.ColumnCount = 1;
                    content.ColumnStyles.Clear();
                    content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    content.RowStyles.Clear();
                    content.RowStyles.Add(new RowStyle(SizeType.Absolute, 500F)); // Chiều cao cố định cho bảng sản phẩm
                    content.RowStyles.Add(new RowStyle(SizeType.Absolute, 580F)); // Chiều cao cố định cho bảng giỏ hàng & thanh toán

                    content.Controls.Add(productPanel, 0, 0);
                    content.Controls.Add(cartPanel, 0, 1);

                    productPanel.Margin = new Padding(0, 0, 0, 10);
                    cartPanel.Margin = new Padding(0, 0, 0, 0);
                }
                root.Dock = DockStyle.Top;
                root.Height = 66 + 500 + 580 + 40; // Tăng chiều cao để hiện thanh cuộn dọc mượt mà
            }
        }

        private void CalculateChange()
        {
            if (txtCashReceived == null || lblChange == null)
                return;

            decimal total = cart.Sum(x => x.Subtotal);
            int points = 0;
            if (int.TryParse(txtRedeemPoints?.Text.Trim(), out int parsedPoints) && parsedPoints >= 0
                && selectedCustomerId.HasValue && parsedPoints <= availableRewardPoints
                && parsedPoints * 1000m <= total)
                points = parsedPoints;
            decimal payable = total - points * 1000m;
            if (!decimal.TryParse(
                txtCashReceived.Text.Trim(),
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture,
                out decimal cash))
            {
                lblChange.Text = "Số tiền không hợp lệ";
                return;
            }

            decimal difference = cash - payable;
            lblChange.Text = difference >= 0
                ? difference.ToString("N0") + " đ"
                : "Còn thiếu " + Math.Abs(difference).ToString("N0") + " đ";
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
                AutoGenerateColumns = false,
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
            CalculateChange();
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

        private sealed class CustomerLookupResult
        {
            public int CustomerId { get; set; }
            public string CustomerName { get; set; } = "";
            public string PhoneNumber { get; set; } = "";
            public int RewardPoints { get; set; }
            public string MembershipRank { get; set; } = "";
        }

        private async void BtnLookupCustomer_Click(object? sender, EventArgs e)
        {
            string phone = txtCustomerPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại khách hàng.", "Tra cứu khách hàng",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCustomerPhone.Focus();
                return;
            }

            btnLookupCustomer.Enabled = false;
            try
            {
                string url = "customers/lookup?phone=" + Uri.EscapeDataString(phone);
                using var lookupResponse = await ApiClientService.Client.GetAsync(url);
                if (!lookupResponse.IsSuccessStatusCode)
                {
                    selectedCustomerId = null;
                    availableRewardPoints = 0;
                    lblCustomerInfo.Text = "Khách hàng: Không tìm thấy";
                    lblCustomerPoints.Text = "Điểm có thể đổi: 0";
                    string apiMessage = "Không tìm thấy khách hàng theo số điện thoại này.";
                    string body = await lookupResponse.Content.ReadAsStringAsync();
                    try
                    {
                        using var errorDoc = System.Text.Json.JsonDocument.Parse(body);
                        if (errorDoc.RootElement.TryGetProperty("message", out var message))
                            apiMessage = message.GetString() ?? apiMessage;
                    }
                    catch { }
                    MessageBox.Show(apiMessage, "Tra cứu khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CalculateChange();
                    return;
                }

                var customer = await lookupResponse.Content.ReadFromJsonAsync<CustomerLookupResult>();
                if (customer == null)
                    throw new InvalidOperationException("API không trả về thông tin khách hàng hợp lệ.");

                selectedCustomerId = customer.CustomerId;
                availableRewardPoints = customer.RewardPoints;
                lblCustomerInfo.Text = $"Khách hàng: {customer.CustomerName} ({customer.MembershipRank})";
                lblCustomerPoints.Text = $"Điểm có thể đổi: {availableRewardPoints}";
                txtRedeemPoints.Text = "0";
                CalculateChange();
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MessageBox.Show("Không thể tra cứu khách hàng. Vui lòng kiểm tra API và kết nối mạng.",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tra cứu khách hàng thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLookupCustomer.Enabled = true;
            }
        }

        private async void BtnPay_Click(object? sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống.");
                return;
            }

            decimal total = cart.Sum(x => x.Subtotal);
            string customerPhone = txtCustomerPhone.Text.Trim();
            int redeemPoints = 0;
            if (!int.TryParse(txtRedeemPoints.Text.Trim(), out redeemPoints) || redeemPoints < 0)
            {
                MessageBox.Show("Số điểm muốn đổi phải là số nguyên không âm.", "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRedeemPoints.Focus();
                return;
            }
            if (redeemPoints > 0 && !selectedCustomerId.HasValue)
            {
                MessageBox.Show("Vui lòng tra cứu số điện thoại và xác nhận khách hàng trước khi đổi điểm.",
                    "Chưa chọn khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerPhone.Focus();
                return;
            }
            if (redeemPoints > availableRewardPoints)
            {
                MessageBox.Show($"Khách hàng chỉ có {availableRewardPoints} điểm. Vui lòng nhập số điểm không vượt quá số điểm hiện có.",
                    "Không đủ điểm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRedeemPoints.Focus();
                return;
            }
            if (redeemPoints * 1000m > total)
            {
                MessageBox.Show("Số điểm đổi vượt quá tổng tiền hóa đơn.", "Không thể đổi điểm",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRedeemPoints.Focus();
                return;
            }
            decimal discount = redeemPoints * 1000m;
            decimal finalAmount = total - discount;

            if (!decimal.TryParse(
                txtCashReceived.Text.Trim(),
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture,
                out decimal cashReceived) || cashReceived < 0)
            {
                MessageBox.Show("Tiền khách đưa không hợp lệ. Vui lòng nhập số tiền không âm.",
                    "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCashReceived.Focus();
                return;
            }

            if (cashReceived < finalAmount)
            {
                MessageBox.Show($"Khách còn thiếu {finalAmount - cashReceived:N0} đ.",
                    "Chưa đủ tiền thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCashReceived.Focus();
                return;
            }

            decimal change = cashReceived - finalAmount;
            lblChange.Text = change.ToString("N0") + " đ";

            if (MessageBox.Show(
                $"Tổng tiền: {total:N0} đ\nGiảm từ điểm thưởng: {discount:N0} đ\nCần thanh toán: {finalAmount:N0} đ\n\nXác nhận thanh toán và lưu hóa đơn?",
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
                    CustomerPhone = string.IsNullOrWhiteSpace(customerPhone) ? null : customerPhone,
                    RedeemPoints = redeemPoints,
                    CashReceived = cashReceived,
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
                    string errorBody = await response.Content.ReadAsStringAsync();
                    string errorMessage = errorBody;
                    try
                    {
                        using var errorJson = System.Text.Json.JsonDocument.Parse(errorBody);
                        if (errorJson.RootElement.TryGetProperty("message", out var messageElement))
                            errorMessage = messageElement.GetString() ?? errorBody;
                    }
                    catch { }

                    MessageBox.Show(
                        "Thanh toán thất bại. Hóa đơn chưa được lưu.\n" + errorMessage,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                string result = await response.Content.ReadAsStringAsync();

                cart.Clear();
                RefreshCart();
                txtCashReceived.Text = "0";
                txtCustomerPhone.Clear();
                txtRedeemPoints.Text = "0";
                selectedCustomerId = null;
                availableRewardPoints = 0;
                await LoadProductsAsync();

                string successMessage = "Thanh toán thành công! Hóa đơn đã được lưu vào SQL Server.";
                try
                {
                    using var resultJson = System.Text.Json.JsonDocument.Parse(result);
                    var data = resultJson.RootElement;
                    if (data.TryGetProperty("orderId", out var orderId))
                        successMessage += $"\nMã hóa đơn: {orderId}";
                    if (data.TryGetProperty("totalAmount", out var amount))
                        successMessage += $"\nThực thu: {amount.GetDecimal():N0} đ";
                    if (data.TryGetProperty("changeAmount", out var changeAmount))
                        successMessage += $"\nTiền thối: {changeAmount.GetDecimal():N0} đ";
                    if (data.TryGetProperty("redeemedPoints", out var redeemed))
                        successMessage += $"\nĐiểm đã đổi: {redeemed}";
                }
                catch { }

                MessageBox.Show(successMessage, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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