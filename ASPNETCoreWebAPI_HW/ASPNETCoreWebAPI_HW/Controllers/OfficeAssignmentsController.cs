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
    public class OfficeAssignmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public OfficeAssignmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/OfficeAssignments
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<OfficeAssignment>>> GetOfficeAssignments()
        {
            return await _context.OfficeAssignment.ToListAsync();
        }

        // GET: api/OfficeAssignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeAssignment>> GetOfficeAssignmentById(int id)
        {
            var officeAssignment = await _context.OfficeAssignment.FindAsync(id);

            if (officeAssignment == null)
            {
                return NotFound();
            }

            return officeAssignment;
        }

        // PUT: api/OfficeAssignments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfficeAssignment(int id, OfficeAssignment officeAssignment)
        {
            if (id != officeAssignment.InstructorId)
            {
                return BadRequest();
            }

            _context.Entry(officeAssignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficeAssignmentExists(id))
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

        // POST: api/OfficeAssignments
        [HttpPost("")]
        public async Task<ActionResult<OfficeAssignment>> PostOfficeAssignment(OfficeAssignment officeAssignment)
        {
            _context.OfficeAssignment.Add(officeAssignment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OfficeAssignmentExists(officeAssignment.InstructorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOfficeAssignment", new { id = officeAssignment.InstructorId }, officeAssignment);
        }

        // DELETE: api/OfficeAssignments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OfficeAssignment>> DeleteOfficeAssignmentById(int id)
        {
            var officeAssignment = await _context.OfficeAssignment.FindAsync(id);
            if (officeAssignment == null)
            {
                return NotFound();
            }

            _context.OfficeAssignment.Remove(officeAssignment);
            await _context.SaveChangesAsync();

            return officeAssignment;
        }

        private bool OfficeAssignmentExists(int id)
        {
            return _context.OfficeAssignment.Any(e => e.InstructorId == id);
        }
    }
}
