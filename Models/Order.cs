﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PGC.Data;

namespace PGC.Models {
    public class Order {
        public int Id { get; set; }

        [Display (Name = "Номер")]
        public string Number { get; set; }

        [Display (Name = "Зміст наказу")]
        public string Context { get; set; }

        [Display (Name = "Дата")]
        [DataType (DataType.Date)]
        public DateTime Date { get; set; }

        // public int OrdertypeId { get; set; }

        [Display (Name = "Вид наказу")]
        public OrderType OrderType { get; set; }

        //   public ICollection<AspirantOrder> AspirantsOrders { get; set; }

        // не включаются в БД
        [NotMapped]
        [Display (Name = "Аспіранти")]
        public virtual ICollection<Aspirant> Aspirants { get; set; }
    }
}