using System.Text.Json.Serialization;

namespace VereinsVerwaltung;

public class MannschaftsMitglied
{
    #region Eigenschaften
    private string _userName;
    private string _nachname;
    private string _vorname;
    private string _password;
    private bool _istEingeloggt;
    #endregion

    #region Assessoren/Modifikatoren
    public string UserName { get => _userName; set => _userName = value; }
    public string Nachname { get => _nachname; set => _nachname = value; }
    public string Vorname { get => _vorname; set => _vorname = value; }
    public string Password { get => _password; set => _password = value; }
    public bool IstEingeloggt { get => _istEingeloggt; set => _istEingeloggt = value; }
    #endregion

    #region Konstruktoren
    public MannschaftsMitglied()
    {
        _userName = string.Empty;
        _password = string.Empty;
        _vorname = string.Empty;
        _nachname = string.Empty;
        _istEingeloggt = false;
    }
    
    [JsonConstructor]
    public MannschaftsMitglied(string userName, string vorname, string nachname, string password, bool istEingelogt)
    {
        _userName = userName;
        _vorname = vorname;
        _nachname = nachname;
        _password = password;
        _istEingeloggt = istEingelogt;
    }
    
    public MannschaftsMitglied(MannschaftsMitglied anderesMitglied)
    {
        _userName = anderesMitglied._userName;
        _vorname = anderesMitglied._vorname;
        _nachname = anderesMitglied._nachname;
        _password = anderesMitglied._password;
        _istEingeloggt = anderesMitglied._istEingeloggt;
    }
    #endregion

    #region Methoden
    public bool Einloggen(string userName, string password)
    {
        if (userName == _userName && password == _password)
        {
            _istEingeloggt = true;
            return true;
        }
        
        _istEingeloggt = false;
        return false;
    }

    public void Ausloggen() => _istEingeloggt = false;
    
    public bool AendereUsername(string neuerUsername)
    {
        if (!_istEingeloggt || string.IsNullOrWhiteSpace(neuerUsername))
            return false;
        
        _userName = neuerUsername;
        return true;
    }
    #endregion
}