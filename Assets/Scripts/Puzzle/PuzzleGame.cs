using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    #region Public variables

    //Prefab que muestra los espacios vacios del puzzle
    public GameObject blankSpace;

    //Guarda las figuras que tendrá el puzzle para instanciarlas luego
    public GameObject[] figures;

    //Tiempo en el que saldrá cada figura
    [Range(0, 5)]
    public int figureDelay = 5;

    //Radio del círculo del puzzle
    [Range(0, 10)]
    public int radius = 5;

    #endregion

    #region Private variables
    
    //Guarda la posición inicial del puzzle
    Vector2 initPosition;

    #endregion

    void Awake()
    {
        initPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Desactiva la gravedad en la scena
        NoGravity();

        //Crear el círculo del puzzle
        CreateCircle();

        //Inicia la creación de las figuras cada cierto tiempo
        StartCoroutine("CreateFigure");
    }

    void OnMouseDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(mousePosition, initPosition);

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        transform.rotation = Quaternion.Euler(0, 0, angle);

        print(angle);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    //Cambia la gravedad de la scena a cero
    void NoGravity()
    {
        Physics2D.gravity = Vector2.zero;
    }

    //Crea el círculo del puzzle con todos los espacios en blanco
    void CreateCircle()
    {
        float angle = 360f / figures.Length;

        for(int i=0; i<figures.Length; i++)
        {
            Vector2 tempPosition = new Vector2(Mathf.Cos(Mathf.Deg2Rad * i * angle), Mathf.Sin(Mathf.Deg2Rad * i * angle)) * radius;
            Transform tempFigure = Instantiate(blankSpace, tempPosition, Quaternion.identity).transform;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, (new Vector2(tempFigure.position.x, tempFigure.position.y) - initPosition).normalized);

            tempFigure.rotation = targetRotation * Quaternion.Euler(0, 0, 90f);
            tempFigure.SetParent(transform);
        }
    }

    //Crea figuras cada cierto tiempo
    IEnumerator CreateFigure()
    {
        while (true)
        {
            Vector2 targetBlankSpace = transform.GetChild(Random.Range(0, transform.childCount)).position;
            Vector2 targetDirection = (targetBlankSpace - initPosition).normalized;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, targetDirection);

            Figure tempFigure = Instantiate(figures[Random.Range(0, figures.Length)], initPosition, targetRotation).GetComponent<Figure>();
            tempFigure.TargetDirection = targetDirection;
            yield return new WaitForSeconds(figureDelay);
        }
    }
}
