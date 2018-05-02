namespace ShippingNetworkModel.Data
{
    /// <summary>
    /// Represents a route.
    /// </summary>
    public class Route : IRoute
    {
        /// <summary>
        /// A <see cref="T:ShippingNetworkModel.Data.IPort" /> instance that represents the start point of the route.
        /// </summary>
        public IPort PortFrom { get; }

        /// <summary>
        /// A <see cref="T:ShippingNetworkModel.Data.IPort" /> instance that represents the end point of the route.
        /// </summary>
        public IPort PortTo { get; }

        /// <summary>
        /// The journey time for the route (in days).
        /// </summary>
        public int TravelDays { get; }

        /// <summary>
        /// Constructs a new instance of <see cref="T:ShippingNetworkModel.Data.Route" />.
        /// </summary>
        /// <param name="portFrom">The start point of the route.</param>
        /// <param name="portTo">The end point of the route.</param>
        /// <param name="travelDays">The journey time for the route (in days).</param>
        public Route(IPort portFrom, IPort portTo, int travelDays)
        {
            PortFrom = portFrom;
            PortTo = portTo;
            TravelDays = travelDays;
        }
    }
}
