﻿using System;
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
    [Route ("api/Specialities")]
    public class SpecialitiesController : Controller {
        private readonly AspiranturaContext _context;

        public SpecialitiesController (AspiranturaContext context) {
            _context = context;
        }

        [HttpGet]
        public dynamic GetSpecialities () {
            var sp = _context.Specialities.Include ("Knowledge").Select (i => new { i.Id, i.Name, i.KnowledgeId, i.Knowledge, i.IsUsed });
            return sp;
        }


        // специальности для кафедры
        [HttpGet ("selected/{departmentId}")]
        public dynamic GetSelectedSpecialities ([FromRoute] int departmentId) {

            // пересечение двух множеств по Id
            var specialities = _context.Specialities.Include ("Knowledge").Select (i => new { i.Id, i.Name, i.KnowledgeId, i.Knowledge, i.IsUsed }).ToList ();
            var selectedSpecId = _context.DepartmentSpecialities.Where (d => d.DepartmentId == departmentId).ToList ();

            var selSpec =
                from sp in specialities
            where (selectedSpecId.Any (x => x.SpecialityId == sp.Id))
            select sp;

            return selSpec;
        }

        // GET: api/Specialities/5
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

        // PUT: api/Specialities/5
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

        // POST: api/Specialities
        [HttpPost]
        public async Task<IActionResult> PostSpeciality ([FromBody] Speciality speciality) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            _context.Specialities.Add (speciality);
            await _context.SaveChangesAsync ();

            return CreatedAtAction ("GetSpeciality", new { id = speciality.Id }, speciality);
        }

        // DELETE: api/Specialities/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteSpeciality ([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var speciality = await _context.Specialities.SingleOrDefaultAsync (m => m.Id == id);
            if (speciality == null) {
                return NotFound ();
            }

            _context.Specialities.Remove (speciality);
            await _context.SaveChangesAsync ();

            return Ok (speciality);
        }

        private bool SpecialityExists (int id) {
            return _context.Specialities.Any (e => e.Id == id);
        }
    }
}