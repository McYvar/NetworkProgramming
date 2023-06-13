using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBehaviour : MonoBehaviour
{
    public void GoToScene(int sceneId)
    {
        StartCoroutine(LoadScene(sceneId));
    }

    private IEnumerator LoadScene(int sceneId)
    {
        yield return null;
        SceneManager.LoadScene(sceneId);
    }
}
