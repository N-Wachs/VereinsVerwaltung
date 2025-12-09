using System.Xml.Serialization;

namespace VereinsVerwaltung;

public class Manschaft
{
    #region Eigenschaften
    private List<ManschaftsMitglied> _maschaftsMitglieder;
    #endregion

    #region Assessoren/Modifikatoren
    private List<ManschaftsMitglied> Manschaftsmitglieder { get => _maschaftsMitglieder; set => _maschaftsMitglieder = value; }
    #endregion

    #region Konstruktoren
    public Manschaft()
    {
        _maschaftsMitglieder = new List<ManschaftsMitglied>();

        #region Mitglieder laden oder Testwerte erstellen
        // Trainer
        Manschaftsmitglieder.Add(new Trainer("sabineschulz", "Sabine", "Schulz", Verschlüsselung.Ver("Sabine"), 12, "Taktik"));
        Manschaftsmitglieder.Add(new Trainer("lucasengel", "Lucas", "Engel", Verschlüsselung.Ver("Lucas"), 3, "Fitness"));

        // Spieler
        Manschaftsmitglieder.Add(new Spieler("erikbecker", "Erik", "Becker", Verschlüsselung.Ver("Erik"),
            new Spielerpass(0, 2, "Deutschland")));

        Manschaftsmitglieder.Add(new Spieler("tomschmidt", "Tom", "Schmidt", Verschlüsselung.Ver("Tom"),
            new Spielerpass(4, 15, "Deutschland")));

        Manschaftsmitglieder.Add(new Spieler("leonweiss", "Leon", "Weiss", Verschlüsselung.Ver("Leon"),
            new Spielerpass(10, 22, "Österreich")));

        Manschaftsmitglieder.Add(new Spieler("danielkurt", "Daniel", "Kurt", Verschlüsselung.Ver("Daniel"),
            new Spielerpass(2, 10, "Schweiz")));

        Manschaftsmitglieder.Add(new Spieler("marcobrecht", "Marco", "Brecht", Verschlüsselung.Ver("Marco"),
            new Spielerpass(7, 18, "Deutschland")));

        // Betreuer
        Manschaftsmitglieder.Add(new Betreuer("jennifermeyer", "Jennifer", "Meyer", Verschlüsselung.Ver("Jennifer"),
            "Physiotherapie", 5));
        #endregion
    }

    /// <summary>
    /// Initializes a new instance of the Manschaft class and loads all team member data from XML files in the specified
    /// directory.
    /// </summary>
    /// <remarks>Only XML files with the ".xml" extension in the specified directory are loaded. Each file
    /// must contain a valid serialized object of type ManschaftsMitglied or its derived types. If the directory does
    /// not exist, no members are loaded.</remarks>
    /// <param name="dateiPfad">The path to the directory containing XML files for team members. Each file must represent a valid
    /// ManschaftsMitglied, Trainer, Spieler, or Betreuer object.</param>
    /// <exception cref="Exception">Thrown if an XML file in the specified directory cannot be loaded or deserialized into a valid team member
    /// object.</exception>
    public Manschaft(string dateiPfad)
    {
        _maschaftsMitglieder = new List<ManschaftsMitglied>();

        if (Path.Exists(dateiPfad))
        {
            XmlSerializer serializer = new XmlSerializer(
                typeof(ManschaftsMitglied),
                new Type[] { typeof(Trainer), typeof(Spieler), typeof(Betreuer) }
            );

            foreach (string path in Directory.GetFiles(dateiPfad, "*.xml"))
            {
                using FileStream fs = new FileStream(path, FileMode.Open);

                ManschaftsMitglied? mitglied = (ManschaftsMitglied?)serializer.Deserialize(fs);

                if (mitglied == null)
                {
                    throw new Exception($"Fehler beim Laden der Datei: {path}");
                }

                _maschaftsMitglieder.Add(mitglied);
            }
        }
    }

    public Manschaft(Manschaft andereManschaft)
    {
        _maschaftsMitglieder = new List<ManschaftsMitglied>(andereManschaft.Manschaftsmitglieder) ?? new List<ManschaftsMitglied>();
    }
    #endregion

    #region Methoden
    public void Add(ManschaftsMitglied neuesMitglied) => _maschaftsMitglieder.Add(neuesMitglied);
    public void Rmv(ManschaftsMitglied mitglied) => _maschaftsMitglieder.Remove(mitglied);
    public int GetNumberOfMitglied(ManschaftsMitglied gesuchtesMitglied)
    {
        int gesuchterIndex = -1;

        foreach (ManschaftsMitglied mitglied in Manschaftsmitglieder)
        {
            // Index erhöhen bis zum gesuchten Mitglied
            if (mitglied.Equals(gesuchtesMitglied))
            {
                break;
            }
            gesuchterIndex++;
        }

        return gesuchterIndex;
    }
    public List<ManschaftsMitglied> GetMitglieder() => Manschaftsmitglieder;

    public void EingeloggtesMitgliedSpeichern(ManschaftsMitglied? eingeloggtesMitglied)
    {
        if (!Path.Exists("Mitglieder"))
        {
            Directory.CreateDirectory("Mitglieder");
        }

        XmlSerializer serializer = new XmlSerializer(
            typeof(ManschaftsMitglied),
                new Type[] { typeof(Trainer), typeof(Spieler), typeof(Spielerpass), typeof(Betreuer) }
        );
        using StreamWriter writer = new StreamWriter("Mitglieder/eingeloggt.xml");
        serializer.Serialize(writer, eingeloggtesMitglied);
        writer.Close();
    }
    public void MitgliederSpeichern()
    {
        if (!Path.Exists("Mitglieder"))
        {
            Directory.CreateDirectory("Mitglieder");
        }

        foreach (ManschaftsMitglied mitglied in Manschaftsmitglieder)
        {
            XmlSerializer serializer = new XmlSerializer(
                typeof(ManschaftsMitglied),
                    new Type[] { typeof(Trainer), typeof(Spieler), typeof(Betreuer), typeof(Spielerpass) }
            );
            using FileStream fs = new FileStream($"Mitglieder/{mitglied.UserName}.xml", FileMode.Create);
            serializer.Serialize(fs, mitglied);
        }
    }
    #endregion
}