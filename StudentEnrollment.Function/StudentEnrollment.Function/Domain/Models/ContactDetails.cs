namespace StudentEnrollment.Function.Domain.Models
{
    public class ContactDetails
    {
        public string Address { get; set; } // Should be class but string for simplicity
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public PreferredContactMethod PreferredContactMethod { get; set; }
    }
}
