using System;
using UnityEngine;


public class DynamicEulerRotation : MonoBehaviour
{
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
    
    private SecondOrderDynamics _solver;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 targetCurrentAngles = target.transform.localEulerAngles;
        _solver = new SecondOrderDynamics(f, z, r, targetCurrentAngles);
    }

    void Update()
    {
        Vector3 targetCurrentAngles = target.transform.localEulerAngles;
        Vector3 transformedAngles = _solver.Update(Time.deltaTime, targetCurrentAngles);

        transform.localEulerAngles = transformedAngles;
    }
}
