using System;
using System.Collections.Generic;
using System.Linq;

using ShippingNetworkModel.Data;

namespace ShippingNetworkModel
{
    /// <summary>
    /// Represents a journey calculator that finds routes that satisfies a specified condition.
    /// </summary>
    public class JourneyCalculator : IShortestJourneyTimeFinder, IJourneyRouteLimiter
    {
        #region Fields

        private readonly IShippingNetwork _shippingNetwork;
        private readonly IShortestJourneyTimeFinder _shortestJourneyFinder;
        private readonly IJourneyRouteLimiter _journeyRouteLimiter;

        #endregion Fields

        #region Constractors

        /// <summary>
        /// Constructs a new instance of <see cref="T:ShippingNetworkModel.JourneyCalculator" />.
        /// </summary>
        /// <param name="shippingNetwork">A network of <see cref="T:ShippingNetworkModel.Data.IShippingNetwork" /> which the calculator will work with.</param>
        public JourneyCalculator(IShippingNetwork shippingNetwork)
        {
            if (shippingNetwork == null)
                throw new ArgumentNullException(nameof(shippingNetwork));

            _shippingNetwork = shippingNetwork;
            _shortestJourneyFinder = new DijkstraShortestJourneyTimeFinder(_shippingNetwork);
            _journeyRouteLimiter = new JourneyRouteLimiter(_shippingNetwork);
        }

        /// <summary>
        /// Constructs a new instance of <see cref="T:ShippingNetworkModel.JourneyCalculator" />.
        /// </summary>
        /// <param name="shippingNetwork">A network of <see cref="T:ShippingNetworkModel.Data.IShippingNetwork" /> which the calculator will work with.</param>
        /// <param name="shortestJourneyFinder">A shortest path finder of <see cref="T:ShippingNetworkModel.Data.IShortestJourneyTimeFinder" /> which the calculator will work with.</param>
        /// <param name="journeyRouteLimiter">A routes limiter of <see cref="T:ShippingNetworkModel.Data.IJourneyRouteLimiter" /> which the calculator will work with.</param>
        internal JourneyCalculator(IShippingNetwork shippingNetwork, IShortestJourneyTimeFinder shortestJourneyFinder,
            IJourneyRouteLimiter journeyRouteLimiter)
        {
            if (shippingNetwork == null)
                throw new ArgumentNullException(nameof(shippingNetwork));

            if (shortestJourneyFinder == null)
                throw new ArgumentNullException(nameof(shortestJourneyFinder));

            if (journeyRouteLimiter == null)
                throw new ArgumentNullException(nameof(journeyRouteLimiter));

            _shippingNetwork = shippingNetwork;
            _shortestJourneyFinder = shortestJourneyFinder;
            _journeyRouteLimiter = journeyRouteLimiter;
        }

        #endregion Constractors

        #region Public Methods

        /// <summary>
        /// Returns the number of days that the journey lasts or a null if path is impossible.
        /// </summary>
        /// <param name="path">The ports collection.</param>
        /// <returns>The number of days that the journey lasts.</returns>
        public int? GetJourneyDays(List<IPort> path)
        {
            if (path == null || path.Count < 2)
                return null;

            int travelDays = 0;
            var routes = _shippingNetwork.Routes.ToList();
            for (int i = 0; i < path.Count - 1; i++)
            {
                var portFrom = path[i];
                var portTo = path[i + 1];
                var edge = routes.FirstOrDefault(e => e.PortFrom == portFrom && e.PortTo == portTo);
                if (edge == null)
                    return null;

                travelDays += edge.TravelDays;
            }

            return travelDays;
        }

        /// <summary>
        /// Returns a collection of ports that is the shortest route of journey.
        /// </summary>
        /// <param name="startPort">The start point of the route.</param>
        /// <param name="endPort">The end point of the route.</param>
        /// <returns>A collection of ports that is the shortest route of journey.</returns>
        public List<IPort> GetShortestJourney(IPort startPort, IPort endPort)
        {
            return _shortestJourneyFinder.GetShortestJourney(startPort, endPort);
        }

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
            return _journeyRouteLimiter.GetRoutesWithStopsLimit(startPort, endPort, minCountStops, maxCountStops);
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
            return _journeyRouteLimiter.GetRoutesWithTimeLimit(startPort, endPort, maxDays);
        }

        #endregion Public Methods
    }
}
