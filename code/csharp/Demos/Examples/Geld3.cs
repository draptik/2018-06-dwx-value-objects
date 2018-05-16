using System;
using Examples.Exceptions;

public class Geld3
{
    public int Value { get; }

    public Geld3(int betrag) 
    {
        if (!IsValid(betrag))
        {
            throw new InvalidGeld2ValueException(betrag.ToString());
        }
        this.Value = betrag;
    }
    
    public Geld3 Addiere(Geld3 geld)
    {
        return new Geld3(this.Value + geld.Value);
    }

    public Geld3 Subtrahiere(Geld3 geld)
    {
        if (this.Value - geld.Value < 0)
        {
            throw new InvalidGeld2ValueException("Wert darf nicht kleiner als Null sein.");
        }

        return new Geld3(this.Value - geld.Value);
    }

    
    private bool IsValid(int betrag) => betrag >= 0;
}
