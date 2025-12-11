using System.Xml.Serialization;

namespace VereinsVerwaltung;

public class Controller
{
    #region Eigenschaften
    private UserInterface _interface;
    private Mannschaft _Mannschaft;
    private MannschaftsMitglied? _eingeloggtesMitglied;
    #endregion

    #region Assessoren/Modifikatoren
    private UserInterface Interface { get => _interface; set => _interface = value; }
    private MannschaftsMitglied? EingeloggtesMitglied { get => _eingeloggtesMitglied; set => _eingeloggtesMitglied = value; }
    private List<MannschaftsMitglied> Mitglieder => _Mannschaft.GetMitglieder();
    private Mannschaft AktuelleMannschaft { get => _Mannschaft; set => _Mannschaft = value; }
    #endregion

    #region Konstruktor
    public Controller()
    {
        _interface = new UserInterface();
        if (Path.Exists("Mitglieder"))
            _Mannschaft = new Mannschaft("Mitglieder");
        else 
            _Mannschaft = new Mannschaft();
        _eingeloggtesMitglied = null;

        if (File.Exists("Mitglieder/eingeloggt.xml"))
        {
            // Eingeloggtes Mitglied aus XML-Datei laden
            StreamReader reader = new StreamReader("Mitglieder/eingeloggt.xml");
            string xmlInhalt = reader.ReadToEnd();
            reader.Close();

            // Deserialisierung des eingeloggten Mitglieds
            XmlSerializer serializer = new XmlSerializer(typeof(MannschaftsMitglied), new Type[] { typeof(Trainer), typeof(Spieler), typeof(Betreuer) });
            EingeloggtesMitglied = (MannschaftsMitglied?)serializer.Deserialize(new StringReader(xmlInhalt));
        }
    }
    #endregion

