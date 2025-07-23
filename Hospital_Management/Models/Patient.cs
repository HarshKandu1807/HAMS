using System.Text.Json.Serialization;

namespace Hospital_Management.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ContactNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
