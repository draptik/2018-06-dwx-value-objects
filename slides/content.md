# Value Objects
## on steroids

- <i class="fa fa-user"></i> Patrick Drechsler
- <i class="fa fa-calendar" aria-hidden="true"></i> DWX: 25.06.2018
- <i class="fa fa-twitter" aria-hidden="true"></i> @drechsler
- <i class="fa fa-github" aria-hidden="true"></i> github.com/draptik

x---

#### Patrick Drechsler

- "gelernter" Biologe
- C# Entwickler bei Redheads Ltd.
- aktuelle Schwerpunkte: DDD, FP, Cloud
- Softwerkskammer

Note: 
Wo geht die Reise hin?

- Was ist ein Value Object?
- Wie nutzt man Value Objects?
- Fallstricke
- Killer Features // <- In die Liste aufnehmen?
- On Steroids

x---

## Domain Driven Design

![noborder-ddd-books](resources/ddd-books.png)

Note: 
- Alle Buecher (bis auf Distilled) > 500Seiten
- schließt die Kluft zwischen
    - **Fachlichkeit** (Kunden-Vokabular) und 
    - **techn. Implementierung** (IT-Vokabular)

x---

```csharp
public class Konto
{
    public int Kontostand { get; private set; } = 0;

    public void Einzahlen(int geld) 
    { 
        Kontostand += geld;
    }
}
```

<pre>
<code data-noescape data-trim class="lang-csharp hljs fragment" data-fragment-index="1">
[Fact]
public void Kontostand_ist_nach_Einzahlung_groesser_als_davor()
{
    var konto = new Konto();
    var before = konto.Kontostand;
    konto.Einzahlen(-10); // <---------------------- AUTSCH!
    konto.Kontostand.Should().BeGreaterThan(before);
}
</code>
</pre>

x---

- Problem: Geld ist kein Integer!
- Willkommen in der Welt der Antipattern...<!-- .element: class="fragment" data-fragment-index="1" -->

**Primitive Obsession**<!-- .element: class="fragment" data-fragment-index="2" -->

*Like most other [code] smells, primitive obsessions are born in moments of weakness. "Just a field for storing some data!" the programmer said. Creating a primitive field is so much easier than **making a whole new class**, right?*<!-- .element: class="fragment" data-fragment-index="2" -->

<span class="small fragment" data-fragment-index="2"> https://sourcemaking.com/refactoring/smells/primitive-obsession</span> 

Note:
- Im Zitat steckt schon ein Lösungsansatz ('making a new class')
- Outside-In Approach: Aendern wir zuerst alle Signaturen in der Konto-Klasse...

x---

```csharp
public class Konto
{
    public Geld Kontostand { get; private set; } = new Geld(0);

    public void Einzahlen(Geld geld) 
    { 
        Kontostand = new Geld(Kontostand.Betrag + geld.Betrag);
    }
}
```
Geld Klasse existiert noch nicht...<!-- .element: class="fragment" data-fragment-index="1" -->

x---

```csharp
public class Geld
{
    public int Betrag { get; }                       // <- 1

    public Geld(int betrag)                          // <- 2
    {
        if (!IsValid(betrag))                        // <- 3
        {
            throw new InvalidGeldException(betrag.ToString());
        }

        this.Betrag = betrag;                        // <- 2'
    }

    private bool IsValid(int betrag) => betrag >= 0; // <- 3'
}
```
- Es kann nur gültige Geld-Objekte geben<!-- .element: class="fragment" data-fragment-index="1" -->
- Wert kann nicht verändert werden<!-- .element: class="fragment" data-fragment-index="1" -->


Note:
- schauen wir nochmal auf unsere Konto-Klasse...

x---

## Immutability

- einfacher zu Erstellen & Testen
- keine Seiteneffekte
- keine Null References
- Thread Safe
- verhindert Temporal Coupling


x--

```csharp
[Fact]
public void Geld_schmeisst_wenn_Betrag_zu_gross()
{
    var max = Int32.MaxValue;
    
    Action action = () => new Geld(max + 1);
    
    action.Should().Throw<InvalidGeldException>();
}
```

vernünftige Exception!

(bessere Implemtierung folgt)

x--

### Mehr Verhalten in die Geld-Klasse packen

#### (Preview)

```csharp
public class Geld
{
    public int Betrag { get; }

    // ...

    public Geld Add(Geld geld)
    {
        try 
        {
            return new Geld(this.Betrag + geld.Betrag);
        }
        catch 
        {
            throw new InvalidGeldException(
                $"Ups. Can't add {geld.Betrag} to {Betrag}!");
        }
    }
}
```

