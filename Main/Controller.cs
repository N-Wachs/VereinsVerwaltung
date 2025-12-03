namespace VereinsVerwaltung;

public class Controller
{
    #region Eigenschaften
    private UserInterface _interface;
    private List<ManschaftsMitglied> _mitglieder;
    private ManschaftsMitglied? _eingeloggtesMitglied;
    private MitgliederRepository _repository;
    #endregion

    #region Assessoren/Modifikatoren
    public UserInterface Interface => _interface;
    public ManschaftsMitglied? EingeloggtesMitglied => _eingeloggtesMitglied;
    #endregion

    #region Konstruktoren
    public Controller()
    {
        _interface = new UserInterface();
        _repository = new MitgliederRepository();
        _mitglieder = new List<ManschaftsMitglied>();
        _eingeloggtesMitglied = null;

        #region Mitglieder laden oder Testwerte erstellen
        // Versuche gespeicherte Mitglieder zu laden
        // _mitglieder = _repository.LadeAlleMitglieder();
        
        // Falls keine Mitglieder vorhanden, erstelle Testwerte
        if (_mitglieder.Count == 0)
        {
            // Trainer
            _mitglieder.Add(new Trainer("sabineschulz", "Sabine", "Schulz", Verschlüsselung.Ver("Sabine"), 12, "Taktik", 82000));
            _mitglieder.Add(new Trainer("lucasengel", "Lucas", "Engel", Verschlüsselung.Ver("Lucas"), 3, "Fitness", 55000));

            // Spieler
            _mitglieder.Add(new Spieler("erikbecker", "Erik", "Becker", Verschlüsselung.Ver("Erik"),
                new Spielerpass(0, 2, "Deutschland")));

            _mitglieder.Add(new Spieler("tomschmidt", "Tom", "Schmidt", Verschlüsselung.Ver("Tom"),
                new Spielerpass(4, 15, "Deutschland")));

            _mitglieder.Add(new Spieler("leonweiss", "Leon", "Weiss", Verschlüsselung.Ver("Leon"),
                new Spielerpass(10, 22, "Österreich")));

            _mitglieder.Add(new Spieler("danielkurt", "Daniel", "Kurt", Verschlüsselung.Ver("Daniel"),
                new Spielerpass(2, 10, "Schweiz")));

            _mitglieder.Add(new Spieler("marcobrecht", "Marco", "Brecht", Verschlüsselung.Ver("Marco"),
                new Spielerpass(7, 18, "Deutschland")));

            // Betreuer
            _mitglieder.Add(new Betreuer("jennifermeyer", "Jennifer", "Meyer", Verschlüsselung.Ver("Jennifer"),
                "Physiotherapie", 5));
            // Speichere Testwerte
            // _repository.SpeichereAlleMitglieder(_mitglieder);
        }
        #endregion
    }
    #endregion

    #region Methoden
    public void Start()
    {
        #region Lokale Variablen
        bool wiederholen = true;
        bool beendenEingellogt = false;
        #endregion

        _interface.CursorVisible = false;

        // Überprüfen ob ein User schon eingeloggt ist
        // _eingeloggtesMitglied = _repository.LadeEingeloggtesMitglied();

        do
        {
            // Überprüfen ob ein Mitglied eingeloggt ist.
            if (!(_eingeloggtesMitglied != null && _eingeloggtesMitglied.IstEingeloggt))
            {
                Login();
                continue;
            }

            // Hauptmenü anzeigen
            if (EingeloggtesMitglied is Trainer)
            {
                // Hauptmenü für Trainer
                _interface.AnzeigeHauptmenueTrainer(EingeloggtesMitglied.Vorname + " " + EingeloggtesMitglied.Nachname);

                switch (_interface.Pressed.KeyChar)
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
                _interface.AnzeigeHauptmenueSpieler(spieler.Vorname + " " + spieler.Nachname);
                
                switch (_interface.Pressed.KeyChar)
                {
                    case '1':
                        // Spielerpass anzeigen
                        EigenenSpielerpassAnzeigen(spieler);
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
                _interface.AnzeigeHauptmenueBetreuer(betreuer.Vorname + " " + betreuer.Nachname);
                
                switch (_interface.Pressed.KeyChar)
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
                        // Ausloggen
                        beendenEingellogt = false;
                        wiederholen = false;
                        break;
                    case '6':
                        // Eingeloggt beenden
                        wiederholen = false;
                        beendenEingellogt = true;
                        break;
                }
            }
            else
            {
                // Hauptmenü für noch kommenden Sanitäter oder andere Rollen
            }
        } while (wiederholen);

        #region Speichern beim Beenden
        if (beendenEingellogt && _eingeloggtesMitglied != null)
        {
            // _repository.SpeichereEingeloggtesMitglied(_eingeloggtesMitglied);
        }
        else
        {
            // _repository.LoescheEingeloggtesMitglied();
        }
        
        // Speichere alle Änderungen an Mitgliedern
        // _repository.SpeichereAlleMitglieder(_mitglieder);

        _interface.Goodbye();
        Thread.Sleep(2000);
        #endregion
    }
    
