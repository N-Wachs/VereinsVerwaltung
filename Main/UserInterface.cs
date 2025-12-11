
namespace VereinsVerwaltung;

public class UserInterface
{
    #region Eigenschaften
    private string? _text;
    private ConsoleColor _color;
    private ConsoleKeyInfo _pressed;
    #endregion

    #region Assessoren/Modifikatoren
    public string? Text { get => _text; set => _text = value; }
    public ConsoleColor Color { get => _color; set { _color = value; Console.ForegroundColor = value; } }
    public ConsoleKeyInfo Pressed => _pressed;
    public bool CursorVisible { get => Console.CursorVisible; set => Console.CursorVisible = value; }
    #endregion

    #region Konstruktoren
    public UserInterface()
    {
        _text = string.Empty;
        _color = Console.ForegroundColor;
        _pressed = new ConsoleKeyInfo();
    }
    public UserInterface(string? text, ConsoleColor color)
    {
        _text = text;
        _color = color;
        Console.ForegroundColor = color;
        _pressed = new ConsoleKeyInfo();
    }
    public UserInterface(UserInterface andereUI)
    {
        _text = andereUI.Text;
        _color = andereUI.Color;
        Console.ForegroundColor = _color;
        _pressed = andereUI.Pressed;
    }
    #endregion

    #region Methoden
    public void AnzeigeHauptmenueSpieler(string userName)
    {
        Clear();
        WriteLine($"Willkommen, {userName}");
        WriteLine();
        WriteLine("=== Hauptmenü ===");
        WriteLine("1. Ihren Spielerpass anzeigen");
        WriteLine("2. Nutzernamen ändern");
        WriteLine("3. Datenlöschung beantragen");
        WriteLine("4. Ausgeloggt Beenden");
        WriteLine("5. Eingeloggt Beenden");
        WriteLine();
        Write("Bitte wählen Sie eine Option...");
        while (!(GetKey().KeyChar < '6' && Pressed.KeyChar > '0')) ;
    }
    public void AnzeigeHauptmenueTrainer(string userName)
    {
        Clear();
        WriteLine($"Willkommen, {userName}");
        WriteLine();
        WriteLine("=== Hauptmenü ===");
        WriteLine("1. Spielerpässe anzeigen");
        WriteLine("2. Spieler verwalten");
        WriteLine("3. Nutzernamen ändern");
        WriteLine("4. Datenlöschung beantragen");
        WriteLine("5. Ausgeloggt Beenden");
        WriteLine("6. Eingeloggt Beenden");
        WriteLine();
        Write("Bitte wählen Sie eine Option: ");
        while (!(GetKey().KeyChar < '7' && Pressed.KeyChar > '0')) ;
    }
    public void AnzeigeHauptmenueBetreuer(string userName)
    {
        Clear();
        WriteLine($"Willkommen, {userName}");
        WriteLine();
        WriteLine("=== Hauptmenü ===");
        WriteLine("1. Spieler behandeln");
        WriteLine("2. Behandlungsstatistik anzeigen");
        WriteLine("3. Nutzernamen ändern");
        WriteLine("4. Datenlöschung beantragen");
        WriteLine("5. Spieler anzeigen");
        WriteLine("6. Ausgeloggt Beenden");
        WriteLine("7. Eingeloggt Beenden");
        WriteLine();
        Write("Bitte wählen Sie eine Option: ");
        while (!(GetKey().KeyChar < '8' && Pressed.KeyChar > '0')) ;
    }

    public ConsoleKeyInfo GetKey(bool eingreifen = true) => _pressed = Console.ReadKey(eingreifen);
    public string ReadLine() 
    {
        // Ursprünglichen Cursor-Zustand speichern
        bool cursorVorher = CursorVisible;

        // Cursor sichtbar machen, falls er vorher nicht sichtbar war
        if (!cursorVorher) 
         CursorVisible = true;

        // Eingabe lesen
        string value = Console.ReadLine() ?? string.Empty;

        // Ursprünglichen Cursor-Zustand wiederherstellen
        CursorVisible = cursorVorher;

        // Eingabewert zurückgeben
        return value;
    } 

    public void WriteLine(string text) => Console.WriteLine(text);
    public void WriteLine() => Console.WriteLine();

    public void Write(string text) => Console.Write(text);
    public void Write(char character) => Console.Write(character);

    public void Clear() => Console.Clear();

    public void ResetColor() => Color = ConsoleColor.White;

    public void Goodbye(bool eingeloggtBeenden)
    {
        Clear();
        Color = ConsoleColor.Green;
        if (!eingeloggtBeenden)
            WriteLine("Sie haben sich erfolgreich ausgeloggt.");
        else
            WriteLine("Sie werden beim nächsten mal wieder eingeloggt.");
        WriteLine("Auf Wiedersehen!");
        ResetColor();
    }

