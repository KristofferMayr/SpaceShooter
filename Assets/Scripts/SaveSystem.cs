using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SAVE_PATH = Path.Combine(Application.persistentDataPath, "savegame.json");
    private static readonly string ENCRYPTION_KEY = "PASSWORD";

    [Serializable]
    private class SaveData
    {
        public int highscore;
        public string playerName; // Name des Spielers (noch nicht verwendet)
    }

    // Score speichern
    public static void SaveScore(int score, string playerName)
    {
        Debug.Log("Saving under " + SAVE_PATH);
        try
        {
            SaveData data = new SaveData
            {
                highscore = score,
                playerName = playerName
            };

            string json = JsonUtility.ToJson(data);
            string encryptedJson = SimpleEncrypt(json); // Verschlüsseln
            File.WriteAllText(SAVE_PATH, encryptedJson);
            
            Debug.Log($"Spielstand gespeichert unter: {SAVE_PATH}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Speichern fehlgeschlagen: {e.Message}");
        }
    }

    // Score laden
    public static int LoadScore(out string loadedPlayerName)
    {
        loadedPlayerName = "Player"; // Default-Wert
        
        if (!File.Exists(SAVE_PATH))
        {
            Debug.Log("Kein Spielstand gefunden. Neuer Score: 0");
            return 0;
        }

        try
        {
            string encryptedJson = File.ReadAllText(SAVE_PATH);
            string json = SimpleDecrypt(encryptedJson); // Entschlüsseln
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            loadedPlayerName = data.playerName;
            return data.highscore;
        }
        catch (Exception e)
        {
            Debug.LogError($"Laden fehlgeschlagen: {e.Message}");
            return 0;
        }
    }

    // Einfache Verschlüsselung (XOR)
    private static string SimpleEncrypt(string input)
    {
        char[] output = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = (char)(input[i] ^ ENCRYPTION_KEY[i % ENCRYPTION_KEY.Length]);
        }
        return new string(output);
    }

    private static string SimpleDecrypt(string input)
    {
        return SimpleEncrypt(input);
    }

    // Spielstand löschen (für Debugging)
    public static void DeleteSave()
    {
        if (File.Exists(SAVE_PATH))
        {
            File.Delete(SAVE_PATH);
            Debug.Log("Spielstand gelöscht!");
        }
    }
}