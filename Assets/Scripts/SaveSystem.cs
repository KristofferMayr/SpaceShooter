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
        public string playerName;

        public int highestUnlockedLevel = 1;
    }

    private static SaveData currentSave = null;

    // --------------------------
    // SCORE & NAME
    // --------------------------
    public static void SaveScore(int score, string playerName)
    {
        LoadSaveFile(); // Bestehende Daten laden oder neuen Save erstellen

        currentSave.highscore = score;
        currentSave.playerName = playerName;

        SaveToFile();
    }

    public static int LoadScore(out string loadedPlayerName)
    {
        LoadSaveFile();
        loadedPlayerName = currentSave.playerName;
        return currentSave.highscore;
    }

    // --------------------------
    // LEVEL PROGRESS
    // --------------------------
    public static void UnlockLevel(int levelIndex)
    {
        LoadSaveFile();
        if (levelIndex > currentSave.highestUnlockedLevel)
        {
            currentSave.highestUnlockedLevel = levelIndex;
            SaveToFile();
        }
    }

    public static bool IsLevelUnlocked(int levelIndex)
    {
        LoadSaveFile();
        return levelIndex <= currentSave.highestUnlockedLevel;
    }

    public static int GetHighestUnlockedLevel()
    {
        LoadSaveFile();
        return currentSave.highestUnlockedLevel;
    }

    // --------------------------
    // INTERN: Laden/Speichern
    // --------------------------
    private static void LoadSaveFile()
    {
        if (currentSave != null) return;

        if (File.Exists(SAVE_PATH))
        {
            try
            {
                string encryptedJson = File.ReadAllText(SAVE_PATH);
                string json = SimpleDecrypt(encryptedJson);
                currentSave = JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Fehler beim Laden der Save-Datei: {e.Message}");
                currentSave = new SaveData();
            }
        }
        else
        {
            Debug.Log("Keine Save-Datei gefunden, erstelle neue.");
            currentSave = new SaveData();
        }
    }

    private static void SaveToFile()
    {
        try
        {
            string json = JsonUtility.ToJson(currentSave);
            string encrypted = SimpleEncrypt(json);
            File.WriteAllText(SAVE_PATH, encrypted);
            Debug.Log($"Spielstand gespeichert unter: {SAVE_PATH}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Fehler beim Speichern: {e.Message}");
        }
    }

    // --------------------------
    // ENCRYPTION
    // --------------------------
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

    // --------------------------
    // DEBUG / RESET
    // --------------------------
    public static void DeleteSave()
    {
        currentSave = null;
        if (File.Exists(SAVE_PATH))
        {
            File.Delete(SAVE_PATH);
            Debug.Log("Spielstand gelöscht!");
        }
    }
}