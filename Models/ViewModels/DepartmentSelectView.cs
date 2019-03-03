using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PGC.Models.ViewModels {

    public class DepartmentSelectView {
        public int Id { get; set; }

        public string Acronym { get; set; }

        public string Faculty { get; set; }        
    }
}