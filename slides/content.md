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
class Konto
{
    void Einzahlen(int betrag) { /* ... */ }
}

var konto = new Konto();
konto.Einzahlen(-10);
```
