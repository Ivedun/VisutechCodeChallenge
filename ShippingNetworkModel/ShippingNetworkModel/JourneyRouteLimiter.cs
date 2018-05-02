using System;
using System.Collections.Generic;
using System.Linq;

using ShippingNetworkModel.Data;

namespace ShippingNetworkModel
{
    /// <summary>
    /// Represents a limiter that limits a network routes.
    /// </summary>
    public class JourneyRouteLimiter : IJourneyRouteLimiter
    {
        #region Fields

        private readonly IShippingNetwork _shippingNetwork;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="T:ShippingNetworkModel.JourneyRouteLimiter" />.
        /// </summary>
        /// <param name="shippingNetwork">A network of <see cref="T:ShippingNetworkModel.Data.IShippingNetwork" /> which the limiter will work with.</param>
        public JourneyRouteLimiter(IShippingNetwork shippingNetwork)
        {
            if (shippingNetwork == null)
                throw new ArgumentNullException(nameof(shippingNetwork));

            _shippingNetwork = shippingNetwork;
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Returns a collection of routes that satisfies a specified condition by stop count (used recursion).
        /// </summary>
        /// <param name="currentPort">The point of the routes that we will try visit.</param>
        /// <param name="endPort">The end point of the routes.</param>
        /// <param name="maxCountStops">The maximum number of stops.</param>
        /// <param name="path">The collection of ports that already were visited.</param>
        /// <returns>A collection of routes that satisfies a specified condition by stop count.</returns>
        private List<List<IPort>> GetRoutesWithStopsLimit(IPort currentPort, IPort endPort, int? maxCountStops, List<IPort> path)
        {
            // if the port already in the path we haven't a route
            if (path?.Contains(currentPort) ?? false)
                return null;

            var currentPath = path?.ToList() ?? new List<IPort>();
            currentPath.Add(currentPort);

            var result = new List<List<IPort>>();
            // if the current route is equal the end port we find a route
            if (currentPort == endPort && path?.Count > 0)
            {
                result.Add(currentPath);
                return result;
            }

            // check '>' because the first item is a start
            if (maxCountStops.HasValue && currentPath.Count > maxCountStops.Value)
                return null;

            // check all routes from the current port
            foreach (var route in _shippingNetwork.Routes.Where(r => r.PortFrom == currentPort))
            {
                // if the current route is equal the end port we find a route
                if (route.PortTo == endPort)
                {
                    var findedPath = currentPath.ToList();
                    findedPath.Add(route.PortTo);
                    result.Add(findedPath);
                    continue;
                }

                var nodeRoutes = GetRoutesWithStopsLimit(route.PortTo, endPort, maxCountStops, currentPath);
                if (nodeRoutes != null && nodeRoutes.Count > 0)
                    result.AddRange(nodeRoutes);
            }

            return result;
        }

        /// <summary>
        /// Returns a collection of routes that satisfies a specified condition by stop count (used recursion).
        /// </summary>
        /// <param name="currentPort">The point of the routes that we will try visit.</param>
        /// <param name="endPort">The end point of the routes.</param>
        /// <param name="maxDays">The maximum number of days.</param>
        /// <param name="currentDays">The number of days that the journey already lasts.</param>
        /// <param name="path">The collection of ports that already were visited.</param>
        /// <returns>A collection of routes that satisfies a specified condition by time limit.</returns>
        private List<List<IPort>> GetRoutesWithTimeLimit(IPort currentPort, IPort endPort, int? maxDays, int currentDays, List<IPort> path)
        {
            // if the port already in the path we haven't a route
            if (path?.Contains(currentPort) ?? false)
                return null;

            var currentPath = path?.ToList() ?? new List<IPort>();
            currentPath.Add(currentPort);

            var result = new List<List<IPort>>();
            // if the current route is equal the end port we find a route
            if (currentPort == endPort && path?.Count > 0)
            {
                result.Add(currentPath);
                return result;
            }

            // check all routes from the current port
            foreach (var route in _shippingNetwork.Routes.Where(r => r.PortFrom == currentPort))
            {
                var days = route.TravelDays + currentDays;
                if (maxDays.HasValue && days > maxDays.Value)
                    continue;

                // if the current route is equal the end port we find a route
                if (route.PortTo == endPort)
                {
                    var findedPath = currentPath.ToList();
                    findedPath.Add(route.PortTo);
                    result.Add(findedPath);
                    continue;
                }

                var nodeRoutes = GetRoutesWithTimeLimit(route.PortTo, endPort, maxDays, days, currentPath);
                if (nodeRoutes != null && nodeRoutes.Count > 0)
                    result.AddRange(nodeRoutes);
            }

            return result;
        }


        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Returns a collection of routes that satisfies a specified condition by stop count.
        /// </summary>
        /// <param name="startPort">The start point of the routes.</param>
        /// <param name="endPort">The end point of the routes.</param>
        /// <param name="minCountStops">The minimum number of days.</param>
        /// <param name="maxCountStops">The maximum number of stops.</param>
        /// <returns>A collection of routes that satisfies a specified condition by stop count.</returns>
        public List<List<IPort>> GetRoutesWithStopsLimit(IPort startPort, IPort endPort, int? minCountStops, int? maxCountStops)
        {
            if (minCountStops.HasValue && maxCountStops.HasValue && maxCountStops < minCountStops)
                throw new ArgumentException("Max less than Min");

            if (minCountStops.HasValue && minCountStops.Value < 0)
                throw new ArgumentException("Parameter minCountStops must be positive number");

            if (maxCountStops.HasValue && maxCountStops.Value < 0)
                throw new ArgumentException("Parameter maxCountStops must be positive number biger than 0");

            var routes = GetRoutesWithStopsLimit(startPort, endPort, maxCountStops, path: null);
            if (minCountStops.HasValue)
                return routes.Where(route => route.Count > minCountStops.Value).ToList();

            return routes;
        }

        /// <summary>
        /// Returns a collection of routes that satisfies a specified condition by time limit.
        /// </summary>
        /// <param name="startPort">The start point of the routes.</param>
        /// <param name="endPort">The end point of the routes.</param>
        /// <param name="maxDays">The maximum number of days.</param>
        /// <returns>A collection of routes that satisfies a specified condition by time limit.</returns>
        public List<List<IPort>> GetRoutesWithTimeLimit(IPort startPort, IPort endPort, int? maxDays)
        {
            return GetRoutesWithTimeLimit(startPort, endPort, maxDays, 0, null);
        }

        #endregion Public Methods
    }
}
