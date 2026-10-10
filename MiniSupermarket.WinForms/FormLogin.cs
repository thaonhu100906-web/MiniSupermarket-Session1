
/*
 * Họ tên: Trần Thị Thảo Như
 * MSSV: 2124110075
 * Lớp: CCQ2411C
 * Môn: Lập trình ứng dụng
 */

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Drawing;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormLogin : Form
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7005/api/")
        };

        public FormLogin()
        {
            InitializeComponent();

            // Thay giao diện cũ bằng giao diện mới
            BuildLoginUI();
        }

        private void BuildLoginUI()
        {
            Controls.Clear();

            Text = "Daily Fresh - Đăng nhập";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(850, 500);
            MinimumSize = new Size(850, 500);
            BackColor = Color.White;

            // Bên trái
            Panel leftPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(390, 500),
                BackColor = Color.FromArgb(35, 115, 75)
            };

            Label lblBrand = new Label
            {
                Text = "DAILY FRESH",
                Font = new Font("Arial", 25, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(65, 150)
            };

            Label lblSlogan = new Label
            {
                Text = "Siêu thị tiện lợi\nMua sắm mỗi ngày",
                Font = new Font("Arial", 15),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(68, 210)
            };

            leftPanel.Controls.Add(lblBrand);
            leftPanel.Controls.Add(lblSlogan);

            // Bên phải
            Label lblTitle = new Label
            {
                Text = "ĐĂNG NHẬP",
                Font = new Font("Arial", 23, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 115, 75),
                AutoSize = true,
                Location = new Point(490, 75)
            };

            Label lblUsername = new Label
            {
                Text = "Tài khoản",
                Font = new Font("Arial", 11),
                AutoSize = true,
                Location = new Point(465, 155)
            };

            TextBox txtUsername = new TextBox
            {
                Name = "txtUsername",
                Font = new Font("Arial", 12),
                Location = new Point(465, 185),
                Size = new Size(310, 35)
            };

            Label lblPassword = new Label
            {
                Text = "Mật khẩu",
                Font = new Font("Arial", 11),
                AutoSize = true,
                Location = new Point(465, 235)
            };

            TextBox txtPassword = new TextBox
            {
                Name = "txtPassword",
                Font = new Font("Arial", 12),
                Location = new Point(465, 265),
                Size = new Size(310, 35),
                UseSystemPasswordChar = true
            };

            Button btnLogin = new Button
            {
                Text = "ĐĂNG NHẬP",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(35, 115, 75),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(465, 330),
                Size = new Size(310, 45),
                Cursor = Cursors.Hand
            };

            Label lblStatus = new Label
            {
                Name = "lblStatus",
                Text = "",
                ForeColor = Color.Red,
                Font = new Font("Arial", 9),
                AutoSize = true,
                Location = new Point(465, 390)
            };

            btnLogin.Click += async (sender, e) =>
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    lblStatus.Text = "Vui lòng nhập tài khoản và mật khẩu!";
                    return;
                }

                btnLogin.Enabled = false;
                lblStatus.ForeColor = Color.DarkBlue;
                lblStatus.Text = "Đang đăng nhập...";

                try
                {
                    var loginData = new
                    {
                        Username = username,
                        Password = password
                    };

                    using HttpResponseMessage response =
                        await _client.PostAsJsonAsync("auth/login", loginData);

                    string responseText =
                        await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Sai tài khoản, mật khẩu hoặc lỗi máy chủ.";
                        return;
                    }

                    using JsonDocument doc = JsonDocument.Parse(responseText);

                    if (!doc.RootElement.TryGetProperty("token", out JsonElement tokenElement))
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Phản hồi đăng nhập không có token.";
                        return;
                    }

                    SessionManager.JwtToken =
                        tokenElement.GetString() ?? string.Empty;

                    if (doc.RootElement.TryGetProperty("role", out JsonElement roleElement))
                    {
                        SessionManager.CurrentRole =
                            roleElement.GetString() ?? string.Empty;
                    }

                    SessionManager.CurrentUsername = username;

                    Hide();

                    using (FormMainShell mainForm = new FormMainShell())
                    {
                        mainForm.ShowDialog();
                    }

                    Close();
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Không kết nối được máy chủ.";

                    MessageBox.Show(
                        ex.Message,
                        "Lỗi đăng nhập",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    if (!IsDisposed && Visible)
                    {
                        btnLogin.Enabled = true;
                    }
                }
            };

            Controls.Add(leftPanel);
            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(lblStatus);
        }

        // Giữ các hàm sự kiện mà Designer cũ đang tham chiếu
        private void FormLogin_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }
    }
}