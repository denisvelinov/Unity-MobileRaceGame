using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class SteeringWheelScript : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform rect; //use for find the touch field center point
        [SerializeField] private RectTransform rectImage; //used to rotate the steering wheel image
        [SerializeField] private Image steeringWheelImage;
        private Vector2 centerPoint;
        [SerializeField] private float maximumSteeringAngle = 200f;
        public bool canMove;

        private string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
        private string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

        private bool m_UseX; // Toggle for using the x axis
        private bool m_UseY; // Toggle for using the Y axis

        private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
        private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

        private float wheelAngle = 0f; //to find whether the wheel is held or not

        private void Start()
        {
            GetCenterPoint();
            SetSteeringWheelImageTransparency(0.5f);

            canMove = true;
        }

        void OnEnable()
        {
            CreateVirtualAxes();
        }

        void UpdateVirtualAxes(Vector3 value)
        {
            var delta = (Vector3)centerPoint - value;
            
            m_HorizontalVirtualAxis.Update(-delta.x/maximumSteeringAngle);
            m_VerticalVirtualAxis.Update(delta.y);
        }

        void CreateVirtualAxes()
        {
            m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
            m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
            CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
        }

        // to calculate the center of the image
        private void GetCenterPoint()
        {   
            //to get the position of the corners of the image in the world
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);

            for (int i = 0; i < 4; i++)
            {
                corners[i] = RectTransformUtility.WorldToScreenPoint(null, corners[i]);
            }

            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];
            float width = topRight.x - bottomLeft.x;
            float height = topRight.y - bottomLeft.y;

            Rect _rect = new Rect(bottomLeft.x, topRight.y, width, height);
            centerPoint = new Vector2(_rect.x + _rect.width * 0.5f, _rect.y - _rect.height * 0.5f);
        }
        //end here and we get the centerpoint of the rect image which is stored in the centerPoint

        //for the events
        //IpointerDownHandler , IDragHandler ,   IPointerUpHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            centerPoint = eventData.position;

            SetSteeringWheelImageTransparency(1f);

            // Movement input
            Vector2 inputVector = new Vector2(centerPoint.x, centerPoint.y - 10);
            if (canMove)
            {
                UpdateVirtualAxes(inputVector);
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pointerPos = eventData.position;

            float wheelOffset = Mathf.Abs(pointerPos.x - centerPoint.x);
            float steeringPosFloat = centerPoint.x;

            if (pointerPos.x > centerPoint.x)
            {
                wheelAngle = wheelOffset;
                steeringPosFloat += wheelOffset / 4;
            }
            else
            {
                wheelAngle = -wheelOffset;
                steeringPosFloat -= wheelOffset / 4;
            }

            // Make sure wheel angle never exceeds maximumSteeringAngle
            wheelAngle = Mathf.Clamp(wheelAngle, -maximumSteeringAngle, maximumSteeringAngle);
            
            // Rotate the wheel image
            rectImage.localEulerAngles = new Vector3(0, 0, -1) * wheelAngle;

            // Movement input
            Vector2 inputVector = new Vector2(steeringPosFloat, centerPoint.y - 10);
            if (canMove)
            {
                UpdateVirtualAxes(inputVector);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Set steeringwheel angle to default position
            wheelAngle = 0;
            rectImage.localEulerAngles = new Vector3(0, 0, -1) * wheelAngle;

            SetSteeringWheelImageTransparency(0.5f);

            // Movement input
            Vector2 inputVect = new Vector2(centerPoint.x, centerPoint.y + 10);
            UpdateVirtualAxes(inputVect);
        }
        //end of the eventHandlers here

        // Set steeringwheel transparency
        private void SetSteeringWheelImageTransparency(float amount) 
        {
            steeringWheelImage.color = new Color(steeringWheelImage.color.r, steeringWheelImage.color.g, steeringWheelImage.color.b, amount);
        }

        void OnDisable()
        {
            // remove the joysticks(touchpads) from the cross platform input
            if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
                CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);

            if (CrossPlatformInputManager.AxisExists(verticalAxisName))
                CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
        }
    }
}