namespace Examples
{
    public class Konto4
    {
        public Geld4 Kontostand { get; private set; } = new Geld4(0, Waehrung.EUR);

        public void Einzahlen(Geld4 geld)
        {
            this.Kontostand = Kontostand.Addiere(geld);
        }

        public Geld4 Abheben(Geld4 gewuenschterGeldbetrag)
        {
            this.Kontostand = Kontostand - gewuenschterGeldbetrag; // operator overloading
            return gewuenschterGeldbetrag;
        }

        public void Ueberweise(Geld4 geld, Konto4 empfaengerKonto)
        {
            var abgehobenesGeld = Abheben(geld);
            empfaengerKonto.Einzahlen(abgehobenesGeld);
        }
    }
}