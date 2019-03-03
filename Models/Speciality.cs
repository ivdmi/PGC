using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PGC.Models {
    public class Speciality {
        [Display (Name = "Код")]
        public int Id { get; set; }

        [Display (Name = "Спеціальність")]
        public string Name { get; set; }

        public int KnowledgeId { get; set; }

        public Knowledge Knowledge { get; set; }

        public bool IsUsed { get; set; }

        [NotMapped]
        public virtual ICollection<Aspirant> Aspirants { get; set; }

        public ICollection<DepartmentSpeciality> DepartmentSpecialities { get; set; }
    }
}