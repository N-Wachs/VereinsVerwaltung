using System.ComponentModel.Design;

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
    public string Menu => "(1) Spieler pass einlesen\n(2) Ausloggen";
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
    #endregion
}