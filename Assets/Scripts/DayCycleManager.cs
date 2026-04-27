using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager instance;

    // 24 min
    private const float CYCLE_DURATION = 1440f;

    // Dia: De 7 a 19
    private const float DAY_START = 420f;
    private const float DAY_END = 1140f;

    // Dia: De 19 a 7
    private const float NIGHT_START = 1140f;
    private const float NIGHT_END = 420f;

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

        if (dayTime > CYCLE_DURATION)
        {
            dayTime = 0f;
        }

        PlayerData.GetInstance.SetDayTime(dayTime);

        environmentLight.color = GetTimeColor();

        if(Application.isEditor && Input.GetKeyDown(KeyCode.P)){
            dayTime += 60f;
        }
    }

    private Color GetTimeColor()
    {
        return IsNight ? new Color(0.5f, 0.5f, 1f, 1f) : Color.white;
    }
}
