using UnityEngine;
using TMPro;

public class TimeClock : MonoBehaviour
{
    [Tooltip("Tiempo inicial en segundos")]
    public float tiempoInicial;
    [Tooltip("Escala del tiempo en reloj")]
    [Range(-10.0f, 10.0f)]
    public float escalaDeTiempo = 1;

    private TextMeshProUGUI myText;
    private float tiempoMostradoenSegundos = 0f;

    private float tiempoDelFrameConTimeScale = 0f;
    private float escalaDeTiempoAlPausar, escalaDeTiempoInicial;
    private bool estaPausado = false;
    private bool eventoTiempoCeroInvocado = false;

    //crear delegado para el evento Tiempo cero
    public delegate void AccionTiempoCero();

    //crea evento
    public static event AccionTiempoCero AlLlegarAcero;
    public float duracionNivelEnSegundos = 60f;
    public bool tiempoMaximoAlcanzado = false;


    // Start is called before the first frame update
    void Start()
    {//escala de tiempo orignial
        escalaDeTiempoInicial = escalaDeTiempo;

        myText = GetComponent<TextMeshProUGUI>();
        //acumula tiempos de frame con frame inicial
        tiempoMostradoenSegundos = tiempoInicial;

        ActualizarReloj(tiempoInicial);
    }

    public bool TiempoMaximoSuperado()
    {
        return tiempoMaximoAlcanzado;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!estaPausado)
        {
            //Representa el valor de cada frame en escala de tiempo
            tiempoDelFrameConTimeScale = Time.deltaTime * escalaDeTiempo;
            //acumula el tiempo transcurrido
            tiempoMostradoenSegundos += tiempoDelFrameConTimeScale;
            ActualizarReloj(tiempoMostradoenSegundos);

            if(tiempoMostradoenSegundos > duracionNivelEnSegundos)
            {
                Pausar();
                CambiarARojo();
                tiempoMaximoAlcanzado = true;
            }
        }
    }

    private void CambiarARojo()
    {
        myText.color = Color.red;
    }

    public void CambiarAVerde()
    {
        myText.color = Color.green;
    }

    public void ActualizarReloj(float tiempoEnSegundos)
    {
        int minutos = 0;
        int segundos = 0;
        string textoDelReloj;

        //disparar el evento al llegar a cero
        if (tiempoEnSegundos <= 0 && !eventoTiempoCeroInvocado)
        {
            if (AlLlegarAcero != null)
            {
                AlLlegarAcero();
            }
            eventoTiempoCeroInvocado = true;
        }

        //asegurar que el tiempo No sea negativo
        if (tiempoEnSegundos < 0)
            tiempoEnSegundos = 0;

        minutos = (int)tiempoEnSegundos / 60;
        segundos = (int)tiempoEnSegundos % 60;

        textoDelReloj = minutos.ToString("00") + ":" + segundos.ToString("00");

        myText.text = textoDelReloj;
    }

    public void Pausar()
    {
        if (!estaPausado)
        {
            estaPausado = true;
            escalaDeTiempoAlPausar = escalaDeTiempo;
            escalaDeTiempo = 0;
        }
    }
    public void Continuar()
    {
        if (estaPausado)
        {
            estaPausado = false;
            escalaDeTiempo = escalaDeTiempoAlPausar;

        }

    }

    public void Reiniciar()
    {
        estaPausado = false;
        eventoTiempoCeroInvocado = false;
        escalaDeTiempo = tiempoInicial;
        ActualizarReloj(tiempoMostradoenSegundos);
        tiempoMaximoAlcanzado = false;
    }
}
