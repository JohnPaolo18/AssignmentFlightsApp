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

        //ROMERS PART
        public class Reservations
        {
            //The location of the reservation file.
            private static string Reservation_TXT = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\Resources\Files\reservation.csv");
            private static Random random = new Random();

            //reservation objects
            public static List<Reservation> reservations = new List<Reservation>();
            public static List<Reservation> FindReservations(string reservationCode, string airline, string name)
            {
                List<Reservation> found = new List<Reservation>();

                foreach (Reservation reservation in reservations)
                {
                    //A travel agent can search for an existing reservation that contains the specified reservation code, and/or airline and/or traveller’s full name.
                    if (reservation.Code.Contains(reservationCode) && reservation.Airline.Contains(airline) && reservation.Name.Contains(name))
                    {
                        found.Add(reservation);
                    }
                    else if (reservation.Code.Contains(reservationCode))
                    {
                        found.Add(reservation);
                    }
                    else if (reservation.Airline.Contains(airline))
                    {
                        found.Add(reservation);
                    }
                    else if (reservation.Name.Contains(name))
                    {
                        found.Add(reservation);
                    }

                    //If no matches are found, the list control is empty.
                    //If the user doesn’t enter any input, then all the reservations are displayed in the list.
                    if (found.Count == 0 && string.IsNullOrEmpty(reservationCode) && string.IsNullOrEmpty(airline) && string.IsNullOrEmpty(name))
                    {
                        found = reservations;
                    }
                }

                return found;
            }

            public string GenerateResCode()
            {
                return GenerateReservationCode();
            }

            /**
             * Gets reservation code using a flight.
             * @param flight Flight instance.
             * @return Reservation code.
             */
            public string GenerateReservationCode()
            {
                string reservationCode;

                do
                {
                    char letter = (char)('A' + random.Next(26));
                    string numbers = random.Next(1000, 10000).ToString();
                    reservationCode = letter + numbers;
                } while (IsCodeGenerated(reservationCode, Reservation_TXT));

                return reservationCode;
            }

            private static bool IsCodeGenerated(string reservationCode, string Reservation_TXT)
            {
                if (!File.Exists(reservationCode))
                {
                    return false;
                }

                List<string> existingCode = File.ReadAllLines(Reservation_TXT).ToList();

                return existingCode.Contains(reservationCode);
            }
            public static List<Reservation> GetReservations()
            {
                List<Reservation> res = new List<Reservation>();

                foreach (string line in File.ReadLines(Reservation_TXT))
                {
                    string[] parts = line.Split(",");
                    string reservationCode = parts[0];
                    string flightCode = parts[1];
                    string airline = parts[2];
                    double cost = double.Parse(parts[3]);
                    string name = parts[4];
                    string citizenship = parts[5];
                    string status = parts[6];

                    Reservation newReservation = new Reservation(reservationCode, flightCode, airline, cost, name, citizenship, status);
                    res.Add(newReservation);
                }

                reservations = res;
                return res;
            }
            public void AddReservation(Reservation res)
            {
                File.AppendAllText(Reservation_TXT, $"{res.Code},{res.FlightCode},{res.Airline},{res.Cost},{res.Name},{res.Citizenship},{res.Active}\n");
            }

        }
    }
}
