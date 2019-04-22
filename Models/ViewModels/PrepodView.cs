using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PGC.Models.ViewModels {

    public class PrepodView {
        public int Id { get; set; }

        public string Surename { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Rank { get; set; }

        public string Degree { get; set; }

        public string Position { get; set; }

        [DataType (DataType.Date)]
        public DateTime? Birthday { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Present { get; set; }

        public string DeparmentsString { get; set; }
    }
}