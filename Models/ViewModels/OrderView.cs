using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PGC.Models.ViewModels;

namespace PGC.Models {
    public class OrderView {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Context { get; set; }

        [Display (Name = "Дата")]
        [DataType (DataType.Date)]
        public DateTime Date { get; set; }

        [Display (Name = "Вид наказу")]
        public string OrderType { get; set; }

        public int OrderTypeValue { get; set; }

        public virtual ICollection<int> AspirantIdList { get; set; }
    }
}