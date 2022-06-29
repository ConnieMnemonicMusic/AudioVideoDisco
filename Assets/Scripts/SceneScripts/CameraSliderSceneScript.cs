using Assets.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class CameraSliderSceneScript : BaseSceneScript
    {
        private const int WIDTH = 128;
        private float SlideSpeed = 0.0008f;
        private const float CAM_DEPTH = -2f;
        private float _currCamPan;

        public bool SpriteSwitching;
        private List<GameObject> _claimedSprites = new List<GameObject>();

        public override void Perform(int lengthInBeats)
        {
            ObjectPoolManager.ReturnAllSprites();

            base.Perform(lengthInBeats);
            Cameraman.ZoomCamera(CAM_DEPTH);

            var grid = GridRuler.GetPlanarGrid(WIDTH, 1);
            for(int i = 0; i < WIDTH; i++)
            {
                var sprite = ObjectPoolManager.GetSpriteFromPool();
                _claimedSprites.Add(sprite);
                var newMat = SpriteRandomizer.GetRandomMaterial();
                sprite.GetComponent<PooledSprite>().SetMaterial(newMat);
                //Set position
                var newPos = grid[i, 0];
                newPos.y -= 0.5f;
                sprite.transform.position = newPos;
            }

            if(SpriteSwitching)
            {
                TimeSlicer.OnQuarterBeat += OnQuarterBeat;
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();
            ObjectPoolManager.ReturnAllSprites();
            Cameraman.PanCamera(0);
            _claimedSprites.Clear();

            if (SpriteSwitching)
            {
                TimeSlicer.OnQuarterBeat -= OnQuarterBeat;
            }
        }

        public override void OnBeat()
        {
            //throw new System.NotImplementedException();
        }

        public void OnQuarterBeat()
        {
            foreach(var sprite in _claimedSprites)
            {
                var mat = SpriteRandomizer.GetRandomMaterial();
                sprite.GetComponent<PooledSprite>().SetMaterial(mat);
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(_performing)
            {
                _currCamPan += SlideSpeed;
                Cameraman.PanCamera(_currCamPan);
            }
        }
    }
}