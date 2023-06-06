using TMPro;
using UnityEngine;
using Zenject;

public class Record : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recordText;
    
    private Player _player;
    
    [Inject]
    private void Construct(Player player)
    {
        _player = player;
    }

    private void OnEnable()
    {
        _player.OnPlayerDiedEvent += ShowRecord;
    }

    private void OnDisable()
    {
        _player.OnPlayerDiedEvent -= ShowRecord;
    }

    private void ShowRecord()
    {
        //recordText.text = "Рекорд: " + PlayerPrefs.GetInt("Record", 0);
        recordText.text = "Рекорд: " + Db.ExecuteQueryWithAnswer("SELECT Record FROM Player WHERE Id = 1");
    }
}
