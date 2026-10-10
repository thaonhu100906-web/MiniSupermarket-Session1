
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TranThiThaoNhu_2124110075.API.Data;
using TranThiThaoNhu_2124110075.API.Models;

namespace TranThiThaoNhu_2124110075.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly SupermarketDbContext _context;
        private readonly PasswordHasher<Employee> _passwordHasher = new();

        public EmployeesController(SupermarketDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.Username,
                    e.FullName,
                    e.Role
                })
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employees/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound(new { message = "Không tìm thấy nhân viên." });

            return Ok(new
            {
                employee.EmployeeId,
                employee.Username,
                employee.FullName,
                employee.Role
            });
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new
                {
                    message = "Vui lòng nhập tài khoản, mật khẩu và họ tên."
                });
            }

            if (!IsValidRole(request.Role))
                return BadRequest(new { message = "Vai trò không hợp lệ." });

            bool usernameExists = await _context.Employees
                .AnyAsync(e => e.Username == request.Username);

            if (usernameExists)
                return BadRequest(new { message = "Tên tài khoản đã tồn tại." });

            var employee = new Employee
            {
                Username = request.Username.Trim(),
                FullName = request.FullName.Trim(),
                Role = request.Role,
                PasswordHash = string.Empty
            };

            employee.PasswordHash = _passwordHasher.HashPassword(
                employee, request.Password);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEmployee),
                new { id = employee.EmployeeId },
                new
                {
                    employee.EmployeeId,
                    employee.Username,
                    employee.FullName,
                    employee.Role
                });
        }

        // PUT: api/Employees/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(
            int id, UpdateEmployeeRequest request)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound(new { message = "Không tìm thấy nhân viên." });

            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new
                {
                    message = "Tài khoản và họ tên không được để trống."
                });
            }

            if (!IsValidRole(request.Role))
                return BadRequest(new { message = "Vai trò không hợp lệ." });

            bool usernameExists = await _context.Employees.AnyAsync(
                e => e.Username == request.Username && e.EmployeeId != id);

            if (usernameExists)
                return BadRequest(new { message = "Tên tài khoản đã tồn tại." });

            employee.Username = request.Username.Trim();
            employee.FullName = request.FullName.Trim();
            employee.Role = request.Role;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật nhân viên thành công.",
                employee.EmployeeId,
                employee.Username,
                employee.FullName,
                employee.Role
            });
        }

        // DELETE: api/Employees/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound(new { message = "Không tìm thấy nhân viên." });

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa nhân viên thành công." });
        }

        private static bool IsValidRole(string role)
        {
            return role == "Admin" ||
                   role == "Warehouse" ||
                   role == "Cashier";
        }
    }

    public class CreateEmployeeRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Cashier";
    }

    public class UpdateEmployeeRequest
    {
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Cashier";
    }
}