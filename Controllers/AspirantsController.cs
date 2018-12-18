using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGC.Data;
using PGC.Models;

namespace PGC.Controllers {
    [Produces ("application/json")]
    [Route ("api/Aspirants")]
    public class AspirantsController : Controller {
        private readonly AspiranturaContext _context;

        public AspirantsController (AspiranturaContext context) {
            _context = context;
        }

        // GET: api/Aspirants
        [HttpGet]
        public IEnumerable<Aspirant> GetAspirants () {
            return _context.Aspirants;
        }

        [HttpGet ("statuses")]
        public IEnumerable<ItemData> GetStatuses () {            
            return Helper.GetStatusList();
        }

        // public class StatusItem {
        //     public int Value { get; set; }
        //     public string Text { get; set; }
        // }

        // POST: api/Aspirants
        [HttpPost]
        public async Task<IActionResult> PostAspirant ([FromBody] Aspirant aspirant) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }            
            _context.Aspirants.Add (aspirant);            
            await _context.SaveChangesAsync ();

            int id = aspirant.Id;
            return Ok (id);
        }

        // DELETE: api/Aspirants/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteAspirant ([FromRoute] int id) {
            var kk = Request.Body;
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var aspirant = await _context.Aspirants.SingleOrDefaultAsync (m => m.Id == id);
            if (aspirant == null) {
                return NotFound ();
            }

            _context.Aspirants.Remove (aspirant);
            await _context.SaveChangesAsync ();

            return Ok (aspirant);
        }

        // ===========================================================================================================

        // GET: api/SpecialitiesApi/5
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetSpeciality ([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var speciality = await _context.Specialities.SingleOrDefaultAsync (m => m.Id == id);

            if (speciality == null) {
                return NotFound ();
            }

            return Ok (speciality);
        }

        // PUT: api/SpecialitiesApi/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> PutSpeciality ([FromRoute] int id, [FromBody] Speciality speciality) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            if (id != speciality.Id) {
                return BadRequest ();
            }

            _context.Entry (speciality).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                if (!SpecialityExists (id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        private bool SpecialityExists (int id) {
            return _context.Specialities.Any (e => e.Id == id);
        }

        public class AspirantView {
            public string Name { get; set; }

            public string Surename { get; set; }

            public int StatustypeId { get; set; }

        }
    }
}