x--

```csharp
[Fact]
public void Geld_laesst_sich_addieren()
{
    var geldEins = new Geld(1);
    var geldZwei = new Geld(10);

    geldEins
        .Add(geldZwei).Betrag
        .Should()
        .Be(11);
}
```

x---

Geld ist mehr als nur Betrag

```csharp
public class UeberweisungsService
{
    public void Ueberweise(
        string kontoSender, 
        string kontoEmpfaenger, 
        int geld)
    {
        // ...
    }
}


```

![noborder-currencies](resources/currencies.png)<!-- .element: class="fragment" data-fragment-index="1" -->

x---

Fügen wir **Währung** zur Klasse **Geld** hinzu...

x---

```csharp
public class Geld
{
    public int Betrag { get; }
    public Waehrung Waehrung { get; } // <---------------- NEU

    public Geld(int betrag, Waehrung waehrung) 
    {
        if (!IsValid(betrag, waehrung))
        {
            throw new InvalidGeldException();
        }
        this.Betrag = betrag;
        this.Waehrung = waehrung; // <-------------------- NEU
    }

    private bool IsValid(int betrag, Waehrung waehrung) 
        => betrag >= 0 && waehrung != Waehrung.Undefined;
}
```
- kann nicht verändert werden
- ungültige Geld Objekte gibt es nicht

Note: Waehrung ist eine Enum
TODO Example Code

x---

```csharp
[Fact]
public void Geld_ist_gleich_Geld()
{
    var geld1 = new Geld(1, Waehrung.EUR);
    var geld2 = new Geld(1, Waehrung.EUR);

    geld1.Should().BeEqualTo(geld2); // <-- Fails!
}
```

AUTSCH!

Da müssen wir was machen

x---

## Exkurs: Vergleichbarkeit

"`Equal`"

x--
### Equality by reference

![noborder-equality-by-reference](resources/eq1.png)

"Geld gleich Geld" <i class="fa fa-thumbs-o-down" style="color: red; padding-left:10px"></i>

x--
### Equality by identifier

![noborder-equality-by-identifier](resources/eq2.png)

"Geld gleich Geld" <i class="fa fa-thumbs-o-down" style="color: red; padding-left:10px"></i>

x--
### Equality by structure

![noborder-equality-by-structure](resources/eq3.png)

"Geld gleich Geld" <i class="fa fa-thumbs-o-up" style="color: green; padding-left:10px"></i>

x---

```csharp
public class Geld
{
    // ...
    public override bool Equals(Geld other) 
    {
        return
            other.Betrag == this.Betrag &&
            other.Waehrung == this.Waehrung;
    }

    public override int GetHashCode()
    {
        var hash = 17;
        hash = hash * 31 + (Betrag == null ? 0 : Betrag.GetHashCode());
        hash = hash * 31 + (Waehrung == null ? 0 : Waehrung.GetHashCode());
        return hash;
    }
}
```
```csharp
[Fact]
public void Geld_ist_gleich_Geld()
{
    var geld1 = new Geld(1, Waehrung.EUR);
    var geld2 = new Geld(1, Waehrung.EUR);
    geld1.Should().BeEqual(geld2); // <-- green
}
```

Korrekte Vergleichbarkeit!

x---

### "Geld" ist jetzt stabil

- "Geld" ist nicht im nachhinein änderbar
- "Geld" ist vergleichbar

x---

und ganz nebenbei haben wir zu Fuß ein 

### Value Object 

erstellt

x---

## Value Object

- Ausdrucksstark (*"Expressiveness"*)
    - Methodensignaturen sind verständlich
- Unveränderlich (*"Immutability"*)
- Attributbasierte Vergleichbarkeit (*"Equality by structure"*)
- Logik ist da wo sie hingehört (*"Encapsulation"*)


x---

## DDD Jargon

- **Entity**: Objekt mit Lebenszyklus (Identität)
    - z.B. Kunde
- **Value Object**: Unveränderliches Objekt
    - z.B. Geld, Adresse, Email, ...

Entscheidung ist immer kontextabhängig!<!-- .element: class="fragment" data-fragment-index="1" -->

x---

```csharp
public class Konto
{
    public Geld Kontostand { /* ... */ };
    public Email KontaktEmail { /* ... */ } // <-- neues Value Object
}
```

x---

```csharp
public class Email : ValueObject<Email>
{
    public string Value { get; }
    
    public Email(string input)
    {
        if (!IsValid(input)) {
            throw new InvalidEmailException(input)
        }
        Value = input;
    }
    
    private bool IsValid(string input) => true;

    protected override IEnumerable<object> 
        GetAttributesToIncludeInEqualityCheck()
    {
        return new List<object> {Value};
    }
}
```

