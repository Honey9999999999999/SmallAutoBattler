using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        Destroy(gameObject);
    }

    public static Coroutine StartCoroutineAsynk(IEnumerator routine)
    {
        return instance.StartCoroutine(routine);
    }

    public static void StopCoroutineAsynk(Coroutine coroutine)
    {
        instance.StopCoroutine(coroutine);
    }
}
