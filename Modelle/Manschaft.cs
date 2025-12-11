using System.Xml.Serialization;

namespace VereinsVerwaltung;

public class Mannschaft
{
    #region Eigenschaften
    private List<MannschaftsMitglied> _maschaftsMitglieder;
    #endregion

    #region Assessoren/Modifikatoren
    private List<MannschaftsMitglied> Mannschaftsmitglieder { get => _maschaftsMitglieder; set => _maschaftsMitglieder = value; }
    #endregion

    #region Konstruktoren
    public Mannschaft()
    {
        _maschaftsMitglieder = new List<MannschaftsMitglied>();

        #region Mitglieder laden oder Testwerte erstellen
        // Trainer
        Mannschaftsmitglieder.Add(new Trainer("sabineschulz", "Sabine", "Schulz", Verschlüsselung.Ver("Sabine"), 12, "Taktik"));
        Mannschaftsmitglieder.Add(new Trainer("lucasengel", "Lucas", "Engel", Verschlüsselung.Ver("Lucas"), 3, "Fitness"));

        // Spieler
        Mannschaftsmitglieder.Add(new Spieler("erikbecker", "Erik", "Becker", Verschlüsselung.Ver("Erik"),
            new Spielerpass(0, 2, "Deutschland")));

        Mannschaftsmitglieder.Add(new Spieler("tomschmidt", "Tom", "Schmidt", Verschlüsselung.Ver("Tom"),
            new Spielerpass(4, 15, "Deutschland")));

        Mannschaftsmitglieder.Add(new Spieler("leonweiss", "Leon", "Weiss", Verschlüsselung.Ver("Leon"),
            new Spielerpass(10, 22, "Österreich")));

        Mannschaftsmitglieder.Add(new Spieler("danielkurt", "Daniel", "Kurt", Verschlüsselung.Ver("Daniel"),
            new Spielerpass(2, 10, "Schweiz")));

        Mannschaftsmitglieder.Add(new Spieler("marcobrecht", "Marco", "Brecht", Verschlüsselung.Ver("Marco"),
            new Spielerpass(7, 18, "Deutschland")));

        // Betreuer
        Mannschaftsmitglieder.Add(new Betreuer("jennifermeyer", "Jennifer", "Meyer", Verschlüsselung.Ver("Jennifer"),
            "Physiotherapie", 5));
        #endregion
    }

    /// <summary>
    /// Initializes a new instance of the Mannschaft class and loads all team member data from XML files in the specified
    /// directory.
    /// </summary>
    /// <remarks>Only XML files with the ".xml" extension in the specified directory are loaded. Each file
    /// must contain a valid serialized object of type MannschaftsMitglied or its derived types. If the directory does
    /// not exist, no members are loaded.</remarks>
    /// <param name="dateiPfad">The path to the directory containing XML files for team members. Each file must represent a valid
    /// MannschaftsMitglied, Trainer, Spieler, or Betreuer object.</param>
    /// <exception cref="Exception">Thrown if an XML file in the specified directory cannot be loaded or deserialized into a valid team member
    /// object.</exception>
    public Mannschaft(string dateiPfad)
    {
        _maschaftsMitglieder = new List<MannschaftsMitglied>();

        if (Path.Exists(dateiPfad))
        {
            XmlSerializer serializer = new XmlSerializer(
                typeof(MannschaftsMitglied),
                new Type[] { typeof(Trainer), typeof(Spieler), typeof(Betreuer) }
            );

            foreach (string path in Directory.GetFiles(dateiPfad, "*.xml"))
            {
                using FileStream fs = new FileStream(path, FileMode.Open);

                MannschaftsMitglied? mitglied = (MannschaftsMitglied?)serializer.Deserialize(fs);

                if (mitglied == null)
                {
                    throw new Exception($"Fehler beim Laden der Datei: {path}");
                }

                _maschaftsMitglieder.Add(mitglied);
            }
        }
    }

    public Mannschaft(Mannschaft andereMannschaft)
    {
        _maschaftsMitglieder = new List<MannschaftsMitglied>(andereMannschaft.Mannschaftsmitglieder) ?? new List<MannschaftsMitglied>();
    }
    #endregion

    #region Methoden
    public void Add(MannschaftsMitglied neuesMitglied) => _maschaftsMitglieder.Add(neuesMitglied);
    public void Rmv(MannschaftsMitglied mitglied) => _maschaftsMitglieder.Remove(mitglied);
    public List<MannschaftsMitglied> GetMitglieder() => Mannschaftsmitglieder;

    public void EingeloggtesMitgliedSpeichern(MannschaftsMitglied? eingeloggtesMitglied)
    {
        if (!Path.Exists("Mitglieder"))
        {
            Directory.CreateDirectory("Mitglieder");
        }

        XmlSerializer serializer = new XmlSerializer(
            typeof(MannschaftsMitglied),
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

        foreach (MannschaftsMitglied mitglied in Mannschaftsmitglieder)
        {
            XmlSerializer serializer = new XmlSerializer(
                typeof(MannschaftsMitglied),
                    new Type[] { typeof(Trainer), typeof(Spieler), typeof(Betreuer), typeof(Spielerpass) }
            );
            using FileStream fs = new FileStream($"Mitglieder/{mitglied.UserName}.xml", FileMode.Create);
            serializer.Serialize(fs, mitglied);
        }
    }
    #endregion
}