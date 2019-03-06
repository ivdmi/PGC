using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PGC.Data;

namespace PGC.Models {
    public class AspirantSelectLists {
        private Aspirant aspirant;

        public AspirantSelectLists (Aspirant aspirant, AspiranturaContext context) {
            Statuses = Helper.GetStatuses ();
            Studyforms = Helper.GetStudyForms ();
            Specialities = Helper.GetSpecialities(context);
            Departments = Helper.GetDepartments(context);
            Prepods = Helper.GetPrepods(context);

            if (aspirant != null) {
                selectedStatus = Statuses.FirstOrDefault (k => k.Value == (int) aspirant.StatusType);
                selectedStudyform = Studyforms.FirstOrDefault (k => k.Value == (int) aspirant.StudyForm);
                selectedSpeciality = Specialities.FirstOrDefault (k => k.Value == aspirant.SpecialityId);   // возможно нужна проверка на null
                selectedDepartment = Departments.FirstOrDefault (k => k.Value == aspirant.DepartmentId);
                selectedPrepod = Prepods.FirstOrDefault (k => k.Value == aspirant.PrepodId);            
            }
        }

        public IEnumerable<ItemData> Statuses { get; set; }
        public IEnumerable<ItemData> Studyforms { get; set; }        

        public IEnumerable<ItemData> Specialities { get; set; }

        public IEnumerable<ItemData> Departments { get; set; }

        public IEnumerable<ItemData> Prepods { get; set; }

        // Это чтобы не выискивать в js - передадим готовое
        public ItemData selectedStatus { get; set; }
        public ItemData selectedStudyform { get; set; }
        public ItemData selectedSpeciality { get; set; } 
        public ItemData selectedDepartment { get; set; } 
        public ItemData selectedPrepod { get; set; }       

    }       
}