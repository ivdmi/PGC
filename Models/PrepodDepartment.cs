using System.ComponentModel.DataAnnotations;

// таблица соединения многие ко многим 
namespace PGC.Models {
    public class PrepodDepartment {
        public int DepartmentId { get; set; }
        public int PrepodId { get; set; }

        public virtual Department Department { get; set; }
        public virtual Prepod Prepod { get; set; }
    }
}