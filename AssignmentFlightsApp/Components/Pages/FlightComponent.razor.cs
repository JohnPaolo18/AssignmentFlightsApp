using AssignmentFlightsApp.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssignmentFlightsApp.Components.Pages
{
    public partial class FlightComponent : ComponentBase
    {
        private List<Flight> flights = new List<Flight>();
        private List<Flight> matchingFlights = new List<Flight>();
        private HashSet<string> departureAirport = new HashSet<string>() { "Any" };
        private HashSet<string> arrivalAirport = new HashSet<string>() { "Any" };
        private HashSet<string> weekdays = new HashSet<string> { "Any", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private HashSet<string> FlightCode = new HashSet<string>() { "Flight" };
        private HashSet<string> Airline = new HashSet<string>() { "Airline" };
        private HashSet<string> Time = new HashSet<string>() { "Time" };
        private HashSet<double> CostPerSeat = new HashSet<double>();

        private SelectOption selectOption = new SelectOption();
        private string selectedRow = string.Empty;


        protected override void OnInitialized()
        {
            base.OnInitialized();

            string resDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot/Data");
            string fileName = "flights.csv";
            string filePath = Path.Combine(resDirectory, fileName);

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] words = line.Trim().Split(',');
                    Flight flight = new Flight(words[0], words[1], words[2], words[3], words[4], words[5], int.Parse(words[6]), double.Parse(words[7]));
                    flights.Add(flight);
                    departureAirport.Add(words[2]);
                    arrivalAirport.Add(words[3]);
                    FlightCode.Add(words[0]);
                    Airline.Add(words[1]);
                    Time.Add(words[5]);
                    CostPerSeat.Add(double.Parse(words[7]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the file: {ex.Message}");
            }
        }

        private void SearchFlights()
        {
            matchingFlights = flights.Where(f =>
                (selectOption.DepartureAirport == "Any" || f.From == selectOption.DepartureAirport) &&
                (selectOption.ArrivalAirport == "Any" || f.To == selectOption.ArrivalAirport) &&
                (selectOption.Weekday == "Any" || f.Weekday == selectOption.Weekday)).ToList();

            searchPerformed = true;

            // If there are matching flights, automatically select the first one
            if (matchingFlights.Any())
            {
                var firstMatch = matchingFlights.First();
                selectedFlightCode = firstMatch.FlightCode; // This assumes you have a two-way binding on selectedFlightCode
                                                            // Now, the other fields should automatically update because they are computed properties based on the selected flight code
            }
            else
            {
                selectedFlightCode = null; // Reset the selection
            }

            StateHasChanged(); // This will trigger a UI update
        }

        private void OnSelectedFlight(Flight flight)
        {
            selectedRow = flight.FlightCode;
        }


    }
}
