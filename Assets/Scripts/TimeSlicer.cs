using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using UnityEngine;

public class TimeSlicer : MonoBehaviour
{
    public AudioClip Song;
    public int BPM;
    private int _beats;
    [NonSerialized]
    public float BeatLength;

    public delegate void BeatHit();
    public static event BeatHit OnBeat;
    public static event BeatHit OnQuarterBeat;
    private Stopwatch _stopwatch = new Stopwatch();
    private Stopwatch _qbStopwatch = new Stopwatch();

    private float _latencyAdjustment;
    private float _rollingAverageError = 0.0035f;
    private float BeatLengthAdjusted
    {
        get
        {
            return (BeatLength + _latencyAdjustment) - (_rollingAverageError * 2);
        }
    }
    private float QuarterBeatLengthAdjusted
    {
        get
        {
            return ((BeatLength / 4) + _latencyAdjustment) - (_rollingAverageError * 2);
        }
    }

    //public float EarlyBeatTolerance = 0.01f;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 300;
        _beats = (int)((Song.length / 60) * BPM);
        BeatLength = (Song.length / _beats);

        _stopwatch.Start();
        _qbStopwatch.Start();
    }

    private void Start()
    {
        //OnBeat();
    }

    public float GetTimestampOfBeat(int beat)
    {
        return BeatLength * beat;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBeat(_stopwatch, OnBeat, BeatLengthAdjusted);
        CheckBeat(_qbStopwatch, OnQuarterBeat, QuarterBeatLengthAdjusted);
    }

    private void CheckBeat(Stopwatch stopwatch, BeatHit beatEvent, float targetBeatLength)
    {
        var milliseconds = stopwatch.ElapsedMilliseconds / 1000f;

        if (milliseconds >= targetBeatLength)
        {
            //Removing beat length means we carry over the overshoot, tightening our timing.
            stopwatch.Restart();
            _latencyAdjustment = milliseconds - targetBeatLength;
            UpdateRAE(_latencyAdjustment);
            beatEvent?.Invoke();
        }
    }

    private void UpdateRAE(float error)
    {
        _rollingAverageError = (_rollingAverageError + error) / 2;
        UnityEngine.Debug.Log($"Error: {error}, new RAE: {_rollingAverageError}");
    }
}
