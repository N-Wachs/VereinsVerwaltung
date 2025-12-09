using System;
using System.Text;

namespace VereinsVerwaltung;

public class Verschlüsselung
{
    public static string Ver(string inhalt, string Key = "?ß*§|oeijsdnmöal.-'")
    {
        #region Lokale Variablen
        byte[] verschlüsselt;
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
        byte[] inhaltBytes = Encoding.UTF8.GetBytes(inhalt);
        verschlüsselt = new byte[inhaltBytes.Length];
        
        for (int i = 0; i < inhaltBytes.Length; i++)
        {
            verschlüsselt[i] = (byte)((inhaltBytes[i] + Key[keyIndex % Key.Length]) % 256);
            keyIndex++;
        }
        #endregion

        return Convert.ToBase64String(verschlüsselt);
    }

    public static string Ent(string inhalt, string Key = "?ß*§|oeijsdnmöal.-'")
    {
        #region Lokale Variablen
        byte[] entschlüsselt;
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
        try
        {
            byte[] inhaltBytes = Convert.FromBase64String(inhalt);
            entschlüsselt = new byte[inhaltBytes.Length];
            
            for (int i = 0; i < inhaltBytes.Length; i++)
            {
                entschlüsselt[i] = (byte)((inhaltBytes[i] - Key[keyIndex % Key.Length] + 256) % 256);
                keyIndex++;
            }
            
            return Encoding.UTF8.GetString(entschlüsselt);
        }
        catch
        {
            return "";
        }
        #endregion
    }
}
