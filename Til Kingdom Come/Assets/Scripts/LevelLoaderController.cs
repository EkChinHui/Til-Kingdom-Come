using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderController : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    [SerializeField] public string sceneName;
    public void LoadLevel(string sceneName) {
        StartCoroutine(Helper(sceneName));
    }

    IEnumerator Helper(string sceneName) {
        // Play animation
        transition.SetTrigger("Start");

        // Wait for animation to finish running
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}
