---
title: Dictionary
description: Extensions pour les dictionnaires
---

## Méthodes

### GetRandomKey()

Récupère une clé aléatoire.

```cs
TKey GetRandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dict)
```

Exemple :

```cs
var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
string randomKey = dict.GetRandomKey();
```

## Code

```cs
namespace Tools
{
    public static class DictionaryExtension
    {
        public static TKey GetRandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            if (dict == null || dict.Count == 0)
                throw new InvalidOperationException("Le dictionnaire est vide.");

            int index = UnityEngine.Random.Range(0, dict.Count);
            var keys = new List<TKey>(dict.Keys);
            return keys[index];
        }
    }
}
```