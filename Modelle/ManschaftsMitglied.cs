namespace VereinsVerwaltung;

public class ManschaftsMitglied
{
    #region Eigenschaften
    private string _userName;
    private string _password;
    private bool _istEingeloggt;
    #endregion

    #region Assessoren/Modifikatoren
    public string UserName 
    { 
        get => _userName; 
        set => _userName = _istEingeloggt ? value : _userName; 
    }
    public string Password => _istEingeloggt ? _password : string.Empty;
    public bool IstEingeloggt => _istEingeloggt;
    #endregion

    #region Konstruktoren
    public ManschaftsMitglied()
    {
        _userName = string.Empty;
        _password = string.Empty;
        _istEingeloggt = false;
    }
    public ManschaftsMitglied(string userName, string password, bool istEingelogt)
    {
        _userName = userName;
        _password = password;
        _istEingeloggt = istEingelogt;
    }
    public ManschaftsMitglied(ManschaftsMitglied anderesMitglied)
    {
        _userName = anderesMitglied.UserName;
        _password = anderesMitglied._password;
        _istEingeloggt = anderesMitglied.IstEingeloggt;
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
        else
        {
            _istEingeloggt = false;
            return false;
        }
    }

    public void Ausloggen()
    {
        _istEingeloggt = false;
    }
    #endregion
}