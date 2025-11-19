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
        _pressed = GetKey();
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
        _pressed = GetKey();
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
    #endregion
}