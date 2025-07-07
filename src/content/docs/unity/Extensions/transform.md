---
title: Transform
description: Extensions pour les Transform Unity
---

Extension utilitaire pour simplifier les opérations courantes sur les Transform Unity.

## Utilisation

```cs
using Tools;
```

## Méthodes disponibles

### ResetLocal()
Remet à zéro toutes les transformations locales (position, rotation, échelle) du Transform.

**Syntaxe :**
```cs
Transform ResetLocal(this Transform transform)
```

**Exemple :**
```cs
// Remet un objet à sa position d'origine par rapport à son parent
childObject.transform.ResetLocal();

// Chaînage possible grâce au retour du Transform
GameObject newChild = new GameObject("Child");
newChild.transform.SetParent(parent).ResetLocal();
```

**Valeurs appliquées :**
- `localPosition` : `Vector3.zero`
- `localRotation` : `Quaternion.identity`
- `localScale` : `Vector3.one`

**Notes :**
- Retourne le Transform pour permettre le chaînage de méthodes
- Affecte uniquement les transformations locales, pas les transformations globales

## Code source

```cs
using UnityEngine;

namespace Tools
{
    public static class TransformExtension
    {
        public static Transform ResetLocal(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            return transform;
        }
    }
}
```