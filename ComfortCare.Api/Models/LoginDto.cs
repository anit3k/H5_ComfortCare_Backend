namespace ComfortCare.Api.Models
{
    /// <summary>
    /// User login dto
    /// </summary>
    public class LoginDto
    {
        public string Initials{ get; set; }
        public string Password { get; set; }
    }
}
