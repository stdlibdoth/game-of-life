using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRigRoot : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private Transform m_cameraTransform;

    [Space]
    [Header("Zoom")]
    [SerializeField] private float m_zoomSpeed;
    [SerializeField] private float m_initialZoom;
    [SerializeField] private Vector2 m_zoomRange;


    [Header("Edge Scroll")]
    [SerializeField] private Vector2Int m_scrollEdgeWidth;


    private NavigationInputs m_navigationInputs;

    private Vector2Int m_screenScrollRangeX;
    private Vector2Int m_screenScrollRangeY;
    private bool m_isEdge;
    private Vector2 m_inputPosition;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        m_navigationInputs = new NavigationInputs();
        m_camera.orthographic = true;
    }

    private void OnEnable()
    {
        m_navigationInputs.CameraControlMap.InputPositionAction.performed += InputPositionAction_performed;
        m_navigationInputs.CameraControlMap.InputPagingAction.performed += InputPagingAction_performed;
        m_navigationInputs.CameraControlMap.Enable();

        m_screenScrollRangeX = new Vector2Int(m_scrollEdgeWidth.x, Screen.width - m_scrollEdgeWidth.x);
        m_screenScrollRangeY = new Vector2Int(m_scrollEdgeWidth.y, Screen.height - m_scrollEdgeWidth.y);
        m_camera.orthographicSize = m_initialZoom;
    }


    void Update()
    {
        if (m_isEdge)
        {
            Vector3 delta = Vector3.zero;
            if (m_inputPosition.x <= m_screenScrollRangeX.x)
            {
                delta += -m_cameraTransform.right;
            }
            if (m_inputPosition.x >= m_screenScrollRangeX.y)
            {
                delta += m_cameraTransform.right;
            }
            if (m_inputPosition.y <= m_screenScrollRangeY.x)
            {
                delta += -m_cameraTransform.up;
            }
            if (m_inputPosition.y >= m_screenScrollRangeY.y)
            {
                delta += m_cameraTransform.up;
            }
            delta = delta.normalized * Time.deltaTime * m_camera.orthographicSize*1.5f;
            m_cameraTransform.position += delta;
        }
    }

    private void InputPositionAction_performed(InputAction.CallbackContext obj)
    {
        m_inputPosition = obj.ReadValue<Vector2>();

        if (m_inputPosition.x < 0 || m_inputPosition.x > Screen.width || m_inputPosition.y < 0 || m_inputPosition.y > Screen.height)
            m_isEdge = false;
        else
        {
            m_isEdge = m_inputPosition.x <= m_screenScrollRangeX.x || m_inputPosition.x >= m_screenScrollRangeX.y
                || m_inputPosition.y <= m_screenScrollRangeY.x || m_inputPosition.y >= m_screenScrollRangeY.y;
        }

    }
    private void InputPagingAction_performed(InputAction.CallbackContext obj)
    {
        var val = obj.ReadValue<Vector2>();
        float delta = -math.sign(val.y) * m_zoomSpeed;
        m_camera.orthographicSize = math.clamp(m_camera.orthographicSize + delta, m_zoomRange.x, m_zoomRange.y);
    }
}
