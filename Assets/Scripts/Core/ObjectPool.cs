
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Stack<T> pool = new Stack<T>();

    public ObjectPool(T prefab, Transform parent = null, int initialSize = 0)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialSize; i++) pool.Push(CreateNew());
    }

    private T CreateNew()
    {
        var go = UnityEngine.Object.Instantiate(prefab, parent);
        go.gameObject.SetActive(false);
        return go;
    }

    public T Get()
    {
        if (pool.Count == 0) return CreateNew();
        var item = pool.Pop();
        item.gameObject.SetActive(true);
        return item;
    }

    public void Return(T item)
    {
        item.gameObject.SetActive(false);
        pool.Push(item);
    }
}
