using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MotorCarretera : MonoBehaviour
{
    public GameObject contenedorCallesGO;
    public GameObject[] contenedorCallesArray;

    public float velocidad;
    public bool inicioJuego;
    public bool juegoTerminado;

    int contadorCalles = 0;
    int numeroSelectroCalles;

    public GameObject calleAnterior;
    public GameObject calleNueva;

    public Vector3 medidaLimitePantalla;
    public bool salioDePantalla;
    public GameObject mCamGo;
    public Camera mCampComp;

    public GameObject cocheGO;
    public GameObject audioFXGO;
    public AudioFX audioFXScript;
    public GameObject bgFinalGO;


    public float tama�oCalle;
    void Start()
    {
        InicioJuego();
    }

    void InicioJuego()
    {
        contenedorCallesGO = GameObject.Find("ContenedorCarreteras");

        mCamGo = GameObject.Find("Main Camera");
        mCampComp = mCamGo.GetComponent<Camera>();

        bgFinalGO = GameObject.Find("PanelGameOver");
        bgFinalGO.SetActive(false);

        audioFXGO = GameObject.Find("AudioFX");
        audioFXScript = audioFXGO.GetComponent<AudioFX>();

        cocheGO = GameObject.FindObjectOfType<Coche>().gameObject;

        VelocidadMotorCarrtera();
        MedirPantalla();
        BuscoCalles();

    }

    public void JuegoTerminadoEstados()
    {
        cocheGO.GetComponent<AudioSource>().Stop();
        audioFXScript.FXMusic();
        bgFinalGO.SetActive(true);
    }

    void VelocidadMotorCarrtera()
    {
        velocidad = 18;
    }

    void BuscoCalles()
    {
        contenedorCallesArray = GameObject.FindGameObjectsWithTag("Calle");
        for(int i = 0; i < contenedorCallesArray.Length; i++)
        {
            contenedorCallesArray[i].gameObject.transform.parent = contenedorCallesGO.transform;//incluye el objeto dentro de una jerarqu�a
            contenedorCallesArray[i].gameObject.SetActive(false);
            contenedorCallesArray[i].gameObject.name = "CalleOFF_" + i;
        }
        CrearCalles();
    }

    void CrearCalles()
    {
        contadorCalles ++;
        numeroSelectroCalles = Random.Range(0, contenedorCallesArray.Length);
        GameObject Calle = Instantiate(contenedorCallesArray[numeroSelectroCalles]);/*Clona la calle del array*/
        Calle.SetActive(true);
        Calle.name = "Calle" + contadorCalles;
        Calle.transform.parent = gameObject.transform;
        PosicionoCalles();
    }

    void PosicionoCalles()
    {
        calleAnterior = GameObject.Find("Calle" + (contadorCalles-1));
        calleNueva = GameObject.Find("Calle" + contadorCalles);
        MidoCalle();
        calleNueva.transform.position = new Vector3(calleAnterior.transform.position.x,
            calleAnterior.transform.position.y + tama�oCalle - 12, 0);

        salioDePantalla = false;
    }

    void MidoCalle()
    {
        for(int i = 0; i < calleAnterior.transform.childCount; i++)
        {
            if(calleAnterior.transform.GetChild(i).gameObject.GetComponent<Pieza>() != null)
            {
                float tama�oPieza = calleAnterior.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
                tama�oCalle = tama�oCalle + tama�oPieza;
            }
        }
    }

    void MedirPantalla()
    {
        medidaLimitePantalla = new Vector3(0, mCampComp.ScreenToWorldPoint(new Vector3(0, 0, 0)).y - 0.5f, 0);
    }

    void Update()
    {
        if(inicioJuego == true && juegoTerminado == false)
        {
            transform.Translate(Vector3.down * velocidad * Time.deltaTime);
            if (calleAnterior.transform.position.y + tama�oCalle < medidaLimitePantalla.y && salioDePantalla == false)
            {
                salioDePantalla = true;
                DestruyoCalles();

            }
        }
        
    }

    void DestruyoCalles()
    {
        Destroy(calleAnterior);
        tama�oCalle = 0;
        calleAnterior = null;
        CrearCalles();
    }
}
