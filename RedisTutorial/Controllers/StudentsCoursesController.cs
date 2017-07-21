using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedisTutorial.Models;

namespace RedisTutorial.Controllers
{
    [Produces("application/json")]
    [Route("api/StudentsCourses")]
    public class StudentsCoursesController : Controller
    {
        private readonly UniversityContext _context;

        public StudentsCoursesController(UniversityContext context)
        {
            _context = context;
        }

        // GET: api/StudentsCourses
        [HttpGet]
        public IEnumerable<StudentsCourses> GetStudentsCourses()
        {
            return _context.StudentsCourses;
        }

        // GET: api/StudentsCourses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentsCourses([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentsCourses = await _context.StudentsCourses.SingleOrDefaultAsync(m => m.Id == id);

            if (studentsCourses == null)
            {
                return NotFound();
            }

            return Ok(studentsCourses);
        }

        // PUT: api/StudentsCourses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentsCourses([FromRoute] int id, [FromBody] StudentsCourses studentsCourses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentsCourses.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentsCourses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentsCoursesExists(id))
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

        // POST: api/StudentsCourses
        [HttpPost]
        public async Task<IActionResult> PostStudentsCourses([FromBody] StudentsCourses studentsCourses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudentsCourses.Add(studentsCourses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentsCourses", new { id = studentsCourses.Id }, studentsCourses);
        }

        // DELETE: api/StudentsCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentsCourses([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentsCourses = await _context.StudentsCourses.SingleOrDefaultAsync(m => m.Id == id);
            if (studentsCourses == null)
            {
                return NotFound();
            }

            _context.StudentsCourses.Remove(studentsCourses);
            await _context.SaveChangesAsync();

            return Ok(studentsCourses);
        }

        private bool StudentsCoursesExists(int id)
        {
            return _context.StudentsCourses.Any(e => e.Id == id);
        }
    }
}