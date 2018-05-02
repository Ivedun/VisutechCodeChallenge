using System;
using System.Collections.Generic;

using ShippingNetworkModel;
using ShippingNetworkModel.Data;

using TDG = VisutechChallenge.TestDataGenerator;

namespace VisutechChallenge
{
    public class Program
    {
        private const string InvalidJourneyMessage = "The journey is invalid";

        //creates the enume for clarity test code 
        private enum Cities
        {
            NewYork = 0,
            Liverpool,
            BuenosAires,
            Casablanca,
            CapeTown
        }

        /// <summary>
        /// Shows journey time information on console.
        /// </summary>
        /// <param name="journeyCalculator">A journey calculator of <see cref="T:ShippingNetworkModel.JourneyCalculator" /> which returns a journey information.</param>
        /// <param name="path">A ports collection (journey).</param>
        /// <param name="pathName">A name of journey.</param>
        private static void ShowJourneyTimeInfo(JourneyCalculator journeyCalculator, List<IPort> path, string pathName)
        {
            var journeyDays = journeyCalculator.GetJourneyDays(path);
            if (journeyDays.HasValue)
                Console.WriteLine($"The total journey time for the route '{pathName}' = {journeyDays} days");
            else
                Console.WriteLine($"'{pathName}' = {InvalidJourneyMessage}");
        }

        public static void Main(string[] args)
        {
            try
            {
                var shippingNetwork = TDG.GetGeneratedShippingNetwork();
                var journeyCalculator = new JourneyCalculator(shippingNetwork);

                var newYork = shippingNetwork.GetPortById((int)Cities.NewYork);
                var liverpool = shippingNetwork.GetPortById((int)Cities.Liverpool);
                var buenosAires = shippingNetwork.GetPortById((int)Cities.BuenosAires);
                var casablanca = shippingNetwork.GetPortById((int)Cities.Casablanca);
                var capeTown = shippingNetwork.GetPortById((int)Cities.CapeTown);

                #region The Total Journey Time

                Console.WriteLine("The total journey time:");
                Console.WriteLine();
                var path = new List<IPort>
                {
                    buenosAires,
                    newYork,
                    liverpool
                };
                ShowJourneyTimeInfo(journeyCalculator, path, "BuenosAires -> NewYork -> Liverpool");

                path = new List<IPort>
                {
                    buenosAires,
                    casablanca,
                    liverpool
                };
                ShowJourneyTimeInfo(journeyCalculator, path, "BuenosAires -> Casablanca -> Liverpool");

                path = new List<IPort>
                {
                    buenosAires,
                    capeTown,
                    newYork,
                    liverpool,
                    casablanca
                };
                ShowJourneyTimeInfo(journeyCalculator, path, "BuenosAires -> CapeTown -> NewYork -> Liverpool -> Casablanca");


                path = new List<IPort>
                {
                    buenosAires,
                    capeTown,
                    casablanca
                };
                ShowJourneyTimeInfo(journeyCalculator, path, "BuenosAires -> CapeTown -> Casablanca");

                #endregion The Total Journey Time

                #region Shortest Journey

                Console.WriteLine();
                Console.WriteLine("Find the shortest journey time for the following routes:");
                Console.WriteLine();

                Console.WriteLine("BuenosAires -> Liverpool:");
                path = journeyCalculator.GetShortestJourney(buenosAires, liverpool);
                if (path?.Count > 0)
                    path.ForEach(port => Console.WriteLine(port.Name));
                else
                    Console.WriteLine(InvalidJourneyMessage);

                Console.WriteLine();
                Console.WriteLine("NewYork -> NewYork:");
                path = journeyCalculator.GetShortestJourney(newYork, newYork);
                if (path?.Count > 0)
                    path.ForEach(port => Console.WriteLine(port.Name));
                else
                    Console.WriteLine(InvalidJourneyMessage);

                #endregion Shortest Journey

                #region The Number Of Routes 

                Console.WriteLine();
                var routes = journeyCalculator.GetRoutesWithStopsLimit(liverpool, liverpool, null, 3);
                Console.WriteLine($"The number of routes from Liverpool to Liverpool with a maximum number of 3 stops = {routes?.Count ?? 0}");

                Console.WriteLine();
                routes = journeyCalculator.GetRoutesWithStopsLimit(buenosAires, liverpool, 4, 4);
                Console.WriteLine($"The number of routes from Buenos Aires to Liverpool where exactly 4 stops are made = {routes?.Count ?? 0}");

                Console.WriteLine();
                routes = journeyCalculator.GetRoutesWithTimeLimit(liverpool, liverpool, 25);
                Console.WriteLine($"The number of routes from Liverpool to Liverpool where the journey time is less than or equal to 25 days = {routes?.Count ?? 0}");

                #endregion The Number Of Routes
            }
            catch (Exception ex)
            {
                var showFullInformation = Properties.Settings.Default.ShowFullErrorInformation;
                if (showFullInformation)
                    Console.Error.WriteLine(ex);
                else
                    Console.Error.WriteLine(ex.Message);
            }

            Console.ReadKey(true);
        }
    }
}
