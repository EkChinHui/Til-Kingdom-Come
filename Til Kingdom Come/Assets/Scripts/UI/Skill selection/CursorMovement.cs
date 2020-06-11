using UnityEngine;

namespace UI.Skill_selection
{
    public class CursorMovement : MonoBehaviour {

        public float speed;
        public KeyCode left;
        public KeyCode right;
        public KeyCode up;
        public KeyCode down;
        private float horizontal = 0;
        private float vertical = 0;

        void Update ()
        {
            UpdateInputManager();
            transform.position += new Vector3(horizontal, vertical, 0) * Time.deltaTime * speed;
            // Canvas Screen space has to be set to camera for it to work
            Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x),
                Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y),
                transform.position.z);
        }

        private void UpdateInputManager()
        {
            if (Input.GetKey(left))
            {
                horizontal = -1f;
            } else if (Input.GetKey(right))
            {
                horizontal = 1f;
            }
            else
            {
                horizontal = 0;
            }

            if (Input.GetKey(up))
            {
                vertical = 1f;
            } else if (Input.GetKey(down))
            {
                vertical = -1f;
            }
            else
            {
                vertical = 0;
            }
        }
    }
}