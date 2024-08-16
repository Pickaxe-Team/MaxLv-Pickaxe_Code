using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public T ReturnMyComponent<T>() where T : PoolObject
    {
        T component = this as T;

        if (component == null)
        {
            return null;
        }

        return component;
    }
}