using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PGC.Data;

namespace PGC.Models {
    public class AspirantImport {
        public int Id { get; set; }

        public int AspirantId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surename { get; set; }

        public string Patronymic { get; set; }

        // [DataType (DataType.Date)]
        public int Birthday { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Language { get; set; }

        public string University { get; set; }

        public int UnivFinish { get; set; }

        public string Qualification { get; set; }

        public string Work { get; set; }

        public string Honours { get; set; }
        public int DegreePoint { get; set; }
        public int PublicationQuantity { get; set; }
        public string Indicia { get; set; }
        public string Supervisor { get; set; }
        public string Dorm { get; set; }
        public string Vacation { get; set; }

        // [DataType (DataType.Date)]
        public int DateDoc { get; set; }

        //[DataType (DataType.Date)]
        //public DateTime? InputDate { get; set; }

        public string Sex { get; set; }

        public string Speciality { get; set; }

        public string Department { get; set; }

        public string StudyForm { get; set; }

        public string Budget { get; set; }

    }
}