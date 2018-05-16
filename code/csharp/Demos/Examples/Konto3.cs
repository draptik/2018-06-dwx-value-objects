namespace Examples
{
    public class Konto3
    {
        public Geld3 Kontostand { get; private set; } = new Geld3(0);

        public void Einzahlen(Geld3 geld)
        {
            this.Kontostand = Kontostand.Addiere(geld);
        }

        public Geld3 Abheben(Geld3 gewuenschterGeldbetrag)
        {
            this.Kontostand = Kontostand.Subtrahiere(gewuenschterGeldbetrag);
            return gewuenschterGeldbetrag;
        }

        public void Ueberweise(Geld3 geld, Konto3 empfaengerKonto)
        {
            this.Kontostand.Subtrahiere(geld);
            empfaengerKonto.Einzahlen(geld);
        }
    }
}