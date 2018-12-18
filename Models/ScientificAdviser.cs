using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PGC.Models {
    // Науковий керівник
    public class ScientificAdviser {
        public int Id { get; set; }

        [Display (Name = "Прізвище")]
        [Required]
        [RegularExpression (@"^[а-яА-ЯЇїІіЄє'a-zA-Z]+$", ErrorMessage = "Дозволяються тільки літери")]
        [StringLength (30, ErrorMessage = "Введіть не більше ніж 30 символів")]
        public string Surename { get; set; }

        [Display (Name = "Ім'я")]
        [Required]
        [RegularExpression (@"^[а-яА-ЯЇїІіЄє'a-zA-Z]+$", ErrorMessage = "Дозволяються тільки літери")]
        [StringLength (30, ErrorMessage = "Введіть не більше ніж 30 символів")]
        public string Name { get; set; }

        [Display (Name = "По батькові")]
        [Required]
        [RegularExpression (@"^[а-яА-ЯЇїІіЄє'a-zA-Z]+$", ErrorMessage = "Дозволяються тільки літери")]
        [StringLength (30, ErrorMessage = "Введіть не більше ніж 30 символів")]
        public string Patronymic { get; set; }

        [Display (Name = "Народження")]
        [DataType (DataType.Date)]
        public DateTime? Birthday { get; set; }

        public ICollection<Aspirant> Aspirants { get; set; }

        public string FIO {
            get {
                StringBuilder sb = new StringBuilder (Surename + " " + Name.Substring (0, 1) + ".");
                if (!String.IsNullOrEmpty (Patronymic))
                    sb.Append (Patronymic.Substring (0, 1)).Append (".");
                return sb.ToString ();
            }
        }
    }
}