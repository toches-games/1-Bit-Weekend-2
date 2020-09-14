using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ControllerSound : MonoBehaviour
{
    #region Singleton
    // Singleton MonoBehaviour
    // Asume que solo tendremos un ControllerAudio en la escena
    // Durante el evento Awake, publica el objeto a través de una propiedad
    // estática para que se pueda acceder a ella desde cualquier punto solo 
    // usando ControllerAudio.Instance

    static ControllerSound _instance;
    public static ControllerSound Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Se está accediendo a la instancia Singleton antes de que el componente se inicialice");
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Solo debería haber un ControllerAudio en la escena");
        }
        _instance = this;
    }
    #endregion

    [Header("Sounds")]

    public StudioEventEmitter Item;
    public StudioEventEmitter Wheel;
    public StudioEventEmitter button;
    public StudioEventEmitter ritmicRain;
    public StudioEventEmitter chimesDoor;
    public StudioEventEmitter unCover;
}