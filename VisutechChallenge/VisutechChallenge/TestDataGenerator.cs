using System.Collections.Generic;

using ShippingNetworkModel.Data;

namespace VisutechChallenge
{
    /// <summary>
    /// Provides a set of static methods for generating test data.
    /// </summary>
    public static class TestDataGenerator
    {
        /// <summary>
        /// Creates a <see cref="T:ShippingNetworkModel.Data.IShippingNetwork" /> that represents network
        /// with ports and routes.
        /// </summary>
        /// <returns>a ShippingNetwork instance.</returns>
        public static IShippingNetwork GetGeneratedShippingNetwork()
        {
            var newYork     = new Port(0, "New York");
            var liverpool   = new Port(1, "Liverpool");
            var buenosAires = new Port(2, "Buenos Aires");
            var casablanca  = new Port(3, "Casablanca");
            var capeTown    = new Port(4, "Cape Town");

            var ports = new List<IPort>
            {
                newYork,
                liverpool,
                buenosAires,
                casablanca,
                capeTown
            };

            var shippingNetwork = new ShippingNetwork(ports);

            var routes = new List<IRoute>
            {
                new Route(newYork, liverpool, 4),
                new Route(liverpool, casablanca, 3),
                new Route(liverpool, capeTown, 6),
                new Route(buenosAires, newYork, 6),
                new Route(buenosAires, casablanca, 5),
                new Route(buenosAires, capeTown, 4),
                new Route(casablanca, liverpool, 3),
                new Route(casablanca, capeTown, 6),
                new Route(capeTown, newYork, 8)
            };

            routes.ForEach(route => shippingNetwork.AddRoute(route));
            return shippingNetwork;
        }
    }
}
