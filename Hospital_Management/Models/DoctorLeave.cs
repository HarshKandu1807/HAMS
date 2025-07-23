using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hospital_Management.Models
{
    public class DoctorLeave
    {
        [Key]
        public int LeaveId { get; set; }
        public int DoctorId { get; set; }
        [JsonIgnore]
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}
