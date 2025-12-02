using System.Text.Json;

namespace VereinsVerwaltung;

/// <summary>
/// Zentrales Repository für das Laden und Speichern von Mannschaftsmitgliedern.
/// Verwaltet die Persistierung in JSON-Dateien mit Typ-Erhaltung.
/// </summary>
public class MitgliederRepository
{
    #region Eigenschaften
    private readonly string _mitgliederOrdner = "Mitglieder";
    private readonly JsonSerializerOptions _jsonOptionen;
    #endregion

    #region Konstruktor
    public MitgliederRepository()
    {
        // Konfiguriere JSON-Serialisierung
        _jsonOptionen = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            IncludeFields = true  // Erlaubt Zugriff auf private Felder
        };
        
        // Erstelle Verzeichnis falls nicht vorhanden
        if (!Directory.Exists(_mitgliederOrdner))
        {
            Directory.CreateDirectory(_mitgliederOrdner);
        }
    }
    #endregion

    #region Methoden - Einzelnes Mitglied
    
    /// <summary>
    /// Speichert ein einzelnes Mitglied mit Typ-Information.
    /// </summary>
    public bool SpeichereMitglied(ManschaftsMitglied mitglied, string dateiName)
    {
        try
        {
            string dateiPfad = Path.Combine(_mitgliederOrdner, $"{dateiName}.json");
            string typPfad = Path.Combine(_mitgliederOrdner, $"{dateiName}_typ.txt");
            
            // Serialisiere mit konkretem Typ
            string jsonInhalt = JsonSerializer.Serialize(mitglied, mitglied.GetType(), _jsonOptionen);
            string typName = mitglied.GetType().Name;
            
            File.WriteAllText(dateiPfad, jsonInhalt);
            File.WriteAllText(typPfad, typName);
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    /// <summary>
    /// Lädt ein einzelnes Mitglied mit Typ-Rekonstruktion.
    /// </summary>
    public ManschaftsMitglied? LadeMitglied(string dateiName)
    {
        try
        {
            string dateiPfad = Path.Combine(_mitgliederOrdner, $"{dateiName}.json");
            string typPfad = Path.Combine(_mitgliederOrdner, $"{dateiName}_typ.txt");
            
            if (!File.Exists(dateiPfad) || !File.Exists(typPfad))
                return null;
            
            string jsonInhalt = File.ReadAllText(dateiPfad);
            string typName = File.ReadAllText(typPfad).Trim();
            
            return typName switch
            {
                "Trainer" => JsonSerializer.Deserialize<Trainer>(jsonInhalt, _jsonOptionen),
                "Spieler" => JsonSerializer.Deserialize<Spieler>(jsonInhalt, _jsonOptionen),
                _ => JsonSerializer.Deserialize<ManschaftsMitglied>(jsonInhalt, _jsonOptionen)
            };
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    /// <summary>
    /// Löscht die Dateien eines Mitglieds.
    /// </summary>
    public bool LoescheMitglied(string dateiName)
    {
        try
        {
            string dateiPfad = Path.Combine(_mitgliederOrdner, $"{dateiName}.json");
            string typPfad = Path.Combine(_mitgliederOrdner, $"{dateiName}_typ.txt");
            
            if (File.Exists(dateiPfad))
                File.Delete(dateiPfad);
            
            if (File.Exists(typPfad))
                File.Delete(typPfad);
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    #endregion
    
    #region Methoden - Alle Mitglieder
    
    /// <summary>
    /// Lädt alle gespeicherten Mitglieder aus dem Verzeichnis.
    /// </summary>
    public List<ManschaftsMitglied> LadeAlleMitglieder()
    {
        List<ManschaftsMitglied> mitglieder = new List<ManschaftsMitglied>();
        
        if (!Directory.Exists(_mitgliederOrdner))
            return mitglieder;
        
        // Finde alle JSON-Dateien (ohne _typ.txt)
        string[] jsonDateien = Directory.GetFiles(_mitgliederOrdner, "*.json");
        
        foreach (string dateiPfad in jsonDateien)
        {
            string dateiName = Path.GetFileNameWithoutExtension(dateiPfad);
            var mitglied = LadeMitglied(dateiName);
            
            if (mitglied != null)
                mitglieder.Add(mitglied);
        }
        
        return mitglieder;
    }
    
    /// <summary>
    /// Speichert eine komplette Liste von Mitgliedern.
    /// </summary>
    public bool SpeichereAlleMitglieder(List<ManschaftsMitglied> mitglieder)
    {
        try
        {
            foreach (var mitglied in mitglieder)
            {
                SpeichereMitglied(mitglied, mitglied.UserName);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion

    #region Methoden - Eingeloggtes Mitglied

    /// <summary>
    /// Speichert das aktuell eingeloggte Mitglied (für Persistierung des Login-Status).
    /// </summary>
    public bool SpeichereEingeloggtesMitglied(ManschaftsMitglied mitglied) => SpeichereMitglied(mitglied, "eingeloggtesMitglied");

    /// <summary>
    /// Lädt das zuletzt eingeloggte Mitglied.
    /// </summary>
    public ManschaftsMitglied? LadeEingeloggtesMitglied() => LadeMitglied("eingeloggtesMitglied");

    /// <summary>
    /// Löscht die Datei des eingeloggten Mitglieds (Logout).
    /// </summary>
    public bool LoescheEingeloggtesMitglied() => LoescheMitglied("eingeloggtesMitglied");

    #endregion
}