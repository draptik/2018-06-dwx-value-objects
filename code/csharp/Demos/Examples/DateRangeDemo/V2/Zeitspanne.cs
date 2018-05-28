using System;

namespace Examples.DateRangeDemo.V2
{
    public class Zeitspanne
    {
        public DateTime From { get; }
        public DateTime To { get; }

        public Zeitspanne(DateTime from, DateTime to)
        {
            if (!IsValid(from, to))
            {
                throw new InvalidDateRangeException();
            }
            
            From = from;
            To = to;
        }

        private bool IsValid(DateTime from, DateTime to) => from < to;

        public bool Umfasst<THasErstelltAm>(THasErstelltAm o) 
            where THasErstelltAm : IHaveErstelltAm  => 
            o.ErstelltAm >= From && o.ErstelltAm <= To;
    }
}