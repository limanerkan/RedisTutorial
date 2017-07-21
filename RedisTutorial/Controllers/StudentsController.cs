using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RedisTutorial.Models;
using Newtonsoft.Json;

namespace RedisTutorial.Controllers
{
    [Produces("application/json")]
    [Route("api/Students")]
    public class StudentsController : Controller
    {
        private readonly UniversityContext _context;
        private readonly IDistributedCache _cache;

        public StudentsController(UniversityContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _cache = distributedCache;
        }

        // GET: api/Students
        [HttpGet]
        public IEnumerable<Students> GetStudents()
        {
            return _context.Students;
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetStudentsAsync([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_cache.GetStringAsync(Convert.ToString(id)) == null)
            {
                var students = _context.Students.Where(s => s.Id == id).Select(p => new
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Course = _context.StudentsCourses.Where(cours => cours.StudentId == p.Id).Select(c => new
                    {
                        CouseNam = _context.Courses.Where(cs => cs.Id == c.Id).Select(a => new
                        {
                            Name = a.CourseName,
                        })
                    }).ToList(),


                }).ToList();

                var cache = JsonConvert.SerializeObject(students);
                _cache.SetStringAsync(id.ToString(), cache);
                return Ok(students);
            }

            else
            {
                var cache = await _cache.GetStringAsync(Convert.ToString(id));
                var b = JsonConvert.DeserializeObject(cache);
                return Ok(b);
            }



        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudents([FromRoute] int id, [FromBody] Students students)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != students.Id)
            {
                return BadRequest();
            }

            _context.Entry(students).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentsExists(id))
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

        // POST: api/Students
        [HttpPost]
        public async Task<IActionResult> PostStudents([FromBody] Students students)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Students.Add(students);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudents", new { id = students.Id }, students);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudents([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var students = await _context.Students.SingleOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            _context.Students.Remove(students);
            await _context.SaveChangesAsync();

            return Ok(students);
        }

        private bool StudentsExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}