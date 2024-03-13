using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace AssignmentFlightsApp.Models
{
    public class Flight
    {
        public string Code { get; set; }
        public string Airline { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Weekday { get; set; }
        public string Time { get; set; }
        public int Seats { get; set; }
        public double CostPerSeat { get; set; }

        public Flight(string code, string airline, string from, string to, string weekday, string time, int seats, double costPerSeat)
        {
            Code = code;
            Airline = airline;
            From = from;
            To = to;
            Weekday = weekday;
            Time = time;
            Seats = seats;
            CostPerSeat = costPerSeat;
        }

        public override string ToString()
        {
            return $"{Code}, {Airline}, {From}, {To}, {Weekday}, {Time}, {Seats}, {CostPerSeat}";
        }
        public class ReservationManager
        {
            
            private List<Reservation> reservations = new List<Reservation>();
            private string filePath = "reservations.csv"; // The path to the CSV file

            public ReservationManager()
            {
                // Define the path to wwwroot/Data within your application's directory structure
                string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string dataDirectory = Path.Combine(baseDirectory, "wwwroot", "Data");

                // Ensure the directory exists
                Directory.CreateDirectory(dataDirectory);

                // Set the full file path
                filePath = Path.Combine(dataDirectory, "reservations.csv");
            }

            public Reservation MakeReservation(Flight flight, string name, string citizenship)
            {
                try
                {
                    if (flight == null) throw new ArgumentNullException("No flight selected.");
                    if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name field is empty.");
                    if (string.IsNullOrWhiteSpace(citizenship)) throw new ArgumentException("Citizenship field is empty.");
                    if (flight.Seats <= 0) throw new InvalidOperationException("Flight is completely booked.");

                    // Decrease the available seat count
                    flight.Seats--;

                    // Generate a reservation code - simplistic approach
                    var reservationCode = $"R{new Random().Next(10000, 99999)}";
                    var reservation = new Reservation(reservationCode, flight, name, citizenship);

                    // Add to the local list and save the reservation to CSV
                    reservations.Add(reservation);
                    SaveReservationToCsv(reservation);

                    return reservation;
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    Console.WriteLine($"Error making reservation: {ex.Message}");
                    throw; // Optionally rethrow the exception to signal the error to calling code
                }
            }

            public void SaveReservationToCsv(Reservation reservation)
            {
                bool fileExists = File.Exists(filePath);
                using (var writer = new StreamWriter(filePath, append: true))
                {
                    if (!fileExists)
                    {
                        // Write the header if the file is new
                        writer.WriteLine("ReservationCode,FlightCode,Airline,Day,Time,Cost,Name,Citizenship,IsActive");
                    }

                    // Write the reservation data
                    writer.WriteLine($"{reservation.ReservationCode},{reservation.Code},{reservation.Airline},{reservation.Weekday},{reservation.Time},{reservation.CostPerSeat},{reservation.Name},{reservation.Citizenship},{reservation.IsActive}");
                }
            }
        }
    }
}
