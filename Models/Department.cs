using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PGC.Models {
    // кафедра
    public class Department {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Acronym { get; set; }

        public string Decan { get; set; }

        public string Phone { get; set; }

        public string Phone2 { get; set; }

        public string Email { get; set; }

        public int? FacultyId { get; set; }

        public Faculty Faculty { get; set; }

        public ICollection<DepartmentSpeciality> DepartmentSpecialities { get; set; }

        // не включаются в БД
        [NotMapped]
        public virtual ICollection<Speciality> Specialities { get; set; }

        [NotMapped]
        public virtual ICollection<Aspirant> Aspirants { get; set; }
    }
}