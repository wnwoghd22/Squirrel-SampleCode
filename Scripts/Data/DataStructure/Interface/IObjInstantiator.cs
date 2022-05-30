using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjInstantiator
{
    public int Type { get; }

    GameObject InstantiatePrefab(GameObject prefab);
}
