namespace VereinsVerwaltung;

public class Spielerpass
{
    #region Eigenschaften
    private int _anzahlBehandlungen;
    private int _anzahlTore;
    private string _nationalitaet;
    #endregion

    #region Assessoren/Modifikatoren
    public int AnzahlBehandlungen { get => _anzahlBehandlungen; set => _anzahlBehandlungen = value; }
    public int AnzahlTore { get => _anzahlTore; set => _anzahlTore = value; }
    public string Nationalitaet { get => _nationalitaet; set => _nationalitaet = value; }
    #endregion

    #region Konstruktoren
    public Spielerpass()
    {
        _anzahlBehandlungen = 0;
        _anzahlTore = 0;
        _nationalitaet = "Unbekannt";
    }
    public Spielerpass(int anzahlBehandlungen, int anzahlTore, string nationalitaet)
    {
        _anzahlBehandlungen = anzahlBehandlungen;
        _anzahlTore = anzahlTore;
        _nationalitaet = nationalitaet;
    }
    public Spielerpass(Spielerpass andererPass)
    {
        _anzahlBehandlungen = andererPass.AnzahlBehandlungen;
        _anzahlTore = andererPass.AnzahlTore;
        _nationalitaet = andererPass.Nationalitaet;
    }
    #endregion

    #region Methoden
    public string Anzeigen() => $"Nationalität: {_nationalitaet}\nAnzahl Behandlungen: {_anzahlBehandlungen}\nAnzahl Tore: {_anzahlTore}";
    #endregion
}