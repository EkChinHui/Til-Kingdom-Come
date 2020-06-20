using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    [RequireComponent(typeof(Camera))]
    public class MultipleTargetCamera : MonoBehaviour
    {
        public List<Transform> targets;
        public Vector3 offset;
        public float smoothTime = 0.5f;

        [Header("Zoom")]
        public float minZoom = 40f;
        public float maxZoom = 10f;
        public float zoomLimiter = 20f;
        private Vector3 velocity;
        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (targets.Count == 0)
            {
                return;
            }
            Move();
            Zoom();
        }

        void Zoom()
        {
            float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance()/zoomLimiter);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
        }

        float GetGreatestDistance()
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for(int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }

            return bounds.size.x;
        }

        private void Move()
        {
            Vector3 centerPoint = GetCentrePoint();

            Vector3 newPosition = centerPoint + offset;

            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }

        Vector3 GetCentrePoint()
        {
            if (targets.Count == 1)
            {
                return targets[0].position;
            }

            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for(int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.center;
        }
    }
}
