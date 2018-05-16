using System;
using Examples;
using Examples.Exceptions;

// Jetzt mit Waehrung
public class Geld4
{
    public int Value { get; }

    public Waehrung Waehrung { get; }
    
    public Geld4(int betrag, Waehrung waehrung) 
    {
        if (!IsValid(betrag, waehrung))
        {
            throw new InvalidGeld2ValueException(betrag.ToString());
        }
        this.Value = betrag;
    }
    
    public Geld4 Addiere(Geld4 geld)
    {
        if (this.Waehrung != geld.Waehrung)
        {
            throw new InvalidGeld2ValueException("Waehrungen stimmen nicht ueberein");
        }

        try
        {
            return new Geld4(this.Value + geld.Value, this.Waehrung);
        }
        catch (System.Exception)
        {
            throw new InvalidGeld2ValueException("Wert ist zu gross (groesser als Int32.MaxValue).");
        }
    }

    public static Geld4 operator +(Geld4 g1, Geld4 g2)
    {
        return g1.Addiere(g2);
    }

    public Geld4 Subtrahiere(Geld4 geld)
    {
        if (this.Waehrung != geld.Waehrung)
        {
            throw new InvalidGeld2ValueException("Waehrungen stimmen nicht ueberein");
        }

        if (this.Value - geld.Value < 0)
        {
            throw new InvalidGeld2ValueException("Wert darf nicht kleiner als Null sein.");
        }

        return new Geld4(this.Value - geld.Value, this.Waehrung);
    }

    public static Geld4 operator -(Geld4 g1, Geld4 g2)
    {
        return g1.Subtrahiere(g2);
    }
    
    private bool IsValid(int betrag, Waehrung waehrung) 
        => betrag >= 0 && waehrung != Waehrung.Undefined;
}