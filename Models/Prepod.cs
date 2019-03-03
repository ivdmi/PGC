using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using PGC.Data;

namespace PGC.Models {
    // Науковий керівник
    public class Prepod {
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
        [RegularExpression (@"^[а-яА-ЯЇїІіЄє'a-zA-Z]+$", ErrorMessage = "Дозволяються тільки літери")]
        [StringLength (30, ErrorMessage = "Введіть не більше ніж 30 символів")]
        public string Patronymic { get; set; }

        [Display (Name = "Вчене звання")]
        public Rank Rank { get; set; }

        [Display (Name = "Вчений ступінь")]
        public Degree Degree { get; set; }

        [Display (Name = "Посада")]
        public Position Position { get; set; }

        // public int PositionId { get; set; }

        [Display (Name = "Народження")]
        [DataType (DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Display (Name = "Телефон")]
        public string Phone { get; set; }

        public string Email { get; set; }

        public bool Present { get; set; }

        public ICollection<PrepodDepartment> PrepodDepartment { get; set; }

        // [Display (Name = "Кафедра")]
        // public int? DepartmentId { get; set; }

        [NotMapped]
        public virtual ICollection<Aspirant> Aspirants { get; set; }

        [NotMapped]
        public virtual ICollection<Department> Departments { get; set; }

        public string FIO {
            get {
                if (String.IsNullOrEmpty (Surename) || String.IsNullOrEmpty (Name))
                    return String.Empty;
                StringBuilder sb = new StringBuilder (Surename + " " + Name?.Substring (0, 1) + ".");
                if (!String.IsNullOrEmpty (Patronymic))
                    sb.Append (Patronymic.Substring (0, 1)).Append (".");
                return sb.ToString ();
            }
        }
    }
}