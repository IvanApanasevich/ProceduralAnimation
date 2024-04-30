using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicQuaternionRotation : MonoBehaviour
{
    public enum SolverType
    {
        BasicStable,
        PoleZeroTransform
    }
    public SolverType solverType;
    // f - natural frequency
    // z - damping coefficient
    // r - initial response
    [Range(0.01f, 10f)]
    public float f;
    [Range(0.01f, 10f)]
    public float z;
    [Range(-5f, 10f)]
    public float r;
   
    public GameObject target;
    
    private SecondOrderDynamicsRotation _solverBasic;
    private SecondOrderDynamicsRotationPzt _solverPzt;
    
    // Start is called before the first frame update
    void Start()
    {
        Quaternion startRotation = target.transform.rotation;
        if (solverType == SolverType.BasicStable) _solverBasic = new SecondOrderDynamicsRotation(f, z, r, startRotation);
        else _solverPzt = new SecondOrderDynamicsRotationPzt(f, z, r, startRotation);
    }

    // Update is called once per frame
    void Update()
    {
        var targetRotation = target.transform.rotation;

        var transformedRotation =  solverType == SolverType.BasicStable ? _solverBasic.Update(Time.deltaTime, targetRotation) : 
            _solverPzt.Update(Time.deltaTime, targetRotation);
        
        Debug.Log(transformedRotation);
        transform.rotation = transformedRotation * targetRotation;
    }
}
