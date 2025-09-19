# Secure Coding Lab 1

**SQL Injection**

---

## Formål

At give praktisk erfaring med, hvordan usikker håndtering af brugerinput kan føre til SQL Injection, og hvordan man kan forebygge det gennem *parameteriserede forespørgsler* og gode designvalg.  

Øvelsen understøtter læringsmålene i faget *Sikkerhed for udviklere*, hvor de studerende skal kunne:

- Identificere og håndtere sårbarheder i programmer  
- Anvende security design principles inkl. *security by design*  
- Sikkerhedsvurdere og teste softwarearkitekturer  

---

## Opgave

1. I dette repository findes kodeeksempler i **Node.js/Express** og **ASP.NET Core**.  
2. Koden genererer en færdig database (`books.db`). Hvis i ødelægger den, skal den bare slettes, så oprettes en ny ved næste kørsel.
3. Start med at læse koden og identificér, hvorfor den er sårbar.  
4. Ret koden, så den bruger **parameteriserede forespørgsler** i stedet for at bygge SQL-strings direkte.  
5. Tilføj evt. en simpel inputvalidering (fx: søgeterm skal være 2–50 tegn).  
6. Test jeres løsning:  
   - Normal søgning (fx `Kill`)  
   - Tautologisk input (fx `' OR '1'='1' --`)  
   - UNION-angreb (fx `' UNION ALL SELECT username, password FROM users --`)  
   - Diskutér forskellen på output i usikker vs. sikker version  

---

## Output

- En fungerende version af koden med parameteriserede forespørgsler  
- En kort note (2–3 linjer) om, hvordan I sikrer, at problemet ikke opstår i jeres design fremover  
- En forklaring på, hvorfor parameterisering beskytter mod SQL Injection  

---

## Refleksionsspørgsmål

- Hvordan kunne denne sårbarhed have været undgået, hvis sikkerhed var tænkt ind i designfasen fra starten?  
- Hvilke principper for *security by design* kan kobles til denne øvelse (fx *least privilege*, *fail securely*)?  
- Hvorfor er parameteriserede forespørgsler en bedre løsning end at forsøge at filtrere “farlige” tegn fra input?  
- Hvordan er brugen af string-concatenation i SQL et eksempel på *insecure design*?  

---

## Læringsmål (kobling til faget *Sikkerhed for udviklere*)

Efter øvelsen skal du kunne:

- **Identificere** en klassisk SQL Injection-sårbarhed i kode  
- **Implementere** en sikker løsning med parameteriserede forespørgsler  
- **Diskutere** hvordan usikkert design kan føre til alvorlige fejl  
- **Anvende** principper for *security by design* i dit eget arbejde  

---

## Sådan kører du eksemplerne

### A. Node.js/Express



### B. ASP.NET Core (Minimal API)

1. Byg image

```bash
docker build -t secure-coding-lab-aspnet .
```

2. Kør container

```bash
docker run --rm -p 8080:8080 secure-coding-lab-aspnet
```

3. Test

```bash

# Sikker route (parameteriseret)
curl -k "http://localhost:8080/search?term=Kill"

# (Hvis udkommenteret i koden) Usikker route:
curl -k "http://localhost:8080/search-insecure?term='%20UNION%20ALL%20SELECT%20username%2C%20password%20FROM%20users%20--%20"

```




## Cheat Sheet: Typiske SQL Injection Payloads

Disse payloads kan bruges i den **usikre version** af koden.  
*(OBS: Den sikre version med parameterisering er immun over for dem.)*

| Payload | Effekt |
|---------|--------|
| `' OR '1'='1' --` | Tautologi: returnerer alle rækker fra tabellen |
| `' UNION ALL SELECT username, password FROM users --` | UNION-angreb: eksfiltrerer data fra `users`-tabellen |
| `%' UNION SELECT 'X','Y' --` | Tester kolonneantal ved at tilføje dummy-værdier |
| `'; DELETE FROM books; --` | Forsøg på destruktiv injection (virker kun med drivere, der tillader flere statements) |

👉 Husk: **Parameterisering** betyder, at input behandles som *data* og aldrig kan ændre SQL-strukturen.