    /// <summary>
    /// Zeigt alle Spielerpässe an (Trainer-Funktion).
    /// </summary>
    private void AlleSpielerPaesseAnzeigen()
    {
        List<Spieler> spielerListe = new List<Spieler>();
        
        // Alle Spieler aus der Mitgliederliste filtern
        _interface.Clear();
        foreach (var mitglied in _mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        _interface.AnzeigeAlleSpielerPässe(spielerListe);
    }

    /// <summary>
    /// Zeigt den eigenen Spielerpass des eingeloggten Spielers an.
    /// Nur für Spieler verfügbar.
    /// </summary>
    private void EigenenSpielerpassAnzeigen(Spieler spieler)
    {
        _interface.Clear();
        _interface.Color = ConsoleColor.Cyan;
        _interface.WriteLine("=== Mein Spielerpass ===");
        _interface.ResetColor();
        _interface.WriteLine();
        
        // Persönliche Informationen
        _interface.Color = ConsoleColor.Yellow;
        _interface.WriteLine("Persönliche Daten:");
        _interface.ResetColor();
        _interface.WriteLine($"Name: {spieler.Vorname} {spieler.Nachname}");
        _interface.WriteLine($"Username: {spieler.UserName}");
        _interface.WriteLine();
        
        // Spielerpass-Daten
        _interface.Color = ConsoleColor.Yellow;
        _interface.WriteLine("Spielerpass:");
        _interface.ResetColor();
        _interface.WriteLine($"Nationalität: {spieler.Pass.Nationalitaet}");
        _interface.WriteLine($"Anzahl Tore: {spieler.Pass.AnzahlTore}");
        _interface.WriteLine($"Anzahl Behandlungen: {spieler.Pass.AnzahlBehandlungen}");
        _interface.WriteLine();
        
        // Statistiken
        _interface.Color = ConsoleColor.Yellow;
        _interface.WriteLine("Statistiken:");
        _interface.ResetColor();
        
        if (spieler.Pass.AnzahlTore > 0)
        {
            double toreProBehandlung = spieler.Pass.AnzahlBehandlungen > 0 
                ? (double)spieler.Pass.AnzahlTore / spieler.Pass.AnzahlBehandlungen 
                : spieler.Pass.AnzahlTore;
            _interface.WriteLine($"Tore pro Behandlung: {toreProBehandlung:F2}");
        }
        
        // Leistungsbewertung
        if (spieler.Pass.AnzahlTore >= 20)
        {
            _interface.Color = ConsoleColor.Green;
            _interface.WriteLine("\nStatus: Topspieler");
            _interface.ResetColor();
        }
        else if (spieler.Pass.AnzahlTore >= 10)
        {
            _interface.Color = ConsoleColor.Cyan;
            _interface.WriteLine("\nStatus: Stammspieler");
            _interface.ResetColor();
        }
        else if (spieler.Pass.AnzahlTore >= 5)
        {
            _interface.WriteLine("\nStatus: Aktiver Spieler");
        }
        else
        {
            _interface.WriteLine("\nStatus: Nachwuchsspieler");
        }
        
        _interface.WriteLine();
        _interface.WriteLine("Drücken Sie eine beliebige Taste zum Fortfahren...");
        _interface.GetKey();
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
            _interface.Clear();
            _interface.Color = ConsoleColor.Cyan;
            _interface.WriteLine("=== Spieler Verwalten ===");
            _interface.ResetColor();
            _interface.WriteLine();
            _interface.WriteLine("1. Neuen Spieler hinzufügen");
            _interface.WriteLine("2. Spieler bearbeiten");
            _interface.WriteLine("3. Spieler löschen");
            _interface.WriteLine("4. Alle Spieler anzeigen");
            _interface.WriteLine("5. Zurück zum Hauptmenü");
            _interface.WriteLine();
            _interface.Write("Bitte wählen Sie eine Option: ");
            
            _interface.GetKey();
            
            switch (_interface.Pressed.KeyChar)
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
                    _interface.Color = ConsoleColor.Red;
                    _interface.WriteLine("\n\nUngültige Eingabe!");
                    _interface.ResetColor();
                    Thread.Sleep(1500);
                    break;
            }
        } while (!zurueck);
    }

    /// <summary>
    /// Fügt einen neuen Spieler zur Mitgliederliste hinzu.
    /// </summary>
    private void SpielerHinzufuegen()
    {
        _interface.Clear();
        _interface.Color = ConsoleColor.Cyan;
        _interface.WriteLine("=== Neuen Spieler hinzufügen ===");
        _interface.ResetColor();
        _interface.WriteLine();
        
        _interface.CursorVisible = true;
        
        // Benutzername eingeben
        _interface.Write("Username: ");
        string username = _interface.ReadLine();
        
        // Prüfe ob Username bereits existiert
        foreach (var mitglied in _mitglieder)
        {
            if (mitglied.UserName == username)
            {
                _interface.Color = ConsoleColor.Red;
                _interface.WriteLine("\n❌ Username existiert bereits!");
                _interface.ResetColor();
                _interface.CursorVisible = false;
                Thread.Sleep(2000);
                return;
            }
        }
        
        // Vorname eingeben
        _interface.Write("Vorname: ");
        string vorname = _interface.ReadLine();
        
        // Nachname eingeben
        _interface.Write("Nachname: ");
        string nachname = _interface.ReadLine();
        
        // Passwort eingeben
        _interface.Write("Passwort: ");
        string passwort = Verschlüsselung.Ver(_interface.ReadLine());
        
        // Nationalität eingeben
        _interface.Write("Nationalität: ");
        string nationalitaet = _interface.ReadLine();
        
        _interface.CursorVisible = false;
        
        // Neuen Spieler erstellen
        Spieler neuerSpieler = new Spieler(username, vorname, nachname, passwort, 
            new Spielerpass(0, 0, nationalitaet));
        
        // Zur Liste hinzufügen
        _mitglieder.Add(neuerSpieler);
        
        _interface.Color = ConsoleColor.Green;
        _interface.WriteLine("\n✓ Spieler erfolgreich hinzugefügt!");
        _interface.ResetColor();
        Thread.Sleep(2000);
    }

    /// <summary>
    /// Bearbeitet die Daten eines vorhandenen Spielers.
    /// </summary>
    private void SpielerBearbeiten()
    {
        // Alle Spieler sammeln
        List<Spieler> spielerListe = new List<Spieler>();
        foreach (var mitglied in _mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        if (spielerListe.Count == 0)
        {
            _interface.Clear();
            _interface.Color = ConsoleColor.Yellow;
            _interface.WriteLine("Keine Spieler vorhanden!");
            _interface.ResetColor();
            Thread.Sleep(2000);
            return;
        }
        
        // Spieler auswählen
        _interface.Clear();
        _interface.Color = ConsoleColor.Cyan;
        _interface.WriteLine("=== Spieler bearbeiten ===");
        _interface.ResetColor();
        _interface.WriteLine();
        
        for (int i = 0; i < spielerListe.Count; i++)
        {
            _interface.WriteLine($"{i + 1}. {spielerListe[i].Vorname} {spielerListe[i].Nachname} ({spielerListe[i].UserName})");
        }
        
        _interface.WriteLine();
        _interface.Write("Wählen Sie einen Spieler (Nummer): ");
        
        if (int.TryParse(_interface.ReadLine(), out int auswahl) && auswahl > 0 && auswahl <= spielerListe.Count)
        {
            Spieler ausgewaehlterSpieler = spielerListe[auswahl - 1];
            
            // Bearbeitungsoptionen
            _interface.Clear();
            _interface.Color = ConsoleColor.Cyan;
            _interface.WriteLine($"=== {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname} bearbeiten ===");
            _interface.ResetColor();
            _interface.WriteLine();
            _interface.WriteLine("1. Tore aktualisieren");
            _interface.WriteLine("2. Behandlungen aktualisieren");
            _interface.WriteLine("3. Abbrechen");
            _interface.WriteLine();
            _interface.Write("Wählen Sie eine Option: ");
            
            ConsoleKeyInfo key = _interface.GetKey();
            _interface.WriteLine();
            _interface.WriteLine();
            
            switch (key.KeyChar)
            {
                case '1':
                    _interface.WriteLine($"Aktuelle Tore: {ausgewaehlterSpieler.Pass.AnzahlTore}");
                    _interface.Write("Neue Anzahl Tore: ");
                    if (int.TryParse(_interface.ReadLine(), out int neueTore) && neueTore >= 0)
                    {
                        ausgewaehlterSpieler.Pass.AnzahlTore = neueTore;
                        _interface.Color = ConsoleColor.Green;
                        _interface.WriteLine("\nTore erfolgreich aktualisiert!");
                        _interface.ResetColor();
                    }
                    else
                    {
                        _interface.Color = ConsoleColor.Red;
                        _interface.WriteLine("\nUngültige Eingabe!");
                        _interface.ResetColor();
                    }
                    break;
                    
                case '2':
                    _interface.WriteLine($"Aktuelle Behandlungen: {ausgewaehlterSpieler.Pass.AnzahlBehandlungen}");
                    _interface.Write("Neue Anzahl Behandlungen: ");
                    if (int.TryParse(_interface.ReadLine(), out int neueBehandlungen) && neueBehandlungen >= 0)
                    {
                        ausgewaehlterSpieler.Pass.AnzahlBehandlungen = neueBehandlungen;
                        _interface.Color = ConsoleColor.Green;
                        _interface.WriteLine("\nBehandlungen erfolgreich aktualisiert!");
                        _interface.ResetColor();
                    }
                    else
                    {
                        _interface.Color = ConsoleColor.Red;
                        _interface.WriteLine("\nUngültige Eingabe!");
                        _interface.ResetColor();
                    }
                    break;
                    
                case '3':
                    return;
            }
            
            Thread.Sleep(2000);
        }
        else
        {
            _interface.Color = ConsoleColor.Red;
            _interface.WriteLine("\nUngültige Auswahl!");
            _interface.ResetColor();
            Thread.Sleep(2000);
        }
    }

    /// <summary>
    /// Löscht einen Spieler aus der Mitgliederliste.
    /// </summary>
    private void SpielerLoeschen()
    {
        // Alle Spieler sammeln
        List<Spieler> spielerListe = new List<Spieler>();
        foreach (var mitglied in _mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        _interface.Clear();

        if (spielerListe.Count == 0)
        {
            _interface.Clear();
            _interface.Color = ConsoleColor.Yellow;
            _interface.WriteLine("Keine Spieler vorhanden!");
            _interface.ResetColor();
            Thread.Sleep(2000);
            return;
        }
        
        // Spieler auswählen
        _interface.Clear();
        _interface.Color = ConsoleColor.Red;
        _interface.WriteLine("=== Spieler löschen ===");
        _interface.ResetColor();
        _interface.WriteLine();
        
        for (int i = 0; i < spielerListe.Count; i++)
        {
            _interface.WriteLine($"{i + 1}. {spielerListe[i].Vorname} {spielerListe[i].Nachname} ({spielerListe[i].UserName})");
        }
        
        _interface.WriteLine();
        _interface.Write("Wählen Sie einen Spieler (Nummer): ");
        
        if (int.TryParse(_interface.ReadLine(), out int auswahl) && auswahl > 0 && auswahl <= spielerListe.Count)
        {
            Spieler ausgewaehlterSpieler = spielerListe[auswahl - 1];
            
            // Bestätigung
            _interface.WriteLine();
            _interface.Color = ConsoleColor.Red;
            _interface.WriteLine($"Wirklich {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname} löschen?");
            _interface.ResetColor();
            _interface.WriteLine("(J)a / (N)ein: ");
            
            ConsoleKeyInfo key = _interface.GetKey();
            
            if (key.Key == ConsoleKey.J)
            {
                _mitglieder.Remove(ausgewaehlterSpieler);
                _interface.Color = ConsoleColor.Green;
                _interface.WriteLine("\n\nSpieler erfolgreich gelöscht!");
                _interface.ResetColor();
            }
            else
            {
                _interface.WriteLine("\n\nLöschung abgebrochen.");
            }
            
            Thread.Sleep(2000);
        }
        else
        {
            _interface.Color = ConsoleColor.Red;
            _interface.WriteLine("\nUngültige Auswahl!");
            _interface.ResetColor();
            Thread.Sleep(2000);
        }
    }

    /// <summary>
    /// Ändert den Benutzernamen des eingeloggten Mitglieds.
    /// Funktioniert für Trainer und Spieler.
    /// </summary>
    private void NutzernamenAendern()
    {
        if (_eingeloggtesMitglied == null || !_eingeloggtesMitglied.IstEingeloggt)
        {
            return;
        }
        
        _interface.Clear();
        _interface.Color = ConsoleColor.Cyan;
        _interface.WriteLine("=== Nutzernamen ändern ===");
        _interface.ResetColor();
        _interface.WriteLine();
        
        _interface.WriteLine($"Aktueller Username: {_eingeloggtesMitglied.UserName}");
        _interface.WriteLine();
        
        _interface.CursorVisible = true;
        _interface.Write("Neuer Username (mindestens 3 Zeichen): ");
        string neuerUsername = _interface.ReadLine();
        _interface.CursorVisible = false;
        
        // Validierung der Eingabe
        if (string.IsNullOrWhiteSpace(neuerUsername) || neuerUsername.Length < 3)
        {
            _interface.Color = ConsoleColor.Red;
            _interface.WriteLine("\nUsername muss mindestens 3 Zeichen lang sein!");
            _interface.ResetColor();
            Thread.Sleep(2000);
            return;
        }
        
        // Prüfe ob Username bereits existiert
        foreach (var mitglied in _mitglieder)
        {
            if (mitglied.UserName == neuerUsername && mitglied != _eingeloggtesMitglied)
            {
                _interface.Color = ConsoleColor.Red;
                _interface.WriteLine("\nUsername existiert bereits!");
                _interface.ResetColor();
                Thread.Sleep(2000);
                return;
            }
        }
        
        // Bestätigung
        _interface.WriteLine();
        _interface.Color = ConsoleColor.Yellow;
        _interface.WriteLine($"Username von '{_eingeloggtesMitglied.UserName}' zu '{neuerUsername}' ändern?");
        _interface.ResetColor();
        _interface.WriteLine("(J)a / (N)ein: ");
        
        ConsoleKeyInfo key = _interface.GetKey();
        
        if (key.Key == ConsoleKey.J)
        {
            string alterUsername = _eingeloggtesMitglied.UserName;
            _eingeloggtesMitglied.UserName = neuerUsername;
            
            _interface.Color = ConsoleColor.Green;
            _interface.WriteLine("\n\nUsername erfolgreich geändert!");
            _interface.ResetColor();
            _interface.WriteLine($"Alter Username: {alterUsername}");
            _interface.WriteLine($"Neuer Username: {neuerUsername}");
        }
        else
        {
            _interface.WriteLine("\n\nÄnderung abgebrochen.");
        }
        
        Thread.Sleep(2000);
    }

    /// <summary>
    /// DSGVO-konforme Datenlöschung des eingeloggten Mitglieds beantragen.
    /// Funktioniert für Trainer und Spieler.
    /// Löscht alle persönlichen Daten und beendet die Session.
    /// </summary>
    private void DatenloeschungBeantragen()
    {
        if (_eingeloggtesMitglied == null || !_eingeloggtesMitglied.IstEingeloggt)
        {
            return;
        }
        
        _interface.Clear();
        _interface.Color = ConsoleColor.Red;
        _interface.WriteLine("=== Datenlöschung beantragen ===");
        _interface.ResetColor();
        _interface.WriteLine();
        
        _interface.Color = ConsoleColor.Yellow;
        _interface.WriteLine("WARNUNG: Diese Aktion kann nicht rückgängig gemacht werden!");
        _interface.ResetColor();
        _interface.WriteLine();
        _interface.WriteLine("Folgende Daten werden gelöscht:");
        _interface.WriteLine($"- Benutzerkonto: {_eingeloggtesMitglied.UserName}");
        _interface.WriteLine($"- Name: {_eingeloggtesMitglied.Vorname} {_eingeloggtesMitglied.Nachname}");
        
        // Zusätzliche Informationen je nach Typ
        if (_eingeloggtesMitglied is Spieler spieler)
        {
            _interface.WriteLine($"- Spielerpass mit {spieler.Pass.AnzahlTore} Toren");
        }
        else if (_eingeloggtesMitglied is Trainer trainer)
        {
            _interface.WriteLine($"- Trainerprofil ({trainer.Spezialisierung})");
        }
        else if (_eingeloggtesMitglied is Betreuer betreuer)
        {
            _interface.WriteLine($"- Betreuerprofil ({betreuer.Fachgebiet})");
        }
        
        _interface.WriteLine("- Alle zugehörigen persönlichen Daten");
        _interface.WriteLine();
        
        // Erste Bestätigung
        _interface.Color = ConsoleColor.Red;
        _interface.WriteLine("Sind Sie sicher, dass Sie Ihre Daten löschen möchten?");
        _interface.ResetColor();
        _interface.WriteLine("(J)a / (N)ein: ");
        
        ConsoleKeyInfo key1 = _interface.GetKey();
        
        if (key1.Key != ConsoleKey.J)
        {
            _interface.WriteLine("\n\nDatenlöschung abgebrochen.");
            Thread.Sleep(2000);
            return;
        }
        
        // Zweite Bestätigung zur Sicherheit
        _interface.WriteLine();
        _interface.WriteLine();
        _interface.Color = ConsoleColor.Red;
        _interface.WriteLine("LETZTE WARNUNG!");
        _interface.ResetColor();
        _interface.WriteLine("Ihre Daten werden unwiderruflich gelöscht.");
        _interface.WriteLine();
        _interface.Write("Geben Sie zur Bestätigung 'LÖSCHEN' ein: ");
        
        _interface.CursorVisible = true;
        string bestaetigung = _interface.ReadLine();
        _interface.CursorVisible = false;
        
        if (bestaetigung == "LÖSCHEN")
        {
            // Speichere Informationen für Abschlussmeldung
            string geloeschtesKonto = $"{_eingeloggtesMitglied.Vorname} {_eingeloggtesMitglied.Nachname} ({_eingeloggtesMitglied.UserName})";
            
            // Entferne aus Liste
            _mitglieder.Remove(_eingeloggtesMitglied);
            
            // Lösche gespeicherte Dateien vom Repository
            _repository.LoescheMitglied(_eingeloggtesMitglied.UserName);
            
            // Session beenden
            _eingeloggtesMitglied = null;
            
            // Erfolgsmedung
            _interface.WriteLine();
            _interface.WriteLine();
            _interface.Color = ConsoleColor.Green;
            _interface.WriteLine("Datenlöschung erfolgreich durchgeführt.");
            _interface.ResetColor();
            _interface.WriteLine();
            _interface.WriteLine($"Gelöscht: {geloeschtesKonto}");
            _interface.WriteLine();
            _interface.WriteLine("Alle personenbezogenen Daten wurden entfernt.");
            _interface.WriteLine("Sie werden zum Login weitergeleitet...");
            
            Thread.Sleep(4000);
        }
        else
        {
            _interface.WriteLine();
            _interface.Color = ConsoleColor.Yellow;
            _interface.WriteLine("Falsche Eingabe. Datenlöschung abgebrochen.");
            _interface.ResetColor();
            Thread.Sleep(2000);
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
        foreach (var mitglied in _mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        if (spielerListe.Count == 0)
        {
            _interface.Clear();
            _interface.Color = ConsoleColor.Yellow;
            _interface.WriteLine("Keine Spieler vorhanden!");
            _interface.ResetColor();
            Thread.Sleep(2000);
            return;
        }
        
        // Spieler auswählen
        _interface.Clear();
        _interface.Color = ConsoleColor.Cyan;
        _interface.WriteLine("=== Spieler behandeln ===");
        _interface.ResetColor();
        _interface.WriteLine();
        
        for (int i = 0; i < spielerListe.Count; i++)
        {
            _interface.WriteLine($"{i + 1}. {spielerListe[i].Vorname} {spielerListe[i].Nachname} - Behandlungen: {spielerListe[i].Pass.AnzahlBehandlungen}");
        }
        
        _interface.WriteLine();
        _interface.Write("Wählen Sie einen Spieler (Nummer): ");
        
        if (int.TryParse(_interface.ReadLine(), out int auswahl) && auswahl > 0 && auswahl <= spielerListe.Count)
        {
            Spieler ausgewaehlterSpieler = spielerListe[auswahl - 1];
            
            // Behandlungsdetails
            _interface.Clear();
            _interface.Color = ConsoleColor.Cyan;
            _interface.WriteLine($"=== Behandlung von {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname} ===");
            _interface.ResetColor();
            _interface.WriteLine();
            _interface.WriteLine($"Bisherige Behandlungen: {ausgewaehlterSpieler.Pass.AnzahlBehandlungen}");
            _interface.WriteLine();
            
            // Optional: Diagnose eingeben
            _interface.CursorVisible = true;
            _interface.Write("Diagnose/Notiz (optional): ");
            string diagnose = _interface.ReadLine();
            _interface.CursorVisible = false;
            
            // Bestätigung
            _interface.WriteLine();
            _interface.Color = ConsoleColor.Yellow;
            _interface.WriteLine($"Behandlung von {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname} durchführen?");
            _interface.ResetColor();
            _interface.WriteLine("(J)a / (N)ein: ");
            
            ConsoleKeyInfo key = _interface.GetKey();
            
            if (key.Key == ConsoleKey.J)
            {
                // Behandlung durchführen
                bool erfolg;
                if (!string.IsNullOrWhiteSpace(diagnose))
                {
                    erfolg = betreuer.SpielerBehandeln(ausgewaehlterSpieler, diagnose);
                }
                else
                {
                    erfolg = betreuer.SpielerBehandeln(ausgewaehlterSpieler);
                }
                
                if (erfolg)
                {
                    _interface.WriteLine();
                    _interface.WriteLine();
                    _interface.Color = ConsoleColor.Green;
                    _interface.WriteLine("Behandlung erfolgreich durchgeführt!");
                    _interface.ResetColor();
                    _interface.WriteLine();
                    _interface.WriteLine($"Spieler: {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname}");
                    _interface.WriteLine($"Behandlungen gesamt: {ausgewaehlterSpieler.Pass.AnzahlBehandlungen}");
                    if (!string.IsNullOrWhiteSpace(diagnose))
                    {
                        _interface.WriteLine($"Diagnose: {diagnose}");
                    }
                    _interface.WriteLine();
                    _interface.WriteLine($"Ihre Behandlungen gesamt: {betreuer.AnzahlBehandlungenGesamt}");
                }
                else
                {
                    _interface.Color = ConsoleColor.Red;
                    _interface.WriteLine("\n\nBehandlung konnte nicht durchgeführt werden!");
                    _interface.ResetColor();
                }
            }
            else
            {
                _interface.WriteLine("\n\nBehandlung abgebrochen.");
            }
            
            Thread.Sleep(3000);
        }
        else
        {
            _interface.Color = ConsoleColor.Red;
            _interface.WriteLine("\nUngültige Auswahl!");
            _interface.ResetColor();
            Thread.Sleep(2000);
        }
    }

    /// <summary>
    /// Zeigt die Behandlungsstatistik des Betreuers an.
    /// </summary>
    private void BehandlungsstatistikAnzeigen(Betreuer betreuer)
    {
        _interface.Clear();
        _interface.Color = ConsoleColor.Cyan;
        _interface.WriteLine("=== Behandlungsstatistik ===");
        _interface.ResetColor();
        _interface.WriteLine();
        
        // Persönliche Informationen
        _interface.Color = ConsoleColor.Yellow;
        _interface.WriteLine("Persönliche Daten:");
        _interface.ResetColor();
        _interface.WriteLine($"Name: {betreuer.Vorname} {betreuer.Nachname}");
        _interface.WriteLine($"Username: {betreuer.UserName}");
        _interface.WriteLine($"Fachgebiet: {betreuer.Fachgebiet}");
        _interface.WriteLine();
        
        // Statistiken
        _interface.Color = ConsoleColor.Yellow;
        _interface.WriteLine("Statistiken:");
        _interface.ResetColor();
        _interface.WriteLine($"Berufserfahrung: {betreuer.JahreErfahrung} Jahre");
        _interface.WriteLine($"Behandlungen gesamt: {betreuer.AnzahlBehandlungenGesamt}");
        
        if (betreuer.JahreErfahrung > 0)
        {
            double behandlungenProJahr = betreuer.BehandlungenProJahr();
            _interface.WriteLine($"Behandlungen pro Jahr: {behandlungenProJahr:F2}");
        }
        
        _interface.WriteLine();
        
        // Bewertung basierend auf Behandlungen
        if (betreuer.AnzahlBehandlungenGesamt >= 100)
        {
            _interface.Color = ConsoleColor.Green;
            _interface.WriteLine("Status: Erfahrener Betreuer");
            _interface.ResetColor();
        }
        else if (betreuer.AnzahlBehandlungenGesamt >= 50)
        {
            _interface.Color = ConsoleColor.Cyan;
            _interface.WriteLine("Status: Etablierter Betreuer");
            _interface.ResetColor();
        }
        else if (betreuer.AnzahlBehandlungenGesamt >= 10)
        {
            _interface.WriteLine("Status: Aktiver Betreuer");
        }
        else
        {
            _interface.WriteLine("Status: Neuer Betreuer");
        }
        
        _interface.WriteLine();
        _interface.WriteLine("Drücken Sie eine beliebige Taste zum Fortfahren...");
        _interface.GetKey();
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
            (tempName, tempPass) = _interface.AnzeigeLogin();

            foreach (ManschaftsMitglied temp in _mitglieder)
            {
                if (temp.Einloggen(tempName, tempPass))
                {
                    _eingeloggtesMitglied = temp;
                    return;
                }
            }
            
            // Fehlermeldung anzeigen
            _interface.Color = ConsoleColor.Red;
            _interface.WriteLine("\nFalscher Benutzername oder Passwort!");
            _interface.ResetColor();
            _interface.WriteLine("\nBitte versuchen Sie es erneut...");
            Thread.Sleep(2000);
            
        } while (wiederholen);
    }
    #endregion
}