using System.Collections.Generic;

using ShippingNetworkModel.Data;

namespace ShippingNetworkModel
{
    /// <summary>
    /// Represents a limiter that limits a network routes.
    /// </summary>
    public interface IJourneyRouteLimiter
    {
        /// <summary>
        /// Returns a collection of routes that satisfies a specified condition by stop count.
        /// </summary>
        /// <param name="startPort">The start point of the routes.</param>
        /// <param name="endPort">The end point of the routes.</param>
        /// <param name="minCountStops">The minimum number of days.</param>
        /// <param name="maxCountStops">The maximum number of stops.</param>
        /// <returns>A collection of routes that satisfies a specified condition by stop count.</returns>
        List<List<IPort>> GetRoutesWithStopsLimit(IPort startPort, IPort endPort, int? minCountStops, int? maxCountStops);

        /// <summary>
        /// Returns a collection of routes that satisfies a specified condition by time limit.
        /// </summary>
        /// <param name="startPort">The start point of the routes.</param>
        /// <param name="endPort">The end point of the routes.</param>
        /// <param name="maxDays">The maximum number of days.</param>
        /// <returns>A collection of routes that satisfies a specified condition by time limit.</returns>
        List<List<IPort>> GetRoutesWithTimeLimit(IPort startPort, IPort endPort, int? maxDays);
    }
}