    #region Methoden
    public void Start()
    {
        #region Lokale Variablen
        bool wiederholen = true;
        bool beendenEingellogt = false;
        #endregion

        Interface.CursorVisible = false;

        do
        {
            // Überprüfen ob ein Mitglied eingeloggt ist.
            if (!(EingeloggtesMitglied != null && EingeloggtesMitglied.IstEingeloggt))
            {
                Login();
                continue;
            }

            // Hauptmenü anzeigen
            if (EingeloggtesMitglied is Trainer)
            {
                // Hauptmenü für Trainer
                Interface.AnzeigeHauptmenueTrainer(EingeloggtesMitglied.Vorname + " " + EingeloggtesMitglied.Nachname);

                switch (Interface.Pressed.KeyChar)
                {
                    case '1':
                        AlleSpielerPaesseAnzeigen();
                        break;

                    case '2':
                        // Spieler Verwalten
                        SpielerVerwalten();
                        break;

                    case '3':
                        // Nutzernamen ändern
                        NutzernamenAendern();
                        break;

                    case '4':
                        // Datenlöschung beantragen
                        DatenloeschungBeantragen();
                        break;

                    case '5':
                        // Ausloggen
                        beendenEingellogt = false;
                        wiederholen = false;
                        break;

                    case '6':
                        // Programm beenden
                        wiederholen = false;
                        beendenEingellogt = true;
                        break;
                }
            }
            else if (EingeloggtesMitglied is Spieler spieler)
            {
                // Hauptmenü für Spieler
                Interface.AnzeigeHauptmenueSpieler(spieler.Vorname + " " + spieler.Nachname);
                
                switch (Interface.Pressed.KeyChar)
                {
                    case '1':
                        // Spielerpass anzeigen
                        Interface.EigenenSpielerpassAnzeigen(spieler);
                        break;
                    case '2':
                        // Nutzernamen ändern
                        NutzernamenAendern();
                        break;
                    case '3':
                        // Datenlöschung beantragen
                        DatenloeschungBeantragen();
                        break;
                    case '4':
                        // Ausloggen
                        beendenEingellogt = false;
                        wiederholen = false;
                        break;
                    case '5':
                        // Eingeloggt beenden
                        wiederholen = false;
                        beendenEingellogt = true;
                        break;
                }
            }
            else if (EingeloggtesMitglied is Betreuer betreuer)
            {
                // Hauptmenü für Betreuer
                Interface.AnzeigeHauptmenueBetreuer(betreuer.Vorname + " " + betreuer.Nachname);
                
                switch (Interface.Pressed.KeyChar)
                {
                    case '1':
                        // Spieler behandeln
                        SpielerBehandeln(betreuer);
                        break;
                    case '2':
                        // Behandlungsstatistik anzeigen
                        BehandlungsstatistikAnzeigen(betreuer);
                        break;
                    case '3':
                        // Nutzernamen ändern
                        NutzernamenAendern();
                        break;
                    case '4':
                        // Datenlöschung beantragen
                        DatenloeschungBeantragen();
                        break;
                    case '5':
                        // Spielerliste Anzeigen
                        AlleSpielerPaesseAnzeigen();
                        break;
                    case '6':
                        // Ausloggen
                        beendenEingellogt = false;
                        wiederholen = false;
                        break;

                    case '7':
                        // Eingeloggt beenden
                        wiederholen = false;
                        beendenEingellogt = true;
                        break;

                }
            }
            else
            {
                // Theoretisch nie erreichbar - aber für den Fall der Fälle
                Interface.Color = ConsoleColor.Red;
                Interface.WriteLine("Unbekannter Mitgliedstyp! Bitte kontaktieren Sie den Administrator.");
                Interface.ResetColor();
                Interface.WaitOrKey(3000);
                Interface.WriteLine("Beenden der Sitzung mit Enter?");
                while (Interface.GetKey().Key != ConsoleKey.Enter) ;
                beendenEingellogt = false;
                wiederholen = false;
            }
        } while (wiederholen);

        if (!beendenEingellogt)
        {
            // Mitglied ausloggen
            if (EingeloggtesMitglied != null)
            {
                EingeloggtesMitglied.Ausloggen();
                EingeloggtesMitglied = null;
                if (File.Exists("Mitglieder/eingeloggt.xml"))
                    File.Delete("Mitglieder/eingeloggt.xml");
            }
        }
        else
        {
            AktuelleMannschaft.EingeloggtesMitgliedSpeichern(EingeloggtesMitglied);
        }

        AktuelleMannschaft.MitgliederSpeichern();

        Interface.Goodbye(beendenEingellogt);
        Interface.WaitOrKey(3000);
    }
    
