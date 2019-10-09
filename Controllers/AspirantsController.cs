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
            var l = _context.Aspirants.ToList ();

            var list = _context.Aspirants.
            Include ("Prepod").Include ("Speciality").Include ("Department").Include ("Department.Faculty").ToList ()
                .Select (i => new AspirantView {
                    Id = i.Id,
                        Surename = i.Surename,
                        Name = i.Name,
                        Patronymic = i.Patronymic,
                        FIO = i.FIO,

                        Phone = i.Phone,
                        Email = i.Email,

                        BirthYear = i.Birthday?.Year,
                        InputYear = i.InputDate?.Year,
                        GraduationYear = i.GraduationDate?.Year,
                        ProtectionYear = i.ProtectionDate?.Year,
                        StudyForm = i.StudyForm.GetDisplayName (),
                        StatusType = i.StatusType.GetDisplayName (),
                        Budget = i.Budget ? "б" : "-",
                        Doctorant = i.Doctorant ? "-" : "а",
                        Sex = i.Sex ? "ч" : "ж",
                        Vacation = i.Vacation ? "+" : "-",
                        Protection = i.Protection ? "+" : "-",
                        Present = i.Present ? "+" : "-",
                        P1 = i.P1 ? "+" : "-",
                        P2 = i.P2 ? "+" : "-",
                        P3 = i.P3 ? "+" : "-",
                        P4 = i.P4 ? "+" : "-",
                        P5 = i.P5 ? "+" : "-",
                        P6 = i.P6 ? "+" : "-",
                        P7 = i.P7 ? "+" : "-",
                        Course = i.Course,
                        SpecialityId = i.SpecialityId,
                        Department = i.Department?.Acronym,
                        Faculty = i.Department?.Faculty?.Acronym,
                        Prepod = i.Prepod?.FIO
                }).ToList ();
            return list;
        }

        // один 
        // GET: api/Aspirants/5
        [HttpGet ("{id}")]
        public async Task<IActionResult> Get ([FromRoute] int id) {
            var aspirant = await _context.Aspirants.SingleOrDefaultAsync (m => m.Id == id);
            if (aspirant == null) {
                return NotFound ();
            }
            return Ok (aspirant);
        }

        // [HttpGet ("qq/{id}")]
        // public string Getq ([FromRoute] int id) {

        //     return "QQQQQQQQQQQQQQQQQQQQQQQ";
        // }

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

        [HttpGet ("sforms")]
        public IEnumerable<ItemData> GetStudyForms () {
            return Helper.GetStudyForms ();
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

        // Импорт из файла Excel
        [HttpPost ("import")]
        public async Task<IActionResult> Import ([FromBody] IEnumerable<AspirantImport> aspirants) {
            string badMessage = "<b>До бази даних вже занесені:</b> <br>";
            bool exist = false;

            if (!ModelState.IsValid) {
                return BadRequest ("Перевірте наявність даних в полях Прізвище та Ім'я у строках, починаючи з № 3");
            }

            foreach (AspirantImport aspirantImport in aspirants) {
                Aspirant aspirant = InitAspirantFromAspirantImport (aspirantImport);
                if (aspirant == null) {
                    badMessage += aspirantImport.Surename + " " + aspirantImport.Name + " " + aspirantImport.Patronymic + " " + DateTime.FromOADate (aspirantImport.Birthday).ToShortDateString () + " народж.  <br> \n";
                    exist = true;
                } else {
                    _context.Aspirants.Add (aspirant);
                }
            }
            await _context.SaveChangesAsync ();
            if (exist) {
                return BadRequest (badMessage);
            } else {
                return Ok ();
            }
        }

        // private bool IsAspirantUniq (AspirantImport aspirant) {
        //     bool uniq = false;
        //     if (String.IsNullOrEmpty (aspirant.Patronymic))
        //         uniq = !_context.Prepods.Any (p => p.Surename + p.Name == aspirant.Surename + aspirant.Name);
        //     else
        //         uniq = !_context.Prepods.Any (p => p.Surename + p.Name + p.Patronymic == aspirant.Surename + aspirant.Name + aspirant.Patronymic);

        //     return uniq;
        // }

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
        // [HttpGet ("{id}")]
        // public async Task<IActionResult> GetSpeciality ([FromRoute] int id) {
        //     if (!ModelState.IsValid) {
        //         return BadRequest (ModelState);
        //     }

        //     var speciality = await _context.Specialities.SingleOrDefaultAsync (m => m.Id == id);

        //     if (speciality == null) {
        //         return NotFound ();
        //     }

        //     return Ok (speciality);
        // }

        private bool AspirantExists (int id) {
            return _context.Aspirants.Any (e => e.Id == id);
        }

        private Aspirant InitAspirantFromAspirantImport (AspirantImport aspirantImport) {
            DateTime birthday = DateTime.FromOADate (aspirantImport.Birthday);

            Aspirant aspirant = _context.Aspirants.FirstOrDefault (a => (a.Name == aspirantImport.Name &&
                a.Surename == aspirantImport.Surename &&
                a.Patronymic == aspirantImport.Patronymic &&
                a.Birthday.Value.Date == birthday.Date));

            // Возвращаем null, если аспирант уже есть в БД
            if (aspirant != null) {
                return null;
            }

            aspirant = new Aspirant ();
            aspirant.Name = aspirantImport.Name;
            aspirant.Surename = aspirantImport.Surename;
            aspirant.Patronymic = aspirantImport.Patronymic;
            aspirant.Birthday = birthday;

            aspirant.Phone = aspirantImport.Phone;
            aspirant.Email = aspirantImport.Email;

            aspirant.Doctorant = false;
            aspirant.Present = true;
            aspirant.StatusType = StatusType.новий;
            aspirant.Sex = aspirantImport.Sex.Contains ("чоловік") ? true : false;
            aspirant.Budget = aspirantImport.Budget.Contains ("держзамовлення") ? true : false;

            // Enum из строки
            aspirant.StudyForm = (StudyForm) Enum.Parse (typeof (StudyForm), aspirantImport.StudyForm);

            string acronym = aspirantImport.Department.Split (new char[] { ';' }) [0].Trim ();
            var dep = _context.Departments.FirstOrDefault (d => d.Acronym.Contains (acronym));
            aspirant.DepartmentId = _context.Departments.FirstOrDefault (d => d.Acronym == acronym).Id;

            string specialityId = aspirantImport.Speciality.Split (new char[] { ';' }) [0].Trim ();
            aspirant.SpecialityId = Int32.Parse (specialityId);

            Prepod prepod = _context.Prepods.FirstOrDefault (p => (p.DepartmentsString.ContainsWholeWord (acronym) && p.Surename == aspirantImport.Supervisor));
            if (prepod != null) {
                aspirant.PrepodId = prepod.Id;
            } else {
                aspirant.PrepodId = 0;
            }

            return aspirant;
        }

    }
}