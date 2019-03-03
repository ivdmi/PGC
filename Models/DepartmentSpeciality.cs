using System.ComponentModel.DataAnnotations;

// таблица соединения многие ко многим для табл. Aspirant и Order
namespace PGC.Models {
    public class DepartmentSpeciality {
        public int DepartmentId { get; set; }
        public int SpecialityId { get; set; }

        public virtual Department Department { get; set; }
        public virtual Speciality Speciality { get; set; }
    }
}