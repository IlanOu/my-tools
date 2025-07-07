---
title: Transform
description: Extensions pour les Transform Unity
---

## Méthodes

### ResetLocal()

Remet à zéro les transformations locales.

```cs
Transform ResetLocal(this Transform transform)
```

Exemple :

```cs
childObject.transform.ResetLocal();
// Chaînage possible
newChild.transform.SetParent(parent).ResetLocal();
```

## Code

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