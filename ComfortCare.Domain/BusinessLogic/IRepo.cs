namespace ComfortCare.Domain.BusinessLogic
{
    /// <summary>
    /// This interface is used by the plan-manager to get data from the datasource
    /// </summary>
    public interface IRepo
    {
        // TODO: make returns types after EF migrations
        public void Read();
        public void Create();
        public void Update();
        public void Delete();
    }
}
