using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button retryButton;

    private void OnEnable()
    {
        retryButton.onClick.AddListener(Retry);
    }

    private void OnDisable()
    {
        retryButton.onClick.RemoveListener(Retry);
    }

    public void EnablePanel()
    {
        panel.SetActive(true);
    }

    public void DisablePanel()
    {
        panel.SetActive(false);
    }

    private void Retry()
    {
        SceneManager.LoadScene("Main");
    }
}
