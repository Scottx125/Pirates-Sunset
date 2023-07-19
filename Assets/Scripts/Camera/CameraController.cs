using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour, ICameraInterfaces
{
    [SerializeField]
    private Material _transparentMaterial;
    [SerializeField]
    private GameObject _transparentDistanceObj;
    [SerializeField]
    private float _maxDistanceForTransparency = 20f;
    [SerializeField]
    private float _minDistanceForTransparency = 20f;
    [SerializeField]
    private CinemachineFreeLook _freeLookCam;
    [SerializeField]
    private float _forwardRange = 40f;
    [SerializeField]
    private float _leftRange = 140f;
    [SerializeField]
    private float _rightRange = 140f;
    [SerializeField]
    private float _behindRange = 40f;
    
    private Color _transparency;

    void Start()
    {
        if (_freeLookCam == null) GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        SailTransparency();
    }

    // Uses camera range from a preset target to set the alpha of the sails.
    private void SailTransparency()
    {
        if (_transparentMaterial == null || _transparentDistanceObj == null) return;
        float distance = Vector3.Distance(_freeLookCam.transform.position, _transparentDistanceObj.transform.position);
        _transparency = _transparentMaterial.color;
        _transparency.a = 1f - Mathf.Clamp01((distance - _minDistanceForTransparency) / (_maxDistanceForTransparency - _minDistanceForTransparency));
        _transparentMaterial.color = _transparency;
    }

    // Checks if mouse is enabled.
    public void IsEnabled(bool state)
    {
        if (_freeLookCam == null) return;
        if (!state){
            _freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
            _freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
        } else {
            _freeLookCam.m_XAxis.m_InputAxisName = "";
            _freeLookCam.m_XAxis.m_InputAxisValue = 0f;
            _freeLookCam.m_YAxis.m_InputAxisName = "";
            _freeLookCam.m_YAxis.m_InputAxisValue = 0f;
        }
    }
    // Take the input and convert it into 360 degrees.
    // Then determine the direction.
    public CannonPositionEnum? CalculateFiringPosition()
    {
        float totalRanges = _forwardRange + _leftRange + _rightRange + _behindRange;
        float convertTo360Value = 360f / totalRanges;

        float forwardRange = convertTo360Value * _forwardRange / 2f;
        float rightRange = forwardRange + (convertTo360Value * _rightRange);
        float behindRange = rightRange + (convertTo360Value * _behindRange);
        float leftRange = behindRange + (convertTo360Value * _leftRange);
        
        if (_freeLookCam.m_XAxis.Value > (360f - forwardRange) || _freeLookCam.m_XAxis.Value < forwardRange) return CannonPositionEnum.Forward;
        if (_freeLookCam.m_XAxis.Value > forwardRange && _freeLookCam.m_XAxis.Value < rightRange) return CannonPositionEnum.Right;
        if (_freeLookCam.m_XAxis.Value > rightRange && _freeLookCam.m_XAxis.Value < behindRange) return CannonPositionEnum.Behind;
        if (_freeLookCam.m_XAxis.Value < (360f - forwardRange) && _freeLookCam.m_XAxis.Value > behindRange) return CannonPositionEnum.Left;
        return null;
    }
}
