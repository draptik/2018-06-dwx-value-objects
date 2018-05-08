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

x---

## Wo geht die Reise hin?

- Was ist ein Value Object?
- Wie nutzt man Value Objects?
- Fallstricke
- Killer Features // <- In die Liste aufnehmen?
- On Steroids

Note: Diese Folie zeigen?

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
        this.Kontostand += geld;
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

x---

```csharp
public class Konto
{
    public Geld Kontostand { get; private set; } = new Geld(0);

    public void Einzahlen(Geld geld) 
    { 
        this.Kontostand = new Geld(this.Kontostand.Value + geld.Value);
    }
}
```
Wie sieht die Klasse Geld aus?

x---

```csharp
public class Geld
{
    public int Value { get; }

    public Geld(int betrag) 
    {
        if (!IsValid(betrag))
        {
            throw new InvalidGeldException(betrag.ToString());
        }
        this.Value = geld;
    }

    private bool IsValid(int betrag) => betrag >= 0;
}
```
- Wert kann nicht verändert werden<!-- .element: class="fragment" data-fragment-index="1" -->
- Es kann nur gültige Geld-Objekte geben<!-- .element: class="fragment" data-fragment-index="1" -->

**Immutability**<!-- .element: class="fragment" data-fragment-index="2" -->
