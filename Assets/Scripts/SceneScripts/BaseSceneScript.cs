using UnityEngine;

namespace Assets.Scripts.SceneScripts
{
    public abstract class BaseSceneScript : MonoBehaviour
    {
        public ObjectPoolManager ObjectPoolManager;
        public SpriteRandomizer SpriteRandomizer;
        public Cameraman Cameraman;
        public TimeSlicer TimeSlicer;

        public bool Rainbows;
        protected bool _performing;

        //Launches the scene. Should be fired on the activating beat.
        public virtual void Perform(int lengthInBeats)
        {
            _performing = true;
            Cameraman.Rainbows = Rainbows;
        }
        //Exposes a way for scenes to interact with the beat.
        public abstract void OnBeat();
        //Runs before handing over to the next scene.
        public virtual void CleanUp()
        {
            _performing = false;
            Cameraman.Rainbows = false;
            Cameraman.ResetCamera();
            ObjectPoolManager.ReturnAllSprites();
        }
    }
}
