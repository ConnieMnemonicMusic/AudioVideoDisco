using Assets.Scripts;
using Assets.Scripts.SceneScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class SlowZoomSceneScript : BaseSceneScript
    {
        public bool Reverse;

        private GameObject _mySprite { get; set; }
        private PooledSprite _myPooledSprite { get; set; }
        public Material MySpriteMaterial;

        private float _timeRequired;
        private float _timeElapsed;
        private const float CAMERA_FINAL_Z = -1.2f;
        private const float CAMERA_START_Z = -1.8f;


        public override void Perform(int lengthInBeats)
        {
            _mySprite = ObjectPoolManager.GetSpriteFromPool();
            _myPooledSprite = _mySprite.GetComponent<PooledSprite>();
            _myPooledSprite.SetMaterial(MySpriteMaterial);
            _myPooledSprite.Centre();

            _timeRequired = TimeSlicer.BeatLength * lengthInBeats;
            Cameraman.ZoomCamera(CAMERA_START_Z);

            base.Perform(lengthInBeats);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            ObjectPoolManager.ReturnAllSprites();
            _mySprite = null;
            Cameraman.ResetCamera();
        }

        public override void OnBeat()
        {
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_performing)
            {
                _timeElapsed += Time.deltaTime;
                var progress = _timeElapsed / _timeRequired;
                var newZ = Reverse ? Mathf.Lerp(CAMERA_FINAL_Z, CAMERA_START_Z, progress) : Mathf.Lerp(CAMERA_START_Z, CAMERA_FINAL_Z, progress);
                Cameraman.ZoomCamera(newZ);
            }
        }
    }
}