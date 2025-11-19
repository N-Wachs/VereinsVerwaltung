namespace VereinsVerwaltung;

public class Spieler : ManschaftsMitglied
{
    #region Eigenschaften
    private Spielerpass _pass;
    #endregion

    #region Assessoren/Modifikatoren
    public Spielerpass Pass
    {
        get
        {
            if (_pass == null || !base.IstEingeloggt)
            {
                return new Spielerpass();
            }
            return _pass;
        }
    }
    #endregion

    #region Konstruktoren
    public Spieler() : base()
    {
        _pass = new Spielerpass();
    }
    public Spieler(Spielerpass pass) : base()
    {
        _pass = pass;
    }
    public Spieler(Spieler andererSpieler) : base(andererSpieler)
    {
        _pass = andererSpieler.Pass;
    }
    #endregion

    #region Methoden
    public bool PassLaden()
    {
        bool erfolg = false;

        if (Directory.Exists("Spieler"))
        {
            if (Directory.Exists("Spieler\\Spielerpaesse"))
            {
                string dateiPfad = Path.Combine("Spieler\\Spielerpaesse", $"{base.UserName}_pass.json");
                if (File.Exists(dateiPfad))
                {
                    string jsonInhalt = File.ReadAllText(dateiPfad);
                    _pass = System.Text.Json.JsonSerializer.Deserialize<Spielerpass>(jsonInhalt) ?? new Spielerpass();
                    erfolg = true;
                }
            }
            else             {
                Directory.CreateDirectory("Spieler\\Spielerpaesse");
                erfolg = false;
            }
        }
        else
        {
            Directory.CreateDirectory("Spieler");
            Directory.CreateDirectory("Spieler\\Spielerpaesse");
            erfolg = false;
        }

        return erfolg;
    }
    #endregion
}