using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicActivation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicController.Instance.Play();
        MusicController.Instance.PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
