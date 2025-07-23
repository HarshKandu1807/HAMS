using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management.Models.DTOS
{
    public class AppointmentDTO
    {
        public DateTime AppointmentDate { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string Notes { get; set; }
    }
}