Note: Wo kommt die `ValueObject<T>` Klasse her?

x---

```csharp
public abstract class ValueObject<T> where T : ValueObject<T>
{
    public override bool Equals(object other)     // <-- 1
    {
        return Equals(other as T);
    }

    protected abstract IEnumerable<object> 
        GetAttributesToIncludeInEqualityCheck();  // <-- 2

    public bool Equals(T other)                   // <-- 3
    {
        if (other == null) return false;

        return GetAttributesToIncludeInEqualityCheck()
            .SequenceEqual(
                other.GetAttributesToIncludeInEqualityCheck());
    }

    public override int GetHashCode()             // <-- 4
    {
        var hash = 17;
        foreach (var obj in GetAttributesToIncludeInEqualityCheck())
            hash = hash * 31 + (obj == null ? 0 : obj.GetHashCode());

        return hash;
    }
}

```

x--

### Sugar: Operator overloading

```csharp
public abstract class ValueObject<T> where T : ValueObject<T>
{
    //...
    public static bool operator ==(ValueObject<T> left, 
                                   ValueObject<T> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject<T> left,
                                   ValueObject<T> right)
    {
        return !(left == right);
    }
}
```

x--

### More Sugar: implicit operator

```csharp
var email = new Email("foo@bar.baz");
string s = email.Value;                   // <-- nervt auf Dauer...
```

```csharp
public class Email : ValueObject<Email>
{
    public string Value { get; }

    //...
    public static implicit operator string(Email mail)
    {
        return mail.Value.ToString();
    }
}
```

```csharp
var email = new Email("foo@bar.baz");
string s = email;                         // <-- einfacher
```

x---

## Optionale Value Objects

```csharp
public class BahnKunde
{
    // Optional
    public BonusPunkte BonusPunkte { get; } = new BonusPunkte(null);
}
```

```csharp
public class BonusPunkte : ValueObject<BonusPunkte>
{
    public int Punkte { get; } = 0;

    // ...
    private bool IsValid(int? punkte)
    {
        return punkte == null 
            ? true 
            : punkte > 0;
    }
}
```

x---

## FAQ

- Ist das nicht schlecht für die Performance?
    - Ja, aber...<!-- .element: class="fragment" data-fragment-index="1" -->
- Wann sollte man statt eines Basistyps ein VO einsetzen?
    - Sobald Geschäftslogik im Spiel ist (z.B. Validierung)<!-- .element: class="fragment" data-fragment-index="2" -->
- Funktionieren VOs auch mit meinem OR-Mapper?<!-- .element: class="fragment" data-fragment-index="3" -->
- Wann sollte man VOs vermeiden?<!-- .element: class="fragment" data-fragment-index="4" -->
    - Bei Collections von VOs sollte man aufpassen

x---

## Fallstricke

- Collections
- ORM

x--

### Collections

- Don't do it
- Wenn doch:
    - Umdenken oder
    - Serialisieren

x--

### OR-Mapper

- Wenn moeglich die Domaenenlogik von der ORM Logik entkoppeln.
- Zur Not: 
    - EF kennt `ComplexType` Annotation
    - NHibernate kann auch mit VOs umgehen
    - Beides fuehrt zu technischem Code in der Domaenenlogik (`protected`, `virtual`, etc)


x---

# on Steroids

x---

TODO

Moegliche Beispiele:
- Email: IsCompanyMail
- Geld: Addition mit Wechselkurs?
- ReportingAmount von BMS Projekt (Add, Percentage, etc)

x--

### CompanyMail

```csharp
public class Customer 
{
    public Email Mail { get; set; }
    public CompanyEmail CompanyMail { get; set; }
}

public class CompanyEmail : Email // <-- Value Object
{ 
    // ...
    private bool IsValid(Email mail) 
        => mail.StartsWith("CompanyName");
}

public class SomeOtherClass 
{
    public void RegisterForInternalNewsletter(CompanyEmail mail) 
    { 
        /*...*/
    }
}
```

x---

## Money


x---

## Zusammenfassung

- Value Object: 
    - immer dann, wenn Basistyp und Businesslogik aufeinandertreffen
- Vorteil: Kleine Einheit (immutable) 
    - `->` verständlich
    - `->` weniger denken

Note: Fragen?

x---

- <i class="fa fa-twitter" aria-hidden="true"></i> @drechsler
- <i class="fa fa-github" aria-hidden="true"></i> github.com/draptik
- <i class="fa fa-envelope"></i> patrick.drechsler@redheads.de
