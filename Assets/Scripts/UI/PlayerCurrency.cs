using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{

    // Instancia singleton para poder acceder desde cualquier otro script
    public static PlayerCurrency instance;


    // Panel que muestra el dinero actual del jugador
    [SerializeField] private Transform layout;

    // Puntos a los que se mueve el panel según debe verse o no
    [SerializeField] private Transform showPos;
    [SerializeField] private Transform hidePos;
    

    // Textos que muestran el dinero actual y el cambio a realizar
    [SerializeField] private TextMeshProUGUI txtCurrentAmount;
    [SerializeField] private TextMeshProUGUI txtAmountToAdd;

    [SerializeField] private AudioSource getCoinSFX;
    private float soundEffectPitch = 1f;
    private float soundEffectTimer;


    // Variables que almacenan el dinero actual mostrado y el valor a añadir/restar
    private int currentAmount;
    private int amountToAdd;


    // Bandera que determina si mostrar o no el panel
    private bool show;

    // Bandera que fuerza la muestra del panel
    public bool forceShow;

    // Velocidad de desplazamiento del panel
    private float speed = 2000f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Al iniciar, cargamos el dinero del jugador según su archivo de guardado
        currentAmount = PlayerData.GetInstance.GetCurrency;
        amountToAdd = 0;
        UpdateTexts();
    }

    void Update()
    {
        //layout.position = Vector3.MoveTowards(layout.position, forceShow || show ? showPos.position : hidePos.position, Time.deltaTime * speed);
        layout.position = Vector3.MoveTowards(layout.position, true || show ? showPos.position : hidePos.position, Time.deltaTime * speed);
        if(soundEffectTimer > 0f)
        {
            soundEffectTimer -= Time.deltaTime;
        }
    }


    // Función que muestra un cambio de dinero
    public void ChangeCurrency(int amountToAdd)
    {
        if (amountToAdd > 0)
        {
            if(soundEffectTimer > 0)
            {
                soundEffectPitch = soundEffectPitch + 0.05f;
            }
            else
            {
                soundEffectPitch = 1f;
            }
            getCoinSFX.pitch = soundEffectPitch;
            soundEffectTimer = 2f;
            getCoinSFX.Play();
        }
        // Actualiza el valor a modificar, detiene la corrutina si ya estaba activa y la inicia de nuevo
        this.amountToAdd += amountToAdd;
        StopCoroutine("ChangeCurrencyCoroutine");
        StartCoroutine("ChangeCurrencyCoroutine");
    }


    // Corrutina que muestra el panel, mostrando el dinero actual y mostrando el valor a añadir/restar
    private IEnumerator ChangeCurrencyCoroutine()
    {

        // Muestra el panel con los textos actualizados
        show = true;
        UpdateTexts();

        // Tiempo de espera (para ver la acumulación de dinero añadido en caso de seguir cogiendo monedas)
        yield return new WaitForSeconds(2f);

        float totalAmountToAdd = amountToAdd;

        // Mientras queda dinero que añadir / sustraer
        while(amountToAdd != 0)
        {
            

            // Va traspasando unidad a unidad el dinero del añadido al actual, actualizando así el dinero actual
            if (amountToAdd > 0)
            {
                currentAmount++;
                amountToAdd--;
            }
            else
            {
                currentAmount--;
                amountToAdd++;
            }
            

            // Tras actualizar variables, actualiza textos
            UpdateTexts();


            // Tiempo de espera entre unidad y unidad
            yield return new WaitForSeconds(Mathf.Clamp(4f / totalAmountToAdd, 0f, 0.02f));
        }

        yield return new WaitForSeconds(1f);
        show = false;
    }

    private void UpdateTexts()
    {
        txtCurrentAmount.text = currentAmount.ToString();
        txtAmountToAdd.text = amountToAdd == 0 ? "" : (amountToAdd > 0 ? "+" : "") + amountToAdd.ToString();
    }
}
