using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCoreWebAPI_HW.Models;
using System.Threading;

namespace ASPNETCoreWebAPI_HW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CoursesController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Course
                            .Where(c => c.IsDeleted == null || c.IsDeleted == false) // 排除已標記刪除資料
                            .ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null || course.IsDeleted != null && course.IsDeleted == true) // 排除已標記刪除資料
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                _context.Entry(course).CurrentValues.SetValues(new { DateModified = DateTime.Now });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        [HttpPost("")]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourseById(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            //_context.Course.Remove(course);

            #region 刪除改為標記刪除
            course.IsDeleted = true;
            _context.Course.Update(course);
            #endregion
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }

        // GET: api/GetCourseStudents
        [HttpGet("GetCourseStudents")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetCourseStudents()
        {
            return await _context.VwCourseStudents.ToListAsync();
        }

        // GET: api/GetCourseStudentCount
        [HttpGet("GetCourseStudentCount")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetCourseStudentCount()
        {
            return await _context.VwCourseStudentCount.ToListAsync();
        }
    }
}
