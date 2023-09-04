using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Domain.Entities;
using System.Diagnostics;

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

        #region Public Methods
        /// <summary>
        /// This is the algorithm that calculates the routes, it basically sorts the list of assignment within the current 
        /// time frame and finds the next assignment with the shortest distance in time, and it to the route.
        /// It will continue until the is no next potential assignment, and the restart the loop until there is 
        /// no assignment left that current day
        /// </summary>
        /// <param name="numberOfDays"></param>
        /// <param name="numberOfAssigments"></param>
        /// <returns></returns>
        public List<RouteEntity> CalculateDailyRoutes(int numberOfDays, int numberOfAssignments)
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif

            var plannedRoutes = new List<RouteEntity>();
            var currentDay = DateTime.Now.Date;

            for (int dayIndex = 0; dayIndex < numberOfDays; dayIndex++)
            {
                var availableAssignments = _routeRepo.GetNumberOfAssignments(numberOfAssignments);

                foreach (var assignment in availableAssignments)
                {
                    NormalizeTimeWindows(currentDay, assignment);
                }

                var distances = _routeRepo.GetDistanceses(availableAssignments);

                while (availableAssignments.Any())
                {
                    var routeTimeTracker = currentDay;
                    var startAssignment = availableAssignments.OrderBy(o => o.TimeWindowStart).First();

                    InitializeStartRouteAndAssignmentTimes(startAssignment, ref routeTimeTracker);

                    var routeStartingTime = routeTimeTracker;
                    var currentAssignment = startAssignment;
                    var route = new List<AssignmentEntity> { startAssignment };

                    while (currentAssignment != null)
                    {
                        var nextAssignment = FindNextAssignment(currentAssignment, routeTimeTracker, availableAssignments, distances, route);

                        if (nextAssignment != null)
                        {
                            var totalCurrentRouteHours = ((routeTimeTracker.AddSeconds(nextAssignment.Duration)) - routeStartingTime).TotalHours;

                            if (totalCurrentRouteHours < 8.8)
                            {
                                UpdateRouteTimeAndAssignment(currentAssignment, nextAssignment, ref routeTimeTracker, route, distances);
                                currentAssignment = nextAssignment;
                            }
                            else
                            {
                                currentAssignment = null;
                            }
                        }
                        else
                        {
                            currentAssignment = null;
                        }
                    }

                    AddPlannedRoute(plannedRoutes, route, currentDay);
                    RemoveProcessedAssignments(availableAssignments, route);

                }

                currentDay = currentDay.AddDays(1);
            }

#if DEBUG
            stopwatch.Stop(); // Stop the stopwatch
            double elapsedMinutes = stopwatch.Elapsed.TotalMinutes;
            Console.WriteLine($"Method execution time: {elapsedMinutes}");
