using Hospital_Management.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Prescription
{
    public int PrescriptionId { get; set; }

    public string PrescriptionName { get; set; }

    public int AppointmentId { get; set; }
    [ForeignKey("AppointmentId")]
    public Appointment Appointment { get; set; }
}
