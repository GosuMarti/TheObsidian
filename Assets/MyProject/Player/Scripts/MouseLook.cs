using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class MouseLook
    {
        [SerializeField] 
        private float sensitivityX = 2f;
        [SerializeField] 
        private float sensitivityY = 2f;
        [SerializeField] 
        private bool clampVerticalRotation = true;
        [SerializeField] 
        private float minVerticalRotation = -90f;
        [SerializeField] 
        private float maxVerticalRotation = 90f;
        [SerializeField] 
        private bool smoothRotation = false;
        [SerializeField] 
        private float smoothTime = 5f;
        [SerializeField] 
        private bool lockCursor = true;

        private Quaternion characterTargetRotation;
        private Quaternion cameraTargetRotation;

        public void Init(Transform character, Transform camera)
        {
            characterTargetRotation = character.localRotation;
            cameraTargetRotation = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            float mouseX = Input.GetAxis("MouseX") * sensitivityX;
            float mouseY = Input.GetAxis("MouseY") * sensitivityY;

            characterTargetRotation *= Quaternion.Euler(0f, mouseX, 0f);
            cameraTargetRotation *= Quaternion.Euler(-mouseY, 0f, 0f);

            if (clampVerticalRotation)
            {
                cameraTargetRotation = ClampRotationAroundXAxis(cameraTargetRotation);
            }

            if (smoothRotation)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, characterTargetRotation, smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRotation, smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = characterTargetRotation;
                camera.localRotation = cameraTargetRotation;
            }

            UpdateCursorLock();
        }

        private Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1f;

            float angleX = 2f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, minVerticalRotation, maxVerticalRotation);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void UpdateCursorLock()
        {
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
