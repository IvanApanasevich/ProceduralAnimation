using UnityEngine;

public class DynamicPosition : MonoBehaviour
{
    public enum SolverType
    {
        BasicStable,
        PoleZeroTransform
    }

    // f - natural frequency
    // z - damping coefficient
    // r - initial response
    public SolverType solverType;
    [Range(0.01f, 40f)]
    public float f;
    [Range(0.01f, 40f)]
    public float z;
    [Range(-5f, 10f)]
    public float r;
    [Range(5f, 20f)] 
    public float smooth;
    [Range(0f, 80f)] 
    public float tiltAngel;
    public GameObject target;
    
    private SecondOrderDynamics _solverBasic;
    private SecondOrderDynamicsPzt _solverPzt;
    private Vector3  _targetLastPosition, _dif;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 targetCurrentPosition = target.transform.position;
        _dif = targetCurrentPosition - transform.position;
        if (solverType == SolverType.BasicStable) _solverBasic = new SecondOrderDynamics(f, z, r, targetCurrentPosition);
        else _solverPzt = new SecondOrderDynamicsPzt(f, z, r, targetCurrentPosition);
        _targetLastPosition = targetCurrentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetCurrentPosition = target.transform.position;

        var transformedPosition = solverType == SolverType.BasicStable ? _solverBasic.Update(Time.deltaTime, targetCurrentPosition) : 
                                                                                _solverPzt.Update(Time.deltaTime, targetCurrentPosition);
        
        Vector3 angles = Vector3.zero;
        
        if (Vector3.Distance(targetCurrentPosition, _targetLastPosition) > 0.001f)
        {
            angles = -tiltAngel * Vector3.Normalize(transformedPosition - targetCurrentPosition);
        }
        
        var desiredRotQ = Quaternion.Euler(angles.x, angles.y, angles.z);
        
        transform.position = transformedPosition - _dif;
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * smooth);

        _targetLastPosition = targetCurrentPosition;
    }
}
