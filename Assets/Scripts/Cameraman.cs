using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    public Camera Camera;

    private const float CLOSEST_DEPTH = -1;
    private const float FURTHEST_DEPTH = -8;
    private const float BASE_DEPTH = -10;

    public bool Rainbows;
    private const int _rainbowFrequency = 32;
    private int _rainbowFrame;

    private void Update()
    {
        if (Rainbows)
        {
            if (_rainbowFrame == _rainbowFrequency)
            {
                Camera.backgroundColor = Random.ColorHSV();
                _rainbowFrame = 0;
            }
            else
            {
                _rainbowFrame++;
            }
        }
        else Camera.backgroundColor = Color.black;
    }

    public void ResetCamera()
    {
        ZoomCamera(BASE_DEPTH);
        PanCamera(0);
        CraneCamera(0);
    }

    public void ZoomCamera(float z)
    {
        var currPosition = Camera.transform.position;
        var newPosition = new Vector3(currPosition.x, currPosition.y, z);
        Camera.transform.position = newPosition;
    }

    public void PanCamera(float x)
    {
        var currPosition = Camera.transform.position;
        var newPosition = new Vector3(x, currPosition.y, currPosition.z);
        Camera.transform.position = newPosition;
    }

    public void CraneCamera(float y)
    {
        var currPosition = Camera.transform.position;
        var newPosition = new Vector3(currPosition.x, y, currPosition.z);
        Camera.transform.position = newPosition;
    }

    public void RandomizeDepth()
    {
        var z = Random.Range(CLOSEST_DEPTH, FURTHEST_DEPTH);
        ZoomCamera(z);
    }
}
