using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management.Models.DTOS
{
    public class DoctorLeaveDTO
    {
        public int DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}
