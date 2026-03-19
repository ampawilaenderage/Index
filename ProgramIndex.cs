namespace Index
{
    internal class ProgramIndex
    {
        static void Main(string[] args)
        {
            int nr = 3;
            Belegschaft faGmbHCoKG = new Belegschaft();
            faGmbHCoKG.NeuerMitarbeiter("Hans Schulz", Abteilung.Buchhaltung, "12.04.1958");
            faGmbHCoKG.NeuerMitarbeiter("Anna Schmidt", Abteilung.Buchhaltung, "07.08.1974");
            faGmbHCoKG.NeuerMitarbeiter("Franz Meier", Abteilung.Vetrieb, "16.12.1958");
            faGmbHCoKG.NeuerMitarbeiter("Maria Müller", Abteilung.Vetrieb, "09.01.1998");
            faGmbHCoKG.NeuerMitarbeiter("Karl Ransauer", Abteilung.Entwicklung, "28.08.1964");
            faGmbHCoKG.NeuerMitarbeiter("Susanne Kiesling", Abteilung.Entwicklung, "20.11.2000");


            Console.WriteLine(
                $"[{nr}] Name: {faGmbHCoKG[nr].Name} " +
                $"(geb.: {faGmbHCoKG[nr].Geburtstag.ToShortDateString()}), " +
                $"Abteilung: {faGmbHCoKG[nr].Abteilung}"
            );

            // Step  April 12 birthdays
            Console.WriteLine("\nGeburtstagsliste 12.04.");
            foreach (Mitarbeiter ma in faGmbHCoKG[4, 12])
            {
                Console.WriteLine($"Name: {ma.Name} (geb.: {ma.Geburtstag.ToShortDateString()}), Abteilung: {ma.Abteilung}");
            }

            // Step  August birthdays
            Console.WriteLine("\nGeburtstagsliste August");
            foreach (Mitarbeiter ma in faGmbHCoKG[8, -1])
            {
                Console.WriteLine($"Name: {ma.Name} (geb.: {ma.Geburtstag.ToShortDateString()}), Abteilung: {ma.Abteilung}");

            }

            Console.Title = "C# Erweiterte Techniken - Übung Indexer";

            DatumZeitRechner heute = new DatumZeitRechner();

            // Part a
            Console.WriteLine($"Heute ist {heute["Datum"]}");
            Console.WriteLine($"Es ist jetzt {heute["Zeit"]}");

            // Part b
            Console.WriteLine($"Heute vor einem Jahr: {heute[DatumZeitRechner.Einheit.Jahr, -1]}");
            Console.WriteLine($"Heute in 2 Monaten: {heute[DatumZeitRechner.Einheit.Monat, 2]}");
            Console.WriteLine($"Heute vor 6 Wochen: {heute[DatumZeitRechner.Einheit.Woche, -6]}");
            Console.WriteLine($"Heute in 35 Tagen: {heute[DatumZeitRechner.Einheit.Tag, 35]}");
            Console.WriteLine($"Jetzt vor 3,5 Stunden: {heute[DatumZeitRechner.Einheit.Stunde, -3.5]}");
            Console.WriteLine($"Jetzt in 90 Minuten: {heute[DatumZeitRechner.Einheit.Minute, 90]}");
        }
        enum Abteilung { Vetrieb, Buchhaltung, Entwicklung }

        class Mitarbeiter
        {
            public string Name { get; private set; }
            public Abteilung Abteilung { get; private set; }
            public DateTime Geburtstag { get; private set; }
            public Mitarbeiter(string name, Abteilung abteilung, string geburtstag)
            {
                Name = name;
                Abteilung = abteilung;
                Geburtstag = DateTime.Parse(geburtstag);
            }

            public void Versetzen(Abteilung neueAbteilung)
            {
                Abteilung = neueAbteilung;
            }
        }

        class Belegschaft
        {
            private List<Mitarbeiter> mitarbeiterliste = new List<Mitarbeiter>();

            public void NeuerMitarbeiter(string name, Abteilung abteilung, string geburtstag)
            {
                mitarbeiterliste.Add(new Mitarbeiter(name, abteilung, geburtstag));
            }

            public Mitarbeiter this[int nr]
            {
                get
                {
                    return mitarbeiterliste[nr - 1]; // convert from 1-based to 0-based
                }
            }

            public List<Mitarbeiter> this[int monat, int tag]
            {
                get
                {
                    List<Mitarbeiter> geburtstagsliste = new List<Mitarbeiter>();

                    foreach (Mitarbeiter ma in mitarbeiterliste)
                    {
                        if ((tag == -1 || ma.Geburtstag.Day == tag)
                            && ma.Geburtstag.Month == monat)
                        {
                            geburtstagsliste.Add(ma);
                        }
                    }

                    return geburtstagsliste;
                }
            }
        }

        class DatumZeitRechner
        {
            // Indexer 1 string index ("Datum" or "Zeit")
            public string this[string index]
            {
                get
                {
                    switch (index.ToLower())
                    {
                        case "datum":
                            return DateTime.Today.ToLongDateString();

                        case "zeit":
                        default:
                            return DateTime.Now.ToLongTimeString();
                    }
                }
            }

            // Enum for time units
            public enum Einheit
            {
                Tag,
                Woche,
                Monat,
                Jahr,
                Stunde,
                Minute
            }

            // Indexer 2 unit + value
            public string this[Einheit einheit, double differenz]
            {
                get
                {
                    switch (einheit)
                    {
                        case Einheit.Jahr:
                            return DateTime.Today.AddYears((int)differenz).ToLongDateString();

                        case Einheit.Monat:
                            return DateTime.Today.AddMonths((int)differenz).ToLongDateString();

                        case Einheit.Woche:
                            return DateTime.Today.AddDays(differenz * 7).ToLongDateString();

                        case Einheit.Tag:
                            return DateTime.Today.AddDays(differenz).ToLongDateString();

                        case Einheit.Stunde:
                            return DateTime.Now.AddHours(differenz).ToShortTimeString();

                        case Einheit.Minute:
                            return DateTime.Now.AddMinutes(differenz).ToShortTimeString();

                        default:
                            return DateTime.Now.ToString();
                    }
                }
            }
        }
    }
}
