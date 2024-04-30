using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    public GameObject target;
    private Vector3 _dif;
    
    void Start()
    {
        Vector3 targetCurrentPosition = target.transform.position;
        Vector3 currentChildPosition = transform.position;
        _dif = targetCurrentPosition-currentChildPosition;
    }

    private void Update()
    {
        transform.position = target.transform.position - _dif;
    }
}