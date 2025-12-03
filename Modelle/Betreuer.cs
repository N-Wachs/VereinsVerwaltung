using System.Text.Json.Serialization;

namespace VereinsVerwaltung;

/// <summary>
/// Repräsentiert einen Betreuer, der Spieler medizinisch versorgt und behandelt.
/// Erbt von ManschaftsMitglied und hat Rechte zur Behandlung von Spielern.
/// </summary>
public class Betreuer : ManschaftsMitglied
{
    #region Eigenschaften
    private string _fachgebiet;                     // z.B. "Physiotherapie", "Sportmedizin", "Massage"
    private int _jahreErfahrung;                    // Jahre als Betreuer tätig
    private int _anzahlBehandlungenGesamt;          // Gesamtanzahl durchgeführter Behandlungen
    private DateTime _einstellungsdatum;            // Datum der Einstellung
    #endregion

    #region Assessoren/Modifikatoren
    
    /// <summary>
    /// Fachgebiet des Betreuers.
    /// </summary>
    public string Fachgebiet 
    { 
        get => _fachgebiet; 
        set => _fachgebiet = IstEingeloggt ? value : _fachgebiet; 
    }
    
    /// <summary>
    /// Jahre an Erfahrung als Betreuer.
    /// </summary>
    public int JahreErfahrung => _jahreErfahrung;
    
    /// <summary>
    /// Gesamtanzahl der durchgeführten Behandlungen.
    /// </summary>
    public int AnzahlBehandlungenGesamt => _anzahlBehandlungenGesamt;
    
    /// <summary>
    /// Einstellungsdatum des Betreuers.
    /// </summary>
    public DateTime Einstellungsdatum => _einstellungsdatum;
    #endregion

    #region Konstruktoren
    /// <summary>
    /// Standard-Konstruktor: Erstellt einen Betreuer.
    /// </summary>
    public Betreuer() : base()
    {
        _fachgebiet = "Allgemein";
        _jahreErfahrung = 0;
        _anzahlBehandlungenGesamt = 0;
        _einstellungsdatum = DateTime.Now;
    }

    /// <summary>
    /// Json Konstruktor für Deserialisierung.
    /// </summary>
    [JsonConstructor]
    public Betreuer(string userName, string vorname, string nachname, string password,
                    string fachgebiet, int jahreErfahrung, int anzahlBehandlungenGesamt,
                    DateTime einstellungsdatum)
                    : base(userName, vorname, nachname, password, false)
    {
        _fachgebiet = fachgebiet;
        _jahreErfahrung = jahreErfahrung;
        _anzahlBehandlungenGesamt = anzahlBehandlungenGesamt;
        _einstellungsdatum = einstellungsdatum;
    }

    /// <summary>
    /// Konstruktor mit Benutzerdaten.
    /// </summary>
    public Betreuer(string userName, string vorname, string nachname, string password) 
        : base(userName, vorname, nachname, password, false)
    {
        _fachgebiet = "Allgemein";
        _jahreErfahrung = 0;
        _anzahlBehandlungenGesamt = 0;
        _einstellungsdatum = DateTime.Now;
    }
    
    /// <summary>
    /// Vollständiger Konstruktor mit allen Parametern.
    /// </summary>
    public Betreuer(string userName, string vorname, string nachname, string password,
                    string fachgebiet, int jahreErfahrung)
                    : base(userName, vorname, nachname, password, false)
    {
        _fachgebiet = fachgebiet;
        _jahreErfahrung = jahreErfahrung;
        _anzahlBehandlungenGesamt = 0;
        _einstellungsdatum = DateTime.Now;
    }
    
    /// <summary>
    /// Copy-Konstruktor.
    /// </summary>
    public Betreuer(Betreuer andererBetreuer) : base(andererBetreuer)
    {
        _fachgebiet = andererBetreuer.Fachgebiet;
        _jahreErfahrung = andererBetreuer.JahreErfahrung;
        _anzahlBehandlungenGesamt = andererBetreuer.AnzahlBehandlungenGesamt;
        _einstellungsdatum = andererBetreuer.Einstellungsdatum;
    }
    #endregion

    #region Methoden
    
    /// <summary>
    /// Behandelt einen Spieler und erhöht die Behandlungszähler.
    /// Nur möglich wenn der Betreuer eingeloggt ist.
    /// </summary>
    /// <param name="spieler">Der zu behandelnde Spieler</param>
    /// <returns>True wenn die Behandlung erfolgreich war, sonst false</returns>
    public bool SpielerBehandeln(Spieler spieler)
    {
        if (!IstEingeloggt || spieler == null)
            return false;
        
        // Erhöhe Behandlungszähler beim Spieler
        spieler.Pass.AnzahlBehandlungen++;
        
        // Erhöhe Behandlungszähler beim Betreuer
        _anzahlBehandlungenGesamt++;
        
        return true;
    }
    
    /// <summary>
    /// Behandelt einen Spieler mit einer Notiz/Diagnose.
    /// </summary>
    /// <param name="spieler">Der zu behandelnde Spieler</param>
    /// <param name="diagnose">Diagnose oder Behandlungsnotiz</param>
    /// <returns>True wenn die Behandlung erfolgreich war, sonst false</returns>
    public bool SpielerBehandeln(Spieler spieler, string diagnose)
    {
        if (!IstEingeloggt || spieler == null || string.IsNullOrWhiteSpace(diagnose))
            return false;
        
        // Erhöhe Behandlungszähler beim Spieler
        spieler.Pass.AnzahlBehandlungen++;
        
        // Erhöhe Behandlungszähler beim Betreuer
        _anzahlBehandlungenGesamt++;
        
        return true;
    }
        
    /// <summary>
    /// Berechnet die durchschnittliche Anzahl an Behandlungen pro Jahr Erfahrung.
    /// </summary>
    /// <returns>Durchschnittliche Behandlungen pro Jahr</returns>
    public double BehandlungenProJahr()
    {
        if (_jahreErfahrung <= 0)
            return _anzahlBehandlungenGesamt;
        
        return (double)_anzahlBehandlungenGesamt / _jahreErfahrung;
    }

    /// <summary>
    /// Gibt eine formatierte Übersicht über den Betreuer zurück.
    /// </summary>
    public string BetreuerInfo => $"Betreuer: {UserName}\n" +
                                  $"Name: {Vorname} {Nachname}\n" +
                                  $"Fachgebiet: {_fachgebiet}\n" +
                                  $"Erfahrung: {_jahreErfahrung} Jahre\n" +
                                  $"Behandlungen gesamt: {_anzahlBehandlungenGesamt}\n";
    #endregion
}
