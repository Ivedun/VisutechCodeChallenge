using System;
using System.Collections.Generic;
using System.Linq;

using ShippingNetworkModel.Data;

namespace ShippingNetworkModel
{
    /// <summary>
    /// Represents a finder that finds a shortest route by dijkstra algorithm.
    /// </summary>
    public class DijkstraShortestJourneyTimeFinder : IShortestJourneyTimeFinder
    {
        #region Fields

        private readonly IShippingNetwork _shippingNetwork;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="T:ShippingNetworkModel.DijkstraShortestJourneyTimeFinder" />.
        /// </summary>
        /// <param name="shippingNetwork">A network of <see cref="T:ShippingNetworkModel.Data.IShippingNetwork" /> which the finder will work with.</param>
        public DijkstraShortestJourneyTimeFinder(IShippingNetwork shippingNetwork)
        {
            if (shippingNetwork == null)
                throw new ArgumentNullException(nameof(shippingNetwork));

            _shippingNetwork = shippingNetwork;
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Finds a shortest journey path when the start port not is equal to the end port (used dijkstra recursion algorithm).
        /// </summary>
        /// <param name="startPort">The start point of the route.</param>
        /// <param name="endPort">The end point of the route.</param>
        /// <param name="daysCount">The number of days that the journey lasts.</param>
        /// <returns>A collection of ports that is the shortest route of journey.</returns>
        private List<IPort> GetShortestJourneyByDijkstra(IPort startPort, IPort endPort, out int? daysCount)
        {
            // in the start all ports are not visited
            var notVisited = _shippingNetwork.Ports.ToList();
            var track = new Dictionary<IPort, DijkstraData>
            {
                [startPort] = new DijkstraData {Price = 0, Previous = null}
            };
            var routes = _shippingNetwork.Routes.ToList();

            daysCount = null;
            while (true)
            {
                IPort toOpen = null;
                var bestPrice = int.MaxValue;
                // tries to get port with a minimal price
                foreach (var port in notVisited)
                {
                    if (track.ContainsKey(port) && track[port].Price < bestPrice)
                    {
                        bestPrice = track[port].Price;
                        toOpen = port;
                    }
                }

                // the path not exists
                if (toOpen == null)
                    return null;
                // we found the path
                if (toOpen == endPort)
                    break;

                // check all routes from the current port
                foreach (var route in routes.Where(r => r.PortFrom == toOpen))
                {
                    var currentPrice = track[toOpen].Price + route.TravelDays;
                    var nextNode = route.PortTo;
                    if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
                    {
                        track[nextNode] = new DijkstraData { Previous = toOpen, Price = currentPrice };
                    }
                }

                notVisited.Remove(toOpen);
            }

            daysCount = track[endPort].Price;

            // creates a collection of ports that is the shortest path
            var result = new List<IPort>();
            while (endPort != null)
            {
                result.Add(endPort);
                endPort = track[endPort].Previous;
            }
            result.Reverse();
            return result;
        }

        /// <summary>
        /// Finds a shortest journey path when the start port is equal to the end port.
        /// </summary>
        /// <param name="startPort">The start point of the route.</param>
        /// <returns>A collection of ports that is the shortest route of journey.</returns>
        private List<IPort> GetShortestJourneyInSamePlace(IPort startPort)
        {
            int? resultDaysCount = null;
            List<IPort> bestPath = null;
            // check all routes TO the start port
            foreach (var route in _shippingNetwork.Routes.Where(r => r.PortTo == startPort))
            {
                int? daysCount;
                // finds a shortest journey path by dijkstra algorithm.
                var path = GetShortestJourneyByDijkstra(startPort, route.PortFrom, out daysCount);
                // if path not exists than skip
                if (!daysCount.HasValue)
                    continue;

                daysCount += route.TravelDays;
                // if we found shortest path than remains it
                if (!resultDaysCount.HasValue || resultDaysCount.Value >= daysCount)
                {
                    resultDaysCount = daysCount;
                    bestPath = path;
                }
            }

            bestPath?.Add(startPort);
            return bestPath;
        }


        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Returns a collection of ports that is the shortest route of journey.
        /// </summary>
        /// <param name="startPort">The start point of the route.</param>
        /// <param name="endPort">The end point of the route.</param>
        /// <returns>A collection of ports that is the shortest route of journey.</returns>
        public List<IPort> GetShortestJourney(IPort startPort, IPort endPort)
        {
            if (startPort == endPort)
                return GetShortestJourneyInSamePlace(startPort);

            int? daysCount;
            return GetShortestJourneyByDijkstra(startPort, endPort, out daysCount);
        }

        #endregion Public Methods


        #region Private Classes

        /// <summary>
        /// Represents a addition data for the dijkstra algorithm.
        /// </summary>
        private class DijkstraData
        {
            public IPort Previous { get; set; }
            public int Price { get; set; }
        }

        #endregion Private Classes

    }
}
