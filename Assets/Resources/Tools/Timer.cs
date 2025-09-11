using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    public event Action OnStarted;
    public event Action OnStoped;
    public event Action OnTick;

    private Coroutine timerCoroutine;

    public float CurrentTime { get; private set; }
    public float CurrentTickTime { get; private set; }
    private float maxTime;

    public float MaxTickTime
    {
        get => maxTickTime;
        set => maxTickTime = Mathf.Max(0, value);
    }
    private float maxTickTime;
    private float lastTick;

    public float TimeRatio => CurrentTime / maxTime;
    public bool IsRunning => CurrentTime > 0;

    public void Start(float second)
    {
        if(timerCoroutine == null)
        {
            maxTime = second;
            CurrentTime = second;
            lastTick = second;
            timerCoroutine = CoroutineManager.StartCoroutineAsynk(TimerRoutine());
            OnStarted?.Invoke();
        }        
    }
    public void StartTicks(float second)
    {
        if (timerCoroutine == null)
        {
            MaxTickTime = second;
            timerCoroutine = CoroutineManager.StartCoroutineAsynk(TimerTickRoutine());
            OnStarted?.Invoke();
        }
    }

    public void Stop()
    {
        Reset();
        OnStoped?.Invoke();
    }

    public void Reset()
    {
        CurrentTime = 0;
        CurrentTickTime = 0;

        if (timerCoroutine != null)
        {            
            CoroutineManager.StopCoroutineAsynk(timerCoroutine);
        }
    }

    public void Resume()
    {
        if (timerCoroutine == null)
        {
            if (CurrentTime > 0)
            {
                timerCoroutine = CoroutineManager.StartCoroutineAsynk(TimerRoutine());
                return;
            }

            if (CurrentTickTime > 0)
            {
                timerCoroutine = CoroutineManager.StartCoroutineAsynk(TimerTickRoutine());
                return;
            }
        }        
    }
    public void Pause()
    {
        if (timerCoroutine != null)
        {
            CoroutineManager.StopCoroutineAsynk(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator TimerRoutine()
    {
        while (CurrentTime > 0)
        {
            yield return null;

            CurrentTime -= Time.deltaTime;
            CurrentTickTime = lastTick - CurrentTime;

            if (CurrentTickTime >= maxTickTime)
            {
                lastTick = CurrentTime;
                OnTick?.Invoke();
            }
        }

        Stop();
    }

    private IEnumerator TimerTickRoutine()
    {
        while (true)
        {
            yield return null;

            CurrentTickTime += Time.deltaTime;

            if (CurrentTickTime >= maxTickTime)
            {                
                OnTick?.Invoke();
                CurrentTickTime = 0.001f;
            }
        }
    }
}
