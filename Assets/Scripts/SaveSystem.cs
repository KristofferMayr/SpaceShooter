using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SAVE_PATH = Path.Combine(Application.persistentDataPath, "savegame.json");
    private static readonly string ENCRYPTION_KEY = "PASSWORD";

    [Serializable]
    private class SaveData
    {
        public List<int> levelIndexes = new List<int>();
        public List<int> highscoreValues = new List<int>();
        public string playerName;
        public int highestUnlockedLevel = 1;

        public Dictionary<int, int> ToDictionary()
        {
            var dictionary = new Dictionary<int, int>();
            for (int i = 0; i < levelIndexes.Count; i++)
            {
                if (i < highscoreValues.Count) // Sicherstellen, dass der Index existiert
                {
                    dictionary[levelIndexes[i]] = highscoreValues[i];
                }
            }
            return dictionary;
        }

        public void FromDictionary(Dictionary<int, int> dictionary)
        {
            levelIndexes = new List<int>(dictionary.Keys);
            highscoreValues = new List<int>(dictionary.Values);
        }
    }

    private static SaveData currentSave = null;

    // --------------------------
    // SCORE & NAME
    // --------------------------
    public static void SaveScore(int levelIndex, int score, string playerName)
    {
        LoadSaveFile();
        
        // Konvertiere zu Dictionary für die Bearbeitung
        var highscores = currentSave.ToDictionary();
        
        // Highscore aktualisieren wenn nötig
        if (!highscores.ContainsKey(levelIndex) || score > highscores[levelIndex])
        {
            highscores[levelIndex] = score;
            currentSave.FromDictionary(highscores);
            currentSave.playerName = playerName;
            SaveToFile();
            Debug.Log($"Highscore für Level {levelIndex} gespeichert: {score}");
        }
    }

    public static int LoadScore(int levelIndex, out string loadedPlayerName)
    {
        LoadSaveFile();
        loadedPlayerName = currentSave.playerName;
        
        var highscores = currentSave.ToDictionary();
        if (highscores.TryGetValue(levelIndex, out int score))
        {
            Debug.Log($"Highscore für Level {levelIndex} geladen: {score}");
            return score;
        }
        Debug.Log($"Kein Highscore für Level {levelIndex} gefunden, gebe 0 zurück");
        return 0;
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
            Debug.Log($"Level {levelIndex} freigeschaltet!");
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
                
                // Initialisiere Listen falls null
                if (currentSave.levelIndexes == null) currentSave.levelIndexes = new List<int>();
                if (currentSave.highscoreValues == null) currentSave.highscoreValues = new List<int>();
                
                Debug.Log($"Spielstand geladen. Highscores: {currentSave.levelIndexes.Count} Einträge");
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

    // --------------------------
    // ZUSÄTZLICHE METHODEN
    // --------------------------
    public static Dictionary<int, int> GetAllHighscores()
    {
        LoadSaveFile();
        return currentSave.ToDictionary();
    }

    public static void ResetLevelHighscore(int levelIndex)
    {
        LoadSaveFile();
        var highscores = currentSave.ToDictionary();
        if (highscores.ContainsKey(levelIndex))
        {
            highscores.Remove(levelIndex);
            currentSave.FromDictionary(highscores);
            SaveToFile();
            Debug.Log($"Highscore für Level {levelIndex} zurückgesetzt");
        }
    }

    // Debug-Methode zur Anzeige aller Highscores
    public static void DebugPrintAllHighscores()
    {
        var highscores = GetAllHighscores();
        Debug.Log("Aktuelle Highscores:");
        foreach (var entry in highscores)
        {
            Debug.Log($"Level {entry.Key}: {entry.Value}");
        }
    }
}