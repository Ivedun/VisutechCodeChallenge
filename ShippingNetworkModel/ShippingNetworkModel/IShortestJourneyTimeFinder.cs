using System.Collections.Generic;

using ShippingNetworkModel.Data;

namespace ShippingNetworkModel
{
    /// <summary>
    /// Represents a finder that finds a shortest route.
    /// </summary>
    public interface IShortestJourneyTimeFinder
    {
        /// <summary>
        /// Returns a collection of ports that is the shortest route of journey.
        /// </summary>
        /// <param name="startPort">The start point of the route.</param>
        /// <param name="endPort">The end point of the route.</param>
        /// <returns>A collection of ports that is the shortest route of journey.</returns>
        List<IPort> GetShortestJourney(IPort startPort, IPort endPort);
    }
}
