namespace StudentEnrollment.Function.Domain.Models
{
    public class DatabaseResult
    {
        public bool Success { get { return this.ErrorMessage == null; } }

        public string ErrorMessage { get; set; }
    }
}
