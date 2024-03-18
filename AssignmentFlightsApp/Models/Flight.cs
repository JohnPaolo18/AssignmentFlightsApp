namespace AssignmentFlightsApp.Models
{
    public class Flight
    {
        public string FlightCode { get; set; }
        public string Airline { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Weekday { get; set; }
        public string Time { get; set; }
        public int Seats { get; set; }
        public double CostPerSeat { get; set; }

        // Constructor
        public Flight(string flightCode, string airline, string from, string to, string weekday, string time, int seats, double costPerSeat)
        {
            FlightCode = flightCode;
            Airline = airline;
            From = from;
            To = to;
            Weekday = weekday;
            Time = time;
            Seats = seats;
            CostPerSeat = costPerSeat;
        }

    }
}
