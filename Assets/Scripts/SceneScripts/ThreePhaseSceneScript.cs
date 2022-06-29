using Assets.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class ThreePhaseSceneScript : BaseSceneScript
    {
        private List<GameObject> _claimedSprites = new List<GameObject>();

        private const int WIDTH = 32;
        public float Frequency = 16;
        public float Amplitude = 2;
        private Vector3[,] _gridMeasures;
        private float _phaseOffset;
        private const float _phaseIncrement = 0.001f;

        [Range(0.0f, 1.0f)]
        public float ThreePhaseOffset = 0.666f;
        public Material SpriteMat;
        public bool QBSpriteSwitching;

        // Update is called once per frame
        void Update()
        {
            if (_performing)
            {
                _phaseOffset += _phaseIncrement;

                for(int phase = 0; phase < 3; phase++)
                {
                    CalculateYOnSine(phase);
                    for (int i = 0; i < WIDTH; i++)
                    {
                        var sprite = _claimedSprites[i + (phase * WIDTH)];
                        sprite.transform.position = _gridMeasures[i, 0];
                    }
                }
            }
        }

        public override void Perform(int lengthInBeats)
        {
            base.Perform(lengthInBeats);

            TimeSlicer.OnBeat += OnBeat;
            if(QBSpriteSwitching)
            {
                TimeSlicer.OnQuarterBeat += OnQuarterBeat;
            }
            _gridMeasures = GridRuler.GetPlanarGrid(WIDTH, 1);

            for (int phase = 0; phase < 3; phase++)
            {
                CalculateYOnSine(phase);
                for (int i = 0; i < WIDTH; i++)
                {
                    var sprite = ObjectPoolManager.GetSpriteFromPool();
                    _claimedSprites.Add(sprite);
                    sprite.GetComponent<PooledSprite>().SetMaterial(SpriteMat);
                    //Set position
                    sprite.transform.position = _gridMeasures[i, 0];
                }
            }
        }

        private void CalculateYOnSine(int phase)
        {
            for (int i = 0; i < WIDTH; i++)
            {
                var x = i == 0 ? 0 : ((float)i / WIDTH) + _phaseOffset + (phase * ThreePhaseOffset);
                x *= Frequency;
                var y = (Mathf.Sin(x) * Amplitude);
                _gridMeasures[i, 0].y = y;
            }
        }

        private void SetSpriteMats()
        {
            foreach(var sprite in _claimedSprites)
            {
                sprite.GetComponent<PooledSprite>().SetMaterial(SpriteMat);
            }
        }

        public override void OnBeat()
        {
            //throw new System.NotImplementedException();
        }

        private void OnQuarterBeat()
        {
            SpriteMat = SpriteRandomizer.GetRandomMaterial();
            SetSpriteMats();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            ObjectPoolManager.ReturnAllSprites();
            _claimedSprites.Clear();

            if (QBSpriteSwitching)
            {
                TimeSlicer.OnQuarterBeat -= OnQuarterBeat;
            }
        }
    }
}