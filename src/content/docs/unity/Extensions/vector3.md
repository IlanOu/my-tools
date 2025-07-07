---
title: Vector3
description: Extensions pour les Vector3 Unity
---

Extension utilitaire pour simplifier les opérations courantes sur les Vector3 Unity.

## Utilisation

```cs
using Tools;
```

## Méthodes disponibles

### With()
Crée un nouveau Vector3 en remplaçant sélectivement certaines composantes.

**Syntaxe :**
```cs
Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
```

**Exemple :**
```cs
Vector3 position = new Vector3(1, 2, 3);

// Change seulement Y
Vector3 newPos = position.With(y: 5f); // (1, 5, 3)

// Change X et Z
Vector3 another = position.With(x: 0f, z: 10f); // (0, 2, 10)
```

### Add()
Crée un nouveau Vector3 en ajoutant sélectivement des valeurs aux composantes.

**Syntaxe :**
```cs
Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
```

**Exemple :**
```cs
Vector3 position = new Vector3(1, 2, 3);

// Ajoute seulement à Y
Vector3 higher = position.Add(y: 2f); // (1, 4, 3)

// Ajoute à X et Z
Vector3 moved = position.Add(x: -1f, z: 5f); // (0, 2, 8)
```

**Notes :**
- Les paramètres non spécifiés (null) conservent leur valeur originale
- Ces méthodes créent de nouveaux Vector3 sans modifier l'original

## Code source

```cs
using UnityEngine;

namespace Tools
{
    public static class Vector3Extension
    {
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }

        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
        }
    }
}
```