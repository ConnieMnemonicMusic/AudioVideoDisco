using Assets.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class CircleSpinSceneScript : BaseSceneScript
    {
        //Circle definitions
        [Serializable]
        public struct CircleDefinition
        {
            public float Radius;
            public int Count;
            public Material Sprite;
            public float RotationSpeed;
        }
        public CircleDefinition[] CircleDefinitionsArray;
        public List<CircleDefinition> CircleDefinitions { get; set; }
        private List<GameObject> ClockCenters = new List<GameObject>();

        //Vars
        public bool QBCircleFlicker;
        public bool SpriteSwitching;
        private int _flickerIndex;

        private List<Vector3> _clockPoints;

        public override void Perform(int lengthInBeats)
        {
            if (QBCircleFlicker)
            {
                TimeSlicer.OnQuarterBeat += OnQuarterBeat;
            }
            //Circle definitions
            CircleDefinitions = new List<CircleDefinition>(CircleDefinitionsArray);

            for (int i = 0; i < CircleDefinitions.Count; i++)
            {
                var circle = CircleDefinitions[i];

                ClockCenters.Add(new GameObject($"ClockCenter{i}"));
                //circle.ClockCenter = new GameObject("ClockCenter");
                //circle.ClockCenter.transform.position = new Vector3(0, 0, 0);

                _clockPoints = CircleRuler.GetCirclePositions(circle.Count, circle.Radius);

                for (int j = 0; j < circle.Count; j++)
                {
                    var point = _clockPoints[j];

                    var sprite = ObjectPoolManager.GetSpriteFromPool();
                    sprite.GetComponent<PooledSprite>().SetMaterial(circle.Sprite);
                    //Zero position in worldspace
                    sprite.transform.position = ClockCenters[i].transform.position;
                    sprite.transform.SetParent(ClockCenters[i].transform);
                    //Set position relavant to clock center.
                    sprite.transform.position = point;

                    //Set rotation
                    var degreesPerPoint = 360 / circle.Count;

                    sprite.transform.Rotate(0, 0, (degreesPerPoint * (j + 1)) + 180);
                }
            }
            Cameraman.ResetCamera();
            base.Perform(lengthInBeats);
        }

        public override void OnBeat()
        {
            //ProcessFlicker();
        }

        private void OnQuarterBeat()
        {
            ProcessFlicker();

            if(SpriteSwitching)
            {
                foreach(var circle in ClockCenters)
                {
                    var sprite = SpriteRandomizer.GetRandomMaterial();
                    SetSpriteMatForCircle(circle.transform, sprite);
                }
            }
        }

        private void ProcessFlicker()
        {
            _flickerIndex = mod((_flickerIndex + 1), (ClockCenters.Count));
            var previous = mod((_flickerIndex - 1), (ClockCenters.Count));

            SetEnabledForCircle(ClockCenters[previous].transform, false);
            SetEnabledForCircle(ClockCenters[_flickerIndex].transform, true);
        }

        private void SetEnabledForCircle(Transform circle, bool enabled)
        {
            for (int j = 0; j < circle.transform.childCount; j++)
            {
                circle.transform.GetChild(j).gameObject.SetActive(enabled);
            }
        }

        private void EnableAllCircles()
        {
            foreach(var circle in ClockCenters)
            {
                SetEnabledForCircle(circle.transform, true);
            }
        }

        private void SetSpriteMatForCircle(Transform circle, Material mat)
        {
            for (int j = 0; j < circle.transform.childCount; j++)
            {
                circle.transform.GetChild(j).gameObject.GetComponent<PooledSprite>().SetMaterial(mat);
            }
        }

        public override void CleanUp()
        {
            ObjectPoolManager.ReturnAllSprites();
            EnableAllCircles(); //Make sure they're re-enabled so we don't have invisible sprites in the pool.
            base.CleanUp();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_performing)
            {
                for(int i = 0; i < CircleDefinitions.Count; i++)
                {
                    ClockCenters[i].transform.Rotate(0, 0, CircleDefinitions[i].RotationSpeed);
                }
            }
        }

        int mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}