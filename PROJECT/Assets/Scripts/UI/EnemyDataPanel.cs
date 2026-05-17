using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDataPanel : MonoBehaviour
{
    [SerializeField] private Image enemyImage;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI defeatCountText;
    [SerializeField] private TextMeshProUGUI basicDescText;
    [SerializeField] private GameObject advancedData;
    [SerializeField] private TextMeshProUGUI advancedDescText;
    [SerializeField] private GameObject killsRemaining;

    private int enemyDefeatCount;
    private bool showBasicData;
    private bool showAdvancedData;

    public void LoadEnemyData(Enemy enemy)
    {
        enemyDefeatCount = PlayerData.GetInstance.GetEnemyDefeatCount(enemy.GetEnemyType);
        showBasicData = enemyDefeatCount > 0;
        showAdvancedData = enemyDefeatCount >= enemy.GetRequiredDefeatedCount;

        enemyImage.sprite = enemy.GetEnemySprite;
        enemyImage.color = showBasicData ? Color.white : Color.black;

        enemyNameText.text = showBasicData ? enemy.GetEnemyName : "???";
        defeatCountText.text = enemyDefeatCount.ToString();
        basicDescText.text = showBasicData ? enemy.GetEnemyBasicDesc : "";

        advancedData.SetActive(showAdvancedData);
        advancedDescText.text = enemy.GetEnemyAdvDesc;
        killsRemaining.SetActive(!showAdvancedData);
    }
}
