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
    [Route ("api/Prepods")]
    public class PrepodsController : Controller {
        private readonly AspiranturaContext _context;

        public PrepodsController (AspiranturaContext context) {
            _context = context;
        }

        // список преподавателей
        // GET: api/Prepods
        [HttpGet]
        public IEnumerable<PrepodView> Get () {
            // ToList() до Select() - чтобы не было исключений чтения из БД - ожидает Enum
            var list = _context.Prepods.ToList ().Select (i => new PrepodView {
                Email = i.Email,
                    Id = i.Id,
                    Name = i.Name,
                    Phone = i.Phone,
                    Birthday = i.Birthday,
                    Surename = i.Surename,
                    Degree = i.Degree.GetDisplayName (),
                    Position = i.Position.GetDisplayName (),
                    Rank = i.Rank.GetDisplayName (),
                    Patronymic = i.Patronymic,
                    Present = i.Present ? "Є" : "-",
                    DeparmentsString = i.DepartmentsString
            }).ToList ();
            return list;
        }

        // один преподаватель
        // GET: api/Prepods/5
        [HttpGet ("{id}")]
        public async Task<IActionResult> Get ([FromRoute] int id) {
            var prepod = await _context.Prepods.SingleOrDefaultAsync (m => m.Id == id);
            if (prepod == null) {
                return NotFound ();
            }
            return Ok (prepod);
        }

        // Добавить запись - Create
        // POST: api/Prepods
        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] Prepod prepod) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }
            prepod.DepartmentsString = GetDepartmentsString (prepod);

            _context.Prepods.Add (prepod);
            await _context.SaveChangesAsync ();
            await EditAssignedDepartments (prepod);
            return Ok (prepod.Id);
        }

        // Редактировать запись - Edit
        // PUT: api/Prepods/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> Put ([FromRoute] int id, [FromBody] Prepod prepod) {
            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            prepod.DepartmentsString = GetDepartmentsString (prepod);

            // // заполнение поля акронимами кафедр
            // prepod.DepartmentsString = String.Empty;
            // foreach (var item in prepod.Departments.OrderBy (o => o.Acronym)) {
            //     var dep = _context.Departments.FirstOrDefault (i => i.Id == item.Id);
            //     if (String.IsNullOrEmpty (prepod.DepartmentsString))
            //         prepod.DepartmentsString = dep?.Acronym;
            //     else
            //         prepod.DepartmentsString = prepod.DepartmentsString + ", " + dep?.Acronym;
            // }

            _context.Entry (prepod).State = EntityState.Modified;
            try {
                await EditAssignedDepartments (prepod);
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                // if (!DepartmentExists (id)) {
                //     return NotFound ();
                // } else {
                //     throw;
                // }
            }
            return NoContent ();
        }

        // Удалить запись - Delete
        // DELETE: api/Prepods/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete ([FromRoute] int id) {

            if (!ModelState.IsValid) {
                return BadRequest (ModelState);
            }

            var item = await _context.Prepods.SingleOrDefaultAsync (m => m.Id == id);
            if (item == null) {
                return NotFound ();
            }

            _context.Prepods.Remove (item);
            await _context.SaveChangesAsync ();

            return Ok (item);
        }

        // -------------- вспомагательные Get ----------------

        // FOR DELETE
        // перечень всех кафедр для Select (PrepodDepartmentList.vue)
        [HttpGet ("departments")]
        public dynamic GetSelectNames () {
            var kafedraList = _context.Departments.Include ("Faculty").Select (i => new { i.Id, i.Acronym, i.Faculty });
            return kafedraList;
        }

        // FOR DELETE
        // перечень выбранных кафедр для препода (PrepodDepartmentSelectedList.vue)
        [HttpGet ("workplaces/{prepodId}")]
        public dynamic GetSelectedDepartments ([FromRoute] int prepodId) {

            // пересечение двух множеств по Id
            var departments = _context.Departments.Include ("Faculty").Select (i => new { i.Id, i.Acronym, i.Faculty });

            var selectedDepId = _context.PrepodsDepartments.Where (d => d.PrepodId == prepodId);

            var selDep =
                from dp in departments
            where (selectedDepId.Any (x => x.DepartmentId == dp.Id))
            select dp;
            return selDep;
        }

        // Enums и Selected items для препода
        [HttpGet ("lists/{id}")]
        public async Task<PrepodSelectLists> GetListsAsync ([FromRoute] int id) {

            Prepod prepod = await _context.Prepods.FirstOrDefaultAsync (m => m.Id == id);
            PrepodSelectLists prepodSelectLists = new PrepodSelectLists (prepod);
            return prepodSelectLists;
        }

        // Импорт из файла Excel
        [HttpPost ("import")]
        public async Task<IActionResult> Import ([FromBody] IEnumerable<Prepod> prepods) {
            if (!ModelState.IsValid) {
                return BadRequest ("Перевірте наявність даних в полях Surename та Name в усіх строках, починаючи з другої.");
            }

            foreach (var item in prepods) {
                if (!IsPrepodUniqByName (item))
                    return BadRequest ("Викладач " + item.FIO + " вже є в БД. Видаліть або виправте запис та повторіть імпорт");
            }

            _context.Prepods.AddRange (prepods);
            await _context.SaveChangesAsync ();

            return Ok ();
        }

        // Импорт из файла Excel (з формуляра відділу кадрів)
        [HttpPost ("formularImport")]
        public async Task<IActionResult> FormularImport ([FromBody] IEnumerable<Prepod> importPrepods) {
            if (!ModelState.IsValid) {
                return BadRequest ("Перевірте наявність даних в полях Surename та Name в усіх строках, починаючи з другої.");
            }

            List<Prepod> dbPrepods = _context.Prepods.ToList ();

            foreach (var dbPrepod in dbPrepods) {
                // сначала делаем всех неактивніми, а затем по IDImport - активными и вносим изменения
                dbPrepod.Present = false;
                dbPrepod.Departments = new List<Department> ();
            }

            foreach (var importPrepod in importPrepods) {

                // Распарсить полный путь до акронима
                Department department = _context.Departments.FirstOrDefault (d => d.Acronym == importPrepod.DepartmentsString);
                if (department == null) {
                    return BadRequest ("Перевірте Кафедру для " + importPrepod.Surename);
                }

                Prepod dbPrepod = dbPrepods.FirstOrDefault (p => p.ImportId == importPrepod.ImportId);

                if (dbPrepod == null) {
                    dbPrepod = CopyPrepod (new Prepod () { Departments = new List<Department> () }, importPrepod, department);
                    // dbPrepod.Departments = new List<Department> ();
                    dbPrepods.Add (dbPrepod);
                    _context.Prepods.Add (dbPrepod);
                    await _context.SaveChangesAsync ();
                } else {
                    dbPrepod = CopyPrepod (dbPrepod, importPrepod, department);
                }
            }

            foreach (var dbPrepod in dbPrepods) {
                await EditAssignedDepartments (dbPrepod);
            }

            await _context.SaveChangesAsync ();

            return Ok ();
        }

        private Prepod CopyPrepod (Prepod dbPrepod, Prepod importPrepod, Department department) {

            dbPrepod.Departments.Add (department);

            // повторно встречается - совместитель
            if (dbPrepod.Present) {
                dbPrepod.DepartmentsString = dbPrepod.DepartmentsString + ", " + importPrepod.DepartmentsString;
            } else {
                dbPrepod.Present = true;

                dbPrepod.ImportId = importPrepod.ImportId;
                dbPrepod.Degree = importPrepod.Degree;
                dbPrepod.DepartmentsString = importPrepod.DepartmentsString;
                dbPrepod.Name = importPrepod.Name;
                dbPrepod.Patronymic = importPrepod.Patronymic;
                dbPrepod.Position = importPrepod.Position;
                dbPrepod.Rank = importPrepod.Rank;
                dbPrepod.Surename = importPrepod.Surename;

                // ДОПОЛНЕНИИ ИНФОРМАЦИИ ОБ импортируемых ПРЕПОДАВАТЕЛЯХ СОБСТВЕННЫМИ ДАННЫМИ - не перезаписывать (тлф, , почта, ...)
                // dbPrepod.Birthday 
                // dbPrepod.Email;
                // dbPrepod.Phone;
            }

            return dbPrepod;
        }

        private bool IsPrepodUniq (Prepod prepod, List<Prepod> dbPrepods) {
            return !dbPrepods.Any (p => p.ImportId == prepod.ImportId);
        }

        private bool IsPrepodUniqByName (Prepod prepod) {
            bool uniq = false;
            if (String.IsNullOrEmpty (prepod.Patronymic))
                uniq = !_context.Prepods.Any (p => p.Surename + p.Name == prepod.Surename + prepod.Name);
            else
                uniq = !_context.Prepods.Any (p => p.Surename + p.Name + p.Patronymic == prepod.Surename + prepod.Name + prepod.Patronymic);

            return uniq;
        }

        // изменить список кафедр (мест работы)
        private async Task EditAssignedDepartments (Prepod prepod) {

            // пересечение двух множеств по Id
            var workPlaces = prepod.Departments.ToList ();
            var prepodDepartments = _context.PrepodsDepartments.Where (i => i.PrepodId == prepod.Id).ToList ();

            // выбрать в PrepodDepartments записи, которые отсутствуют в workPlaces (prepod.Departments) - для удаления
            var forDelete =
                (from d in prepodDepartments where (!workPlaces.Any (x => x.Id == d.DepartmentId)) select d).ToList ();

            // выбрать в workPlaces записи, которые отсутствуют в PrepodDepartments - для добавления
            var forAdd =
                (from d in workPlaces where (!prepodDepartments.Any (x => x.DepartmentId == d.Id)) select (new PrepodDepartment { PrepodId = prepod.Id, DepartmentId = d.Id })).ToList ();

            _context.PrepodsDepartments.RemoveRange (forDelete);
            _context.PrepodsDepartments.AddRange (forAdd);

            await _context.SaveChangesAsync ();
        }

        // // новый список кафедр (мест работы) - при создании нового  препода
        // private async Task AddAssignedDepartments (Prepod prepod) {
        //     foreach (var speciality in prepod.Departments) {
        //         DepartmentSpeciality departmentSpeciality = new DepartmentSpeciality { DepartmentId = department.Id, SpecialityId = speciality.Id };
        //         _context.DepartmentSpecialities.Add (departmentSpeciality);
        //     }
        //     await _context.SaveChangesAsync ();
        // }

        // ------------------

        // =========================================================================================================== 

        // названия кафедр для выпадающего списка 
        [HttpGet ("names")] public IEnumerable<ItemData> GetNames () {
            IList<ItemData> list = new List<ItemData> ();
            var sp = _context.Departments.Include ("Faculty");
            foreach (var item in sp) {
                char ch = Convert.ToChar (160);
                // string space = new String (ch, 5 - item.Id.ToString ().Length);
                string spaceA = new String (ch, 8 - item.Acronym.ToString ().Length);
                string txt = item.Acronym + spaceA + item.Faculty.Acronym;
                list.Add (new ItemData () { Value = item.Id, Text = txt });
            }
            return list;
        }

        // заполнение поля акронимами кафедр
        private string GetDepartmentsString (Prepod prepod) {
            string departmentsString = String.Empty;
            foreach (var item in prepod.Departments.OrderBy (o => o.Acronym)) {
                var dep = _context.Departments.FirstOrDefault (i => i.Id == item.Id);
                if (String.IsNullOrEmpty (departmentsString))
                    departmentsString = dep?.Acronym;
                else
                    departmentsString = departmentsString + ", " + dep?.Acronym;
            }

            return departmentsString;
        }

        private bool DepartmentExists (int id) {
            return _context.Departments.Any (e => e.Id == id);
        }
    }
}