---
title: Dictionary
description: Extensions pour les dictionnaires
---

Extension utilitaire pour simplifier les opérations courantes sur les dictionnaires.

## Utilisation

```cs
using Tools;
```

## Méthodes disponibles

### GetRandomKey()
Récupère une clé aléatoire du dictionnaire en utilisant le générateur de nombres aléatoires d'Unity.

**Syntaxe :**
```cs
TKey GetRandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dict)
```

**Exemple :**
```cs
var weapons = new Dictionary<string, int>
{
    { "sword", 10 },
    { "bow", 15 },
    { "staff", 20 }
};

string randomWeapon = weapons.GetRandomKey();
// Retourne "sword", "bow" ou "staff" aléatoirement
```

**Notes :**
- Lève une `InvalidOperationException` si le dictionnaire est vide ou null
- Utilise `UnityEngine.Random` pour la génération aléatoire

## Code source

```cs
namespace Tools
{
    public static class DictionnaryExtension
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