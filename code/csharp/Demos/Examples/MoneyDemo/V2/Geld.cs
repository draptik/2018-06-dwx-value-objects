using Examples.Exceptions;

namespace Examples.MoneyDemo.V2
{
    public class Geld
    {
        public int Value { get; }

        public Geld(int betrag)
        {
            if (!IsValid(betrag))
            {
                throw new InvalidGeld2ValueException(betrag.ToString());
            }
            this.Value = betrag;
        }

        private bool IsValid(int betrag) => betrag >= 0;
    }
}