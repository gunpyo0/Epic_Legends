using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float duration;
    float internalTimer;

    public Timer(float dur_)
    {
        duration = dur_;
        TimeManager.now.timers.Add(this);
    }

    public void calc()
    {
        internalTimer += Time.deltaTime;

        if (internalTimer >= float.MaxValue - 10)
        {
            internalTimer = float.MaxValue; // to avoid overflow
        }
    }

    public bool check()
    {
        if (internalTimer >= duration)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void reset(float dur_)
    {
        duration = dur_;
        internalTimer = 0f;
    }

}

public class TimeManager : MonoBehaviour
{
    public static TimeManager now;
    
    public List<Timer> timers;

    void Awake()
    {
        now = this;
        timers = new List<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var time in timers)
        {
            time.calc();
        }
    }
}
