using UnityEngine;


public class Joystick : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasTransform;
    [SerializeField] private RectTransform _joystickWrapper;
    [SerializeField] private RectTransform _joystickOutline;
    [SerializeField] private RectTransform _handle;

    private Vector2 _getAxis;
    private Vector2 _joystickStartPos;
    private Vector2 _offset;

    private float _radius;    

    private bool _isOnDrag = false;

    public float Horizontal => _getAxis.x;
    public float Vertical => _getAxis.y;    
    public bool IsOnDrag => _isOnDrag;

    private void Start()
    {
        _joystickWrapper.sizeDelta = new Vector2(_canvasTransform.rect.width, _canvasTransform.rect.height / 2);
        _joystickWrapper.anchoredPosition = new Vector2(0, -(_joystickWrapper.rect.height / 2));
        _joystickOutline.anchoredPosition = new Vector2(_joystickWrapper.rect.width / 2, _joystickWrapper.rect.height / 2);     
        _joystickStartPos = new Vector2(_joystickOutline.anchoredPosition.x, _joystickOutline.anchoredPosition.y);
        _offset = _joystickStartPos;
        _radius = _handle.sizeDelta.x;
    }

    private void Update()
    {      
        Vector3 mousePos = Input.mousePosition;        

        if (Input.GetMouseButton(0))
        {
            if (!_isOnDrag)
            {
                _joystickOutline.anchoredPosition = new Vector2(mousePos.x, mousePos.y);
                _offset = _joystickOutline.anchoredPosition;
                _isOnDrag = true;
            }

            Vector3 handleOnMousePos = new Vector3(mousePos.x - _offset.x, mousePos.y - _offset.y, _handle.position.z);

            _handle.localPosition = Vector3.ClampMagnitude(handleOnMousePos, _radius);            
        }
        else
        {
            _joystickOutline.anchoredPosition = _joystickStartPos;
            _isOnDrag = false;
            _handle.localPosition = Vector3.zero;
        }

        _getAxis = _handle.localPosition.normalized;
    }
}
