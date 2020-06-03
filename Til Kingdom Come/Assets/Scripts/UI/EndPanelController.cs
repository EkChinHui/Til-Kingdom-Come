using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EndPanelController : MonoBehaviour
    {
        public EndPanelController endPanel;
        public RedVictoryScreenController redVictoryScreen;
        public BlueVictoryScreenController blueVictoryScreen;
        public void EndGame(int player)
        {
            endPanel.gameObject.SetActive(true);
            if (player == 1) {
                redVictoryScreen.gameObject.SetActive(true);
            } else if (player == 2) {
                blueVictoryScreen.gameObject.SetActive(true);
            }
        }

        // called with the restart button
        [UsedImplicitly]
        public void ReloadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
