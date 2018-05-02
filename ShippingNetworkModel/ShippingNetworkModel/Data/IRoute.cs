namespace ShippingNetworkModel.Data
{
    /// <summary>
    /// Represents a route.
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        /// A <see cref="T:ShippingNetworkModel.Data.IPort" /> instance that represents the start point of the route.
        /// </summary>
        IPort PortFrom { get; }
        /// <summary>
        /// A <see cref="T:ShippingNetworkModel.Data.IPort" /> instance that represents the end point of the route.
        /// </summary>
        IPort PortTo { get; }
        /// <summary>
        /// The journey time for the route (in days).
        /// </summary>
        int TravelDays { get; }
    }
}
