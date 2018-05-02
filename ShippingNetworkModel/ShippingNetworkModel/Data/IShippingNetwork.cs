using System.Collections.Generic;

namespace ShippingNetworkModel.Data
{
    /// <summary>
    /// Represents a shipping network with ports and routes.
    /// </summary>
    public interface IShippingNetwork
    {
        /// <summary>
        /// An enumerable that iterates the network ports.
        /// </summary>
        IEnumerable<IPort> Ports { get; }
        /// <summary>
        /// An enumerable that iterates the network routes.
        /// </summary>
        IEnumerable<IRoute> Routes { get; }

        /// <summary>
        /// Add <see cref="T:ShippingNetworkModel.Data.IRoute" /> to network collection.
        /// </summary>
        /// <param name="route">The route.</param>
        void AddRoute(IRoute route);
        /// <summary>
        /// Removes <see cref="T:ShippingNetworkModel.Data.IRoute" /> from network collection.
        /// </summary>
        /// <param name="portFrom">The start point of the route.</param>
        /// <param name="portTo">The end point of the route.</param>
        void DeleteRoute(IPort portFrom, IPort portTo);
        /// <summary>
        /// Returns a <see cref="T:ShippingNetworkModel.Data.IPort" /> that satisfies a specified condition or a null value if no such element exists.
        /// </summary>
        /// <param name="portId">The port Id.</param>
        /// <returns>a <see cref="T:ShippingNetworkModel.Data.IPort" /> from the network port collection.</returns>
        IPort GetPortById(int portId);
    }
}
