using UnityEngine;

namespace GamePlay
{

    /// <summary>
    /// Camera Effects
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        /// <summary>
        /// Amount of Shake
        /// </summary>
        public Vector3 amount = new Vector3(1f, 1f, 0);

        /// <summary>
        /// Duration of Shake
        /// </summary>
        public float duration = 1;

        /// <summary>
        /// Shake Speed
        /// </summary>
        public float speed = 10;

        /// <summary>
        /// Amount over Lifetime [0,1]
        /// </summary>
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        /// <summary>
        /// Set it to true: The camera position is set in reference to the old position of the camera
        /// Set it to false: The camera position is set in absolute values or is fixed to an object
        /// </summary>
        public bool deltaMovement = true;

        protected Camera cam;
        protected float time = 0;
        protected Vector3 lastPos;
        protected Vector3 nextPos;
        protected float lastFoV;
        protected float nextFoV;
        protected bool destroyAfterPlay;

        /// <summary>
        /// awake
        /// </summary>
        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        /// <summary>
        /// Do the shake
        /// </summary>
        public static void ShakeOnce(float duration = 1f, float speed = 10f, Vector3? amount = null, Camera camera = null, bool deltaMovement = true, AnimationCurve curve = null)
        {
            //set data
            var instance = ((camera != null) ? camera : Camera.main)?.gameObject.AddComponent<CameraShake>();
            instance.duration = duration;
            instance.speed = speed;
            if (amount != null)
                instance.amount = (Vector3)amount;
            if (curve != null)
                instance.curve = curve;
            instance.deltaMovement = deltaMovement;

            //one time
            instance.destroyAfterPlay = true;
            instance.Shake();
        }

        /// <summary>
        /// Do the shake
        /// </summary>
        public void Shake()
        {
            ResetCam();
            time = duration;
        }

        private void LateUpdate()
        {
            if (time > 0)
            {
                //do something
                time -= Time.deltaTime;
                if (time > 0)
                {
                    //next position based on perlin noise
                    nextPos = (Mathf.PerlinNoise(time * speed, time * speed * 2) - 0.5f) * amount.x * transform.right * curve.Evaluate(1f - time / duration) +
                              (Mathf.PerlinNoise(time * speed * 2, time * speed) - 0.5f) * amount.y * transform.up * curve.Evaluate(1f - time / duration);
                    nextFoV = (Mathf.PerlinNoise(time * speed * 2, time * speed * 2) - 0.5f) * amount.z * curve.Evaluate(1f - time / duration);

                    cam.fieldOfView += (nextFoV - lastFoV);
                    cam.transform.Translate(deltaMovement ? (nextPos - lastPos) : nextPos);

                    lastPos = nextPos;
                    lastFoV = nextFoV;
                }
                else
                {
                    //last frame
                    ResetCam();
                    if (destroyAfterPlay)
                        Destroy(this);
                }
            }
        }

        private void ResetCam()
        {
            //reset the last delta
            cam.transform.Translate(deltaMovement ? -lastPos : Vector3.zero);
            cam.fieldOfView -= lastFoV;

            //clear values
            lastPos = nextPos = Vector3.zero;
            lastFoV = nextFoV = 0f;
        }
    }

}