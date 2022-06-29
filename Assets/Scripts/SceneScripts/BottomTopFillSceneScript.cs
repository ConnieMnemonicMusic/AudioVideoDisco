using Assets.Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class BottomTopFillSceneScript : BaseSceneScript
    {
        public Material MySpriteMaterial;
        public bool QBSpriteSwitching;

        private const int WIDTH = 31;
        private int _height;
        private Vector3[,] _grid;
        private int _currBeat;

        private const float CAMERA_DEPTH = -6f;
        private List<GameObject> _claimedSprites = new List<GameObject>();


        public override void Perform(int lengthInBeats)
        {
            Cameraman.ZoomCamera(CAMERA_DEPTH);
            TimeSlicer.OnBeat += OnBeat;

            if (QBSpriteSwitching) TimeSlicer.OnQuarterBeat += OnQuarterBeat;

            _height = lengthInBeats;
            _grid = GridRuler.GetPlanarGrid(WIDTH, _height);

            base.Perform(lengthInBeats);
        }

        public override void OnBeat()
        {
            if (_performing)
            {
                AddRow();
                _currBeat++;
            }
        }

        public void OnQuarterBeat()
        {
            if(_performing)
            {
                foreach(var sprite in _claimedSprites)
                {
                    MySpriteMaterial = SpriteRandomizer.GetRandomMaterial();
                    sprite.GetComponent<PooledSprite>().SetMaterial(MySpriteMaterial);
                }
            }
        }

        private void AddRow()
        {
            for(int i = 0; i < WIDTH; i++)
            {
                if(QBSpriteSwitching)
                {
                    MySpriteMaterial = SpriteRandomizer.GetRandomMaterial();
                }

                //Find row
                var rowIndex = _height - _currBeat;

                var mySprite = ObjectPoolManager.GetSpriteFromPool();
                _claimedSprites.Add(mySprite);
                mySprite.GetComponent<PooledSprite>().SetMaterial(MySpriteMaterial);
                var gridPos = _grid[i, _currBeat];
                mySprite.transform.position = gridPos;
            }
        }

        public override void CleanUp()
        {
            ObjectPoolManager.ReturnAllSprites();
            _claimedSprites.Clear();
            TimeSlicer.OnBeat -= OnBeat;
            if (QBSpriteSwitching) TimeSlicer.OnQuarterBeat -= OnQuarterBeat;
            Cameraman.ResetCamera();
            base.CleanUp();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}