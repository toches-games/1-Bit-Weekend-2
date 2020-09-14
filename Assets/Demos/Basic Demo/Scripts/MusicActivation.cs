using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicActivation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicController.Instance.Play();
        MusicController.Instance.PlayMenuStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        MusicController.Instance.PlayIntro();
        GetComponent<StudioEventEmitter>().Play();
    }
}
