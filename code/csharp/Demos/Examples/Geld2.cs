using Examples.Exceptions;

public class Geld2
{
    public int Value { get; }

    public Geld2(int betrag) 
    {
        if (!IsValid(betrag))
        {
            throw new InvalidGeld2ValueException(betrag.ToString());
        }
        this.Value = betrag;
    }

    private bool IsValid(int betrag) => betrag >= 0;
}
