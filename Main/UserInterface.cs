

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
        WriteLine("4. Ausgelogt Beenden");
        WriteLine("5. Eingelogt Beenden");
        WriteLine();
        Write("Bitte wählen Sie eine Option...");
        while (!(GetKey().KeyChar < '6' && _pressed.KeyChar > '0')) ;
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
        WriteLine("5. Ausgelogt Beenden");
        WriteLine("6. Eingelogt Beenden");
        WriteLine();
        Write("Bitte wählen Sie eine Option: ");
        while (!(GetKey().KeyChar < '7' && _pressed.KeyChar > '0')) ;
    }

    public ConsoleKeyInfo GetKey(bool eingreifen = true) => _pressed = Console.ReadKey(eingreifen);
    public string ReadLine() => Console.ReadLine() ?? string.Empty;

    public void WriteLine(string text) => Console.WriteLine(text);
    public void WriteLine() => Console.WriteLine();

    public void Write(string text) => Console.Write(text);
    public void Write(char character) => Console.Write(character);

    public void Clear() => Console.Clear();

    public void ResetColor() => Color = ConsoleColor.White;

    public void Goodbye()
    {
        Clear();
        Color = ConsoleColor.Green;
        WriteLine("Auf Wiedersehen!");
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
            WriteLine(spieler.Anzeigen() + "\n");
        }
        WriteLine("\n");
        WriteLine("Drücken Sie Enter um fortzufahren...");
        while (GetKey().Key != ConsoleKey.Enter) ;
    }
    #endregion
}