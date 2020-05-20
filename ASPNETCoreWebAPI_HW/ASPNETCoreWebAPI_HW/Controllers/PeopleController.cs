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
    public class PeopleController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public PeopleController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Person
                                    .Where(p => p.IsDeleted == null || p.IsDeleted == false) // 排除已標記刪除資料
                                    .ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPersonById(int id)
        {
            var person = await _context.Person.FindAsync(id);

            if (person == null || person.IsDeleted != null && person.IsDeleted == true) // 排除已標記刪除資料
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                _context.Entry(person).CurrentValues.SetValues(new { DateModified = DateTime.Now });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People
        [HttpPost("")]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePersonById(int id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            //_context.Person.Remove(person);

            #region 刪除改為標記刪除
            person.IsDeleted = true;
            _context.Person.Update(person);
            #endregion
            await _context.SaveChangesAsync();

            return person;
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
