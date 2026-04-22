using TMPro;
using UnityEngine;

public class FastTravel : MonoBehaviour
{
    [SerializeField] private Checkpoint checkpoint;
    [SerializeField] private TextMeshProUGUI zoneName;

    void Start()
    {
        zoneName.text = checkpoint.GetName;
    }

    public void Travel()
    {
        ZoneLoader.instance.ChangeZone(PlayerData.GetInstance.GetCheckpoint.GetScene, checkpoint.GetScene, checkpoint.GetPosition);
        PlayerData.GetInstance.SetCheckpoint(checkpoint);
    }
}
