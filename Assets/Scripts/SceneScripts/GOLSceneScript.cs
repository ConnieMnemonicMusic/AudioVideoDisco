using Assets.Scripts.Helpers;
using Assets.Scripts.SceneScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOLSceneScript : BaseSceneScript
{
    public Material SpriteMat;
    public bool QuarterBeatProcessing;
    public bool SpriteSwitching;

    private const int WIDTH = 16;
    private const int HEIGHT = 16;
    private const float CAMERA_Z = -15f;
    private const float CAMERA_Y = 0.5f;

    private Vector3[,] _worldspaceGrid;
    private bool[,] _golGrid;

    public override void Perform(int lengthInBeats)
    {
        _worldspaceGrid = GridRuler.GetPlanarGrid(WIDTH, HEIGHT);
        InitGol();
        PlayAndDisplayGol();

        if(!QuarterBeatProcessing)
        {
            TimeSlicer.OnBeat += OnBeat;
        }
        else
        {
            TimeSlicer.OnQuarterBeat += OnQuarterBeat;
        }

        base.Perform(lengthInBeats);
        Cameraman.ZoomCamera(CAMERA_Z);
        Cameraman.CraneCamera(CAMERA_Y);
    }

    public override void CleanUp()
    {
        if (!QuarterBeatProcessing)
        {
            TimeSlicer.OnBeat -= OnBeat;
        }
        else TimeSlicer.OnQuarterBeat -= OnQuarterBeat;
        Cameraman.ResetCamera();
        ObjectPoolManager.ReturnAllSprites();
        base.CleanUp();
    }

    public override void OnBeat()
    {
        PlayAndDisplayGol();
    }

    private void OnQuarterBeat()
    {
        PlayAndDisplayGol();
    }

    private void PlayAndDisplayGol()
    {
        bool[,] _newGrid = new bool[WIDTH, HEIGHT];
        ObjectPoolManager.ReturnAllSprites();

        if (SpriteSwitching) SpriteMat = SpriteRandomizer.GetRandomMaterial();

        for(int y = 0; y < HEIGHT; y++)
        {
            for(int x = 0; x < WIDTH; x++)
            {
                //Calculate GOL
                var prevCellState = _golGrid[x, y];
                var newState = IsAlive(GetNeighbourLiveCount(x, y), prevCellState);
                _newGrid[x, y] = newState;

                //Display
                var alive = newState;
                if (alive)
                {
                    var mySprite = ObjectPoolManager.GetSpriteFromPool();
                    mySprite.GetComponent<PooledSprite>().SetMaterial(SpriteMat);
                    mySprite.transform.position = _worldspaceGrid[x, y];
                }
            }
        }
        _golGrid = _newGrid;
    }

    //Grid wrapped with mod
    private int GetNeighbourLiveCount(int x, int y)
    {
        int liveCount = 0;
        bool[] neighbours = new bool[8];

        //Left to right
        //Top
        neighbours[0] = _golGrid[mod(x - 1, WIDTH), mod(y - 1, HEIGHT)];
        neighbours[1] = _golGrid[x, mod(y - 1, HEIGHT)];
        neighbours[2] = _golGrid[mod(x + 1, WIDTH), mod(y - 1, HEIGHT)];
        //Mid
        neighbours[3] = _golGrid[mod(x - 1, WIDTH), y];
        neighbours[4] = _golGrid[mod(x + 1, WIDTH), y];
        //Bot
        neighbours[5] = _golGrid[mod(x - 1, WIDTH), mod(y + 1, HEIGHT)];
        neighbours[6] = _golGrid[x, mod(y + 1, HEIGHT)];
        neighbours[7] = _golGrid[mod(x + 1, WIDTH), mod(y + 1, HEIGHT)];

        foreach(var cell in neighbours)
        {
            if (cell) liveCount++;
        }

        return liveCount;
    }

    private bool IsAlive(int neighbourCount, bool isAlreadyAlive)
    {
        if(isAlreadyAlive)
        {
            if (neighbourCount < 2 || neighbourCount > 3) return false;
            return true;
        }
        else //If started dead
        {
            if (neighbourCount == 3) return true;
            return false;
        }
    }

    //Yeah, this is ugly.
    //But hey, it's flexible!
    private void InitGol()
    {
        _golGrid = new bool[WIDTH, HEIGHT]
        {
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false }

        };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
