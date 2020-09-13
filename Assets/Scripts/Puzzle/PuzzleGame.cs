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
    public float radius = 5;

    //Velocidad de rotación del círculo
    [Range(0, 5)]
    public int rotationSpeed = 5;

    #endregion

    #region Private variables
    
    //Guarda la posición inicial del puzzle
    Vector2 initPosition;

    //Guarda el index de la figura actual
    int currentFigureIndex;

    //Referencia al renderer de la figura actual
    SpriteRenderer tempRenderer;

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

        //Si presiona algun boton del raton o las teclas para moverse
        if (leftClick || rightClick || h != 0)
        {
            //Sound
            if (!ControllerSound.Instance.Wheel.IsPlaying())
            {
                ControllerSound.Instance.Wheel.Play();
            }

            //Se gira hacia la derecha >
            if (leftClick || h > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - angle), rotationSpeed * Time.deltaTime);
            }

            //Se gira hacia la izquierda <
            if(rightClick || h < 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle), rotationSpeed * Time.deltaTime);
            }

        }
        else
        {
            ControllerSound.Instance.Wheel.Stop();

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

            //Si no hay espacios en blanco quiere decir que completó el puzzle
            if (blankSpaces.childCount == 0)
            {
                print("Puzzle completado!");
                yield break;
            }

            int randomBlankSpaceIndex = Random.Range(0, blankSpaces.childCount);

            int randomFigureIndex = Random.Range(0, blankSpaces.childCount);
            Transform targetBlankSpace = blankSpaces.GetChild(randomBlankSpaceIndex);

            currentFigureIndex = randomFigureIndex;

            tempRenderer = blankSpaces.GetChild(currentFigureIndex).GetComponent<SpriteRenderer>();
            tempRenderer.color = new Color(tempRenderer.color.r, tempRenderer.color.g, tempRenderer.color.b, 1);

            yield return new WaitForSeconds(reactionDelay);

            //Haya la dirección y rotación que tendrá la figura inicialmente
            Vector2 targetBlankSpacePosition = targetBlankSpace.position;
            Vector2 targetDirection = (targetBlankSpacePosition - initPosition).normalized;
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, targetDirection);

            //Instancia la figura en la escena
            Figure tempFigure = Instantiate(figures[int.Parse(blankSpaces.GetChild(currentFigureIndex).name)], initPosition, targetRotation).GetComponent<Figure>();
            tempFigure.TargetDirection = targetDirection;
            tempFigure.TargetIndex = currentFigureIndex;
            tempFigure.TargetRotation = targetRotation * Quaternion.Euler(0, 0, 90);
        }
    }

    //Cualquier cosa que salga del tablero se destruye
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Si lo que se sale es la figura que se colocó en una posición incorrecta
        if (collision.CompareTag("CorrectPosition"))
        {
            //SOund
            ControllerSound.Instance.Item.Play();
            ControllerSound.Instance.Item.EventInstance.setParameterByName("HoroscopeTake", 1);
            tempRenderer.color = new Color(tempRenderer.color.r, tempRenderer.color.g, tempRenderer.color.b, 0);
        }
        
        Destroy(collision.gameObject);
       
    }
}
