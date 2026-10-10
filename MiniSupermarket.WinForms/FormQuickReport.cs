
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormQuickReport : Form
    {
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;
        private Label lblRevenue = null!;
        private Label lblOrderCount = null!;
        private Label lblAverage = null!;
        private DataGridView dgvReport = null!;
        private Button btnFilter = null!;

        private sealed class OrderReportItem
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public int ItemCount { get; set; }
            public decimal TotalAmount { get; set; }
        }

        public FormQuickReport()
        {
            InitializeComponent();
            Controls.Clear();
            BuildLayout();

            Load += FormQuickReport_Load;
        }

        private async void FormQuickReport_Load(object? sender, EventArgs e)
        {
            await LoadReportAsync();
        }

        private void BuildLayout()
        {
            Text = "Báo cáo doanh thu";
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);
            Dock = DockStyle.Fill;
            AutoScroll = true;
            MinimumSize = new Size(750, 500);

            Controls.Add(new Label
            {
                Text = "BÁO CÁO DOANH THU",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 75),
                Location = new Point(24, 18),
                AutoSize = true
            });

            Controls.Add(new Label
            {
                Text = "Theo dõi doanh thu và hóa đơn thực tế từ SQL Server",
                ForeColor = Color.DimGray,
                Location = new Point(27, 58),
                AutoSize = true
            });

            var filterPanel = new Panel
            {
                Location = new Point(24, 95),
                Size = new Size(700, 95),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White
            };
            Controls.Add(filterPanel);

            filterPanel.Controls.Add(new Label
            {
                Text = "Từ ngày",
                Location = new Point(15, 14),
                AutoSize = true
            });

            dtpFrom = new DateTimePicker
            {
                Location = new Point(15, 40),
                Size = new Size(180, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(-7)
            };
            filterPanel.Controls.Add(dtpFrom);

            filterPanel.Controls.Add(new Label
            {
                Text = "Đến ngày",
                Location = new Point(215, 14),
                AutoSize = true
            });

            dtpTo = new DateTimePicker
            {
                Location = new Point(215, 40),
                Size = new Size(180, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            filterPanel.Controls.Add(dtpTo);

            btnFilter = new Button
            {
                Text = "Lọc báo cáo",
                Location = new Point(420, 38),
                Size = new Size(125, 35),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.Click += BtnFilter_Click;
            filterPanel.Controls.Add(btnFilter);

            var btnToday = new Button
            {
                Text = "Hôm nay",
                Location = new Point(555, 38),
                Size = new Size(105, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnToday.FlatAppearance.BorderSize = 0;
            btnToday.Click += async (s, e) =>
            {
                dtpFrom.Value = DateTime.Today;
                dtpTo.Value = DateTime.Today;
                await LoadReportAsync();
            };
            filterPanel.Controls.Add(btnToday);

            var cards = new Panel
            {
                Location = new Point(24, 205),
                Size = new Size(700, 110),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.Transparent
            };
            Controls.Add(cards);

            lblRevenue = AddSummaryCard(
                cards, "Tổng doanh thu", 0,
                Color.FromArgb(25, 135, 84));

            lblOrderCount = AddSummaryCard(
                cards, "Số hóa đơn", 235,
                Color.FromArgb(13, 110, 253));

            lblAverage = AddSummaryCard(
                cards, "Trung bình/hóa đơn", 470,
                Color.FromArgb(111, 66, 193));

            dgvReport = new DataGridView
            {
                Location = new Point(24, 335),
                Size = new Size(700, 280),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right,
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

            dgvReport.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(35, 75, 95);
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReport.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            dgvReport.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 235, 245);
            dgvReport.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvReport.Columns.Add("OrderId", "Mã hóa đơn");
            dgvReport.Columns.Add("OrderDate", "Ngày lập");
            dgvReport.Columns.Add("ItemCount", "Số lượng SP");
            dgvReport.Columns.Add("Total", "Tổng tiền");

            dgvReport.Columns["OrderDate"]!.DefaultCellStyle.Format =
                "dd/MM/yyyy HH:mm";
            dgvReport.Columns["Total"]!.DefaultCellStyle.Format = "N0";

            Controls.Add(dgvReport);
        }

        private Label AddSummaryCard(
            Panel parent, string title, int x, Color color)
        {
            var card = new Panel
            {
                Location = new Point(x, 0),
                Size = new Size(220, 100),
                BackColor = Color.White
            };
            parent.Controls.Add(card);

            card.Controls.Add(new Label
            {
                Text = title,
                Location = new Point(12, 12),
                AutoSize = true,
                ForeColor = Color.DimGray
            });

            var value = new Label
            {
                Text = "0",
                Location = new Point(12, 42),
                AutoSize = true,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = color
            };
            card.Controls.Add(value);

            return value;
        }

        private async void BtnFilter_Click(object? sender, EventArgs e)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show(
                    "Ngày bắt đầu không được lớn hơn ngày kết thúc.",
                    "Ngày không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await LoadReportAsync();
        }

        private async Task LoadReportAsync()
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
                return;
            }

            btnFilter.Enabled = false;

            try
            {
                string from = Uri.EscapeDataString(
                    dtpFrom.Value.Date.ToString("yyyy-MM-dd"));

                string to = Uri.EscapeDataString(
                    dtpTo.Value.Date.ToString("yyyy-MM-dd"));

                var orders = await ApiClientService.Client
                    .GetFromJsonAsync<System.Collections.Generic.List<OrderReportItem>>(
                        $"orders?from={from}&to={to}");

                dgvReport.Rows.Clear();

                orders ??= new System.Collections.Generic.List<OrderReportItem>();

                foreach (var order in orders.OrderByDescending(x => x.OrderDate))
                {
                    dgvReport.Rows.Add(
                        order.OrderId,
                        order.OrderDate,
                        order.ItemCount,
                        order.TotalAmount);
                }

                decimal revenue = orders.Sum(x => x.TotalAmount);
                int orderCount = orders.Count;

                lblRevenue.Text = revenue.ToString("N0") + " đ";
                lblOrderCount.Text = orderCount.ToString();
                lblAverage.Text = orderCount == 0
                    ? "0 đ"
                    : (revenue / orderCount).ToString("N0") + " đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không tải được báo cáo doanh thu từ API.\n" +
                    "Hãy kiểm tra API đang chạy và đã thêm GET api/orders vào OrdersController.\n\n" +
                    ex.Message,
                    "Lỗi báo cáo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnFilter.Enabled = true;
            }
        }
    }
}