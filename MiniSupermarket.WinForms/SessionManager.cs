
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

namespace MiniSupermarket.WinForms
{
    public static class SessionManager
    {
        //========================================================================================38
        public static string JwtToken { get; set; } = string.Empty;
        //========================================================================================
        public static string CurrentUsername { get; set; } = string.Empty;

        public static string CurrentRole { get; set; } = string.Empty;
    }
}
