using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        private Vector3 initialPosCamera;

        private void Start()
        {
            initialPosCamera = transform.position;
        }

        private void Update()
        {
            if (target == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if(obj == null)
            {
                return ;
            }
            target = obj.transform;
            offset = transform.position - target.position;
        }
            Vector3 targetPosition = new Vector3(transform.position.x,transform.position.y,this.gameObject.transform.position.z);
            targetPos = targetPosition + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }
        void ResetCamera()
        {
            offset = Vector3.zero;
            transform.position =initialPosCamera;
        }

    }