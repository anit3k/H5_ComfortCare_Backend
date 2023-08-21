using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;

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
        public List<RouteEntity> CalculateDaylyRoutes(List<AssignmentEntity> allAssignments, List<DistanceEntity> distances, DateTime routeStartTime, DateTime routeEndTime)
        {
            List<AssignmentEntity> availableAssignments = allAssignments;
            List<RouteEntity> plannedRoutes = new List<RouteEntity>();

            while (availableAssignments.Any())
            {
                DateTime routeTimeTracker = routeStartTime;
                AssignmentEntity startAssignment = availableAssignments.First();

                startAssignment.ArrivalTime = startAssignment.TimeWindowStart;
                routeTimeTracker = routeTimeTracker.AddSeconds(startAssignment.Duration);

                AssignmentEntity currentAssignment = startAssignment;

                List<AssignmentEntity> assignmentsToRemove = new List<AssignmentEntity>();
                List<AssignmentEntity> route = new List<AssignmentEntity> { startAssignment };

                while (currentAssignment != null)
                {
                    AssignmentEntity nextAssignment = null;
                    double shortestDistanceToPotentialNext = Double.MaxValue;

                    foreach (var potentialNextAssignment in availableAssignments.Where(a => a != currentAssignment && a.TimeWindowEnd >= routeTimeTracker && route.Contains(a) == false))
                    {
                        var distanceToFromCurrentToPotentialNextTuple = distances.FirstOrDefault(d =>
                            (d.AssignmentOne == currentAssignment && d.AssignmentTwo == potentialNextAssignment) ||
                            (d.AssignmentTwo == currentAssignment && d.AssignmentOne == potentialNextAssignment));

                        if (distanceToFromCurrentToPotentialNextTuple == null)
                        {
                            // TODO: implement distance api here, or should it live in the data layer?!?!?!?!?!?
                            
                        }

                        var travelTimeFromCurrentToPotentialNext = distanceToFromCurrentToPotentialNextTuple.DistanceBetween;
                        if (travelTimeFromCurrentToPotentialNext < shortestDistanceToPotentialNext)
                        {
                            shortestDistanceToPotentialNext = travelTimeFromCurrentToPotentialNext;
                            nextAssignment = potentialNextAssignment;
                        }

                    }

                    if (nextAssignment != null)
                    {
                        routeTimeTracker = routeTimeTracker.AddSeconds(shortestDistanceToPotentialNext);
                        nextAssignment.ArrivalTime = routeTimeTracker;

                        routeTimeTracker = routeTimeTracker.AddSeconds(nextAssignment.Duration);

                        route.Add(nextAssignment);
                        assignmentsToRemove.Add(nextAssignment);
                        currentAssignment = nextAssignment;
                    }
                    else
                    {
                        currentAssignment = null;
                    }
                }

                plannedRoutes.Add(new RouteEntity() { RouteGuid = Guid.NewGuid(), Assignments = route});
                availableAssignments.Remove(startAssignment);
                foreach (var assignmentToRemove in assignmentsToRemove)
                {
                    availableAssignments.Remove(assignmentToRemove);
                }

            }

            return plannedRoutes;
        }
        #endregion

    }
}
