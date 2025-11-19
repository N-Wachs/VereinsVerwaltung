namespace VereinsVerwaltung;

public class Manschaft
{
    #region Eigenschaften
    private List<ManschaftsMitglied> _maschaftsMitglieder;
    #endregion

    #region Assessoren/Modifikatoren
    public List<ManschaftsMitglied> Manschaftsmitglieder => _maschaftsMitglieder;
    #endregion

    #region Konstruktoren
    public Manschaft()
    {
        _maschaftsMitglieder = new List<ManschaftsMitglied>();
    }
    public Manschaft(List<ManschaftsMitglied> manschaftsMitglieder)
    {
        _maschaftsMitglieder = manschaftsMitglieder;
    }
    public Manschaft(Manschaft andereManschaft)
    {
        _maschaftsMitglieder = new List<ManschaftsMitglied>(andereManschaft.Manschaftsmitglieder);
    }
    #endregion

    #region Methoden
    public void MitgliedHinzufuegen(ManschaftsMitglied neuesMitglied) => _maschaftsMitglieder.Add(neuesMitglied);
    public void MitgliedEntfernen(ManschaftsMitglied mitglied) => _maschaftsMitglieder.Remove(mitglied);
    #endregion
}