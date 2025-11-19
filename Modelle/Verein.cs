namespace VereinsVerwaltung;

public class Verein
{
    #region Eigenschaften
    private List<Manschaft> _manschaften;
    #endregion

    #region Assessoren/Modifikatoren
    public List<Manschaft> Manschaften => _manschaften;
    #endregion

    #region Konstruktoren
    public Verein()
    {
        _manschaften = new List<Manschaft>();
    }
    public Verein(List<Manschaft> manschaften)
    {
        _manschaften = manschaften;
    }
    public Verein(Verein andererVerein)
    {
        _manschaften = new List<Manschaft>(andererVerein.Manschaften);
    }
    #endregion

    #region Methoden
    public void ManschaftHinzufuegen(Manschaft neueManschaft) => _manschaften.Add(neueManschaft);
    public void ManschaftEntfernen(Manschaft manschaft) => _manschaften.Remove(manschaft);
    #endregion
}