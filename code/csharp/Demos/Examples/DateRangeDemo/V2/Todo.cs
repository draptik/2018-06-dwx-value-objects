using System;

namespace Examples.DateRangeDemo.V2
{
    public class Todo : IHaveErstelltAm
    {
        public string Name { get; set; }

        public DateTime ErstelltAm { get; set; }
    }
}