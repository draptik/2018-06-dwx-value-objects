namespace Examples
{
    public class Konto2
    {
        public Geld2 Kontostand { get; private set; } = new Geld2(0);

        public void Einzahlen(Geld2 geld)
        {
            this.Kontostand = new Geld2(this.Kontostand.Value + geld.Value);
        }

    }
}