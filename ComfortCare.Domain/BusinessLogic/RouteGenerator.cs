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
        #endregion

        #region Constructor
        public RouteGenerator(IRouteConstructionRepo routeRepo)
        {
            _routeRepo = routeRepo;
        }
        #endregion

        #region Methods
        public List<RouteEntity> CalculateDaylyRoutes(DateTime routeStartTime, DateTime routeEndTime)
        {
            List<AssignmentEntity> availableAssignments = _routeRepo.GetAssignmentsInPeriod(routeStartTime, routeEndTime);
            var distances = _routeRepo.GetDistanceses(availableAssignments);



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
                            (d.AssignmentOne == currentAssignment.Id && d.AssignmentTwo == potentialNextAssignment.Id) ||
                            (d.AssignmentTwo == currentAssignment.Id && d.AssignmentOne == potentialNextAssignment.Id));

                        // TODO: We need to find the error resulting in null exception, in the "distanceToFromCurrentToPotentialNextTuple".
                        if (distanceToFromCurrentToPotentialNextTuple != null)
                        {
                            var travelTimeFromCurrentToPotentialNext = distanceToFromCurrentToPotentialNextTuple.DistanceBetween;
                            if (travelTimeFromCurrentToPotentialNext != 0)
                            {
                                if (travelTimeFromCurrentToPotentialNext < shortestDistanceToPotentialNext)
                                {
                                    shortestDistanceToPotentialNext = travelTimeFromCurrentToPotentialNext;
                                    nextAssignment = potentialNextAssignment;
                                }  
                            }
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


            // TODO: Remove when finished debug test.
            int intCounter = 0;

            foreach (var plannedRoute in plannedRoutes) { 
                foreach (var assignment in plannedRoute.Assignments)
                {
                    intCounter++;
                }
            }


            return plannedRoutes;
        }
        #endregion

    }
}
