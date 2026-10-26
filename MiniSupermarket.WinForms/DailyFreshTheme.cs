using System;
using System.Drawing;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    /// <summary>
    /// Giao diện dùng chung Daily Fresh. Chỉ chuẩn hóa hiển thị, không thay đổi xử lý nghiệp vụ/API.
    /// </summary>
    internal static class DailyFreshTheme
    {
        internal static readonly Color Cream = Color.FromArgb(250, 248, 239);
        internal static readonly Color Sage = Color.FromArgb(105, 139, 103);
        internal static readonly Color SageDark = Color.FromArgb(72, 103, 73);
        internal static readonly Color SagePale = Color.FromArgb(232, 240, 226);
        internal static readonly Color Peach = Color.FromArgb(230, 145, 105);
        internal static readonly Color TextDark = Color.FromArgb(49, 68, 51);
        internal static readonly Color TextGray = Color.FromArgb(112, 126, 108);
        internal static readonly Color Border = Color.FromArgb(225, 232, 218);

        internal static void Apply(Control root)
        {
            if (root == null) return;
            ApplyToChildren(root);
        }

        private static void ApplyToChildren(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is DataGridView grid)
                {
                    grid.BackgroundColor = Color.White;
                    grid.BorderStyle = BorderStyle.None;
                    grid.GridColor = Border;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Sage;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
                    grid.DefaultCellStyle.BackColor = Color.White;
                    grid.DefaultCellStyle.ForeColor = TextDark;
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 225, 204);
                    grid.DefaultCellStyle.SelectionForeColor = TextDark;
                    grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                    grid.DefaultCellStyle.Padding = new Padding(4);
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 244);
                    grid.ColumnHeadersHeight = Math.Max(grid.ColumnHeadersHeight, 36);
                }
                else if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.UseVisualStyleBackColor = false;
                    button.FlatAppearance.BorderSize = 0;
                    button.Cursor = Cursors.Hand;
                    button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    string text = (button.Text ?? string.Empty).ToLowerInvariant();
                    Color color = text.Contains("xóa") || text.Contains("đăng xuất")
                        ? Color.FromArgb(190, 91, 78)
                        : text.Contains("thanh toán") || text.Contains("cập nhật")
                            ? Peach
                            : text.Contains("làm mới") || text.Contains("hủy")
                                ? Color.FromArgb(135, 148, 128)
                                : Sage;
                    button.BackColor = color;
                    button.ForeColor = Color.White;
                    button.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(color, 0.08F);
                    button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(color, 0.15F);
                }
                else if (control is TextBox textBox)
                {
                    textBox.Font = new Font("Segoe UI", 10F);
                    textBox.ForeColor = TextDark;
                    textBox.BackColor = Color.FromArgb(252, 252, 248);
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (control is ComboBox combo)
                {
                    combo.Font = new Font("Segoe UI", 10F);
                    combo.ForeColor = TextDark;
                    combo.BackColor = Color.White;
                    combo.FlatStyle = FlatStyle.Flat;
                }
                else if (control is Label label)
                {
                    label.Font = new Font("Segoe UI", Math.Max(9F, label.Font.Size), label.Font.Style);
                    if (label.ForeColor == Color.White && IsLight(control.Parent?.BackColor ?? Color.White))
                        label.ForeColor = TextDark;
                }

                ApplyToChildren(control);
            }
        }

        private static bool IsLight(Color color) =>
            (color.R * 299 + color.G * 587 + color.B * 114) / 1000 > 185;
    }
}