#endif

            return plannedRoutes;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method will set the date for the current
        /// assignments time window
        /// </summary>
        /// <param name="currentDay"></param>
        /// <param name="assignment"></param>
        private void NormalizeTimeWindows(DateTime currentDay, AssignmentEntity assignment)
        {
            assignment.TimeWindowStart = NormalizeTime(currentDay, assignment.TimeWindowStart);
            assignment.TimeWindowEnd = NormalizeTime(currentDay, assignment.TimeWindowEnd);

            if (assignment.TimeWindowEnd <= assignment.TimeWindowStart)
            {
                assignment.TimeWindowEnd = assignment.TimeWindowEnd.AddDays(1);
            }
        }

        /// <summary>
        /// Sets the current time frame
        /// </summary>
        /// <param name="currentDay"></param>
        /// <param name="time"></param>
        /// <returns>adjusted time frame</returns>
        private DateTime NormalizeTime(DateTime currentDay, DateTime time)
        {
            return currentDay.AddMilliseconds(time.TimeOfDay.TotalMilliseconds);
        }

        /// <summary>
        /// this method sets the time for the start of a new route and the time
        /// for the first assigment of the route
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="routeTimeTracker"></param>
        private void InitializeStartRouteAndAssignmentTimes(AssignmentEntity assignment, ref DateTime routeTimeTracker)
        {
            assignment.ArrivalTime = assignment.TimeWindowStart;
            routeTimeTracker = assignment.ArrivalTime.AddSeconds(assignment.Duration);
        }

        /// <summary>
        /// This method finds the next assignment that is in the current time frame, depended on the route time tracker
        /// and the distance from current to next assignment measured in time
        /// </summary>
        /// <param name="currentAssignment"></param>
        /// <param name="routeTimeTracker"></param>
        /// <param name="availableAssignments"></param>
        /// <param name="distances"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        private AssignmentEntity FindNextAssignment(AssignmentEntity currentAssignment, DateTime routeTimeTracker,
                        List<AssignmentEntity> availableAssignments, List<DistanceEntity> distances, List<AssignmentEntity> route)
        {
            AssignmentEntity nextAssignment = null;
            double shortestDistanceToPotentialNext = double.MaxValue;

            var availableAssignmentsAtTheCurrentTime = availableAssignments.Where(a =>
                a != currentAssignment &&
                a.TimeWindowStart <= routeTimeTracker && a.TimeWindowEnd >= routeTimeTracker
                && !route.Any(routeAssignment => routeAssignment.Id == a.Id))
                .ToList();

            foreach (var potentialNextAssignment in availableAssignmentsAtTheCurrentTime)
            {
                var distanceToPotentialNext = distances.FirstOrDefault(d =>
                    (d.AssignmentOne == currentAssignment.Id && d.AssignmentTwo == potentialNextAssignment.Id) ||
                    (d.AssignmentTwo == currentAssignment.Id && d.AssignmentOne == potentialNextAssignment.Id));

                if (distanceToPotentialNext != null)
                {
                    var travelTimeFromCurrentToPotentialNext = distanceToPotentialNext.DistanceBetween;
                    if (travelTimeFromCurrentToPotentialNext != 0 && travelTimeFromCurrentToPotentialNext < shortestDistanceToPotentialNext)
                    {
                        shortestDistanceToPotentialNext = travelTimeFromCurrentToPotentialNext;
                        nextAssignment = potentialNextAssignment;
                    }
                }
            }

            return nextAssignment;
        }

        /// <summary>
        /// This method adds the next assignment to the current route list,
        /// and updates the time tracker for the route
        /// </summary>
        /// <param name="nextAssignment"></param>
        /// <param name="routeTimeTracker"></param>
        /// <param name="route"></param>
        /// <param name="distances"></param>
        private void UpdateRouteTimeAndAssignment(AssignmentEntity currentAssignment, AssignmentEntity nextAssignment, ref DateTime routeTimeTracker, List<AssignmentEntity> route, List<DistanceEntity> distances)
        {
            routeTimeTracker = routeTimeTracker.AddSeconds(GetTravelTime(currentAssignment, nextAssignment, distances));
            nextAssignment.ArrivalTime = routeTimeTracker;
            routeTimeTracker = routeTimeTracker.AddSeconds(nextAssignment.Duration);
            route.Add(nextAssignment);
        }

        /// <summary>
        /// Get the distance from current assignment to the next assignment
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="distances"></param>
        /// <returns>returns travel time in seconds</returns>
        private double GetTravelTime(AssignmentEntity currentAssignment, AssignmentEntity nextAssignment, List<DistanceEntity> distances)
        {
            var currentDistance = distances.FirstOrDefault(d =>
                (d.AssignmentOne == currentAssignment.Id && d.AssignmentTwo == nextAssignment.Id) ||
                (d.AssignmentTwo == currentAssignment.Id && d.AssignmentOne == nextAssignment.Id));

            return currentDistance?.DistanceBetween ?? 0;
        }

        /// <summary>
        /// This method adds the current route to the collection of planned routes
        /// </summary>
        /// <param name="plannedRoutes"></param>
        /// <param name="route"></param>
        /// <param name="currentDay"></param>
        private void AddPlannedRoute(List<RouteEntity> plannedRoutes, List<AssignmentEntity> route, DateTime currentDay)
        {
            plannedRoutes.Add(new RouteEntity() { RouteGuid = Guid.NewGuid(), Assignments = route, RouteDate = currentDay });
        }

        /// <summary>
        /// This method removes assignments add to the planned routes from the collection
        /// of assignments that are available
        /// </summary>
        /// <param name="availableAssignments"></param>
        /// <param name="route"></param>
        private void RemoveProcessedAssignments(List<AssignmentEntity> availableAssignments, List<AssignmentEntity> route)
        {
            availableAssignments.RemoveAll(route.Contains);
        }
        #endregion
    }
}
