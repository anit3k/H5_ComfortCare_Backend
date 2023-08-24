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
        private readonly IRouteRepo _routeRepo;
        #endregion

        #region Constructor
        public RouteGenerator(IRouteRepo routeRepo)
        {
            _routeRepo = routeRepo;
        }
        #endregion

        #region Methods
        public List<RouteEntity> CalculateDaylyRoutes(int numberOfDays, int numberOfAssigments)
        {
            List<RouteEntity> plannedRoutes = new List<RouteEntity>();
            var currentDay = DateTime.Now.Date;

            for (int i = 0; i < numberOfDays; i++)
            {
                List<AssignmentEntity> availableAssignments = _routeRepo.GetNumberOfAssignments(numberOfAssigments);

                foreach (var item in availableAssignments)
                {
                    item.TimeWindowStart = currentDay.AddMilliseconds(item.TimeWindowStart.TimeOfDay.TotalMilliseconds);
                    if (item.TimeWindowStart == currentDay.AddHours(23))
                    {
                        item.TimeWindowEnd = currentDay.AddMilliseconds(item.TimeWindowEnd.TimeOfDay.TotalMilliseconds);
                        item.TimeWindowEnd = item.TimeWindowEnd.AddDays(1);
                    }
                    else
                    {
                        item.TimeWindowEnd = currentDay.AddMilliseconds(item.TimeWindowEnd.TimeOfDay.TotalMilliseconds);
                    }
                }

                var distances = _routeRepo.GetDistanceses(availableAssignments);

                while (availableAssignments.Any())
                {
                    DateTime routeTimeTracker = currentDay;
                    AssignmentEntity startAssignment = availableAssignments.OrderBy(o => o.TimeWindowStart).First();

                    startAssignment.ArrivalTime = startAssignment.TimeWindowStart;
                    routeTimeTracker = startAssignment.ArrivalTime;
                    routeTimeTracker = routeTimeTracker.AddSeconds(startAssignment.Duration);

                    AssignmentEntity currentAssignment = startAssignment;

                    List<AssignmentEntity> assignmentsToRemove = new List<AssignmentEntity>();
                    List<AssignmentEntity> route = new List<AssignmentEntity> { startAssignment };

                    while (currentAssignment != null)
                    {
                        AssignmentEntity nextAssignment = null;
                        double shortestDistanceToPotentialNext = Double.MaxValue;
 
                        var availableAssigmentsAtThecurrentTime = availableAssignments.Where(a =>
                                                            a != currentAssignment &&
                                                            a.TimeWindowStart < routeTimeTracker && a.TimeWindowEnd > routeTimeTracker 
                                                            && !route.Contains(a))
                                                            .ToList();

                        foreach(var potentialNextAssignment in availableAssigmentsAtThecurrentTime)
                        {
                            var distanceToFromCurrentToPotentialNextTuple = distances.FirstOrDefault(d =>
                                (d.AssignmentOne == currentAssignment.Id && d.AssignmentTwo == potentialNextAssignment.Id) ||
                                (d.AssignmentTwo == currentAssignment.Id && d.AssignmentOne == potentialNextAssignment.Id));

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

                    plannedRoutes.Add(new RouteEntity() { RouteGuid = Guid.NewGuid(), Assignments = route });
                    availableAssignments.Remove(startAssignment);
                    foreach (var assignmentToRemove in assignmentsToRemove)
                    {
                        availableAssignments.Remove(assignmentToRemove);
                    }

                }

                currentDay = currentDay.AddDays(i);
            }

            return plannedRoutes;
        }
        #endregion
    }
}
