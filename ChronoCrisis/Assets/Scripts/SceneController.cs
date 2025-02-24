using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.LogWarning("Duplicate SceneController destroyed.");
            Destroy(gameObject);
        }
    }

    public void ChangeScene(int sceneIndex)
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Save();
        }

        Debug.Log("Changing scene to index: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
}
