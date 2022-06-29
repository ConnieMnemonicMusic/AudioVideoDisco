using Assets.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class SineWaveSceneScript : BaseSceneScript
    {
        public bool QBSpriteSwitching;
        public Material SpriteMat;

        private List<GameObject> _claimedSprites = new List<GameObject>();

        private const int WIDTH = 32;
        private const float FREQUENCY = 32;
        private Vector3[,] _gridMeasures;
        private float _phaseOffset;
        private const float _phaseIncrement = 0.001f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(_performing)
            {
                _phaseOffset += _phaseIncrement;
                CalculateYOnSine();

                for (int i = 0; i < WIDTH; i++)
                {
                    var sprite = _claimedSprites[i];
                    sprite.transform.position = _gridMeasures[i, 0];
                }
            }
        }

        public override void Perform(int lengthInBeats)
        {
            if (QBSpriteSwitching) TimeSlicer.OnQuarterBeat += OnQuarterBeat;

            base.Perform(lengthInBeats);

            TimeSlicer.OnBeat += OnBeat;
            _gridMeasures = GridRuler.GetPlanarGrid(WIDTH, 1);
            CalculateYOnSine();

            for(int i = 0; i < WIDTH; i++)
            {
                var sprite = ObjectPoolManager.GetSpriteFromPool();
                sprite.GetComponent<PooledSprite>().SetMaterial(SpriteMat);
                _claimedSprites.Add(sprite);
                //Set position
                sprite.transform.position = _gridMeasures[i, 0];
            }
        }

        private void CalculateYOnSine()
        {
            for(int i = 0; i < WIDTH; i++)
            {
                var x = i == 0 ? 0 : ((float)i / WIDTH) + _phaseOffset;
                x *= FREQUENCY;
                var y = Mathf.Sin(x);
                _gridMeasures[i, 0].y = y;
            }
        }

        public override void OnBeat()
        {
            //throw new System.NotImplementedException();
        }

        private void OnQuarterBeat()
        {
            SpriteMat = SpriteRandomizer.GetRandomMaterial();
            for(int i = 0; i < WIDTH; i++)
            {
                _claimedSprites[i].GetComponent<PooledSprite>().SetMaterial(SpriteMat);
            }
        }

        public override void CleanUp()
        {
            if (QBSpriteSwitching) TimeSlicer.OnQuarterBeat -= OnQuarterBeat;

            base.CleanUp();
            ObjectPoolManager.ReturnAllSprites();
            _claimedSprites.Clear();
        }
    }
}