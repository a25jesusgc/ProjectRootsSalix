using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    private Image image;
    public bool transitionFinished { get; private set; }

    private const float DURATION = 0.5f;

    public static TransitionController instance; //Singleton
    void Awake()
    {
        // Singleton pattern
        instance = this;
    }

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void PlayTransition(bool show)
    {
        StartCoroutine(TransitionCoroutine(show));
    }

    private IEnumerator TransitionCoroutine(bool show)
    {
        transitionFinished = false;

        float time = 0f;
        while (time < DURATION)
        {
            time += Time.deltaTime;

            float origin = show ? 1f : 0f;
            float target = show ? 0f : 1f;
            image.color = new Color(0f, 0f, 0f, Mathf.Lerp(origin, target, time / DURATION));

            yield return null;
        }

        transitionFinished = true;
    }
}
