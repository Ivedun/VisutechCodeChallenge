namespace ShippingNetworkModel.Data
{
    /// <summary>
    /// Represents a port
    /// </summary>
    public class Port : IPort
    {
        /// <summary>
        /// The port Id.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// The port name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructs a new instance of <see cref="T:ShippingNetworkModel.Data.Port" />.
        /// </summary>
        /// <param name="id">The port Id.</param>
        /// <param name="name">The port name.</param>
        public Port(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
