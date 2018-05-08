using System;

namespace Examples
{
    public class Konto1
    {
        public int Kontostand { get; private set; } = 0;

        public void Einzahlen(int geld) 
        { 
            this.Kontostand += geld;
        }
    }
}
