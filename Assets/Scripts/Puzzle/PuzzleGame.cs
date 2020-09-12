using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PuzzleGame : MonoBehaviour
{
    #region Public variables

    //Prefab que muestra los espacios vacios del puzzle
    public GameObject blankSpace;

    //Guarda las posiciones de los espacios en blanco
    public Transform blankSpaces;

    //Guarda las figuras que tendrá el puzzle para instanciarlas luego
    public GameObject[] figures;

    //Tiempo en el que saldrá cada figura
    [Range(0, 5)]
    public int figureDelay = 5;

    //Tiempo que tarda despues de cambiar el color del espacio vacio
    [Range(0, 5)]
    public int reactionDelay = 2;

    //Radio del círculo del puzzle
    [Range(0, 10)]
    public int radius = 5;

    //Velocidad de rotación del círculo
    [Range(0, 5)]
    public int rotationSpeed = 5;

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

    void Update()
    {
        bool leftClick = Input.GetMouseButton(0);
        bool rightClick = Input.GetMouseButton(1);

        float h = Input.GetAxisRaw("Horizontal");

        float angle = 360f / figures.Length;

        if (leftClick || rightClick || h != 0)
        {
            if (leftClick || h > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - angle), rotationSpeed * Time.deltaTime);
            }

            if(rightClick || h < 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle), rotationSpeed * Time.deltaTime);
            }
        }
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

            tempFigure.name = i.ToString();
            tempFigure.rotation = targetRotation * Quaternion.Euler(0, 0, 90f);
            tempFigure.SetParent(blankSpaces);
        }
    }

    //Crea figuras cada cierto tiempo
    IEnumerator CreateFigure()
    {
        while (true)
        {
            yield return new WaitForSeconds(figureDelay);

            int randomBlankSpaceIndex = Random.Range(0, blankSpaces.childCount);
            int randomFigureIndex = Random.Range(0, blankSpaces.childCount);
            Transform targetBlankSpace = blankSpaces.GetChild(randomBlankSpaceIndex);

            blankSpaces.GetChild(randomFigureIndex).GetComponent<SpriteRenderer>().color = Color.red;

            yield return new WaitForSeconds(reactionDelay);

            Vector2 targetBlankSpacePosition = targetBlankSpace.position;
            Vector2 targetDirection = (targetBlankSpacePosition - initPosition).normalized;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, targetDirection);

            Figure tempFigure = Instantiate(figures[randomFigureIndex], initPosition, targetRotation).GetComponent<Figure>();
            tempFigure.TargetDirection = targetDirection;
            tempFigure.TargetIndex = randomFigureIndex;
        }
    }
}
