using System;
using Examples;
using Examples.Exceptions;


namespace Examples.MoneyDemo.V4
{

    // TODO Replace int with decimal
    // TODO Add formatting (ToString), culture specific
    // TODO Add division and multiplication
    public class Geld
    {
        public int Value { get; }

        public Waehrung Waehrung { get; }

        public Geld(int betrag, Waehrung waehrung)
        {
            if (!IsValid(betrag, waehrung))
            {
                throw new InvalidGeld2ValueException(betrag.ToString());
            }
            this.Value = betrag;
        }

        public Geld Addiere(Geld geld)
        {
            if (this.Waehrung != geld.Waehrung)
            {
                throw new InvalidGeld2ValueException("Waehrungen stimmen nicht ueberein");
            }

            try
            {
                return new Geld(this.Value + geld.Value, this.Waehrung);
            }
            catch (System.Exception)
            {
                throw new InvalidGeld2ValueException("Wert ist zu gross (groesser als Int32.MaxValue).");
            }
        }

        public static Geld operator +(Geld g1, Geld g2) => g1.Addiere(g2);

        public Geld Subtrahiere(Geld geld)
        {
            if (this.Waehrung != geld.Waehrung)
            {
                throw new InvalidGeld2ValueException("Waehrungen stimmen nicht ueberein");
            }

            return new Geld(this.Value - geld.Value, this.Waehrung);
        }

        public static Geld operator -(Geld g1, Geld g2) => g1.Subtrahiere(g2);

        private bool IsValid(int betrag, Waehrung waehrung)
            => betrag >= 0 && waehrung != Waehrung.Undefined;
    }

}