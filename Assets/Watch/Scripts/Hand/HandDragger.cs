using UnityEngine;

namespace ClockEngine
{
    public interface IAngleDraggable
    {
        void AddAngle(double deltaAngle);
    }

    public class HandDragger : MonoBehaviour
    {
        private IAngleDraggable draggable;

        private bool isDragging = false;
        private float rotationOffset;

        public void Initialize(IAngleDraggable draggable)
        {
            this.draggable = draggable;
        }

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
            
            this.draggable.AddAngle(deltaAngle);
        }

        private float CalculateAngle()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePos - this.transform.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}