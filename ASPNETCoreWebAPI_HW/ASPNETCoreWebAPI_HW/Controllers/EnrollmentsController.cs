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
    public class EnrollmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public EnrollmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Enrollments
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
        {
            return await _context.Enrollment.ToListAsync();
        }

        // GET: api/Enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollmentById(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }

        // PUT: api/Enrollments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollment(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return BadRequest();
            }

            _context.Entry(enrollment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(id))
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

        // POST: api/Enrollments
        [HttpPost("")]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
            _context.Enrollment.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollment", new { id = enrollment.EnrollmentId }, enrollment);
        }

        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Enrollment>> DeleteEnrollmentById(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();

            return enrollment;
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.EnrollmentId == id);
        }
    }
}
