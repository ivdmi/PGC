using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PGC.Models.ViewModels {

    public class DepartmentView : Department {
        public string SpecialitiesString { get; set; }
    }
}