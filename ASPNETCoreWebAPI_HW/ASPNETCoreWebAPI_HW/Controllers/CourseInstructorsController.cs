using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCoreWebAPI_HW.Models;

namespace ASPNETCoreWebAPI_HW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseInstructorsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CourseInstructorsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/CourseInstructors
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CourseInstructor>>> GetCourseInstructors()
        {
            return await _context.CourseInstructor.ToListAsync();
        }

        // GET: api/CourseInstructors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseInstructor>> GetCourseInstructorById(int id)
        {
            var courseInstructor = await _context.CourseInstructor.FindAsync(id);

            if (courseInstructor == null)
            {
                return NotFound();
            }

            return courseInstructor;
        }

        // PUT: api/CourseInstructors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseInstructor(int id, CourseInstructor courseInstructor)
        {
            if (id != courseInstructor.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(courseInstructor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseInstructorExists(id))
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

        // POST: api/CourseInstructors
        [HttpPost("")]
        public async Task<ActionResult<CourseInstructor>> PostCourseInstructor(CourseInstructor courseInstructor)
        {
            _context.CourseInstructor.Add(courseInstructor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseInstructorExists(courseInstructor.CourseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourseInstructor", new { id = courseInstructor.CourseId }, courseInstructor);
        }

        // DELETE: api/CourseInstructors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseInstructor>> DeleteCourseInstructorById(int id)
        {
            var courseInstructor = await _context.CourseInstructor.FindAsync(id);
            if (courseInstructor == null)
            {
                return NotFound();
            }

            _context.CourseInstructor.Remove(courseInstructor);
            await _context.SaveChangesAsync();

            return courseInstructor;
        }

        private bool CourseInstructorExists(int id)
        {
            return _context.CourseInstructor.Any(e => e.CourseId == id);
        }
    }
}
