using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PGC.Models.ViewModels {

    public class AspirantView {
        public int Id { get; set; }

        public string Surename { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string FIO { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public int? BirthYear { get; set; }
        public int? InputYear { get; set; }
        public int? GraduationYear { get; set; }
        public int? ProtectionYear { get; set; }

        [Display (Name = "Денна/Вечірня/Заочна")]
        public string StudyForm { get; set; }

        [Display (Name = "Бюджет/Контракт")]
        public string Budget { get; set; }

        [Display (Name = "Докторант/Аспірант")]
        public string Doctorant { get; set; }

        [Display (Name = "Чоловік/Жінка")]
        public string Sex { get; set; }

        [Display (Name = "Захист")]
        public string Protection { get; set; }

        public string Present { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
        public string P5 { get; set; }
        public string P6 { get; set; }
        public string P7 { get; set; }

        public int Course { get; set; }

        public int? SpecialityId { get; set; }

        //		public string SpecialityName { get; set; }

        public string Department { get; set; }
        public string Faculty { get; set; }

        public string Prepod { get; set; }

        public string StatusType { get; set; }
    }
}