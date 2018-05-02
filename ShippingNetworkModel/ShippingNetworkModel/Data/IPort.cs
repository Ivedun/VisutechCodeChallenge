namespace ShippingNetworkModel.Data
{
    /// <summary>
    /// Represents a Port.
    /// </summary>
    public interface IPort
    {
        /// <summary>
        /// The port Id.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// The port name.
        /// </summary>
        string Name { get; }
    }
}
