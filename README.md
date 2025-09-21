# Word Game

## Overview

Players are given a random set of letters and must form valid words from them. Each letter scores 1 point, and the goal is to submit high-scoring valid words. A word is only valid if:

- It exists in the dictionary list (`Resources/words.txt`)
- It can be formed using the currently available letters
- It hasn't used up letters already consumed by previous submissions

To make word validation performant, the system uses **word signatures** – precomputed letter frequency arrays.

---

## File Structure

```
Assets/
├── Resources/
│   ├── GameConfig.asset        # ScriptableObject holding config paths
│   ├── words.txt               # Raw list of valid words (1 per line)
│   └── words_cache.json        # Cached word signature data (auto-generated)
├── Scenes/
│   └── Game.unity              # Main Unity scene
├── Scripts/
    ├── Data/
    │   ├── DataConfig.cs
    │   └── DataManager.cs
    ├── GameConfig.cs
    ├── GameUI.cs
    ├── IGame.cs
    ├── UIObjectScore.cs
    ├── WordGame.cs
    └── WordSignatureUtils.cs
``` 

---

## Data Pipeline

### 1. Build Phase (Editor Only)

Triggered manually via Unity menu: **`Data → Build`**

Steps:
1. Load `words.txt` from `Resources/`
2. Normalize and sanitize:
   - Trim and lowercase
   - Remove empty/non-alphabetic entries
   - Remove duplicates
3. Convert to word signatures:
   - `"apple"` → `[1,0,0,0,1,0,...,2,...]`
4. Save as `words_cache.json` in `Resources/`:
   - Minified JSON
   - Each entry: `{ word: "apple", signature: [1,0,...] }`

> Use `GameConfig.dataFile` and `cacheDataFile` to configure these names.

---

### 2. Load Phase (Runtime)

At game launch or reset:
- Load `words_cache.json`
- Deserialize into `List<WordEntry>`
- Build a `HashSet<string>` for fast lookup
- Store in memory for runtime validation

---

## Word Signature Format

Each word is stored as:
```json
{
  "word": "apple",
  "signature": [1,0,0,0,1,0,0,0,0,0,0,1,0,0,0,2,0,0,0,0,0,0,0,0,0,0]
}
```

- Signature length: 26 (for letters a–z)
- Each index represents the count of that letter in the word

This makes it efficient to:
- Check if a word is valid
- Compare it against the current letter pool

---

## Scoring and Rules

- Valid words are scored by their letter count
- Top N scores are shown (see `GameConfig.scoreEntries`)
- Words can't reuse letters already used
- Invalid submissions are ignored

---

## Example Test Cases

### Given Letters: `t e s t i n g`
`set` → 3 points  
`ting` → 4 points  
`testingz` → invalid (z not in letters)  
`test` (twice) → 2nd ignored (letters already used)