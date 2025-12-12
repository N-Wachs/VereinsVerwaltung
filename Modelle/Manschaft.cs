using System.Collections;
using System.Text.Json;
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
                if (path.Contains("List"))
                {
                    continue;
                }
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

    public void EingeloggtesMitgliedSpeichernXML(MannschaftsMitglied? eingeloggtesMitglied)
    {
        if (!Path.Exists("MitgliederXML"))
        {
            Directory.CreateDirectory("MitgliederXML");
        }

        XmlSerializer serializer = new XmlSerializer(
            typeof(MannschaftsMitglied),
                new Type[] { typeof(Trainer), typeof(Spieler), typeof(Spielerpass), typeof(Betreuer) }
        );
        using StreamWriter writer = new StreamWriter("MitgliederXML/eingeloggt.xml");
        serializer.Serialize(writer, eingeloggtesMitglied);
        writer.Close();
    }
    public void MitgliederSpeichernXML()
    {
        if (!Path.Exists("MitgliederXML"))
        {
            Directory.CreateDirectory("MitgliederXML");
        }
        else
        {
            foreach (string path in Directory.GetFiles("MitgliederXML"))
            {
                File.Delete(path);
            }
        }

        XmlSerializer serializer = new XmlSerializer(
            typeof(MannschaftsMitglied),
                new Type[] { typeof(Trainer), typeof(Spieler), typeof(Betreuer), typeof(Spielerpass), typeof(ArrayList) }
        );
        ArrayList arrayList = new ArrayList();
        foreach (MannschaftsMitglied mitglied in Mannschaftsmitglieder)
        {
            using StreamWriter writer = new StreamWriter($"MitgliederXML/{mitglied.UserName}.xml", false);
            serializer.Serialize(writer, mitglied);
            arrayList.Add(mitglied);
        }

        serializer = new XmlSerializer(
    typeof(ArrayList),
        new Type[] { typeof(Trainer), typeof(Spieler), typeof(Betreuer), typeof(Spielerpass) }
);
        using StreamWriter writer1 = new StreamWriter($"MitgliederXML/Liste.xml", false);
        serializer.Serialize(writer1, arrayList);
    }


    public void EingeloggtesMitgliedSpeichernJSON(MannschaftsMitglied? eingeloggtesMitglied)
    {
        if (!Path.Exists("MitgliederJSON"))
        {
            Directory.CreateDirectory("MitgliederJSON");
        }

        if (eingeloggtesMitglied == null) return;

        using StreamWriter writer = new StreamWriter($"MitgliederJSON/{eingeloggtesMitglied}.json", false);
        writer.WriteLine(JsonSerializer.Serialize(eingeloggtesMitglied));
        writer.Close();
    }
    public void MitgliederSpeichernJSON()
    {
        if (!Path.Exists("MitgliederJSON"))
        {
            Directory.CreateDirectory("MitgliederJSON");
        }
        else
        {
            foreach (string path in Directory.GetFiles("MitgliederJSON"))
            {
                File.Delete(path);
            }
        }

        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        ArrayList arrayList = new ArrayList();
        foreach (MannschaftsMitglied mitglied in Mannschaftsmitglieder)
        {
            using StreamWriter writer = new StreamWriter($"MitgliederJSON/{mitglied.UserName}.json", false);
            writer.WriteLine(JsonSerializer.Serialize(mitglied, options));
            writer.Close();
            arrayList.Add(mitglied);
        }

        using StreamWriter writer1 = new StreamWriter("MitgliederJSON/List.json", false);
        writer1.WriteLine(JsonSerializer.Serialize(arrayList, options));
        writer1.Close();
    }

    public MannschaftsMitglied? EingeloggtesMitgliedLadenJSON()
    {
        if (!Directory.Exists("MitgliederJSON"))
        {
            Directory.CreateDirectory("MitgliederJSON");
            return null;
        }

        string jsonInhalt = string.Empty;
        MannschaftsMitglied? resultat = null;
        StreamReader reader = new StreamReader("MitgliederJSON/eingeloggt.json");
        jsonInhalt = reader.ReadToEnd();
        reader.Close();

        resultat = JsonSerializer.Deserialize<MannschaftsMitglied>(jsonInhalt);

        return resultat;
    }
    public void MitgliederLadenJSON()
    {
        if (!Directory.Exists("MitgliederJSON"))
        {
            Directory.CreateDirectory("MitliederJSON");
            return;
        }

        if (Directory.GetFiles("MitlgiederJSON").Count() == 0)
        {
            return;
        }

        Mannschaftsmitglieder = new List<MannschaftsMitglied>();
        foreach (string path in Directory.GetFiles("MitgliederJSON"))
        {
            if (path.Contains("List")) continue;
            using StreamReader reader = new StreamReader(path);
            string jsonInhalt = reader.ReadToEnd().Trim();
            reader.Close();
            Mannschaftsmitglieder.Add(JsonSerializer.Deserialize<MannschaftsMitglied>(path) ?? new Spieler());
        }
    }
    #endregion
}