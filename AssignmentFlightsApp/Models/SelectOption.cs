using System.Xml.Linq; // Ensure this namespace is necessary for your project

namespace AssignmentFlightsApp.Models
{
    public class SelectOption
    {
        public string ArrivalAirport { get; set; }
        public string DepartureAirport { get; set; }
        public string Weekday { get; set; }
        public string Code { get; set; } // Ensure these properties are used appropriately
        public string Airline { get; set; }
        public string Time { get; set; }
        public double CostPerSeat { get; set; }
    }

    public class Reservation
    {
        private Flight flight;

        public string FlightCode { get; set; }
        public string ReservationCode { get; set; }
        public string Code { get; set; }
        public string Airline { get; set; }
        public string Weekday { get; set; }
        public string Time { get; set; }
        public double Cost { get; set; }
        public double CostPerSeat { get; set; }
        public string Name { get; set; }
        public string Citizenship { get; set; }
        public string Status { get; set; }
        public object Active { get; internal set; }
        public bool IsActive { get; set; } = true;

        // Correctly placed constructor
        public Reservation(string reservationCode, Flight flight, string name, string citizenship, double cost, string code, string flightCode, string airline, string status)
        {
            ReservationCode = reservationCode;
            Code = flight.Code;
            Airline = flight.Airline;
            Weekday = flight.Weekday;
            Time = flight.Time;
            FlightCode = flightCode;
            Cost = cost;
            Status = status;
            Airline = airline;
            CostPerSeat = flight.CostPerSeat;
            Name = name;
            Citizenship = citizenship;
            IsActive = true; // Assuming a new reservation is always active initially
        }

        public Reservation(string code, string flightCode, string airline, double cost, string name, string citizenship, string status)
        {
            Code = code;
            FlightCode = flightCode;
            Airline = airline;
            Cost = cost;
            Name = name;
            Citizenship = citizenship;
            Status = status;
        }

        public Reservation(string reservationCode, Flight flight, string name, string citizenship)
        {
            ReservationCode = reservationCode;
            this.flight = flight;
            Name = name;
            Citizenship = citizenship;
        }
    }


}
