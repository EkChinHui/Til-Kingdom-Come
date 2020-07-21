using System.Collections;
using GamePlay;
using GamePlay.Player;
using TMPro;
using UnityEngine;

namespace UI.GameUI.Round_Start
{
    public class RoundStartPanelController : MonoBehaviour
    {
        public TextMeshProUGUI roundNumberText;
        public PauseMenuController pauseMenuController;
        private RectTransform rectTransform;
        private float roundNumber = 1;
        private float speed = 500f;
        private float startYAxis;
        private float endYAxis = 0;
        private bool lowering = false;
        private bool waiting = false;
        private bool raising = false;
        private bool playSound = false;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            startYAxis = rectTransform.anchoredPosition.y;
            if (PlayerInput.onDisableInput != null) PlayerInput.onDisableInput();
            lowering = true;
            playSound = true;
            pauseMenuController.canPause = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (lowering && !waiting && !raising) {
                if (playSound) {
                    AudioManager.instance.PlaySoundEffect("Round Start");
                    playSound = false;
                }
                Lower();
            } else if (!lowering && waiting && !raising) {
                StartCoroutine(WaitFor(1));
            } else if (!lowering && !waiting && raising) {
                Raise();
            }
        }

        public void Lower() {
            if(rectTransform.anchoredPosition.y > endYAxis) {
                rectTransform.anchoredPosition -= new Vector2(0, Time.deltaTime * speed);
            } else {
                lowering = false;
                waiting = true;
            }
        }
        public void Raise() {
            if(rectTransform.anchoredPosition.y < startYAxis) {
                rectTransform.anchoredPosition += new Vector2(0, Time.deltaTime * speed);
            } else {
                raising = false;
                if (PlayerInput.onEnableInput != null) PlayerInput.onEnableInput();
                pauseMenuController.canPause = true;
            }
        }
        IEnumerator WaitFor(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            waiting = false;
            raising = true;
        }
        private void UpdateRoundNumber() {
            roundNumber++;
            roundNumberText.text = "Round " + roundNumber;
        }

        public void nextRound() {
            UpdateRoundNumber();
            if (PlayerInput.onDisableInput != null) PlayerInput.onDisableInput();
            lowering = true;
            pauseMenuController.canPause = false;
            playSound = true;
        }
    }
}
