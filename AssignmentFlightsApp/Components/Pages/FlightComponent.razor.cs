using AssignmentFlightsApp.Models;
using Microsoft.AspNetCore.Components;
using static AssignmentFlightsApp.Models.Flight;

namespace AssignmentFlightsApp.Components.Pages
{
    public partial class FlightComponent : ComponentBase
    {
        List<Flight> flights = new List<Flight>();
        List<Flight> matchingFlights = new List<Flight>();
        HashSet<string> departureAirport = new HashSet<string>() { "Any" };
        HashSet<string> arrivalAirport = new HashSet<string>() { "Any" };
        HashSet<string> weekdays = new HashSet<string> { "Any", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        HashSet<string> Code = new HashSet<string>() { "Flight" };
        HashSet<string> Airline = new HashSet<string>() { "Airline" };
        HashSet<string> Time = new HashSet<string>() { "Time" };
        HashSet<double> CostPerSeat = new HashSet<double>();

        SelectOption selectOption = new SelectOption();

        string selectedRow = string.Empty;


        protected override void OnInitialized()
        {
            base.OnInitialized();

            string resDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot/Data");

            //specify the file name you want to read
            string fileName = "flights.csv";

            // Combine the directory and file name to get the full path
            string filePath = Path.Combine(resDirectory, fileName);

            try
            {
                // Read the contents of the file
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] words = line.Trim().Split(',');
                    Flight flight = new Flight(words[0], words[1], words[2], words[3], words[4], words[5], int.Parse(words[6]), double.Parse(words[7]));
                    flights.Add(flight);
                    departureAirport.Add(words[2]);
                    arrivalAirport.Add(words[3]);
                    Code.Add(words[0]);
                    Airline.Add(words[1]);
                    Time.Add(words[5]);
                    CostPerSeat.Add(double.Parse(words[7]));
                }
            }
            catch (Exception ex)
            {
                // Handle any exeptions that may occur
                Console.WriteLine($"Error reading the file: {ex.Message}");
            }
        }

        private void SearchFlights()
        {
            if (selectOption.DepartureAirport != "Any")
            {
                matchingFlights = flights.Where(f => f.From == selectOption.DepartureAirport).ToList();
            }
            else
            {
                matchingFlights = flights;
            }
        }

        private void OnSelectedFlight(Flight flight)
        {
            selectedRow = flight.Code;
        }
        private ReservationManager reservationManager = new ReservationManager();
        private Reservation reservation; // This needs to be set or created beforehand

        private void SaveReservation()
        {
            reservationManager.SaveReservationToCsv(reservation);
        }
    }
}   