    /// <summary>
    /// Zeigt alle Spielerpässe an (Trainer-Funktion).
    /// </summary>
    private void AlleSpielerPaesseAnzeigen()
    {
        List<Spieler> spielerListe = new List<Spieler>();
        
        // Alle Spieler aus der Mitgliederliste filtern
        Interface.Clear();
        foreach (var mitglied in Mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        Interface.AnzeigeAlleSpielerPässe(spielerListe);
    }

    /// <summary>
    /// Verwaltungsmenü für Spieler (Trainer-Funktion).
    /// Ermöglicht Hinzufügen, Bearbeiten und Löschen von Spielern.
    /// </summary>
    private void SpielerVerwalten()
    {
        bool zurueck = false;
        
        do
        {
            // Verwaltungsmenü anzeigen
            Interface.Clear();
            Interface.Color = ConsoleColor.Cyan;
            Interface.WriteLine("=== Spieler Verwalten ===");
            Interface.ResetColor();
            Interface.WriteLine();
            Interface.WriteLine("1. Neuen Spieler hinzufügen");
            Interface.WriteLine("2. Spieler bearbeiten");
            Interface.WriteLine("3. Spieler löschen");
            Interface.WriteLine("4. Alle Spieler infos anzeigen");
            Interface.WriteLine("5. Zurück zum Hauptmenü");
            Interface.WriteLine();
            Interface.Write("Bitte wählen Sie eine Option: ");
            
            Interface.GetKey();
            
            switch (Interface.Pressed.KeyChar)
            {
                case '1':
                    SpielerHinzufuegen();
                    break;
                case '2':
                    SpielerBearbeiten();
                    break;
                case '3':
                    SpielerLoeschen();
                    break;
                case '4':
                    AlleSpielerPaesseAnzeigen();
                    break;
                case '5':
                    zurueck = true;
                    break;
                default:
                    Interface.Color = ConsoleColor.Red;
                    Interface.WriteLine("\n\nUngültige Eingabe!");
                    Interface.ResetColor();
                    Interface.WaitOrKey(1500);
                    break;
            }
        } while (!zurueck);
    }

    /// <summary>
    /// Fügt einen neuen Spieler zur Mitgliederliste hinzu.
    /// </summary>
    private void SpielerHinzufuegen()
    {
        Interface.Clear();
        Interface.Color = ConsoleColor.Cyan;
        Interface.WriteLine("=== Neuen Spieler hinzufügen ===");
        Interface.ResetColor();
        Interface.WriteLine();
        
        Interface.CursorVisible = true;
        
        // Benutzername eingeben
        Interface.Write("Username: ");
        string username = Interface.ReadLine();
        
        // Prüfe ob Username bereits existiert
        foreach (var mitglied in Mitglieder)
        {
            if (mitglied.UserName == username)
            {
                Interface.Color = ConsoleColor.Red;
                Interface.WriteLine("\nUsername existiert bereits!");
                Interface.ResetColor();
                Interface.CursorVisible = false;
                Interface.WaitOrKey(2000);
                return;
            }
        }
        
        // Vorname eingeben
        Interface.Write("Vorname: ");
        string vorname = Interface.ReadLine();
        
        // Nachname eingeben
        Interface.Write("Nachname: ");
        string nachname = Interface.ReadLine();
        
        // Passwort eingeben
        Interface.Write("Passwort: ");
        string passwort = Verschlüsselung.Ver(Interface.ReadLine());
        
        // Nationalität eingeben
        Interface.Write("Nationalität: ");
        string nationalitaet = Interface.ReadLine();
        
        Interface.CursorVisible = false;
        
        // Neuen Spieler erstellen
        Spieler neuerSpieler = new Spieler(username, vorname, nachname, passwort, 
            new Spielerpass(0, 0, nationalitaet));
        
        // Zur Liste hinzufügen
        AktuelleMannschaft.Add(neuerSpieler);
        
        Interface.Color = ConsoleColor.Green;
        Interface.WriteLine("\nSpieler erfolgreich hinzugefügt!");
        Interface.ResetColor();
        Interface.WaitOrKey(2000);
    }

    /// <summary>
    /// Bearbeitet die Daten eines vorhandenen Spielers.
    /// </summary>
    private void SpielerBearbeiten()
    {
        // Alle Spieler sammeln
        List<Spieler> spielerListe = new List<Spieler>();
        foreach (var mitglied in Mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        if (spielerListe.Count == 0)
        {
            Interface.Clear();
            Interface.Color = ConsoleColor.Yellow;
            Interface.WriteLine("Keine Spieler vorhanden!");
            Interface.ResetColor();
            Interface.WaitOrKey(2000);
            return;
        }
        
        // Spieler auswählen
        Interface.Clear();
        Interface.Color = ConsoleColor.Cyan;
        Interface.WriteLine("=== Spieler bearbeiten ===");
        Interface.ResetColor();
        Interface.WriteLine();
        
        for (int i = 0; i < spielerListe.Count; i++)
        {
            Interface.WriteLine($"{i + 1}. {spielerListe[i].Vorname} {spielerListe[i].Nachname} ({spielerListe[i].UserName})");
        }
        
        Interface.WriteLine();
        Interface.Write("Wählen Sie einen Spieler (Nummer): ");
        
        if (int.TryParse(Interface.ReadLine(), out int auswahl) && auswahl > 0 && auswahl <= spielerListe.Count)
        {
            Spieler ausgewaehlterSpieler = spielerListe[auswahl - 1];
            
            // Bearbeitungsoptionen
            Interface.Clear();
            Interface.Color = ConsoleColor.Cyan;
            Interface.WriteLine($"=== {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname} bearbeiten ===");
            Interface.ResetColor();
            Interface.WriteLine();
            Interface.WriteLine("1. Tore aktualisieren");
            Interface.WriteLine("2. Behandlungen aktualisieren");
            Interface.WriteLine("3. Abbrechen");
            Interface.WriteLine();
            Interface.Write("Wählen Sie eine Option: ");
            
            Interface.GetKey();
            Interface.WriteLine();
            Interface.WriteLine();
            
            switch (Interface.Pressed.KeyChar)
            {
                case '1':
                    Interface.WriteLine($"Aktuelle Tore: {ausgewaehlterSpieler.Pass.AnzahlTore}");
                    Interface.Write("Neue Anzahl Tore: ");
                    if (int.TryParse(Interface.ReadLine(), out int neueTore) && neueTore >= 0)
                    {
                        ausgewaehlterSpieler.Pass.AnzahlTore = neueTore;
                        Interface.Color = ConsoleColor.Green;
                        Interface.WriteLine("\nTore erfolgreich aktualisiert!");
                        Interface.ResetColor();
                    }
                    else
                    {
                        Interface.Color = ConsoleColor.Red;
                        Interface.WriteLine("\nUngültige Eingabe!");
                        Interface.ResetColor();
                    }
                    break;
                    
                case '2':
                    Interface.WriteLine($"Aktuelle Behandlungen: {ausgewaehlterSpieler.Pass.AnzahlBehandlungen}");
                    Interface.Write("Neue Anzahl Behandlungen: ");
                    if (int.TryParse(Interface.ReadLine(), out int neueBehandlungen) && neueBehandlungen >= 0)
                    {
                        ausgewaehlterSpieler.Pass.AnzahlBehandlungen = neueBehandlungen;
                        Interface.Color = ConsoleColor.Green;
                        Interface.WriteLine("\nBehandlungen erfolgreich aktualisiert!");
                        Interface.ResetColor();
                    }
                    else
                    {
                        Interface.Color = ConsoleColor.Red;
                        Interface.WriteLine("\nUngültige Eingabe!");
                        Interface.ResetColor();
                    }
                    break;
                    
                case '3':
                    return;
            }

            // Aktualisiere Spieler in der Mannschaftsliste
            AktuelleMannschaft.Rmv(ausgewaehlterSpieler);
            AktuelleMannschaft.Add(ausgewaehlterSpieler);

            Interface.WaitOrKey(2000);
        }
        else
        {
            Interface.Color = ConsoleColor.Red;
            Interface.WriteLine("\nUngültige Auswahl!");
            Interface.ResetColor();
            Interface.WaitOrKey(2000);
        }
    }

    /// <summary>
    /// Löscht einen Spieler aus der Mitgliederliste.
    /// </summary>
    private void SpielerLoeschen()
    {
        // Alle Spieler sammeln
        List<Spieler> spielerListe = new List<Spieler>();
        foreach (var mitglied in Mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        Interface.Clear();

        if (spielerListe.Count == 0)
        {
            Interface.Clear();
            Interface.Color = ConsoleColor.Yellow;
            Interface.WriteLine("Keine Spieler vorhanden!");
            Interface.ResetColor();
            Interface.WaitOrKey(2000);
            return;
        }
        
        // Spieler auswählen
        Interface.Clear();
        Interface.Color = ConsoleColor.Red;
        Interface.WriteLine("=== Spieler löschen ===");
        Interface.ResetColor();
        Interface.WriteLine();
        
        for (int i = 0; i < spielerListe.Count; i++)
        {
            Interface.WriteLine($"{i + 1}. {spielerListe[i].Vorname} {spielerListe[i].Nachname} ({spielerListe[i].UserName})");
        }
        
        Interface.WriteLine();
        Interface.Write("Wählen Sie einen Spieler (Nummer): ");
        
        if (int.TryParse(Interface.ReadLine(), out int auswahl) && auswahl > 0 && auswahl <= spielerListe.Count)
        {
            Spieler ausgewaehlterSpieler = spielerListe[auswahl - 1];
            
            // Bestätigung
            Interface.WriteLine();
            Interface.Color = ConsoleColor.Red;
            Interface.WriteLine($"Wirklich {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname} löschen?");
            Interface.ResetColor();
            Interface.WriteLine("(J)a / (N)ein: ");
            
            ConsoleKeyInfo key = Interface.GetKey();
            
            if (key.Key == ConsoleKey.J)
            {
                AktuelleMannschaft.Rmv(ausgewaehlterSpieler);
                Interface.Color = ConsoleColor.Green;
                Interface.WriteLine("\n\nSpieler erfolgreich gelöscht!");
                Interface.ResetColor();
            }
            else
            {
                Interface.WriteLine("\n\nLöschung abgebrochen.");
            }
            
            Interface.WaitOrKey(2000);
        }
        else
        {
            Interface.Color = ConsoleColor.Red;
            Interface.WriteLine("\nUngültige Auswahl!");
            Interface.ResetColor();
            Interface.WaitOrKey(2000);
        }
    }

    /// <summary>
    /// Ändert den Benutzernamen des eingeloggten Mitglieds.
    /// Funktioniert für Trainer und Spieler.
    /// </summary>
    private void NutzernamenAendern()
    {
        if (EingeloggtesMitglied == null || !EingeloggtesMitglied.IstEingeloggt)
        {
            return;
        }
        
        Interface.Clear();
        Interface.Color = ConsoleColor.Cyan;
        Interface.WriteLine("=== Nutzernamen ändern ===");
        Interface.ResetColor();
        Interface.WriteLine();
        
        Interface.WriteLine($"Aktueller Username: {EingeloggtesMitglied.UserName}");
        Interface.WriteLine();
        
        Interface.CursorVisible = true;
        Interface.Write("Neuer Username (mindestens 3 Zeichen): ");
        string neuerUsername = Interface.ReadLine();
        Interface.CursorVisible = false;
        
        // Validierung der Eingabe
        if (string.IsNullOrWhiteSpace(neuerUsername) || neuerUsername.Length < 3)
        {
            Interface.Color = ConsoleColor.Red;
            Interface.WriteLine("\nUsername muss mindestens 3 Zeichen lang sein!");
            Interface.ResetColor();
            Interface.WaitOrKey(2000);
            return;
        }
        
        // Prüfe ob Username bereits existiert
        foreach (var mitglied in Mitglieder)
        {
            if (mitglied.UserName == neuerUsername && mitglied != EingeloggtesMitglied)
            {
                Interface.Color = ConsoleColor.Red;
                Interface.WriteLine("\nUsername existiert bereits!");
                Interface.ResetColor();
                Interface.WaitOrKey(2000);
                return;
            }
        }
        
        // Bestätigung
        Interface.WriteLine();
        Interface.Color = ConsoleColor.Yellow;
        Interface.WriteLine($"Username von '{EingeloggtesMitglied.UserName}' zu '{neuerUsername}' ändern?");
        Interface.ResetColor();
        Interface.WriteLine("(J)a / (N)ein: ");
        
        ConsoleKeyInfo key = Interface.GetKey(false);
        
        if (key.Key == ConsoleKey.J)
        {
            string alterUsername = EingeloggtesMitglied.UserName;
            EingeloggtesMitglied.UserName = neuerUsername;

            // Aktualisiere Mitglied in der Mannschaftsliste
            AktuelleMannschaft.Rmv(EingeloggtesMitglied);
            AktuelleMannschaft.Add(EingeloggtesMitglied);

            Interface.Color = ConsoleColor.Green;
            Interface.WriteLine("\n\nUsername erfolgreich geändert!");
            Interface.ResetColor();
            Interface.WriteLine($"Alter Username: {alterUsername}");
            Interface.WriteLine($"Neuer Username: {neuerUsername}");
        }
        else
        {
            Interface.WriteLine("\n\nÄnderung abgebrochen.");
        }
        
        Interface.WaitOrKey(2000);
    }

    /// <summary>
    /// DSGVO-konforme Datenlöschung des eingeloggten Mitglieds beantragen.
    /// Funktioniert für Trainer und Spieler.
    /// Löscht alle persönlichen Daten und beendet die Session.
    /// </summary>
    private void DatenloeschungBeantragen()
    {
        if (EingeloggtesMitglied == null || !EingeloggtesMitglied.IstEingeloggt)
        {
            return;
        }
        
        Interface.Clear();
        Interface.Color = ConsoleColor.Red;
        Interface.WriteLine("=== Datenlöschung beantragen ===");
        Interface.ResetColor();
        Interface.WriteLine();
        
        Interface.Color = ConsoleColor.Yellow;
        Interface.WriteLine("WARNUNG: Diese Aktion kann nicht rückgängig gemacht werden!");
        Interface.ResetColor();
        Interface.WriteLine();
        Interface.WriteLine("Folgende Daten werden gelöscht:");
        Interface.WriteLine($"- Benutzerkonto: {EingeloggtesMitglied.UserName}");
        Interface.WriteLine($"- Name: {EingeloggtesMitglied.Vorname} {EingeloggtesMitglied.Nachname}");
        
        // Zusätzliche Informationen je nach Typ
        if (EingeloggtesMitglied is Spieler spieler)
        {
            Interface.WriteLine($"- Spielerpass mit {spieler.Pass.AnzahlTore} Toren");
        }
        else if (EingeloggtesMitglied is Trainer trainer)
        {
            Interface.WriteLine($"- Trainerprofil ({trainer.Spezialisierung})");
        }
        else if (EingeloggtesMitglied is Betreuer betreuer)
        {
            Interface.WriteLine($"- Betreuerprofil ({betreuer.Fachgebiet})");
        }
        
        Interface.WriteLine("- Alle zugehörigen persönlichen Daten");
        Interface.WriteLine();
        
        // Erste Bestätigung
        Interface.Color = ConsoleColor.Red;
        Interface.WriteLine("Sind Sie sicher, dass Sie Ihre Daten löschen möchten?");
        Interface.ResetColor();
        Interface.WriteLine("(J)a / (N)ein: ");
        
        ConsoleKeyInfo key1 = Interface.GetKey();
        
        if (key1.Key != ConsoleKey.J)
        {
            Interface.WriteLine("\n\nDatenlöschung abgebrochen.");
            Interface.WaitOrKey(2000);
            return;
        }
        
        // Zweite Bestätigung zur Sicherheit
        Interface.WriteLine();
        Interface.WriteLine();
        Interface.Color = ConsoleColor.Red;
        Interface.WriteLine("LETZTE WARNUNG!");
        Interface.ResetColor();
        Interface.WriteLine("Ihre Daten werden unwiderruflich gelöscht.");
        Interface.WriteLine();
        Interface.Write("Geben Sie zur Bestätigung 'LÖSCHEN' ein: ");
        
        Interface.CursorVisible = true;
        string bestaetigung = Interface.ReadLine();
        Interface.CursorVisible = false;
        
        if (bestaetigung == "LÖSCHEN")
        {
            // Speichere Informationen für Abschlussmeldung
            string geloeschtesKonto = $"{EingeloggtesMitglied.Vorname} {EingeloggtesMitglied.Nachname} ({EingeloggtesMitglied.UserName})";
            
            // Entferne aus Liste
            AktuelleMannschaft.Rmv(EingeloggtesMitglied);
                        
            // Session beenden
            EingeloggtesMitglied = null;
            
            // Erfolgsmedung
            Interface.WriteLine();
            Interface.WriteLine();
            Interface.Color = ConsoleColor.Green;
            Interface.WriteLine("Datenlöschung erfolgreich durchgeführt.");
            Interface.ResetColor();
            Interface.WriteLine();
            Interface.WriteLine($"Gelöscht: {geloeschtesKonto}");
            Interface.WriteLine();
            Interface.WriteLine("Alle personenbezogenen Daten wurden entfernt.");
            Interface.WriteLine("Sie werden zum Login weitergeleitet...");
            
            Interface.WaitOrKey(4000);
        }
        else
        {
            Interface.WriteLine();
            Interface.Color = ConsoleColor.Yellow;
            Interface.WriteLine("Falsche Eingabe. Datenlöschung abgebrochen.");
            Interface.ResetColor();
            Interface.WaitOrKey(2000);
        }
    }

    /// <summary>
    /// Ermöglicht dem Betreuer, einen Spieler zu behandeln.
    /// Erhöht die Behandlungszähler beim Spieler und beim Betreuer.
    /// </summary>
    private void SpielerBehandeln(Betreuer betreuer)
    {
        // Alle Spieler sammeln
        List<Spieler> spielerListe = new List<Spieler>();
        foreach (var mitglied in Mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        if (spielerListe.Count == 0)
        {
            Interface.Clear();
            Interface.Color = ConsoleColor.Yellow;
            Interface.WriteLine("Keine Spieler vorhanden!");
            Interface.ResetColor();
            Interface.WaitOrKey(2000);
            return;
        }
        
        // Spieler auswählen
        Interface.Clear();
        Interface.Color = ConsoleColor.Cyan;
        Interface.WriteLine("=== Spieler behandeln ===");
        Interface.ResetColor();
        Interface.WriteLine();
        
        for (int i = 0; i < spielerListe.Count; i++)
        {
            Interface.WriteLine($"{i + 1}. {spielerListe[i].Vorname} {spielerListe[i].Nachname} - Behandlungen: {spielerListe[i].Pass.AnzahlBehandlungen}");
        }
        
        Interface.WriteLine();
        Interface.Write("Wählen Sie einen Spieler (Nummer): ");
        
        if (int.TryParse(Interface.ReadLine(), out int auswahl) && auswahl > 0 && auswahl <= spielerListe.Count)
        {
            Spieler ausgewaehlterSpieler = spielerListe[auswahl - 1];
            
            // Behandlungsdetails
            Interface.Clear();
            Interface.Color = ConsoleColor.Cyan;
            Interface.WriteLine($"=== Behandlung von {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname} ===");
            Interface.ResetColor();
            Interface.WriteLine();
            Interface.WriteLine($"Bisherige Behandlungen: {ausgewaehlterSpieler.Pass.AnzahlBehandlungen}");
            Interface.WriteLine();
            
            // Optional: Diagnose eingeben
            Interface.CursorVisible = true;
            Interface.Write("Diagnose/Notiz (optional): ");
            string diagnose = Interface.ReadLine();
            Interface.CursorVisible = false;
            
            // Bestätigung
            Interface.WriteLine();
            Interface.Color = ConsoleColor.Yellow;
            Interface.WriteLine($"Behandlung von {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname} durchführen?");
            Interface.ResetColor();
            Interface.WriteLine("(J)a / (N)ein: ");
            
            ConsoleKeyInfo key = Interface.GetKey();
            
            if (key.Key == ConsoleKey.J)
            {
                // Behandlung durchführen
                bool erfolg;
                if (!string.IsNullOrWhiteSpace(diagnose))
                {
                    diagnose += " Datum:" + DateTime.UtcNow.Date.ToShortDateString();
                    erfolg = betreuer.SpielerBehandeln(ref ausgewaehlterSpieler, diagnose);
                }
                else
                {
                    erfolg = betreuer.SpielerBehandeln(ref ausgewaehlterSpieler);
                }

                // Aktualisiere Spieler in der Mannschaftsliste
                AktuelleMannschaft.Rmv(ausgewaehlterSpieler);
                AktuelleMannschaft.Add(ausgewaehlterSpieler);

                if (erfolg)
                {
                    Interface.WriteLine();
                    Interface.WriteLine();
                    Interface.Color = ConsoleColor.Green;
                    Interface.WriteLine("Behandlung erfolgreich durchgeführt!");
                    Interface.ResetColor();
                    Interface.WriteLine();
                    Interface.WriteLine($"Spieler: {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname}");
                    Interface.WriteLine($"Behandlungen gesamt: {ausgewaehlterSpieler.Pass.AnzahlBehandlungen}");
                    if (!string.IsNullOrWhiteSpace(diagnose))
                    {
                        Interface.WriteLine($"Diagnose: {diagnose}");
                    }
                    Interface.WriteLine();
                    Interface.WriteLine($"Ihre Behandlungen gesamt: {betreuer.AnzahlBehandlungenGesamt}");
                }
                else
                {
                    Interface.Color = ConsoleColor.Red;
                    Interface.WriteLine("\n\nBehandlung konnte nicht durchgeführt werden!");
                    Interface.ResetColor();
                }
            }
            else
            {
                Interface.WriteLine("\n\nBehandlung abgebrochen.");
            }
            
            Interface.WaitOrKey(3000);
        }
        else
        {
            Interface.Color = ConsoleColor.Red;
            Interface.WriteLine("\nUngültige Auswahl!");
            Interface.ResetColor();
            Interface.WaitOrKey(2000);
        }
    }

    /// <summary>
    /// Zeigt die Behandlungsstatistik des Betreuers an.
    /// </summary>
    private void BehandlungsstatistikAnzeigen(Betreuer betreuer)
    {
        Interface.Clear();
        Interface.Color = ConsoleColor.Cyan;
        Interface.WriteLine("=== Behandlungsstatistik ===");
        Interface.ResetColor();
        Interface.WriteLine();
        
        // Persönliche Informationen
        Interface.Color = ConsoleColor.Yellow;
        Interface.WriteLine("Persönliche Daten:");
        Interface.ResetColor();
        Interface.WriteLine($"Name: {betreuer.Vorname} {betreuer.Nachname}");
        Interface.WriteLine($"Username: {betreuer.UserName}");
        Interface.WriteLine($"Fachgebiet: {betreuer.Fachgebiet}");
        Interface.WriteLine();
        
        // Statistiken
        Interface.Color = ConsoleColor.Yellow;
        Interface.WriteLine("Statistiken:");
        Interface.ResetColor();
        Interface.WriteLine($"Berufserfahrung: {betreuer.JahreErfahrung} Jahre");
        Interface.WriteLine($"Behandlungen gesamt: {betreuer.AnzahlBehandlungenGesamt}");
        
        if (betreuer.JahreErfahrung > 0)
        {
            double behandlungenProJahr = betreuer.BehandlungenProJahr();
            Interface.WriteLine($"Behandlungen pro Jahr: {behandlungenProJahr:F2}");
        }
        
        Interface.WriteLine();
        
        // Bewertung basierend auf Behandlungen
        if (betreuer.AnzahlBehandlungenGesamt >= 100)
        {
            Interface.Color = ConsoleColor.Green;
            Interface.WriteLine("Status: Erfahrener Betreuer");
            Interface.ResetColor();
        }
        else if (betreuer.AnzahlBehandlungenGesamt >= 50)
        {
            Interface.Color = ConsoleColor.Cyan;
            Interface.WriteLine("Status: Etablierter Betreuer");
            Interface.ResetColor();
        }
        else if (betreuer.AnzahlBehandlungenGesamt >= 10)
        {
            Interface.WriteLine("Status: Aktiver Betreuer");
        }
        else
        {
            Interface.WriteLine("Status: Neuer Betreuer");
        }
        
        Interface.WriteLine();
        Interface.WriteLine("Drücken Sie eine beliebige Taste zum Fortfahren...");
        Interface.GetKey();
    }

    /// <summary>
    /// Verwaltet den Login-Prozess für Benutzer.
    /// </summary>
    private void Login()
    {
        bool wiederholen = true;
        string tempName = string.Empty;
        string tempPass = string.Empty;

        do
        {
            (tempName, tempPass) = Interface.AnzeigeLogin();

            foreach (MannschaftsMitglied temp in Mitglieder)
            {
                if (temp.Einloggen(tempName, tempPass))
                {
                    EingeloggtesMitglied = temp;
                    return;
                }
            }
            
            // Fehlermeldung anzeigen
            Interface.Color = ConsoleColor.Red;
            Interface.WriteLine("\nFalscher Benutzername oder Passwort!");
            Interface.ResetColor();
            Interface.WriteLine("\nBitte versuchen Sie es erneut...");
            Interface.WaitOrKey(2000);
            
        } while (wiederholen);
    }
    #endregion
}