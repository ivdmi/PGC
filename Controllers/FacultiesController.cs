using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGC.Data;
using PGC.Models;
using PGC.Models.ViewModels;

namespace PGC.Controllers {
    [Produces ("application/json")]
    [Route ("api/Faculties")]
    public class FacultiesController : Controller {
        private readonly AspiranturaContext _context;

        public FacultiesController (AspiranturaContext context) {
            _context = context;
        }

        // GET: api/Faculties
        [HttpGet]
        public IEnumerable<Faculty> Get() {
            return _context.Faculties.Where(f => f.Id != 0);
        }

        // названия факультетов для выпадающего списка
        [HttpGet ("names")]
        public IEnumerable<ItemData> GetNames () {
            // var names = _context.Faculties.Where(f => f.Id != 0).Select (i => new ItemData{             
            var names = _context.Faculties.Select (i => new ItemData{     
                            Value = i.Id,
                            Text = i.Acronym
             }).ToList(); 
        return names;
        }

        // Добавить запись - Create
        // POST: api/Faculties
        [HttpPost]        
        public async Task<IActionResult> Post([FromBody] Faculty faculty) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            _context.Faculties.Add (faculty);            
            await _context.SaveChangesAsync ();
            return Ok (faculty.Id);
        }
        

        // Удалить запись
        // DELETE: api/Faculties/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete ([FromRoute] int id) {
            
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            var item = await _context.Faculties.SingleOrDefaultAsync (m => m.Id == id);
            if (item == null) {
                return NotFound ();
            }
            _context.Faculties.Remove (item);
            try{
                await _context.SaveChangesAsync ();
                return Ok (item);            
            }
            catch (Exception ex)
            {
                var message = ex.InnerException.Message;
                return BadRequest (message);
            }            
        }

        // Редактировать запись
        // PUT: api/Faculties/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Faculty item) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            if (id != item.Id) {
                return BadRequest ();
            }

            _context.Entry (item).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                if (!FaculityExists (id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        // =========================================================================================================== 
      

        private bool FaculityExists (int id) {
            return _context.Faculties.Any (e => e.Id == id);
        }        

    }
}