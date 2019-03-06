using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PGC.Data;

namespace PGC.Models {
    public class Aspirant {
        public int Id { get; set; }

        [Display (Name = "Ім'я")]
        [Required]
        [RegularExpression (@"^[а-яА-ЯЇїІіЄє'a-zA-Z]+$", ErrorMessage = "Дозволяються тільки літери")]
        [StringLength (30)]
        public string Name { get; set; }

        [Display (Name = "Прізвище")]
        [Required]
        [StringLength (30, ErrorMessage = "Введіть не більше ніж 30 символів")]
        public string Surename { get; set; }

        [Display (Name = "По батькові")]
        public string Patronymic { get; set; }

        [Display (Name = "Народження")]
        [DataType (DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Display (Name = "Телефон")]
        public string Phone { get; set; }

        public string Email { get; set; }
        
        [DataType (DataType.Date)]
        public DateTime? InputDate { get; set; }

        [DataType (DataType.Date)]
        public DateTime? GraduationDate { get; set; }
        
        [DataType (DataType.Date)]
        public DateTime? ProtectionDate { get; set; }

        [Display(Name = "Стаціонар/Заочна")]
        public bool Stationary { get; set; }

        [Display(Name = "Бюджет/Контракт")]
        public bool Budget { get; set; }

        [Display (Name = "Докторант/Аспірант")]
        public bool Doctorant { get; set; }

        [Display (Name = "Чоловік/Жінка")]
        public bool Sex { get; set; }

        [Display (Name = "Захист")]
        public bool Protection { get; set; }

        public bool Present {get; set;}

        [Range (0, 5)]
        [Display (Name = "Курс")]
        public int Course { get; set; }

        // public int? StatustypeId { get; set; }

        // ключи

        [Display (Name = "Спеціальність")]
        public int? SpecialityId { get; set; }        
                
        [Display (Name = "Кафедра")]
        public int? DepartmentId { get; set; }

 //       public int? StudyformId { get; set; }
        

        public int? PrepodId { get; set; }

        //  -------------------- навигационные свойства ------------------------

        public Speciality Speciality { get; set; }        

        public Department Department {get;set;}
        public Prepod Prepod { get; set; }

        [Display (Name = "Статус")]
        public StatusTypeEnum StatusType { get; set; }

        public StudyForm  StudyForm {get;set;}

        public ICollection<AspirantOrder> AspirantOrders { get; set; }

        [Display (Name = "Вступ")]
        public int? InputYear {
            get {
                if (InputDate != null) {
                    return InputDate.Value.Year;
                } else {
                    return null;
                }
            }
        }

        public string FIO {
            get {
                if (!String.IsNullOrEmpty (Surename)) {
                    StringBuilder sb = new StringBuilder (Surename + " ");
                    if (!String.IsNullOrEmpty (Name)) {
                        sb.Append (Name.Substring (0, 1)).Append (".");
                        if (!String.IsNullOrEmpty (Patronymic))
                            sb.Append (Patronymic.Substring (0, 1)).Append (".");
                    }
                    return sb.ToString ();
                } else { return String.Empty; }

            }
        }
    }
}