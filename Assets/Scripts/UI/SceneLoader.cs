 using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // لود کردن صحنه با نام
    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f; // اطمینان از ریست شدن زمان بازی
        SceneManager.LoadScene(sceneName);
    }

    // لود کردن صحنه با شماره ایندکس (اختیاری)
    public void LoadSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }
}
