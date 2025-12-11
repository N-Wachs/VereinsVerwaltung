namespace VereinsVerwaltung;

public class Spielerpass
{
    #region Eigenschaften
    private int _anzahlBehandlungen;
    private int _anzahlTore;
    private string _nationalitaet;
    private List<string> _diagnosen;
    #endregion

    #region Assessoren/Modifikatoren
    public int AnzahlBehandlungen { get => _anzahlBehandlungen; set => _anzahlBehandlungen = value; }
    public int AnzahlTore { get => _anzahlTore; set => _anzahlTore = value; }
    public string Nationalitaet { get => _nationalitaet; set => _nationalitaet = value; }
    public List<string> Diagnosen { get => _diagnosen; set => _diagnosen = value; }
    #endregion

    #region Konstruktoren
    public Spielerpass()
    {
        _anzahlBehandlungen = 0;
        _anzahlTore = 0;
        _nationalitaet = "Unbekannt";
        _diagnosen = new List<string>();
    }
    public Spielerpass(int anzahlBehandlungen, int anzahlTore, string nationalitaet)
    {
        _anzahlBehandlungen = anzahlBehandlungen;
        _anzahlTore = anzahlTore;
        _nationalitaet = nationalitaet;
        _diagnosen = new List<string>();
    }
    public Spielerpass(Spielerpass andererPass)
    {
        _anzahlBehandlungen = andererPass.AnzahlBehandlungen;
        _anzahlTore = andererPass.AnzahlTore;
        _nationalitaet = andererPass.Nationalitaet;
        _diagnosen = new List<string>();
    }
    #endregion

    #region Methoden
    public string Anzeigen()
    {
        string diagnosenAusgabe = string.Empty;
        foreach (string inhalt in Diagnosen) { diagnosenAusgabe += inhalt + "; "; }
        if (diagnosenAusgabe.Length == 0) diagnosenAusgabe = "Keine";
        return $"Nationalität: {Nationalitaet}\nAnzahl Behandlungen: {AnzahlBehandlungen}\nAlle Diagnosen: {diagnosenAusgabe}\nAnzahl Tore: {AnzahlTore}";
    }
    #endregion
}