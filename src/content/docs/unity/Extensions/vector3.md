---
title: Vector3
description: Extensions pour les Vector3 Unity
---

## Méthodes

### With()

Crée un nouveau Vector3 en remplaçant certaines composantes.

```cs
Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
```

Exemple :

```cs
Vector3 pos = new Vector3(1, 2, 3);
Vector3 newPos = pos.With(y: 5f); // (1, 5, 3)
```

### Add()

Crée un nouveau Vector3 en ajoutant des valeurs aux composantes.

```cs
Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
```

Exemple :

```cs
Vector3 pos = new Vector3(1, 2, 3);
Vector3 higher = pos.Add(y: 2f); // (1, 4, 3)
```

## Code

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