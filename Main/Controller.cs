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
        _eingeloggtesMitglied = _repository.LadeEingeloggtesMitglied();

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
                        break;

                    case '3':
                        // Nutzernamen ändern
                        break;

                    case '4':
                        // Datenlöschung beantragen
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
            else if (EingeloggtesMitglied is Spieler)
            {
                // Hauptmenü für Spieler
                _interface.AnzeigeHauptmenueSpieler(EingeloggtesMitglied.Vorname + " " + EingeloggtesMitglied.Nachname);
                
                switch (_interface.Pressed.KeyChar)
                {
                    case '1':
                        // Spielerpass anzeigen
                        break;
                    case '2':
                        // Nutzernamen ändern
                        break;
                    case '3':
                        // Datenlöschung beantragen
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
    
    private void AlleSpielerPaesseAnzeigen()
    {
        List<Spieler> spielerListe = new List<Spieler>();
        
        // Alle Spieler aus der Mitgliederliste filtern
        foreach (var mitglied in _mitglieder)
        {
            if (mitglied is Spieler spieler)
            {
                spielerListe.Add(spieler);
            }
        }
        
        _interface.AnzeigeAlleSpielerPässe(spielerListe);
    }

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