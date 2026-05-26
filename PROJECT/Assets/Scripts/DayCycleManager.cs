using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager instance;

    // 24 min
    private const float CYCLE_DURATION = 1440f;

    // Dia: De 9 a 21
    private const float DAY_START = 540f;
    private const float DAY_END = 1260f;

    // Mediodía a las 15
    private const float MID_DAY = 900f;

    // Noche: De 21 a 9
    private const float NIGHT_START = 1260f;
    private const float NIGHT_END = 540f;

    // Medianoche a la 3
    private const float MID_NIGHT = 180f;

    // Duración de la transicion de color (6 horas)
    private const float COLOR_CYCLE = 180f;

    // Colores para la iluminacion
    [SerializeField] private Color DAWN_COLOR;
    [SerializeField] private Color MID_DAY_COLOR;
    [SerializeField] private Color DUSK_COLOR;
    [SerializeField] private Color MID_NIGHT_COLOR;

    // Imagen para el reloj
    [SerializeField] private Image clockSprite;


    [SerializeField] private Light2D environmentLight;

    private float dayTime;

    public bool IsDay => dayTime >= DAY_START && dayTime < DAY_END;
    public bool IsNight => dayTime >= NIGHT_START || dayTime < NIGHT_END;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        dayTime = PlayerData.GetInstance.GetDayTime;
        environmentLight.color = GetTimeColor();
    }

    void Update()
    {
        if(GlobalUtils.pause) return;

        dayTime += Time.deltaTime;

        if(Application.isEditor && Input.GetKeyDown(KeyCode.P)){
            dayTime += 60f;
        }

        if (dayTime > CYCLE_DURATION)
        {
            dayTime = 0f;
        }

        PlayerData.GetInstance.SetDayTime(dayTime);

        environmentLight.color = GetTimeColor();

        clockSprite.fillAmount = (dayTime > (CYCLE_DURATION / 2f) ? dayTime - (CYCLE_DURATION / 2f) : dayTime) / (CYCLE_DURATION / 2f);
        Debug.Log(dayTime / 60f);
    }

    private Color GetTimeColor()
    {
        Color cycleStartColor = Color.white;
        Color cycleEndColor = Color.white;
        float t;

        if (IsDay)
        {
            if (dayTime < MID_DAY)
            {
                cycleStartColor = MID_DAY_COLOR;
                cycleEndColor = MID_DAY_COLOR;
                t = dayTime - DAY_START;
            }
            else
            {
                t = dayTime - MID_DAY;

                if (t < COLOR_CYCLE)
                {
                    cycleStartColor = MID_DAY_COLOR;
                    cycleEndColor = DUSK_COLOR;
                } else
                {
                    t -= COLOR_CYCLE;
                    cycleStartColor = DUSK_COLOR;
                    cycleEndColor = MID_NIGHT_COLOR;
                }
            }
        }
        else
        {
            if (dayTime < MID_NIGHT || dayTime >= NIGHT_START)
            {
                cycleStartColor = MID_NIGHT_COLOR;
                cycleEndColor = MID_NIGHT_COLOR;
                if(dayTime < MID_NIGHT)
                {
                    t = dayTime + CYCLE_DURATION - NIGHT_START;
                }
                else
                {
                    t = dayTime - NIGHT_START;
                }
            }
            else
            {
                t = dayTime - MID_NIGHT;

                if (t < COLOR_CYCLE)
                {
                    cycleStartColor = MID_NIGHT_COLOR;
                    cycleEndColor = DAWN_COLOR;
                } else
                {
                    t -= COLOR_CYCLE;
                    cycleStartColor = DAWN_COLOR;
                    cycleEndColor = MID_DAY_COLOR;
                }
            }
        }

        t = t / COLOR_CYCLE;

        return new Color(Mathf.Lerp(cycleStartColor.r, cycleEndColor.r, t), Mathf.Lerp(cycleStartColor.g, cycleEndColor.g, t), Mathf.Lerp(cycleStartColor.b, cycleEndColor.b, t));
    }
}
