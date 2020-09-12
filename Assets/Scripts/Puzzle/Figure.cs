using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    #region Public variables

    //Velocidad con la que se moverá la figura
    [Range(0, 5)]
    public int speed = 5;

    public Vector2 TargetDirection { get; set; }

    public int TargetIndex { get; set; }
    #endregion

    #region Private variables

    //Referencia al rigidbody
    Rigidbody2D rig;

    //Referencia al collider
    BoxCollider2D boxCollider;

    //Referencia a el objecto padre de posiciones correctas
    Transform correctPosition;

    #endregion

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        correctPosition = GameObject.Find("CorrectPosition").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        rig.velocity = TargetDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CorrectPosition"))
        {
            return;
        }

        if(other.transform.GetSiblingIndex() == TargetIndex)
        {
            rig.velocity = Vector2.zero;
            transform.SetParent(correctPosition);
            transform.localPosition = other.transform.localPosition;
            transform.localRotation = other.transform.localRotation;
            Destroy(rig);
            Destroy(other.gameObject);
        }
    }
}
