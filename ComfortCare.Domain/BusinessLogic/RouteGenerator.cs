using ComfortCare.Domain.BusinessLogic.interfaces;

namespace ComfortCare.Domain.BusinessLogic
{
    /// <summary>
    /// This class is used to generate routes to the ComfortCare Eco system, the generator
    /// will generate routes for a single day at a time, so wrap it in a foreach if you need to calculate
    /// multiple days
    /// </summary>
    public class RouteGenerator
    {
        #region fields
        private readonly IRouteConstructionRepo _routeRepo;
        private readonly IDistanceRequest _distanceApi;
        #endregion

        #region Constructor
        public RouteGenerator(IRouteConstructionRepo routeRepo, IDistanceRequest distanceApi)
        {
            _routeRepo = routeRepo;
            _distanceApi = distanceApi;
        }
        #endregion

        #region Methods

        #endregion

    }
}
