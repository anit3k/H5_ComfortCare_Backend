namespace ComfortCare.Data
{
    /// <summary>
    /// This interface is used to validate if the current user exist in the database
    /// </summary>
    public interface IValidate
    {
        bool ValidateUser(string initials, string password);
    }
}
