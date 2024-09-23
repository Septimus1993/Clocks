using UnityEngine;

namespace ClockEngine
{
    public delegate void TimeAddCallback(double time);

    public class HandDragger : MonoBehaviour
    {
        private bool isDragging = false;
        private float rotationOffset;

        public event TimeAddCallback onAngleChanged; 

        private void Update()
        {
            UpdateDrag();

            if (Input.GetMouseButtonUp(0))
                this.isDragging = false;
        }

        private void OnMouseDown()
        {
            if (!this.enabled)
                return;

            var angle = CalculateAngle();

            this.isDragging = true;
            this.rotationOffset = angle - this.transform.eulerAngles.z;
        }

        private void UpdateDrag()
        {
            if (!this.isDragging)
                return;

            var angle = CalculateAngle() - this.rotationOffset;
            var deltaAngle = Mathf.DeltaAngle(angle, this.transform.eulerAngles.z);
            
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
            this.onAngleChanged?.Invoke(deltaAngle);
        }

        private float CalculateAngle()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePos - this.transform.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}