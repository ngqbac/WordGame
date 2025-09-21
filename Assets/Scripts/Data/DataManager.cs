using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Data
{
    public static class DataManager
    {
        public static List<WordEntry> LoadData()
        {
            var cached = Resources.Load<TextAsset>(GameConfig.Instance.cacheDataFile);
            if (cached == null)
            {
                Debug.Log("Try to build word cache");
                BuildData();
                cached = Resources.Load<TextAsset>(GameConfig.Instance.cacheDataFile);
                if (cached == null)
                {
                    Debug.LogError("Failed to load cached word file after build");
                    return new List<WordEntry>();
                }
            }

            var wrapper = JsonUtility.FromJson<WordEntryListWrapper>(cached.text);
            if (wrapper is { entries: not null }) return wrapper.entries;
            Debug.LogError("Failed to deserialize word cache");
            return new List<WordEntry>();
        }

#if UNITY_EDITOR    
        [MenuItem("Data/Build")]
#endif
        public static void BuildData()
        {
            var wordText = Resources.Load<TextAsset>(GameConfig.Instance.dataFile);
            var words = wordText.text
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLowerInvariant())
                .Where(w => w.All(char.IsLetter))
                .Distinct()
                .ToList();

            Debug.Log($"{words.Count} word found");

            var entries = words.Select(w => new WordEntry
            {
                word = w,
                signature = WordSignatureUtils.GetSignature(w)
            }).ToList();
            Debug.Log($"{entries.Count} entries found");
            
            SaveToJson(entries);
        }

        private static void SaveToJson(List<WordEntry> entries)
        {
            var cacheFilePath = $"Assets/Resources/{GameConfig.Instance.cacheDataFile}.json";
            using var writer = new StreamWriter(cacheFilePath);
            writer.WriteLine("{");
            writer.WriteLine("  \"entries\": [");

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                var signature = "[" + string.Join(", ", entry.signature) + "]";
                writer.WriteLine("    {");
                writer.WriteLine($"      \"word\": \"{entry.word}\",");
                writer.WriteLine($"      \"signature\": {signature}");
                writer.Write("    }");
                if (i < entries.Count - 1) writer.Write(",");
                writer.WriteLine();
            }

            writer.WriteLine("  ]");
            writer.WriteLine("}");

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        [Serializable]
        private class WordEntryListWrapper
        {
            public List<WordEntry> entries;
        }
    }
}
