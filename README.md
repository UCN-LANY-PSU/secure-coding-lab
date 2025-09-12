# Secure Coding Lab 1

**SQL Injection**

## Formål

At give praktisk erfaring med, hvordan usikker håndtering af brugerinput kan føre til SQL Injection, og hvordan man kan forebygge det gennem *parameteriserede forespørgsler* og gode designvalg.

---

## Opgave

1. I dette repository findes kodeeksempler i **Node.js/Express** og **ASP.NET Core**.
2. Start med at læse koden og identificér, hvorfor den er sårbar.
3. Ret koden, så den bruger **parameteriserede forespørgsler** i stedet for at bygge SQL-strings direkte.
4. Tilføj evt. en simpel inputvalidering (fx: søgeterm skal være 2–50 tegn).
5. Test jeres løsning:

   - Normal søgning (fx `Kill`).
   - Ondsindet input (fx `'; DELETE FROM books; --`).
   - Diskutér forskellen på output i usikker vs. sikker version.

---

## Output

- En fungerende version af koden med parameteriserede forespørgsler.
- En kort note (2–3 linjer) om, hvordan I sikrer, at problemet ikke opstår i jeres design fremover.

---

## Refleksionsspørgsmål

- Hvordan kunne denne sårbarhed have været undgået, hvis sikkerhed var tænkt ind i designfasen fra starten?
- Hvilke principper for *security by design* kan kobles til denne øvelse (fx *least privilege*, *fail securely*)?
- Hvorfor er parameteriserede forespørgsler en bedre løsning end at forsøge at filtrere “farlige” tegn fra input?

---

## Læringsmål (kobling til faget *Sikkerhed for udviklere*)

Efter øvelsen skal du kunne:

- **Identificere** en klassisk SQL Injection-sårbarhed i kode.
- **Implementere** en sikker løsning med parameteriserede forespørgsler.
- **Diskutere** hvordan usikkert design kan føre til alvorlige fejl.
- **Anvende** principper for *security by design* i dit eget arbejde.
