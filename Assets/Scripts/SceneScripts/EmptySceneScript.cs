using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    //Does nothing.
    //Used for blank space.
    public class EmptySceneScript : BaseSceneScript
    {
        public override void CleanUp()
        {
            Cameraman.ResetCamera();
            //throw new System.NotImplementedException();
        }

        public override void OnBeat()
        {
            //throw new System.NotImplementedException();
        }

        public override void Perform(int lengthInBeats)
        {
            //throw new System.NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}