namespace VereinsVerwaltung;

/// <summary>
/// Repräsentiert einen Trainer, der eine Mannschaft betreut und verwaltet.
/// Erbt von MannschaftsMitglied und hat erweiterte Rechte zur Spielerverwaltung.
/// </summary>
public class Trainer : MannschaftsMitglied
{
    #region Eigenschaften
    private DateTime _einstellungsdatum;// Datum der Vertragsunterzeichnung
    private int _jahreErfahrung;        // Jahre als Trainer tätig
    private string _spezialisierung;    // z.B. "Jugendtrainer", "Torwarttrainer", "Athletiktrainer"
    private List<string> _erfolge;      // Liste von Erfolgen (Meisterschaften, Pokale, etc.)
    #endregion

    #region Assessoren/Modifikatoren
    
    /// <summary>
    /// Einstellungsdatum des Trainers.
    /// </summary>
    public DateTime Einstellungsdatum { get => _einstellungsdatum; set => _einstellungsdatum = value; }

    /// <summary>
    /// Jahre an Erfahrung als Trainer.
    /// </summary>
    public int JahreErfahrung { get => _jahreErfahrung; set => _jahreErfahrung = value; }

    /// <summary>
    /// Spezialisierung des Trainers.
    /// </summary>
    public string Spezialisierung { get => _spezialisierung; set => _spezialisierung = value; }
    
    /// <summary>
    /// Liste der Erfolge des Trainers (nur lesbar).
    /// </summary>
    public List<string> Erfolge { get => _erfolge; set => _erfolge = value; }
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
    }
    
    /// <summary>
    /// Vollständiger Konstruktor mit allen Parametern.
    /// </summary>
    public Trainer(string userName, string vorname, string nachname, string password, 
                   int jahreErfahrung, string spezialisierung) 
                   : base(userName, vorname, nachname, password, false)
    {
        _einstellungsdatum = DateTime.Now;
        _jahreErfahrung = jahreErfahrung;
        _spezialisierung = spezialisierung;
        _erfolge = new List<string>();
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
