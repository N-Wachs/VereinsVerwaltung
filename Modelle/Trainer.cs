namespace VereinsVerwaltung;

public class Trainer : ManschaftsMitglied
{
    #region Eigenschaften
    private Manschaft _zugeordneteManschaft;
    #endregion

    #region Assessoren/Modifikatoren
    public Manschaft ZugeordneteManschaft => _zugeordneteManschaft;
    #endregion

    #region Konstruktoren
    public Trainer() : base()
    {
        _zugeordneteManschaft = new Manschaft();
    }
    public Trainer(Manschaft zugeordneteManschaft) : base()
    {
        _zugeordneteManschaft = zugeordneteManschaft;
    }
    public Trainer(Trainer andererTrainer) : base(andererTrainer)
    {
        _zugeordneteManschaft = andererTrainer.ZugeordneteManschaft;
    }
    #endregion

    #region Methoden
    public void MannschaftsMitgliedHinzufuegen(ManschaftsMitglied neuesMitglied)
    {
        if (base.IstEingeloggt)
            _zugeordneteManschaft.MitgliedHinzufuegen(neuesMitglied);
    }
    public void MannschaftsMitgliedEntfernen(ManschaftsMitglied mitglied)
    {
        if (base.IstEingeloggt)
            _zugeordneteManschaft.MitgliedEntfernen(mitglied);
    }
    #endregion
}
