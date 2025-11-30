namespace VereinsVerwaltung;

public class Controller
{
    #region Eigenschaften
    private UserInterface _interface;
    private List<Manschaft> _manschaften;
    private ManschaftsMitglied? _eingeloggtesMitglied;
    #endregion

    #region Assessoren/Modifikatoren
    public UserInterface Interface => _interface;
    public List<Manschaft> AktuellerVerein => _manschaften;
    public ManschaftsMitglied? EingeloggtesMitglied => _eingeloggtesMitglied;
    #endregion

    #region Konstruktoren
    public Controller()
    {
        _interface = new UserInterface();
        _manschaften = new List<Manschaft>();
        _eingeloggtesMitglied = null;

        #region Testwerte
        #endregion
    }
    #endregion

    #region Methoden
    public void Start()
    {
        #region Lokale Variablen
        bool wiederholen = true;
        bool richtigeEingabe = true;
        string hauptmenue = string.Empty;
        #endregion

        _interface.CursorVisible = false;

        #region Überprüfen ob ein User schon eingeloggt ist
        if (Directory.Exists("Mitglieder"))
        {
            string dateiPfad = Path.Combine("Mitglieder", "eingeloggtesMitglied.json");
            if (File.Exists(dateiPfad))
            {
                string jsonInhalt = File.ReadAllText(dateiPfad);
                _eingeloggtesMitglied = System.Text.Json.JsonSerializer.Deserialize<ManschaftsMitglied>(jsonInhalt);
            }
        }
        else
        {
            Directory.CreateDirectory("Mitglieder");
        }
        #endregion

        do
        {
            // Überprüfen ob ein Mitglied eingeloggt ist.
            if (_eingeloggtesMitglied != null && _eingeloggtesMitglied.IstEingeloggt)
            {
                // Überprüfen welche art an Mitglied eingeloggt ist
                if (_eingeloggtesMitglied is Trainer)
                {
                    hauptmenue = ((Trainer)_eingeloggtesMitglied).Menu;
                }
                else if (_eingeloggtesMitglied is Spieler)
                {
                    hauptmenue = ((Spieler)_eingeloggtesMitglied).Menu;
                }
            }
            else
            {
                // Einloggen
                Login();
                continue;
            }

            // Mögliche Wiederholung basierend auf falscher Eingabe
            do
            {
                richtigeEingabe = true;
                switch (_interface.Pressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        // Aktion für Option 1
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        // Aktion für Option 2
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        // Beenden
                        wiederholen = false;
                        _interface.Goodbye();
                        Thread.Sleep(2000);
                        break;
                    default:
                        // Ungültige Eingabe
                        richtigeEingabe = false;
                        _interface.GetKey();
                        break;
                }
            } while (!richtigeEingabe);
        } while (wiederholen);
    }

    private void Login()
    {
        bool wiederholen = true;
        string tempName = string.Empty;
        string tempPass = string.Empty;

        do
        {
            (tempName, tempPass) = _interface.AnzeigeLogin();

            foreach (Manschaft temp in _manschaften)
            {
                for (int i = 0; i < temp.Manschaftsmitglieder.Count(); i++)
                {
                    if (temp.Manschaftsmitglieder[i].UserName == tempName && temp.Manschaftsmitglieder[i].Password == tempPass)
                    {
                        _eingeloggtesMitglied = temp.Manschaftsmitglieder[i];
                        return;
                    }
                }
            }
        } while (wiederholen);

    }
    #endregion
}