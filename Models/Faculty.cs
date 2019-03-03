using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

    // Факультет
namespace PGC.Models {
    public class Faculty {        
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Acronym { get; set; }

        public string Decan { get; set; }
         
        public string Phone { get; set; }

        public string Phone2 { get; set; }

        public string Email { get; set; }
        
        public ICollection<Department> Departments { get; set; }
    }    
}