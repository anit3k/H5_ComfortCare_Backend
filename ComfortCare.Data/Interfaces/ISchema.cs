using ComfortCare.Data.Models;

namespace ComfortCare.Data.Interfaces
{
    /// <summary>
    /// This interface is the used by the controller to get the schema for the active user.
    /// </summary>
    public interface ISchema
    {
        Tuple<string, List<Assignment>> CurrentSchema(string employeeInitials, string employeePassword);
    }
}
