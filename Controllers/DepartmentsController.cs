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
    [Route ("api/Departments")]
    public class DepartmentsController : Controller {
        private readonly AspiranturaContext _context;

        public DepartmentsController (AspiranturaContext context) {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public IEnumerable<DepartmentView> Get () {

            var kafedraList = _context.Departments.Include ("Faculty").Select (i => new DepartmentView {
                Acronym = i.Acronym,
                    Decan = i.Decan,
                    Email = i.Email,
                    Id = i.Id,
                    Name = i.Name,
                    Phone = i.Phone,
                    Phone2 = i.Phone2,
                    Faculty = i.Faculty,
                    Specialities = i.Specialities
            }).ToList ();

            // Формирую составное поле - из др. таблицы  - МАКС - КАК СДЕЛАТЬ ЧЕРЕЗ LINQ - ? 
            // foreach (var kaf in kafedraList) {
            //     foreach (var ds in _context.DepartmentSpecialities.OrderBy (c => c.SpecialityId)) {
            //         if (kaf.Id == ds.DepartmentId) {
            //             if (String.IsNullOrEmpty (kaf.SpecialitiesString))
            //                 kaf.SpecialitiesString = ds.SpecialityId.ToString ();
            //             else
            //                 kaf.SpecialitiesString = kaf.SpecialitiesString + ", " + ds.SpecialityId.ToString ();
            //         }
            //     }
            // }

            return kafedraList;
        }

        // GET: api/Departments/5
        [HttpGet ("{id}")]
        public async Task<IActionResult> Get ([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            var department = await _context.Departments.SingleOrDefaultAsync (m => m.Id == id);
            if (department == null) {
                return NotFound ();
            }
            return Ok (department);
        }

        // Добавить запись - Create
        // POST: api/Department
        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] Department department) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            _context.Departments.Add (department);
            await _context.SaveChangesAsync ();
            await AddAssignedSpecialities (department);
            return Ok (department.Id);
        }

        [HttpPost ("import")]
        // public async Task<IEnumerable<DepartmentView>> Import ([FromBody] IEnumerable<Department> departments) {
        public async Task<IActionResult> Import ([FromBody] IEnumerable<Department> departments) {
            if (!ModelState.IsValid) {
                return BadRequest ("Перевірте наявність даних в полях Name та Acronim в усіх строках, починаючи з другої.");
            }

            foreach (var item in departments) {
                if (!IsDepartmentUniq (item))
                    return BadRequest ("Підрозділ " + item.Acronym + " вже є в БД. Видаліть або виправте запис та повторіть імпорт");
            }

            _context.Departments.AddRange (departments);
            await _context.SaveChangesAsync ();

            return Ok ();
        }

        private bool IsDepartmentUniq (Department department) {
            // проверить уникальность по Акрониму
            return !_context.Departments.Any (p => p.Acronym == department.Acronym);
        }

        // Редактировать запись - Edit
        // PUT: api/Department/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> Put ([FromRoute] int id, [FromBody] Department department) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            if (id != department.Id) {
                return BadRequest ();
            }

            _context.Entry (department).State = EntityState.Modified;
            try {
                await EditAssignedSpecialities (department);
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                if (!DepartmentExists (id)) {
                    return NotFound ();
                } else {
                    throw;
                }
            }
            return NoContent ();
        }

        // новый список специальностей на кафедре
        private async Task AddAssignedSpecialities (Department department) {
            foreach (var speciality in department.Specialities) {
                DepartmentSpeciality departmentSpeciality = new DepartmentSpeciality { DepartmentId = department.Id, SpecialityId = speciality.Id };
                _context.DepartmentSpecialities.Add (departmentSpeciality);
            }
            await _context.SaveChangesAsync ();
        }

        // изменить список специальностей на кафедре
        private async Task EditAssignedSpecialities (Department department) {
            // ПЕРЕДЕЛАТЬ ТАК: 

            // пересечение двух множеств по Id
            // var specialities = _context.Specialities.Include ("Knowledge").Select (i => new { i.Id, i.Name, i.KnowledgeId, i.Knowledge, i.IsUsed }).ToList();
            // var selectedSpecId = _context.DepartmentSpecialities.Where (d => d.DepartmentId == departmentId).ToList();

            // var selSpec =
            //     from sp in specialities 
            //     where (selectedSpecId.Any (x => x.SpecialityId == sp.Id))
            //     select sp;

            // записи в _context.DepartmentSpecialities для выбранной кафедры
            var depSpecialities = _context.DepartmentSpecialities.Where (d => d.DepartmentId == department.Id);

            // удалить не выбранные записи            
            foreach (var item in depSpecialities) {
                // если выбрали хотя бы одну специальность
                if (department.Specialities != null) {
                    bool exist = department.Specialities.Any (s => s.Id == item.SpecialityId);
                    if (!exist) {
                        _context.DepartmentSpecialities.Remove (item);
                    }
                } else {
                    // если список специальностей пустой
                    _context.DepartmentSpecialities.Remove (item);
                }
            }

            // _context.SaveChanges ();

            // depSpecialities = _context.DepartmentSpecialities.Where(d => d.DepartmentId == department.Id);

            if (department.Specialities != null) {
                // добавить выбранные новые записи
                foreach (var speciality in department.Specialities) {
                    if (!depSpecialities.Any (ds => ds.SpecialityId == speciality.Id)) {
                        _context.DepartmentSpecialities.Add (new DepartmentSpeciality { DepartmentId = department.Id, SpecialityId = speciality.Id });
                    }
                }
            }
            await _context.SaveChangesAsync ();
        }

        // Удалить запись
        // DELETE: api/Department/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete ([FromRoute] int id) {

            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var item = await _context.Departments.SingleOrDefaultAsync (m => m.Id == id);
            if (item == null) {
                return NotFound ();
            }

            _context.Departments.Remove (item);
            await _context.SaveChangesAsync ();

            return Ok (item);
        }

        // =========================================================================================================== 

        // названия кафедр для выпадающего списка 
//        [HttpGet ("names")]
        // public IEnumerable<ItemData> GetNames () {
        //     IList<ItemData> list = new List<ItemData> ();
        //     var sp = _context.Departments.Include ("Faculty");
        //     foreach (var item in sp) {
        //         char ch = Convert.ToChar (160);
        //         // string space = new String (ch, 5 - item.Id.ToString ().Length);
        //         string spaceA = new String (ch, 8 - item.Acronym.ToString ().Length);
        //         string txt = item.Acronym + spaceA + item.Faculty?.Acronym;
        //         list.Add (new ItemData () { Value = item.Id, Text = txt });
        //     }
        //     return list;
        // }

        private bool DepartmentExists (int id) {
            return _context.Departments.Any (e => e.Id == id);
        }

        // public class DepartmentFromUI{
        // public int Id { get; set; }

        // public string Number { get; set; }

        // public string Context { get; set; }

        // public DateTime Date { get; set; }

        // public int DepartmenttypeId { get; set; }

        // public ICollection<Aspirant> Aspirants { get; set; }
        // }
    }
}