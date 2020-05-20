using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCoreWebAPI_HW.Models;
using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ASPNETCoreWebAPI_HW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public DepartmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Department
                                    .Where(d => d.IsDeleted == null || d.IsDeleted == false) // 排除已標記刪除資料
                                    .ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null || department.IsDeleted != null && department.IsDeleted == true) // 排除已標記刪除資料
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            byte[] rowVersion = await _context.Department
                                    .Where(d => d.DepartmentId == id)
                                    .Select(c => c.RowVersion)
                                    .FirstOrDefaultAsync();
            department.RowVersion = rowVersion;
            department.DateModified = DateTime.Now;
            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                #region 預存程序
                //SqlParameter departmentID = new SqlParameter("@DepartmentID", department.DepartmentId);
                //SqlParameter name = new SqlParameter("@Name", department.Name);
                //SqlParameter budget = new SqlParameter("@Budget", department.Budget);
                //SqlParameter startDate = new SqlParameter("@StartDate", department.StartDate);
                //SqlParameter instructorID = new SqlParameter("@InstructorID", department.InstructorId);
                //SqlParameter rowVersion_Original = new SqlParameter("@RowVersion_Original", rowVersion);
                //await _context.Database.ExecuteSqlRawAsync("EXEC Department_Update @DepartmentID, @Name, @Budget, @StartDate, @InstructorID, @RowVersion_Original", departmentID, name, budget, startDate, instructorID, rowVersion_Original);
                #endregion
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Departments
        [HttpPost]
        public ActionResult<Department> PostDepartment(Department department)
        {
            //_context.Department.Add(department);
            //await _context.SaveChangesAsync();

            #region 預存程序
            SqlParameter name = new SqlParameter("@Name", department.Name);
            SqlParameter budget = new SqlParameter("@Budget", department.Budget);
            SqlParameter startDate = new SqlParameter("@StartDate", department.StartDate);
            SqlParameter instructorID = new SqlParameter("@InstructorID", department.InstructorId);
            department.DepartmentId = _context.Department
                                        .FromSqlRaw("EXEC dbo.Department_Insert @Name, @Budget, @StartDate, @InstructorID", name, budget, startDate, instructorID)
                                        .Select(d => d.DepartmentId)
                                        .ToList()
                                        .First();
            #endregion

            return CreatedAtAction("GetDepartments", new { id = department.DepartmentId }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Department>> DeleteDepartmentById(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            //_context.Department.Remove(department);

            #region 刪除改為標記刪除
            department.IsDeleted = true;
            _context.Department.Update(department);
            #endregion
            await _context.SaveChangesAsync();

            #region 預存程序
            //SqlParameter departmentID = new SqlParameter("@DepartmentID", department.DepartmentId);
            //SqlParameter rowVersion_Original = new SqlParameter("@RowVersion_Original", department.RowVersion);
            //await _context.Database.ExecuteSqlRawAsync("EXEC dbo.Department_Delete @DepartmentID, @RowVersion_Original", departmentID, rowVersion_Original);
            #endregion

            return department;
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DepartmentId == id);
        }

        // GET: api/GetDepartmentCourseCount
        [HttpGet("GetDepartmentCourseCount")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetDepartmentCourseCount()
        {
            return await _context.VwDepartmentCourseCount.FromSqlRaw("SELECT * FROM dbo.VwDepartmentCourseCount").ToListAsync();
        }
    }
}
