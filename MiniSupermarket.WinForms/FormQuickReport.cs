using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MiniSupermarket.WinForms
{
    public partial class FormQuickReport : Form
    {
        private DateTimePicker dtpFrom = null!, dtpTo = null!;
        private Label lblRevenue = null!, lblOrderCount = null!, lblCustomers = null!, lblLowStock = null!;
        private DataGridView dgvReport = null!, dgvBestSellers = null!;
        private Chart chartRevenue = null!;
        private Button btnFilter = null!;
        private FlowLayoutPanel mainContainer = null!;
        private FlowLayoutPanel filterPanel = null!;
        private TableLayoutPanel cardsTable = null!;
        private List<RevenueDay> revenueDays = new();

        private sealed class OrderReportItem { public int OrderId { get; set; } public DateTime OrderDate { get; set; } public int ItemCount { get; set; } public decimal TotalAmount { get; set; } }
        private sealed class RevenueDay { public DateTime Date { get; set; } public decimal Revenue { get; set; } public int OrderCount { get; set; } }
        private sealed class DashboardSummary { public decimal RevenueToday { get; set; } public int OrderCount { get; set; } public int NewCustomersToday { get; set; } public int LowStockCount { get; set; } }
        private sealed class BestSeller { public int ProductId { get; set; } public string ProductName { get; set; } = ""; public int QuantitySold { get; set; } public decimal Revenue { get; set; } }

        public FormQuickReport()
        {
            InitializeComponent();
            Controls.Clear();
            BuildLayout();
            DailyFreshTheme.Apply(this);
            Load += async (_, _) => await LoadAllAsync();
        }

        private void BuildLayout()
        {
            Text = "Dashboard & Báo cáo";
            Dock = DockStyle.Fill;
            AutoScroll = true;
            BackColor = Color.FromArgb(244, 246, 249);
            Font = new Font("Segoe UI", 9.5f);
            MinimumSize = new Size(650, 550);

            mainContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.FromArgb(244, 246, 249),
                Padding = new Padding(22)
            };
            Controls.Add(mainContainer);

            // 1. Tiêu đề trang
            var pnlHeader = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, WrapContents = false, Margin = new Padding(0, 0, 0, 15) };
            var title = new Label { Text = "DASHBOARD & BÁO CÁO", Font = new Font("Segoe UI", 18f, FontStyle.Bold), ForeColor = Color.FromArgb(33, 37, 41), AutoSize = true };
            var subTitle = new Label { Text = "Tổng quan hoạt động kinh doanh, doanh thu và sản phẩm bán chạy tại Daily Fresh", AutoSize = true, ForeColor = Color.FromArgb(108, 117, 125), Margin = new Padding(0, 4, 0, 0), Font = new Font("Segoe UI", 10f, FontStyle.Regular) };
            pnlHeader.Controls.Add(title);
            pnlHeader.Controls.Add(subTitle);
            mainContainer.Controls.Add(pnlHeader);

            // 2. Khu vực Lọc và Xuất báo cáo
            filterPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.White,
                Padding = new Padding(15),
                Margin = new Padding(0, 0, 0, 18)
            };

            var pnlFrom = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Margin = new Padding(0, 0, 15, 5) };
            pnlFrom.Controls.Add(new Label { Text = "Từ ngày", AutoSize = true, ForeColor = Color.FromArgb(73, 80, 87), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), Margin = new Padding(0, 0, 0, 4) });
            dtpFrom = new DateTimePicker { Width = 140, Height = 28, Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(-6) };
            pnlFrom.Controls.Add(dtpFrom);
            filterPanel.Controls.Add(pnlFrom);

            var pnlTo = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Margin = new Padding(0, 0, 15, 5) };
            pnlTo.Controls.Add(new Label { Text = "Đến ngày", AutoSize = true, ForeColor = Color.FromArgb(73, 80, 87), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), Margin = new Padding(0, 0, 0, 4) });
            dtpTo = new DateTimePicker { Width = 140, Height = 28, Format = DateTimePickerFormat.Short, Value = DateTime.Today };
            pnlTo.Controls.Add(dtpTo);
            filterPanel.Controls.Add(pnlTo);

            var pnlButtons = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true, WrapContents = true, Margin = new Padding(0, 20, 0, 0) };

            btnFilter = MakeButton("🔍 Lọc báo cáo", 125, Color.FromArgb(40, 167, 69));
            btnFilter.Click += async (_, _) =>
            {
                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Ngày không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                await LoadReportAsync();
            };
            pnlButtons.Controls.Add(btnFilter);

            var exportProducts = MakeButton("📁 Danh mục CSV", 135, Color.FromArgb(108, 117, 125));
            exportProducts.Click += async (_, _) => await DownloadCsvAsync("reports/export/products.csv", "DanhMucSanPham.csv");
            pnlButtons.Controls.Add(exportProducts);

            var exportOrders = MakeButton("📄 Hóa đơn CSV", 135, Color.FromArgb(108, 117, 125));
            exportOrders.Click += async (_, _) => await DownloadCsvAsync($"reports/export/orders.csv?from={dtpFrom.Value:yyyy-MM-dd}&to={dtpTo.Value:yyyy-MM-dd}", "BaoCaoHoaDon.csv");
            pnlButtons.Controls.Add(exportOrders);

            var exportRevenue = MakeButton("📊 Doanh thu CSV", 135, Color.FromArgb(108, 117, 125));
            exportRevenue.Click += async (_, _) => await DownloadCsvAsync($"reports/export/revenue.csv?from={dtpFrom.Value:yyyy-MM-dd}&to={dtpTo.Value:yyyy-MM-dd}", "BaoCaoDoanhThu.csv");
            pnlButtons.Controls.Add(exportRevenue);

            filterPanel.Controls.Add(pnlButtons);
            mainContainer.Controls.Add(filterPanel);

            // 3. Các thẻ Card thống kê
            cardsTable = new TableLayoutPanel
            {
                RowCount = 1,
                ColumnCount = 4,
                Height = 95,
                Margin = new Padding(0, 0, 0, 18),
                BackColor = Color.Transparent
            };
            cardsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            cardsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            cardsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            cardsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            lblRevenue = AddCard(cardsTable, "DOANH THU HÔM NAY", 0, Color.FromArgb(40, 167, 69));
            lblOrderCount = AddCard(cardsTable, "TỔNG HÓA ĐƠN", 1, Color.FromArgb(0, 123, 255));
            lblCustomers = AddCard(cardsTable, "KHÁCH HÀNG MỚI", 2, Color.FromArgb(111, 66, 193));
            lblLowStock = AddCard(cardsTable, "SẮP HẾT HÀNG", 3, Color.FromArgb(220, 53, 69));
            mainContainer.Controls.Add(cardsTable);

            // 4. Biểu đồ doanh thu
            mainContainer.Controls.Add(new Label { Text = "BIỂU ĐỒ DOANH THU THEO NGÀY", Font = new Font("Segoe UI", 11.5f, FontStyle.Bold), AutoSize = true, ForeColor = Color.FromArgb(33, 37, 41), Margin = new Padding(0, 5, 0, 8) });
            chartRevenue = new Chart
            {
                Height = 240,
                BackColor = Color.White,
                BorderlineColor = Color.FromArgb(222, 226, 230),
                BorderlineDashStyle = ChartDashStyle.Solid,
                BorderlineWidth = 1,
                Margin = new Padding(0, 0, 0, 18)
            };
            var revenueArea = new ChartArea("RevenueArea");
            revenueArea.BackColor = Color.White;
            revenueArea.AxisX.MajorGrid.Enabled = false;
            revenueArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            revenueArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(73, 80, 87);
            revenueArea.AxisX.Interval = 1;
            revenueArea.AxisY.MajorGrid.LineColor = Color.FromArgb(235, 238, 242);
            revenueArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9f);
            revenueArea.AxisY.LabelStyle.Format = "N0";
            revenueArea.AxisY.IsStartedFromZero = true;
            chartRevenue.ChartAreas.Add(revenueArea);
            var revenueSeries = new Series("Doanh thu")
            {
                ChartArea = "RevenueArea",
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(40, 167, 69),
                BorderWidth = 1,
                IsValueShownAsLabel = true,
                LabelFormat = "N0",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold)
            };
            chartRevenue.Series.Add(revenueSeries);
            mainContainer.Controls.Add(chartRevenue);

            // 5. Top 5 sản phẩm bán chạy
            mainContainer.Controls.Add(new Label { Text = "TOP 5 SẢN PHẨM BÁN CHẠY", Font = new Font("Segoe UI", 11.5f, FontStyle.Bold), AutoSize = true, ForeColor = Color.FromArgb(33, 37, 41), Margin = new Padding(0, 5, 0, 8) });
            dgvBestSellers = MakeGrid(new Size(100, 170));
            dgvBestSellers.Columns.Add("ProductName", "Sản phẩm");
            dgvBestSellers.Columns.Add("QuantitySold", "Số lượng đã bán");
            dgvBestSellers.Columns.Add("Revenue", "Doanh thu");
            dgvBestSellers.Columns["Revenue"]!.DefaultCellStyle.Format = "N0";
            mainContainer.Controls.Add(dgvBestSellers);

            // 6. Chi tiết hóa đơn
            mainContainer.Controls.Add(new Label { Text = "CHI TIẾT HÓA ĐƠN", Font = new Font("Segoe UI", 11.5f, FontStyle.Bold), AutoSize = true, ForeColor = Color.FromArgb(33, 37, 41), Margin = new Padding(0, 10, 0, 8) });
            dgvReport = MakeGrid(new Size(100, 260));
            dgvReport.Columns.Add("OrderId", "Mã hóa đơn");
            dgvReport.Columns.Add("OrderDate", "Ngày lập");
            dgvReport.Columns.Add("ItemCount", "Số lượng SP");
            dgvReport.Columns.Add("TotalAmount", "Tổng tiền");
            dgvReport.Columns["OrderDate"]!.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvReport.Columns["TotalAmount"]!.DefaultCellStyle.Format = "N0";
            mainContainer.Controls.Add(dgvReport);

            SizeChanged += (_, _) => AdjustControlsWidth();
            Shown += (_, _) => AdjustControlsWidth();
        }

        private void AdjustControlsWidth()
        {
            if (mainContainer == null || mainContainer.IsDisposed) return;
            int targetWidth = Math.Max(500, mainContainer.ClientSize.Width - 44);

            filterPanel.Width = targetWidth;
            cardsTable.Width = targetWidth;
            chartRevenue.Width = targetWidth;
            dgvBestSellers.Width = targetWidth;
            dgvReport.Width = targetWidth;
        }

        private Button MakeButton(string text, int width, Color backColor)
        {
            var b = new Button
            {
                Text = text,
                Size = new Size(width, 36),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 5),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private DataGridView MakeGrid(Size size)
        {
            var g = new DataGridView
            {
                Size = size,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                Margin = new Padding(0, 0, 0, 18)
            };
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            g.ColumnHeadersHeight = 42;
            g.RowTemplate.Height = 34;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(222, 226, 230);
            g.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            g.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            return g;
        }

        private Label AddCard(TableLayoutPanel parent, string title, int columnIndex, Color accentColor)
        {
            var p = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(columnIndex == 0 ? 0 : 6, 0, columnIndex == 3 ? 0 : 6, 0),
                Padding = new Padding(12)
            };

            var indicator = new Panel { Dock = DockStyle.Left, Width = 5, BackColor = accentColor };
            p.Controls.Add(indicator);

            var lblTitle = new Label { Text = title, Location = new Point(16, 12), AutoSize = true, ForeColor = Color.FromArgb(108, 117, 125), Font = new Font("Segoe UI", 8.5f, FontStyle.Bold) };
            var v = new Label { Text = "…", Location = new Point(16, 36), AutoSize = true, Font = new Font("Segoe UI", 16f, FontStyle.Bold), ForeColor = Color.FromArgb(33, 37, 41) };

            p.Controls.Add(lblTitle);
            p.Controls.Add(v);

            parent.Controls.Add(p, columnIndex, 0);
            return v;
        }

        private async Task LoadAllAsync()
        {
            if (IsDisposed || Disposing) return;
            await LoadDashboardAsync();
            if (IsDisposed || Disposing) return;
            await LoadReportAsync();
        }

        private async Task LoadDashboardAsync()
        {
            try
            {
                var s = await ApiClientService.Client.GetFromJsonAsync<DashboardSummary>("reports/dashboard-summary");
                if (IsDisposed || Disposing || !IsHandleCreated || s == null) return;

                lblRevenue.Text = s.RevenueToday.ToString("N0") + " đ";
                lblOrderCount.Text = s.OrderCount.ToString("N0");
                lblCustomers.Text = s.NewCustomersToday.ToString("N0");
                lblLowStock.Text = s.LowStockCount.ToString("N0");
            }
            catch (Exception ex)
            {
                if (IsDisposed || Disposing || !IsHandleCreated) return;
                MessageBox.Show("Không tải được số liệu tổng quan.\n" + ex.Message, "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool _isLoadingReport = false;

        private async Task LoadReportAsync()
        {
            if (IsDisposed || Disposing || !IsHandleCreated || _isLoadingReport) return;

            _isLoadingReport = true;
            btnFilter.Enabled = false;

            try
            {
                string from = dtpFrom.Value.ToString("yyyy-MM-dd");
                string to = dtpTo.Value.ToString("yyyy-MM-dd");

                var orders = await ApiClientService.Client.GetFromJsonAsync<List<OrderReportItem>>($"orders?from={from}&to={to}");
                if (IsDisposed || Disposing || !IsHandleCreated || dgvReport.Columns.Count == 0) return;

                dgvReport.Rows.Clear();
                foreach (var o in (orders ?? new List<OrderReportItem>()).OrderByDescending(x => x.OrderDate))
                {
                    if (IsDisposed || Disposing || !IsHandleCreated) return;
                    dgvReport.Rows.Add(o.OrderId, o.OrderDate, o.ItemCount, o.TotalAmount);
                }

                revenueDays = await ApiClientService.Client.GetFromJsonAsync<List<RevenueDay>>($"reports/revenue-by-day?from={from}&to={to}") ?? new List<RevenueDay>();
                if (IsDisposed || Disposing || !IsHandleCreated) return;

                chartRevenue.Series["Doanh thu"].Points.Clear();
                foreach (var day in revenueDays)
                    chartRevenue.Series["Doanh thu"].Points.AddXY(day.Date.ToString("dd/MM"), day.Revenue);
                chartRevenue.Invalidate();

                var sellers = await ApiClientService.Client.GetFromJsonAsync<List<BestSeller>>("reports/best-sellers?take=5");
                if (IsDisposed || Disposing || !IsHandleCreated || dgvBestSellers.Columns.Count == 0) return;

                dgvBestSellers.Rows.Clear();
                foreach (var x in sellers ?? new List<BestSeller>())
                {
                    if (IsDisposed || Disposing || !IsHandleCreated) return;
                    dgvBestSellers.Rows.Add(x.ProductName, x.QuantitySold, x.Revenue);
                }
            }
            catch (Exception ex)
            {
                if (IsDisposed || Disposing || !IsHandleCreated) return;
                MessageBox.Show("Không tải được báo cáo.\n" + ex.Message, "Lỗi báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoadingReport = false;
                if (!IsDisposed && !Disposing && IsHandleCreated) btnFilter.Enabled = true;
            }
        }

        private async Task DownloadCsvAsync(string endpoint, string suggestedName)
        {
            using var dialog = new SaveFileDialog { Filter = "Tệp CSV (*.csv)|*.csv", FileName = suggestedName, Title = "Xuất báo cáo CSV" };
            if (dialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                var response = await ApiClientService.Client.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("API không cho phép xuất dữ liệu. Hãy đăng nhập bằng tài khoản Quản lý.", "Không thể xuất", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var bytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(dialog.FileName, bytes);
                MessageBox.Show("Đã xuất tệp CSV thành công.", "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xuất tệp.\n" + ex.Message, "Lỗi xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormQuickReport_Load(object sender, EventArgs e) { }

        private void FormQuickReport_Load_1(object sender, EventArgs e)
        {

        }
    }
}