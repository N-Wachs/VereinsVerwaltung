using System.Text.Json.Serialization;

namespace VereinsVerwaltung;

/// <summary>
/// Repräsentiert einen Trainer, der eine Mannschaft betreut und verwaltet.
/// Erbt von ManschaftsMitglied und hat erweiterte Rechte zur Spielerverwaltung.
/// </summary>
public class Trainer : ManschaftsMitglied
{
    #region Eigenschaften
    private DateTime _einstellungsdatum;// Datum der Vertragsunterzeichnung
    private int _jahreErfahrung;        // Jahre als Trainer tätig
    private string _spezialisierung;    // z.B. "Jugendtrainer", "Torwarttrainer", "Athletiktrainer"
    private List<string> _erfolge;      // Liste von Erfolgen (Meisterschaften, Pokale, etc.)
    private int _gehalt;            // Monatliches Gehalt
    #endregion

    #region Assessoren/Modifikatoren
    
    /// <summary>
    /// Einstellungsdatum des Trainers.
    /// </summary>
    public DateTime Einstellungsdatum => _einstellungsdatum;
    
    /// <summary>
    /// Jahre an Erfahrung als Trainer.
    /// </summary>
    public int JahreErfahrung => _jahreErfahrung;
    
    /// <summary>
    /// Spezialisierung des Trainers.
    /// </summary>
    public string Spezialisierung 
    { 
        get => _spezialisierung; 
        set => _spezialisierung = IstEingeloggt ? value : _spezialisierung; 
    }
    
    /// <summary>
    /// Liste der Erfolge des Trainers (nur lesbar).
    /// </summary>
    public List<string> Erfolge => _erfolge;
    
    /// <summary>
    /// Monatliches Gehalt (nur für eingeloggte Benutzer sichtbar).
    /// </summary>
    public decimal Gehalt => IstEingeloggt ? _gehalt : 0m;
    #endregion

    #region Konstruktoren
    /// <summary>
    /// Standard-Konstruktor: Erstellt einen Trainer ohne Mannschaft.
    /// </summary>
    public Trainer() : base()
    {
        _einstellungsdatum = DateTime.Now;
        _jahreErfahrung = 0;
        _spezialisierung = "Allgemein";
        _erfolge = new List<string>();
        _gehalt = 0;
    }

    /// <summary>
    /// Json Konstruktor.
    /// </summary>
    [JsonConstructor]
    public Trainer(string userName, string vorname, string nachname, string password, 
                   DateTime einstellungsdatum, int jahreErfahrung, string spezialisierung, 
                   List<string> erfolge, int gehalt) 
                   : base(userName, vorname, nachname, password, false)
    {
        _einstellungsdatum = einstellungsdatum;
        _jahreErfahrung = jahreErfahrung;
        _spezialisierung = spezialisierung;
        _erfolge = erfolge ?? new List<string>();
        _gehalt = gehalt;
    }

    /// <summary>
    /// Konstruktor mit Benutzerdaten.
    /// </summary>
    public Trainer(string userName, string vorname, string nachname, string password) : base(userName, vorname, nachname, password, false)
    {
        _einstellungsdatum = DateTime.Now;
        _jahreErfahrung = 0;
        _spezialisierung = "Allgemein";
        _erfolge = new List<string>();
        _gehalt = 0;
    }
    
    /// <summary>
    /// Vollständiger Konstruktor mit allen Parametern.
    /// </summary>
    public Trainer(string userName, string vorname, string nachname, string password, 
                   int jahreErfahrung, string spezialisierung, int gehalt) 
                   : base(userName, vorname, nachname, password, false)
    {
        _einstellungsdatum = DateTime.Now;
        _jahreErfahrung = jahreErfahrung;
        _spezialisierung = spezialisierung;
        _erfolge = new List<string>();
        _gehalt = gehalt;
    }
    
    /// <summary>
    /// Copy-Konstruktor.
    /// </summary>
    public Trainer(Trainer andererTrainer) : base(andererTrainer)
    {
        _einstellungsdatum = andererTrainer.Einstellungsdatum;
        _jahreErfahrung = andererTrainer.JahreErfahrung;
        _spezialisierung = andererTrainer.Spezialisierung;
        _erfolge = new List<string>(andererTrainer.Erfolge);
        _gehalt = andererTrainer._gehalt;
    }
    #endregion

    #region Methoden                
    /// <summary>
    /// Gibt eine formatierte Übersicht über den Trainer zurück.
    /// </summary>
    public string TrainerInfo => $"Trainer: {UserName}\n" +
                                 $"Erfahrung: {_jahreErfahrung} Jahre\n" +
                                 $"Spezialisierung: {_spezialisierung}\n" +
                                 $"Erfolge: {_erfolge.Count}";
    #endregion
}
