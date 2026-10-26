
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.Json;

namespace MiniSupermarket.WinForms
{
    public partial class FormCustomerManagement : Form
    {
        private DataGridView dgvCustomer = null!;
        private TextBox txtName = null!;
        private TextBox txtPhone = null!;
        private TextBox txtAddress = null!;
        private TextBox txtRewardPoints = null!;
        private ComboBox cboMembershipRank = null!;
        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private Button btnPagePrevious = null!;
        private Button btnPageNext = null!;
        private Label lblPageInfo = null!;
        private int currentPage = 1;
        private const int pageSize = 10;
        private int totalPages = 1;
        private int totalCount = 0;
        private bool loadingRows = false;

        public FormCustomerManagement()
        {
            InitializeComponent();
            InitializeCustomLayout();
            DailyFreshTheme.Apply(this);
            LoadCustomerData();
        }

        private void InitializeCustomLayout()
        {
            Text = "Quản lý Khách hàng";
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(244, 245, 247);

            Controls.Add(new Label
            {
                Text = "Quản lý Khách hàng",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(25, 20),
                AutoSize = true
            });

            Controls.Add(new Label
            {
                Text = "Thêm, cập nhật và quản lý thông tin khách hàng",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(27, 58),
                AutoSize = true
            });

            var tablePanel = new Panel
            {
                Location = new Point(25, 95),
                Size = new Size(650, 480),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(tablePanel);

            dgvCustomer = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(628, 418),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoGenerateColumns = true
            };
            dgvCustomer.SelectionChanged += DgvCustomer_SelectionChanged;
            var paginationPanel = new Panel { Location = new Point(10, 432), Size = new Size(628, 36), Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            btnPagePrevious = new Button { Text = "← Trang trước", Location = new Point(0, 3), Size = new Size(115, 30), Enabled = false };
            btnPageNext = new Button { Text = "Trang sau →", Location = new Point(513, 3), Size = new Size(115, 30) };
            lblPageInfo = new Label { Text = "Trang 1/1", Location = new Point(120, 3), Size = new Size(388, 30), TextAlign = ContentAlignment.MiddleCenter };
            btnPagePrevious.Click += async (sender, args) => { if (currentPage > 1) { currentPage--; await LoadCustomerDataAsync(); } };
            btnPageNext.Click += async (sender, args) => { if (currentPage < totalPages) { currentPage++; await LoadCustomerDataAsync(); } };
            paginationPanel.Controls.Add(btnPagePrevious); paginationPanel.Controls.Add(lblPageInfo); paginationPanel.Controls.Add(btnPageNext);
            paginationPanel.Resize += (sender, args) => { btnPageNext.Left = paginationPanel.ClientSize.Width - btnPageNext.Width; lblPageInfo.Width = Math.Max(80, btnPageNext.Left - btnPagePrevious.Right); };
            tablePanel.Controls.Add(dgvCustomer);
            tablePanel.Controls.Add(paginationPanel);

            var detailPanel = new Panel
            {
                Location = new Point(695, 95),
                Size = new Size(350, 480),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(detailPanel);

            detailPanel.Controls.Add(new Label
            {
                Text = "Thông tin khách hàng",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                AutoSize = true
            });

            CreateLabel(detailPanel, "Họ tên", 20, 50);
            txtName = CreateTextBox(detailPanel, 20, 72);

            CreateLabel(detailPanel, "Số điện thoại", 20, 110);
            txtPhone = CreateTextBox(detailPanel, 20, 132);

            CreateLabel(detailPanel, "Địa chỉ", 20, 170);
            txtAddress = CreateTextBox(detailPanel, 20, 192);

            CreateLabel(detailPanel, "Điểm thưởng", 20, 230);
            txtRewardPoints = CreateTextBox(detailPanel, 20, 252);

            CreateLabel(detailPanel, "Hạng thành viên", 20, 290);
            cboMembershipRank = new ComboBox
            {
                Location = new Point(20, 312),
                Size = new Size(310, 28),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboMembershipRank.Items.AddRange(new object[] { "Chuẩn", "Bạc", "Vàng" });
            cboMembershipRank.SelectedIndex = 0;
            detailPanel.Controls.Add(cboMembershipRank);

            btnAdd = CreateButton("Thêm mới", 20, 355, Color.FromArgb(40, 167, 69));
            btnAdd.Click += BtnAdd_Click;
            detailPanel.Controls.Add(btnAdd);

            btnUpdate = CreateButton("Cập nhật", 180, 355, Color.FromArgb(23, 162, 184));
            btnUpdate.Click += BtnUpdate_Click;
            detailPanel.Controls.Add(btnUpdate);

            btnDelete = CreateButton("Xóa", 20, 405, Color.FromArgb(220, 53, 69));
            btnDelete.Click += BtnDelete_Click;
            detailPanel.Controls.Add(btnDelete);

            btnRefresh = CreateButton("Làm mới", 180, 405, Color.FromArgb(108, 117, 125));
            btnRefresh.Click += BtnRefresh_Click;
            detailPanel.Controls.Add(btnRefresh);
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
                Size = new Size(310, 27),
                Font = new Font("Segoe UI", 10)
            };

            parent.Controls.Add(textBox);
            return textBox;
        }

        private Button CreateButton(string text, int x, int y, Color color)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(145, 38),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private async void LoadCustomerData() => await LoadCustomerDataAsync();

        private async System.Threading.Tasks.Task LoadCustomerDataAsync()
        {
            try
            {
                loadingRows = true;
                var jsonResult = await ApiClientService.GetDataWithTokenAsync(
                    $"customers/paged?pageNumber={currentPage}&pageSize={pageSize}");
                var result = JsonSerializer.Deserialize<PagedListResponse<CustomerModel>>(jsonResult,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var customers = result?.Items ?? new List<CustomerModel>();
                totalCount = result?.TotalCount ?? customers.Count;
                totalPages = Math.Max(1, result?.TotalPages ?? 1);
                if (currentPage > totalPages) { currentPage = totalPages; loadingRows = false; await LoadCustomerDataAsync(); return; }
                dgvCustomer.DataSource = null;
                dgvCustomer.DataSource = customers;
                lblPageInfo.Text = $"Trang {currentPage}/{totalPages} • Tổng {totalCount} khách hàng";
                btnPagePrevious.Enabled = currentPage > 1;
                btnPageNext.Enabled = currentPage < totalPages;

                if (dgvCustomer.Columns["CustomerId"] != null)
                    dgvCustomer.Columns["CustomerId"]!.HeaderText = "Mã khách hàng";

                if (dgvCustomer.Columns["CustomerName"] != null)
                    dgvCustomer.Columns["CustomerName"]!.HeaderText = "Họ tên";

                if (dgvCustomer.Columns["PhoneNumber"] != null)
                    dgvCustomer.Columns["PhoneNumber"]!.HeaderText = "Số điện thoại";

                if (dgvCustomer.Columns["Address"] != null)
                    dgvCustomer.Columns["Address"]!.HeaderText = "Địa chỉ";

                if (dgvCustomer.Columns["RewardPoints"] != null)
                    dgvCustomer.Columns["RewardPoints"]!.HeaderText = "Điểm thưởng";

                if (dgvCustomer.Columns["MembershipRank"] != null)
                    dgvCustomer.Columns["MembershipRank"]!.HeaderText = "Hạng thành viên";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách khách hàng: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally { loadingRows = false; }
        }

        private void DgvCustomer_SelectionChanged(object? sender, EventArgs e)
        {
            if (loadingRows || dgvCustomer.SelectedRows.Count == 0)
                return;

            var row = dgvCustomer.SelectedRows[0];

            txtName.Text = row.Cells["CustomerName"].Value?.ToString() ?? "";
            txtPhone.Text = row.Cells["PhoneNumber"].Value?.ToString() ?? "";
            txtAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";
            txtRewardPoints.Text = row.Cells["RewardPoints"].Value?.ToString() ?? "0";

            string rank = row.Cells["MembershipRank"].Value?.ToString() ?? "Chuẩn";
            cboMembershipRank.SelectedItem = rank;

            if (cboMembershipRank.SelectedIndex < 0)
                cboMembershipRank.SelectedIndex = 0;
        }

        private bool ValidateInput()
        {
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

            if (!int.TryParse(txtRewardPoints.Text.Trim(), out int points) || points < 0)
            {
                MessageBox.Show("Điểm thưởng phải là số nguyên không âm!");
                txtRewardPoints.Focus();
                return false;
            }

            return true;
        }

        private object GetCustomerInput()
        {
            return new
            {
                CustomerName = txtName.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                RewardPoints = int.Parse(txtRewardPoints.Text.Trim()),
                MembershipRank = cboMembershipRank.SelectedItem?.ToString() ?? "Chuẩn"
            };
        }

        private async void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                bool success = await ApiClientService.PostDataWithTokenAsync(
                    "customers", GetCustomerInput());

                if (success)
                {
                    MessageBox.Show("Thêm khách hàng thành công!");
                    ClearInputs();
                    currentPage = 1;
                    LoadCustomerData();
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm khách hàng: " + ex.Message);
            }
        }

        private async void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật!");
                return;
            }

            if (!ValidateInput())
                return;

            var customerId = dgvCustomer.SelectedRows[0].Cells["CustomerId"].Value;

            try
            {
                bool success = await ApiClientService.PutDataWithTokenAsync(
                    $"customers/{customerId}", GetCustomerInput());

                if (success)
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!");
                    LoadCustomerData();
                }
                else
                {
                    MessageBox.Show("Cập nhật khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật khách hàng: " + ex.Message);
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
                return;
            }

            var customerId = dgvCustomer.SelectedRows[0].Cells["CustomerId"].Value;

            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa khách hàng này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool success = await ApiClientService.DeleteDataWithTokenAsync(
                    $"customers/{customerId}");

                if (success)
                {
                    MessageBox.Show("Xóa khách hàng thành công!");
                    ClearInputs();
                    currentPage = 1;
                    LoadCustomerData();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa khách hàng: " + ex.Message);
            }
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            ClearInputs();
            currentPage = 1;
            LoadCustomerData();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtRewardPoints.Text = "0";
            cboMembershipRank.SelectedIndex = 0;
            dgvCustomer.ClearSelection();
        }

        private void FormCustomerManagement_Load(object sender, EventArgs e)
        {
        }
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