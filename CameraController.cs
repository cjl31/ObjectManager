    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private ShowMenuAnimation menuAnimation;
        [SerializeField] private ElementsMenu elementsMenu;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private EventSystem eventSystem;   

        // Camera settings
        [SerializeField] private float distance = 10.0f;
        [SerializeField] private float mouseSensitivity = 150.0f;
        [SerializeField] private float scrollSensitivity = 3.0f;
        [SerializeField] private float translateSensitivity = 10.0f;
        [SerializeField] private float minY = -20f;
        [SerializeField] private float maxY = 80f;
        [SerializeField] private float minDistance = 2.0f;
        [SerializeField] private float maxDistance = 20.0f;
        [SerializeField] private float smoothMoveSpeed = 5.0f;
        [SerializeField] private float smoothRotateSpeed = 5.0f;
        
        // State variables
        private Vector3 lookAtPosition = Vector3.zero;
        private float currentX = 0f;
        private float currentY = 0f;
        private bool isMoving = false; // Если isMoving == true (мы начали движение ранее вне UI то можем двигаться) 
        
        public void UpdateFocus()
        {
            var selectedElements = elementsMenu.elements.Where(elem => elem.isSelected).ToList();
            if (selectedElements.Count > 0)
            {
                Vector3 position = selectedElements.Aggregate(Vector3.zero, (acc, elem) => acc + elem.referenceObject.transform.position) 
                / selectedElements.Count;
                ChangeLookAtPosition(position);
            }
        } 

        public bool CheckOnUI()
        {
            if(!menuAnimation.isMenuOpen)
            {
                return false;
            }
            PointerEventData pointerData = new PointerEventData(eventSystem) { position = Input.mousePosition };
            var results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerData, results);
            return results.Count > 0 && !isMoving;
        }

        private void Update() 
        {
           if(!CheckOnUI())
            {  
                HandleMouseInput();        
            }    
        }
        
        private void FixedUpdate()
        {
            SmoothFocusOnTarget();
        }   

        private void HandleMouseInput()
        {
            if (Input.GetMouseButton(1))
            {
                isMoving = true;
                HandleCameraRotation();
            }
            if (Input.GetMouseButton(2))
            {
                isMoving = true;
                HandleCameraTranslation();
            }
            if (Input.GetMouseButtonDown(0))
            {
                FocusOnObjectFromMouse();
            }
            HandleCameraZoom();

            if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
            {
                isMoving = false;
            }
        }

        private void FocusOnObjectFromMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    ColorChangeableObject interactable = hit.collider.GetComponent<ColorChangeableObject>();
                    if (interactable != null)
                    {
                        ChangeLookAtPosition(interactable.transform.position);
                    }
                }
        }

        private void ChangeLookAtPosition(Vector3 newTarget)
        {
            lookAtPosition = newTarget;
        }

        private void HandleCameraTranslation()
        {
                float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                Vector3 move = -transform.right * moveX - transform.up * moveY;
                lookAtPosition += move * translateSensitivity * Time.deltaTime;
        }
    
        private void HandleCameraRotation()
        {
                currentX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                currentY = Mathf.Clamp(currentY, minY, maxY);
        }
        
        private void SmoothFocusOnTarget()
        {
            // Smooth rotate
            Quaternion targetRotation = Quaternion.Euler(currentY, currentX, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothRotateSpeed * Time.deltaTime);

            // Smooth translate
            Vector3 targetPosition = lookAtPosition - targetRotation * Vector3.forward * distance;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothMoveSpeed * Time.deltaTime);
        }
        
        private void HandleCameraZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                distance -= scroll * scrollSensitivity;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            }
        }
    }
