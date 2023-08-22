using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

namespace ComfortCare.Domain.BusinessLogic
{

    /// <summary>
    /// This class is used to populate routes with employees, it should use the rules for workinghours
    /// and look at the induvidual employee to calculate their specific working time etc...
    /// TODO: Kent, please rewrite this section when done
    /// </summary>
    public class SchemaGenerator
    {


        #region Fields
        private readonly IEmployeesRepo _employeesRepo;
        #endregion




        #region Constructor 
        public SchemaGenerator(IEmployeesRepo employeesRepo)
        {
            _employeesRepo = employeesRepo;
        }
        #endregion




        #region Methods
        public void GenerateSchema()
        {

        }
        #endregion
    }
}


