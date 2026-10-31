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
    public partial class FormCustomerManagement : Form
    {
        private DataGridView dgvCustomer = null!;
        private TextBox txtName = null!;
        private TextBox txtPhone = null!;
        private TextBox txtAddress = null!;
        private TextBox txtRewardPoints = null!;
        private ComboBox cmbMembershipRank = null!;
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

        public FormCustomerManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();
            DailyFreshTheme.Apply(this);

            Load += FormCustomerManagement_Load;
            Resize += FormCustomerManagement_Resize;
        }

        private async void FormCustomerManagement_Load(object? sender, EventArgs e)
        {
            await LoadCustomerData();
        }

        private void InitializeCustomLayout()
        {
            Text = "Quản lý Khách hàng";
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(244, 246, 248);
            AutoScroll = true;

            Controls.Add(new Label
            {
                Text = "Quản lý Khách hàng",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 15),
                AutoSize = true
            });

            Controls.Add(new Label
            {
                Text = "Thêm, cập nhật và quản lý thông tin khách hàng thành viên",
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
                PlaceholderText = "Nhập tên hoặc số điện thoại..."
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

            dgvCustomer = new DataGridView
            {
                Dock = DockStyle.None,
                Location = new Point(8, 8),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
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
                EnableHeadersVisualStyles = false
            };

            dgvCustomer.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(35, 75, 95);
            dgvCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomer.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);
            dgvCustomer.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvCustomer.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 235, 245);
            dgvCustomer.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvCustomer.SelectionChanged += DgvCustomer_SelectionChanged;

            tablePanel.Padding = new Padding(8);
            tablePanel.Controls.Add(dgvCustomer);

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
                Text = "Thông tin khách hàng",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(18, 12),
                AutoSize = true
            });

            CreateLabel(detailPanel, "Họ tên khách hàng", 18, 48);
            txtName = CreateTextBox(detailPanel, 18, 70);

            CreateLabel(detailPanel, "Số điện thoại", 18, 105);
            txtPhone = CreateTextBox(detailPanel, 18, 127);

            CreateLabel(detailPanel, "Địa chỉ", 18, 162);
            txtAddress = CreateTextBox(detailPanel, 18, 184);

            CreateLabel(detailPanel, "Điểm thưởng", 18, 219);
            txtRewardPoints = CreateTextBox(detailPanel, 18, 241);

            CreateLabel(detailPanel, "Hạng thành viên", 18, 276);
            cmbMembershipRank = new ComboBox
            {
                Location = new Point(18, 298),
                Size = new Size(280, 28),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cmbMembershipRank.Items.AddRange(new object[] { "Chuẩn", "Bạc", "Vàng" });
            cmbMembershipRank.SelectedIndex = 0;
            detailPanel.Controls.Add(cmbMembershipRank);

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

            FormCustomerManagement_Resize(this, EventArgs.Empty);
        }

        private void FormCustomerManagement_Resize(object? sender, EventArgs e)
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

            dgvCustomer.Width = tablePanel.ClientSize.Width - 16;
            dgvCustomer.Height = Math.Max(100, tablePanel.ClientSize.Height - 58);
        }

        private void SetDetailControlWidths(int panelWidth)
        {
            int inputWidth = Math.Max(150, panelWidth - 45);
            txtName.Width = inputWidth;
            txtPhone.Width = inputWidth;
            txtAddress.Width = inputWidth;
            txtRewardPoints.Width = inputWidth;
            cmbMembershipRank.Width = inputWidth;

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

        private async Task LoadCustomerData()
        {
            try
            {
                isLoading = true;

                string url = $"customers/paged?pageNumber={currentPage}&pageSize={pageSize}" +
                             (string.IsNullOrWhiteSpace(currentKeyword) ? string.Empty : "&keyword=" + Uri.EscapeDataString(currentKeyword));

                string json = await ApiClientService.GetDataWithTokenAsync(url);
                var result = JsonSerializer.Deserialize<PagedCustomerResponse>(json, jsonOptions);

                var customers = result?.Items ?? new List<CustomerModel>();
                totalCount = result?.TotalCount ?? customers.Count;
                totalPages = Math.Max(1, result?.TotalPages ?? 1);

                if (currentPage > totalPages)
                {
                    currentPage = totalPages;
                    isLoading = false;
                    await LoadCustomerData();
                    return;
                }

                dgvCustomer.DataSource = customers;
                ConfigureCustomerColumns();
                UpdatePaginationControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách khách hàng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void ConfigureCustomerColumns()
        {
            SetColumnHeader("CustomerId", "Mã KH");
            SetColumnHeader("CustomerName", "Họ tên");
            SetColumnHeader("PhoneNumber", "Số điện thoại");
            SetColumnHeader("Address", "Địa chỉ");
            SetColumnHeader("RewardPoints", "Điểm thưởng");
            SetColumnHeader("MembershipRank", "Hạng thành viên");

            if (dgvCustomer.Columns["RewardPoints"] != null)
                dgvCustomer.Columns["RewardPoints"]!.DefaultCellStyle.Format = "N0";
        }

        private void UpdatePaginationControls()
        {
            lblPagination.Text =
                $"Trang {currentPage}/{totalPages}  •  Tổng {totalCount} khách hàng";
            btnPrevious.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
        }

        private async void BtnPrevious_Click(object? sender, EventArgs e)
        {
            if (currentPage <= 1)
                return;

            currentPage--;
            await LoadCustomerData();
        }

        private async void BtnNext_Click(object? sender, EventArgs e)
        {
            if (currentPage >= totalPages)
                return;

            currentPage++;
            await LoadCustomerData();
        }

        private void SetColumnHeader(string columnName, string header)
        {
            if (dgvCustomer.Columns[columnName] != null)
                dgvCustomer.Columns[columnName]!.HeaderText = header;
        }

        private void DgvCustomer_SelectionChanged(object? sender, EventArgs e)
        {
            if (isLoading || dgvCustomer.SelectedRows.Count == 0)
                return;

            var row = dgvCustomer.SelectedRows[0];

            txtName.Text = row.Cells["CustomerName"]?.Value?.ToString() ?? "";
            txtPhone.Text = row.Cells["PhoneNumber"]?.Value?.ToString() ?? "";
            txtAddress.Text = row.Cells["Address"]?.Value?.ToString() ?? "";
            txtRewardPoints.Text = row.Cells["RewardPoints"]?.Value?.ToString() ?? "0";

            string rank = row.Cells["MembershipRank"]?.Value?.ToString() ?? "Chuẩn";
            cmbMembershipRank.SelectedItem = rank;

            if (cmbMembershipRank.SelectedIndex < 0)
                cmbMembershipRank.SelectedIndex = 0;
        }

        private bool ValidateInputs(out int points)
        {
            points = 0;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng!");
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!");
                txtPhone.Focus();
                return false;
            }

            if (!int.TryParse(txtRewardPoints.Text.Trim(), out points) || points < 0)
            {
                MessageBox.Show("Điểm thưởng phải là số nguyên không âm!");
                txtRewardPoints.Focus();
                return false;
            }

            return true;
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs(out int points))
                return;

            var newCustomer = new
            {
                CustomerName = txtName.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                RewardPoints = points,
                MembershipRank = cmbMembershipRank.SelectedItem?.ToString() ?? "Chuẩn"
            };

            try
            {
                bool success = await ApiClientService.PostDataWithTokenAsync(
                    "customers", newCustomer);

                if (success)
                {
                    MessageBox.Show("Thêm khách hàng thành công!");
                    currentPage = 1;
                    ClearInputs();
                    await LoadCustomerData();
                }
                else
                    MessageBox.Show("Thêm thất bại. Kiểm tra dữ liệu và quyền truy cập.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật!");
                return;
            }

            if (!ValidateInputs(out int points))
                return;

            object? id = dgvCustomer.SelectedRows[0].Cells["CustomerId"]?.Value;
            if (id == null)
            {
                MessageBox.Show("Không lấy được mã khách hàng!");
                return;
            }

            var updatedCustomer = new
            {
                CustomerName = txtName.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                RewardPoints = points,
                MembershipRank = cmbMembershipRank.SelectedItem?.ToString() ?? "Chuẩn"
            };

            try
            {
                bool success = await ApiClientService.PutDataWithTokenAsync(
                    $"customers/{id}", updatedCustomer);

                if (success)
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!");
                    await LoadCustomerData();
                }
                else
                    MessageBox.Show("Cập nhật thất bại. Kiểm tra dữ liệu và quyền truy cập.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật khách hàng: " + ex.Message);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
                return;
            }

            object? id = dgvCustomer.SelectedRows[0].Cells["CustomerId"]?.Value;
            if (id == null)
            {
                MessageBox.Show("Không lấy được mã khách hàng!");
                return;
            }

            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa khách hàng này?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                bool success = await ApiClientService.DeleteDataWithTokenAsync(
                    $"customers/{id}");

                if (success)
                {
                    MessageBox.Show("Xóa khách hàng thành công!");
                    ClearInputs();
                    currentPage = 1;
                    await LoadCustomerData();
                }
                else
                    MessageBox.Show("Xóa thất bại. Khách hàng có thể đang được tham chiếu hóa đơn.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message);
            }
        }

        private async void BtnSearch_Click(object? sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            currentKeyword = keyword;
            currentPage = 1;
            await LoadCustomerData();
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            currentKeyword = string.Empty;
            ClearInputs();
            currentPage = 1;
            await LoadCustomerData();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtRewardPoints.Text = "0";
            cmbMembershipRank.SelectedIndex = 0;
            dgvCustomer.ClearSelection();
        }
    }

    public class PagedCustomerResponse
    {
        public List<CustomerModel> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int RewardPoints { get; set; }
        public string MembershipRank { get; set; } = "Chuẩn";
    }
}