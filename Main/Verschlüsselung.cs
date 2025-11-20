using System;

namespace VereinsVerwaltung;

public class Verschlüsselung
{
    public static string Ver(string inhalt, string Key = "?ß*§|oeijsdnmöal.-'")
    {
        #region Lokale Variablen
        string Verschlüsselt = "";
        int keyIndex = 0;
        #endregion

        #region Eingabeparameter prüfen
        if (inhalt == null || Key == null)
        {
            return "";
        }
        else if (inhalt == "" ||  Key == "")
        {
            return "";
        }
        #endregion

        #region Algorithmus
        for (int i = 0; i < inhalt.Length; i++)
        {
            Verschlüsselt += (char)((char)(inhalt[i] + Key[keyIndex % Key.Length]) % 256);
            keyIndex++;
        }
        #endregion

        return Verschlüsselt;
    }

    public static string Ent(string inhalt, string Key = "?ß*§|oeijsdnmöal.-'")
    {
        #region Lokale Variablen
        string Entschlüsselt = "";
        int keyIndex = 0;
        #endregion

        #region Eingabeparameter prüfen
        if (inhalt == null || Key == null)
        {
            return "";
        }
        else if (inhalt == "" || Key == "")
        {
            return "";
        }
        #endregion

        #region Algorithmus
        for (int i = 0; i < inhalt.Length; i++)
        {
            Entschlüsselt += (char)((char)(inhalt[i] - Key[keyIndex % Key.Length] + 256) % 256);
            keyIndex++;
        }
        #endregion

        return Entschlüsselt;
    }
}
