using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LevelLoaderController : MonoBehaviour
    {
        public Animator transition;
        public float transitionTime = 1f;
        public void LoadLevel(string sceneName) {
            StartCoroutine(Helper(sceneName));
        }

        private IEnumerator Helper(string sceneName) {
            // Play animation
            transition.SetTrigger("Start");

            // Wait for animation to finish running
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
        }

        public void QuitGame()
        {
            Debug.Log("Quiting game");
            Application.Quit();
        }

        public void StopThemeMusic() {
            AudioManager.instance.FadeOut("Theme", 1);
        }
    }
}
