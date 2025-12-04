using System.Text.Json.Serialization;

namespace VereinsVerwaltung;

public class Spieler : ManschaftsMitglied
{
    #region Eigenschaften
    private Spielerpass _pass;
    #endregion

    #region Assessoren/Modifikatoren
    public Spielerpass Pass { get => _pass; set => _pass = value; }
    #endregion

    #region Konstruktoren
    public Spieler() : base()
    {
        _pass = new Spielerpass();
    }

    [JsonConstructor]
    public Spieler(string benutzername, string vorname, string nachname, string passwort) : base(benutzername, vorname, nachname, passwort, false)
    {
        _pass = new Spielerpass();
    }

    public Spieler(string benutzername, string vorname, string nachname, string passwort, Spielerpass spielerpass) : base(benutzername, vorname, nachname, passwort, false)
    {
        _pass = spielerpass;
    }

    public Spieler(Spieler andererSpieler) : base(andererSpieler)
    {
        _pass = andererSpieler.Pass;
    }
    #endregion

    #region Methoden
    public string Anzeigen() => $"== Spieler: {Vorname} {Nachname} ==\n{Pass.Anzeigen()}\n====================";
    #endregion
}
