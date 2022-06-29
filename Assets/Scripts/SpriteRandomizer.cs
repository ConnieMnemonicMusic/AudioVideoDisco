using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    public Material[] MaterialPool;
    //Allows for Tetris-style randomization!
    private List<Material> _randomBag { get; set; } = new List<Material>();

    public Material GetRandomMaterial()
    {
        EnsureBagContainsElements();
        var selection = _randomBag[Random.Range(0, _randomBag.Count)];
        _randomBag.Remove(selection);
        return selection;
    }

    private void EnsureBagContainsElements()
    {
        if(_randomBag.Count == 0)
        {
            _randomBag.AddRange(MaterialPool);
            ShuffleBag();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _randomBag.AddRange(MaterialPool);
        ShuffleBag();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShuffleBag()
    {
        int n = _randomBag.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            Material value = _randomBag[k];
            _randomBag[k] = _randomBag[n];
            _randomBag[n] = value;
        }
    }
}
