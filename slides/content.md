# Value Objects
## on steroids

- <i class="fa fa-user"></i> Patrick Drechsler
- <i class="fa fa-calendar" aria-hidden="true"></i> 25.06.2018
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

```csharp
[Fact]
public void Kontostand_ist_nach_Einzahlung_groesser_als_davor()
{
    var konto = new Konto();
    var before = konto.Kontostand;
    konto.Einzahlen(-10); // <---------------------- AUTSCH!
    konto.Kontostand.Should().BeGreaterThan(before);
}
```

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
    public int Betrag { get; }

    public Geld(int betrag) 
    {
        if (!IsValid(betrag))
        {
            throw new InvalidGeldException(betrag.ToString());
        }
        this.Betrag = betrag;
    }

    private bool IsValid(int betrag) => betrag >= 0;
}
```
- Wert kann nicht verändert werden<!-- .element: class="fragment" data-fragment-index="1" -->
- Es kann nur gültige Geld-Objekte geben<!-- .element: class="fragment" data-fragment-index="1" -->

**Immutability**<!-- .element: class="fragment" data-fragment-index="2" -->

Note:
- schauen wir nochmal auf unsere Konto-Klasse...

x---

## Immutability

```csharp
public class Konto
{
    public Geld Kontostand { get; private set; } = new Geld(0);

    public void Einzahlen(Geld geld) 
    { 
        // Kontostand ist immer ein neues, gültiges Objekt
        Kontostand = new Geld(Kontostand.Betrag + geld.Betrag);
    }
}
```

Note:
Einzahlung kann ungültigen Wert liefern: Dazu kommen wir spaeter!

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

x---

Geld ist mehr als nur Betrag

`1EUR != 1USD != 1BC`

Note: TODO Replace with images of coins/bills/bitcoin-logo

x---

Fügen wir 
- **Währung** 
- zur Klasse **Geld** hinzu...

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

TODO Geld gleich Geld: schlecht

x--
### Equality by identifier

![noborder-equality-by-identifier](resources/eq2.png)

TODO Geld gleich Geld: schlecht

x--
### Equality by structure

![noborder-equality-by-structure](resources/eq3.png)

TODO Geld gleich Geld: Volltreffer!

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
}
```
```csharp
[Fact]
public void Geld_ist_gleich_Geld()
{
    var geld1 = new Geld(1, Waehrung.EUR);
    var geld2 = new Geld(1, Waehrung.EUR);

    geld1.Should().BeEqual(geld2); // <-- greenh
}
```

Korrekte Vergleichbarkeit!

x---

### "Geld" ist jetzt stabil

- "Geld" ist nicht im nachhinein änderbar
- "Geld" ist vergleichbar

x---

## Value Object
- Ausdrucksstärke: 
    - Expressiveness
- Unveränderlichbarkeit: 
    - Immutability
- Attributbasiertevergleichbarkeit: 
    - Equality by structure

x---

## Es geht nicht nur ums Geld...

x---

- TODO Erst zeigen, wie Abstract VO geht
- TODO ODER Ganz viel Logik (on steroids) innerhalb eines VO

x---

Geld ist nicht alles!

```csharp
public class Konto
{
    public Geld Kontostand { /* ... */ };
    public Email KontaktEmail { /* ... */ }
}
```

x---

```csharp
public class Kunde
{
    public Email KontaktEmail { get; set; }
}
```

```csharp
public class Email : ValueObject<Email>
{
    public string Value { get; }
    public Email(string input)
    {
        if (!IsValid(input)) {
            throw new Exception(input)
        }
        Value = input;
    }
    private bool IsValid(string input) => true;
}
```

Wo kommt die `ValueObject<T>` Klasse her?