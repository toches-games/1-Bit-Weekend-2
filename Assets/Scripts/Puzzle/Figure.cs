using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Figure : MonoBehaviour
{
    #region Public variables

    //Velocidad con la que se moverá la figura
    [Range(0, 5)]
    public int speed = 5;

    #endregion

    #region Private variables

    //Referencia al rigidbody
    Rigidbody2D rig;

    //Dirección a la que se moverá
    public Vector2 TargetDirection { get; set; }

    #endregion

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        rig.velocity = TargetDirection * speed;
    }
}
