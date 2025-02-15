using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    // Start is called before the first frame update
    void Start()
    {
        _source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
