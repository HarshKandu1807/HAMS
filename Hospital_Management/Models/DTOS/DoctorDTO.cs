using System.Text.Json.Serialization;

namespace Hospital_Management.Models.DTOS
{
    public class DoctorDTO
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string Specialization { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
