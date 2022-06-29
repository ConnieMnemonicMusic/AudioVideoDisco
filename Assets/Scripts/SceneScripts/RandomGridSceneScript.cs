using Assets.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class RandomGridSceneScript : BaseSceneScript
    {
        //Configuraion
        public bool RandomDepths;
        public bool CameraSlide;
        public bool QuarterBeat;

        private const int MIN_HEIGHT = 20;
        private const int MIN_WIDTH = 30;
        private const int MAX_HEIGHT = 30;
        private const int MAX_WIDTH = 50;
        private const float MAX_SLIDE_RATE = 0.004f;
        private const float MIN_SLIDE_RATE = -0.004f;

        private float _xSlideRate;
        private float _ySlideRate;
        
        public override void Perform(int lengthInBeats)
        {
            if(QuarterBeat)
            {
                TimeSlicer.OnQuarterBeat += OnBeat;
            }
            else
            {
                TimeSlicer.OnBeat += OnBeat;
            }

            //Set slide rates
            if (CameraSlide)
            {
                _xSlideRate = Random.Range(MIN_SLIDE_RATE, MAX_SLIDE_RATE);
                _ySlideRate = Random.Range(MIN_SLIDE_RATE, MAX_SLIDE_RATE);
            }

            base.Perform(lengthInBeats);

            _performing = true;

            CreateAndDisplayGrid();
        }

        private void CreateAndDisplayGrid()
        {
            if(_performing)
            {
                //Clean previous grid.
                ObjectPoolManager.ReturnAllSprites();

                var spriteMat = SpriteRandomizer.GetRandomMaterial();

                //Generate grid
                var height = Random.Range(MIN_HEIGHT, MAX_HEIGHT + 1);
                var width = Random.Range(MIN_WIDTH, MAX_WIDTH + 1);
                var grid = GridRuler.GetPlanarGrid(width, height);

                for (int y = 0; y < height - 1; y++)
                {
                    for (int x = 0; x < width - 1; x++)
                    {
                        var sprite = ObjectPoolManager.GetSpriteFromPool();
                        sprite.transform.position = grid[x, y];
                        sprite.GetComponent<PooledSprite>().SetMaterial(spriteMat);
                    }
                }
            }
        }

        public override void OnBeat()
        {
            if(_performing)
            {
                CreateAndDisplayGrid();
                if (RandomDepths) Cameraman.RandomizeDepth();

                //Set slide rates
                if (CameraSlide)
                {
                    _xSlideRate = Random.Range(MIN_SLIDE_RATE, MAX_SLIDE_RATE);
                    _ySlideRate = Random.Range(MIN_SLIDE_RATE, MAX_SLIDE_RATE);
                }
            }
        }

        public override void CleanUp()
        {
            _performing = false;

            base.CleanUp();

            Cameraman.ResetCamera();

            if (QuarterBeat)
            {
                TimeSlicer.OnQuarterBeat -= OnBeat;
            }
            else
            {
                TimeSlicer.OnBeat -= OnBeat;
            }
            ObjectPoolManager.ReturnAllSprites();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Handle camera sliding
            if(_performing && CameraSlide)
            {
                Cameraman.PanCamera(Cameraman.Camera.transform.position.x + _xSlideRate);
                Cameraman.CraneCamera(Cameraman.Camera.transform.position.y + _ySlideRate);
            }
        }
    }
}