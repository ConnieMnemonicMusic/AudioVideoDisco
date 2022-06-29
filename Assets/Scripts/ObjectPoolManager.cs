using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public GameObject SpritePrefab;
        public Transform PoolStashLocation;

        private List<GameObject> _pool = new List<GameObject>();
        private int _poolSize = 2500;
        private List<GameObject> _outOfPool = new List<GameObject>();

        // Use this for initialization
        void Awake()
        {
            for(int i = 0; i < _poolSize; i++)
            {
                _pool.Add(Instantiate(SpritePrefab, PoolStashLocation.position, Quaternion.identity));
            }
        }

        public GameObject GetSpriteFromPool()
        {
            var sprite = _pool.First();
            _outOfPool.Add(sprite);
            _pool.Remove(sprite);
            return sprite;
        }

        public void ReturnSpriteToPool(GameObject sprite)
        {
            _outOfPool.Remove(sprite);
            _pool.Add(sprite);
            sprite.transform.position = PoolStashLocation.position;
            sprite.transform.rotation = Quaternion.identity;
            sprite.transform.SetParent(null);
        }

        public void ReturnAllSprites()
        {
            var outOfPoolCopy = new List<GameObject>(_outOfPool);
            foreach(var sprite in outOfPoolCopy)
            {
                ReturnSpriteToPool(sprite);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}