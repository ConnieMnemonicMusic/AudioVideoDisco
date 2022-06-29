using Assets.Scripts;
using Assets.Scripts.SceneScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class RandomObjectSceneScript : BaseSceneScript
    {
        public bool QBSpriteSwitching;
        private GameObject _mySprite;
        private PooledSprite _mySpriteScript;

        public bool RandomDepths;

        public override void Perform(int lengthInBeats)
        {
            //Sprite setup
            _mySprite = ObjectPoolManager.GetSpriteFromPool();
            _mySpriteScript = _mySprite.GetComponent<PooledSprite>();

            if(QBSpriteSwitching)
            {
                TimeSlicer.OnQuarterBeat += OnBeat;
            }
            else
            {
                TimeSlicer.OnBeat += OnBeat;
            }

            //Center it.
            _mySprite.transform.position = new Vector3(0, 0, 0);

            base.Perform(lengthInBeats);
            _performing = true;
        }

        public override void OnBeat()
        {
            if(_performing)
            {
                if (RandomDepths) Cameraman.RandomizeDepth();

                var newMat = SpriteRandomizer.GetRandomMaterial();
                _mySpriteScript.SetMaterial(newMat);
            }
        }

        public override void CleanUp()
        {
            _performing = false;
            if (QBSpriteSwitching)
            {
                TimeSlicer.OnQuarterBeat -= OnBeat;
            }
            else
            {
                TimeSlicer.OnBeat -= OnBeat;
            }

            ObjectPoolManager.ReturnAllSprites();
            _mySprite = null;
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
