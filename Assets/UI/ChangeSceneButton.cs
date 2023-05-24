using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string sceneName;
    
    private void OnEnable()
    {
        button.onClick.AddListener(ChangeScene);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ChangeScene);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
