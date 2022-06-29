using Assets.Scripts;
using Assets.Scripts.SceneScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [Serializable]
    public struct SceneMarker
    {
        public int DurationInBeats;
        public BaseSceneScript SceneScript;
    }
    public SceneMarker[] ScenePlaylistArray;
    public Queue<SceneMarker> ScenePlaylist;

    public TimeSlicer TimeSlicer;

    private int _sceneRuntimeInBeats;

    // Start is called before the first frame update
    void Start()
    {
        TimeSlicer.OnBeat += OnBeatEvent;

        ScenePlaylist = new Queue<SceneMarker>(ScenePlaylistArray);

        var initScene = ScenePlaylist.Peek();
        initScene.SceneScript.Perform(initScene.DurationInBeats);
    }

    private void OnBeatEvent()
    {
        if(ScenePlaylist.Count == 0)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        else
        {
            _sceneRuntimeInBeats++;
            var currentScene = ScenePlaylist.Peek();

            if (currentScene.DurationInBeats == _sceneRuntimeInBeats)
            {
                //Scene is finished, proceed
                currentScene.SceneScript.CleanUp();
                ScenePlaylist.Dequeue();
                if(ScenePlaylist.Count != 0)
                {
                    var newScene = ScenePlaylist.Peek();
                    newScene.SceneScript.Perform(newScene.DurationInBeats);
                    _sceneRuntimeInBeats = 0;
                }
            }
        }
    }
}