    public (string username, string password) AnzeigeLogin()
    {
        string username = string.Empty;
        string password = string.Empty;
        bool wiederholen = false;

        do
        {
            wiederholen = false;

            CursorVisible = true;
            Clear();
            Color = ConsoleColor.Cyan;
            WriteLine("==== Einlogen ====");
            ResetColor();
            WriteLine();
            WriteLine("Bitte Username eingeben:");

            if (username.Length <= 0)
                username = ReadLine();

            if (username.Length < 3)
            {
                wiederholen = true;
                Color = ConsoleColor.Red;
                WriteLine("Name zu kurz!");
                ResetColor();
                username = string.Empty;
                Thread.Sleep(2000);
            }

            WriteLine();
            WriteLine("Bitte Password eingeben:");
            if (password.Length <= 0 && !wiederholen) 
                password = Verschlüsselung.Ver(ReadLine());

            if (password.Length < 3 && !wiederholen)
            {
                wiederholen = true;
                Color = ConsoleColor.Red;
                CursorVisible = false;
                WriteLine("Password zu kurz!");
                ResetColor();
                username = string.Empty;
                Thread.Sleep(2000);
            }
        } while (wiederholen);

        CursorVisible = false;
        return (username, password);
    }

    public void AnzeigeAlleSpielerPässe(List<Spieler> spielerListe)
    {
        foreach (var spieler in spielerListe)
        {
            SpielerPassAnzeigen(spieler);
        }
        WriteLine("\n");
        WriteLine("^ Sie müssen ggf. scrollen ^");
        WriteLine("Drücken Sie Enter um fortzufahren...");
        while (GetKey().Key != ConsoleKey.Enter) ;
    }
    public void SpielerPassAnzeigen(Spieler spieler) => WriteLine(spieler.Anzeigen());
    /// <summary>
    /// Zeigt den eigenen Spielerpass des eingeloggten Spielers an.
    /// Nur für Spieler verfügbar.
    /// </summary>
    public void EigenenSpielerpassAnzeigen(Spieler spieler)
    {
        Clear();
        Color = ConsoleColor.Cyan;
        WriteLine("=== Mein Spielerpass ===");
        ResetColor();
        WriteLine();

        // Persönliche Informationen
        Color = ConsoleColor.Yellow;
        WriteLine("Persönliche Daten:");
        ResetColor();
        WriteLine($"Name: {spieler.Vorname} {spieler.Nachname}");
        WriteLine($"Username: {spieler.UserName}");
        WriteLine();

        // Spielerpass-Daten
        Color = ConsoleColor.Yellow;
        WriteLine("Spielerpass:");
        ResetColor();
        WriteLine($"Nationalität: {spieler.Pass.Nationalitaet}");
        WriteLine($"Anzahl Tore: {spieler.Pass.AnzahlTore}");
        WriteLine($"Anzahl Behandlungen: {spieler.Pass.AnzahlBehandlungen}");
        WriteLine();

        // Statistiken
        Color = ConsoleColor.Yellow;
        WriteLine("Statistiken:");
        ResetColor();

        double toreProBehandlung = spieler.Pass.AnzahlBehandlungen > 0
            ? (double)spieler.Pass.AnzahlTore / spieler.Pass.AnzahlBehandlungen
            : spieler.Pass.AnzahlTore;
        WriteLine($"Tore pro Behandlung: {toreProBehandlung:F2}");

        // Leistungsbewertung
        if (spieler.Pass.AnzahlTore >= 20)
        {
            Color = ConsoleColor.Green;
            WriteLine("\nStatus: Topspieler");
            ResetColor();
        }
        else if (spieler.Pass.AnzahlTore >= 10)
        {
            Color = ConsoleColor.Cyan;
            WriteLine("\nStatus: Stammspieler");
            ResetColor();
        }
        else if (spieler.Pass.AnzahlTore >= 5)
        {
            WriteLine("\nStatus: Aktiver Spieler");
        }
        else
        {
            WriteLine("\nStatus: Nachwuchsspieler");
        }

        WriteLine();
        WriteLine("Drücken Sie eine beliebige Taste zum Fortfahren...");
        GetKey();
    }

    public void WaitOrKey(int timeMS)
    {
        DateTime dateTime = DateTime.Now.AddMilliseconds(timeMS);
        while (DateTime.Now < dateTime)
        {
            if (Console.KeyAvailable) break;
            Thread.Sleep(20);
        }
    }
    #endregion
}