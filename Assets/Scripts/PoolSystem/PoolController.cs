using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController
{
    private Dictionary<GameObject, Pool> _prefabDictionary = new Dictionary<GameObject, Pool>();
    private Dictionary<GameObject, Pool> _instanceDictionary = new Dictionary<GameObject, Pool>();

    public GameObject Spawn(GameObject prefab, Transform parent = null)
    {
        if (!_prefabDictionary.ContainsKey(prefab))
        {
            _prefabDictionary[prefab] = new Pool(prefab);
        }

        var pool = _prefabDictionary[prefab];
        var instance = pool.Spawn(parent);
        _instanceDictionary.Add(instance, pool);

        return instance;
    }

    public void Despawn(GameObject instance)
    {
        if (_instanceDictionary.ContainsKey(instance))
        {
            _instanceDictionary[instance].Despawn(instance);
            _instanceDictionary.Remove(instance);
        }
        else if (instance != null)
        {
            // Utils.Log(instance.name + " not found in pool... destroying", Color.cyan);
            GameObject.Destroy(instance);
        }
    }

    public void Warm(GameObject prefab, int amount)
    {
        if (!_prefabDictionary.ContainsKey(prefab))
        {
            _prefabDictionary[prefab] = new Pool(prefab);
        }

        _prefabDictionary[prefab].Warm(amount);
    }

    public void ClearAllInactive()
    {
        foreach(var o in _prefabDictionary)
        {
            o.Value.ClearInactive();
        }

        _prefabDictionary = new Dictionary<GameObject, Pool>();
        _instanceDictionary = new Dictionary<GameObject, Pool>();
    }
}
