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
    public partial class FormCustomerManagement : Form
    {
        // Khai báo các control giao diện bằng code thuần
        private DataGridView dgvCustomers = null!;
        private TextBox txtCustomerId = null!;
        private TextBox txtCustomerName = null!;
        private TextBox txtPhoneNumber = null!;
        private TextBox txtAddress = null!;
        private TextBox txtRewardPoints = null!;
        private TextBox txtMembershipRank = null!;
        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnLoad = null!;

        public FormCustomerManagement()
        {
            InitializeComponent();
            InitializeCustomComponents(); // Gọi hàm tự dựng giao diện
        }

        // Hàm tự động khởi tạo giao diện 100% bằng code khớp với phong cách dự án
        private void InitializeCustomComponents()
        {
            this.Text = "QUẢN LÝ KHÁCH HÀNG & TÍCH ĐIỂM";
            this.Size = new Size(1050, 620);
            this.BackColor = Color.FromArgb(244, 245, 247);

            // 1. DataGridView hiển thị danh sách khách hàng (bên trái)
            dgvCustomers = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(620, 520),
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            dgvCustomers.CellClick += dgvCustomers_CellClick;
            this.Controls.Add(dgvCustomers);

            int startX = 660;
            int startY = 20;
            int spacing = 45;

            // 2. Các ô nhập liệu và nhãn thông tin (bên phải)
            this.Controls.Add(new Label { Text = "Mã KH:", Location = new Point(startX, startY), AutoSize = true });
            txtCustomerId = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(220, 25), ReadOnly = true };
            this.Controls.Add(txtCustomerId);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Tên khách hàng:", Location = new Point(startX, startY), AutoSize = true });
            txtCustomerName = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(220, 25) };
            this.Controls.Add(txtCustomerName);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Số điện thoại:", Location = new Point(startX, startY), AutoSize = true });
            txtPhoneNumber = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(220, 25) };
            this.Controls.Add(txtPhoneNumber);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Địa chỉ:", Location = new Point(startX, startY), AutoSize = true });
            txtAddress = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(220, 25) };
            this.Controls.Add(txtAddress);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Điểm thưởng:", Location = new Point(startX, startY), AutoSize = true });
            txtRewardPoints = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(220, 25), ReadOnly = true };
            this.Controls.Add(txtRewardPoints);

            startY += spacing;
            this.Controls.Add(new Label { Text = "Hạng thành viên:", Location = new Point(startX, startY), AutoSize = true });
            txtMembershipRank = new TextBox { Location = new Point(startX + 120, startY), Size = new Size(220, 25), ReadOnly = true };
            this.Controls.Add(txtMembershipRank);

            // 3. Các nút Thêm, Sửa, Xóa, Tải lại
            startY += spacing + 25;
            btnAdd = new Button { Text = "Thêm mới", Location = new Point(startX, startY), Size = new Size(105, 38), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnAdd.Click += btnAdd_Click;
            this.Controls.Add(btnAdd);

            btnUpdate = new Button { Text = "Cập nhật", Location = new Point(startX + 115, startY), Size = new Size(105, 38), BackColor = Color.FromArgb(23, 162, 184), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnUpdate.Click += btnUpdate_Click;
            this.Controls.Add(btnUpdate);

            startY += 48;
            btnDelete = new Button { Text = "Xóa", Location = new Point(startX, startY), Size = new Size(105, 38), BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnDelete.Click += btnDelete_Click;
            this.Controls.Add(btnDelete);

            btnLoad = new Button { Text = "Tải lại", Location = new Point(startX + 115, startY), Size = new Size(105, 38), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnLoad.Click += btnLoad_Click;
            this.Controls.Add(btnLoad);
        }

        // Cấu hình HttpClient gắn kèm Token bảo mật theo chuẩn của dự án
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

        // Sự kiện Load Form
        private async void FormCustomerManagement_Load(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        // Hàm gọi API GET danh sách khách hàng
        private async Task LoadDataAsync()
        {
            try
            {
                using var client = GetAuthenticatedClient();
                var customers = await client.GetFromJsonAsync<List<CustomerDto>>("customers");
                dgvCustomers.DataSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLoad_Click(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        // Click dòng trên DataGridView để đưa dữ liệu lên các ô nhập
        private void dgvCustomers_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtCustomerId.Text = row.Cells["CustomerId"].Value?.ToString() ?? string.Empty;
                txtCustomerName.Text = row.Cells["CustomerName"].Value?.ToString() ?? string.Empty;
                txtPhoneNumber.Text = row.Cells["PhoneNumber"].Value?.ToString() ?? string.Empty;
                txtAddress.Text = row.Cells["Address"]?.Value?.ToString() ?? string.Empty;
                txtRewardPoints.Text = row.Cells["RewardPoints"]?.Value?.ToString() ?? "0";
                txtMembershipRank.Text = row.Cells["MembershipRank"]?.Value?.ToString() ?? string.Empty;
            }
        }

        // Thêm mới khách hàng (POST)
        private async void btnAdd_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) || string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                MessageBox.Show("Vui lòng nhập tên và số điện thoại khách hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newCust = new
            {
                CustomerName = txtCustomerName.Text.Trim(),
                PhoneNumber = txtPhoneNumber.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                RewardPoints = 0,
                MembershipRank = "Chuẩn"
            };

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.PostAsJsonAsync("customers", newCust);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
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

        // Cập nhật thông tin khách hàng (PUT)
        private async void btnUpdate_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa trên bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtCustomerId.Text);
            var updateCust = new
            {
                CustomerId = id,
                CustomerName = txtCustomerName.Text.Trim(),
                PhoneNumber = txtPhoneNumber.Text.Trim(),
                Address = txtAddress.Text.Trim()
            };

            try
            {
                using var client = GetAuthenticatedClient();
                var response = await client.PutAsJsonAsync($"customers/{id}", updateCust);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
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

        // Xóa khách hàng (DELETE)
        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId.Text))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = int.Parse(txtCustomerId.Text);
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa khách hàng ID = {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using var client = GetAuthenticatedClient();
                    var response = await client.DeleteAsync($"customers/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đã xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadDataAsync();
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

        // Hàm xóa trắng ô nhập liệu
        private void ClearInputs()
        {
            txtCustomerId.Text = "";
            txtCustomerName.Text = "";
            txtPhoneNumber.Text = "";
            txtAddress.Text = "";
            txtRewardPoints.Text = "";
            txtMembershipRank.Text = "";
        }
    }

    // Lớp DTO trung gian ánh xạ dữ liệu khách hàng từ API
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int RewardPoints { get; set; }
        public string MembershipRank { get; set; } = string.Empty;
    }
}