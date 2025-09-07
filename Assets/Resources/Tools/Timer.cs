using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    public event Action OnStarted;
    public event Action OnStoped;
    public event Action<float> OnTick;

    private Coroutine timerCoroutine;

    public float CurrentTime { get; private set; }
    private float maxTime;

    public float TickTime
    {
        get => tickTime;
        set => tickTime = Mathf.Max(0, value);
    }
    private float tickTime;
    private float lastTick;

    public float TimeRatio => CurrentTime / maxTime;
    public bool IsRunning => CurrentTime > 0;

    public void Start(float second)
    {
        maxTime = second;
        CurrentTime = second;
        lastTick = second;
        timerCoroutine = CoroutineManager.StartCoroutineAsynk(TimerRoutine());
        OnStarted?.Invoke();
    }
    public void Stop()
    {
        Reset();                
        OnStoped?.Invoke();
    }

    public void Reset()
    {
        if (timerCoroutine != null)
        {
            CurrentTime = 0;
            CoroutineManager.StopCoroutineAsynk(timerCoroutine);
        }
    }

    private IEnumerator TimerRoutine()
    {
        while (CurrentTime > 0)
        {
            yield return null;

            CurrentTime -= Time.deltaTime;

            if (lastTick - CurrentTime >= tickTime)
            {
                lastTick = CurrentTime;
                OnTick?.Invoke(CurrentTime);
            }
        }

        Stop();
    }
}
