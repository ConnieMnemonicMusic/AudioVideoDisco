using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public class ArcadeMachineSceneScript : BaseSceneScript
    {
        public GameObject ArcadeMachine;
        private GameObject _myArcadeMachine;
        public GameObject HighScoreText;

        private const float MACHINE_START_Y = -0.83f;
        private const float MACHINE_START_Z = -8;
        private const float MACHINE_END_Z = -9.3f;

        private float _timeElapsed;
        private float _timeRequired;
        private int _beatsBeforeText;
        private int _beatsHit;

        public override void Perform(int lengthInBeats)
        {
            base.Perform(lengthInBeats);

            _beatsBeforeText = lengthInBeats - 1;
            TimeSlicer.OnBeat += OnBeat;

            _timeRequired = TimeSlicer.BeatLength * (_beatsBeforeText - 1);
            _myArcadeMachine = Instantiate(ArcadeMachine);

            var newPos = new Vector3(_myArcadeMachine.transform.position.x, MACHINE_START_Y, MACHINE_START_Z);
            _myArcadeMachine.transform.position = newPos;
        }

        public override void OnBeat()
        {
            _beatsHit++;
            if(_beatsHit == _beatsBeforeText)
            {
                HighScoreText.SetActive(true);
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();

            Destroy(_myArcadeMachine);
            TimeSlicer.OnBeat -= OnBeat;
            HighScoreText.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(_performing)
            {
                if(_timeElapsed < _timeRequired)
                {
                    _timeElapsed += Time.deltaTime;
                    var progress = _timeElapsed / _timeRequired;
                    var newZ = Mathf.SmoothStep(MACHINE_START_Z, MACHINE_END_Z, progress);
                    var newPos = new Vector3(_myArcadeMachine.transform.position.x, _myArcadeMachine.transform.position.y, newZ);
                    _myArcadeMachine.transform.position = newPos;
                }
                var randomColor = Random.ColorHSV();
                HighScoreText.GetComponent<TextMeshPro>().color = randomColor;
                HighScoreText.GetComponent<TextMeshPro>().fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, randomColor);
            }
        }
    }
}