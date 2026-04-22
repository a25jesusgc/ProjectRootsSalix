using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency instance;

    [SerializeField] private Transform layout;
    [SerializeField] private Transform showPos;
    [SerializeField] private Transform hidePos;
    [SerializeField] private TextMeshProUGUI txtCurrentAmount;
    [SerializeField] private TextMeshProUGUI txtAmountToAdd;

    private int currentAmount;
    private int amountToAdd;

    private bool show;
    public bool forceShow;
    private float speed = 2000f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentAmount = PlayerData.GetInstance.GetCurrency;
        amountToAdd = 0;
        UpdateTexts();
    }

    void Update()
    {
        layout.position = Vector3.MoveTowards(layout.position, forceShow || show ? showPos.position : hidePos.position, Time.deltaTime * speed);
    }

    public void ChangeCurrency(int amountToAdd)
    {
        this.amountToAdd += amountToAdd;
        StopCoroutine("ChangeCurrencyCoroutine");
        StartCoroutine("ChangeCurrencyCoroutine");
    }

    private IEnumerator ChangeCurrencyCoroutine()
    {
        show = true;
        UpdateTexts();

        yield return new WaitForSeconds(2f);

        while(amountToAdd != 0)
        {
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
            
            UpdateTexts();

            yield return new WaitForSeconds(0.02f);
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
