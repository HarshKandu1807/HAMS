namespace Hospital_Management.Models.DTOS
{
    public class PatientDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int ContactNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
