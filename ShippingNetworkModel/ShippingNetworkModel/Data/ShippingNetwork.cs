using System;
using System.Collections.Generic;
using System.Linq;

namespace ShippingNetworkModel.Data
{
    /// <summary>
    /// Represents a shipping network with ports and routes.
    /// </summary>
    public class ShippingNetwork : IShippingNetwork
    {
        #region Fields

        private readonly Dictionary<int, IPort> _ports = new Dictionary<int, IPort>();
        private readonly List<IRoute> _routes = new List<IRoute>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// An enumerable that iterates the network ports.
        /// </summary>
        public IEnumerable<IPort> Ports
        {
            get
            {
                foreach (var port in _ports.Values)
                    yield return port;
            }
        }

        /// <summary>
        /// An enumerable that iterates the network routes.
        /// </summary>
        public IEnumerable<IRoute> Routes
        {
            get
            {
                foreach (var route in _routes)
                    yield return route;
            }
        }

        #endregion Properties

        #region Contructor

        /// <summary>
        /// Constructs a new instance of <see cref="T:ShippingNetworkModel.Data.ShippingNetwork" />.
        /// </summary>
        /// <param name="ports">A collection of <see cref="T:ShippingNetworkModel.Data.IPort" /> that will be added in the network</param>
        public ShippingNetwork(IList<IPort> ports)
        {
            if (ports == null || ports.Count < 2)
                throw new ArgumentException("ports must have more than one element");

            foreach (var port in ports)
            {
                if (_ports.ContainsKey(port.Id))
                    throw new ArgumentException($"The port with Id = {port.Id} already exists");

                _ports[port.Id] = port;
            }
        }

        #endregion Contructor

        #region Public Methods

        /// <summary>
        /// Add <see cref="T:ShippingNetworkModel.Data.IRoute" /> to network collection.
        /// </summary>
        /// <param name="route">The route.</param>
        public void AddRoute(IRoute route)
        {
            if (route.PortFrom == route.PortTo)
                throw new ArgumentException("The route has the same ports 'from' and 'to'");

            if (!_ports.ContainsValue(route.PortFrom) || !_ports.ContainsValue(route.PortTo))
                throw new ArgumentException("The route has port that hasn't in network");

            if (_routes.Any(r => r.PortFrom == route.PortFrom && r.PortTo == route.PortTo))
                throw new ArgumentException("The route with the same ports already exists");

            _routes.Add(route);
        }

        /// <summary>
        /// Removes <see cref="T:ShippingNetworkModel.Data.IRoute" /> from network collection.
        /// </summary>
        /// <param name="portFrom">The start point of the route.</param>
        /// <param name="portTo">The end point of the route.</param>
        public void DeleteRoute(IPort portFrom, IPort portTo)
        {
            var route = _routes.SingleOrDefault(r => r.PortFrom == portFrom && r.PortTo == portTo);
            if (route != null)
                _routes.Remove(route);
        }

        /// <summary>
        /// Returns a <see cref="T:ShippingNetworkModel.Data.IPort" /> that satisfies a specified condition or a null value if no such element exists.
        /// </summary>
        /// <param name="portId">The port Id.</param>
        /// <returns>a <see cref="T:ShippingNetworkModel.Data.IPort" /> from the network port collection.</returns>
        public IPort GetPortById(int portId)
        {
            IPort port;
            _ports.TryGetValue(portId, out port);
            return port;
        }

        #endregion Public Methods
    }
}
