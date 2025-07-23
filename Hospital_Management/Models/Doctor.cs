using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hospital_Management.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }

        public string Specialization { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<Appointment> Appointments { get; set; }
        [JsonIgnore]
        public ICollection<DoctorLeave> DoctorLeave { get; set; }
        
    }
}
