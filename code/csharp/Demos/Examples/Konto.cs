using System;

namespace Examples
{
    public class Konto
    {
        public int Kontostand { get; private set; } = 0;

        public void Einzahlen(int betrag) 
        { 
            this.Kontostand += betrag;
        }
    }
}
