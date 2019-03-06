using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGC.Data;
using PGC.Models;
using PGC.Models.ViewModels;

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
        public IEnumerable<AspirantView> GetAspirants () {
		
		var list = _context.Aspirants.ToList ().Select (i => new AspirantView {
                    Id = i.Id,
                    Surename = i.Surename,
                    Name = i.Name,
                    Patronymic = i.Patronymic,
                    Birthday = i.Birthday,
                    Phone = i.Phone,
                    Email = i.Email,
                    InputYear = i.InputDate?.Year,
                    GraduationYear = i.GraduationDate?.Year,
                    ProtectionYear = i.ProtectionDate?.Year,
                    StudyForm = i.StudyForm.GetDisplayName(),
                    StatusType = i.StatusType.GetDisplayName(),
                    Budget = i.Budget ? "Б" : "к",
                    Doctorant =  i.Doctorant ? "Д" : "а",
                    Sex = i.Sex ? "ч" : "Ж",
                    Protection =  i.Protection ? "З" : "-",
                    Present = i.Present ? "Є" : "-",
                    Course = i.Course,
                    SpecialityId = i.SpecialityId,
                    Deparment = i.Department?.Acronym,
                    Faculty = i.Department?.Faculty?.Acronym,
                    Prepod = i.Prepod?.FIO
            }).ToList ();		
            return list;
        }

        // Редактировать запись
        // PUT: api/Aspirant/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> PutAspirant ([FromRoute] int id, [FromBody] Aspirant aspirant) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            if (id != aspirant.Id) {
                return BadRequest ();
            }

            _context.Entry (aspirant).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                if (!AspirantExists (id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        // Enums и Selected items для аспиранта
        [HttpGet ("lists/{id}")]
        public async Task<AspirantSelectLists> GetListsAsync ([FromRoute] int id) {

            Aspirant aspirant = await _context.Aspirants.FirstOrDefaultAsync (m => m.Id == id);
            AspirantSelectLists aspirantSelectLists = new AspirantSelectLists (aspirant, _context);
            return aspirantSelectLists;
        }


        // Добавить запись
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

        // Удалить запись
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

        // [HttpGet ("statuses")]
        // public IEnumerable<ItemData> GetStatuses () {            
        //     return Helper.GetStatusList();
        // }

        // [HttpGet ("studyforms")]
        // public IEnumerable<ItemData> GetStudyForms () {
        //     return Helper.GetStudyForms();
        // }

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

        private bool AspirantExists (int id) {
            return _context.Aspirants.Any (e => e.Id == id);
        }

        // public class AspirantView {
        //     public string Name { get; set; }

        //     public string Surename { get; set; }

        //     public int StatustypeId { get; set; }

        // }
    }
}