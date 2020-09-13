using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    #region Public variables

    //Velocidad con la que se moverá la figura
    [Range(0, 5)]
    public int speed = 5;

    //Direccion a la que se moverá
    public Vector2 TargetDirection { get; set; }

    //Index al que pertenece esta figura, se comprueba con el index del espacio en blanco correcto
    public int TargetIndex { get; set; }

    //Rotacion inicial de la figura
    public Quaternion TargetRotation { get; set; }
    
    #endregion

    #region Private variables

    //Referencia al rigidbody
    Rigidbody2D rig;

    //Referencia a el objecto padre de posiciones correctas
    Transform correctPosition;

    #endregion

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        correctPosition = GameObject.Find("CorrectPosition").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        rig.velocity = TargetDirection * speed;
        transform.rotation = TargetRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Si no choca con un espacio en blanco ni es la posición correcta no hace nada
        if (!other.CompareTag("SpaceBlank"))
        {
            return;
        }

        //Si choca con el espacio en blanco siendo este la posición correcta
        if(other.transform.GetSiblingIndex() == TargetIndex)
        {
            //Sound
            ControllerSound.Instance.Item.Play();
            ControllerSound.Instance.Item.EventInstance.setParameterByName("HoroscopeTake", 0);

            //Hacemos que su velocidad sea cero para dejar de moverse
            rig.velocity = Vector2.zero;

            //Colocamos como padre al tablero, para que la figura se gire cuando el padre es girado
            transform.SetParent(correctPosition);

            //Igualamos la posicion y rotacion local del espacio en blanco para que la figura quede
            //como se fuece igual al espacio en blanco
            transform.localPosition = other.transform.localPosition;
            transform.localRotation = other.transform.localRotation;

            //Se destruye en rigidbody de la figura
            Destroy(rig);

            //Y se destruye el objeto del espacio en blanco
            Destroy(other.gameObject);
        }
    }
}
