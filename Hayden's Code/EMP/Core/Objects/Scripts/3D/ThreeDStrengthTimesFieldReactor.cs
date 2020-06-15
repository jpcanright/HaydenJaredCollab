using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using EMP.Core;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class ThreeDStrengthTimesFieldReactor : FieldReactor
{
    public ThreeDStrength threeDStrength;
    public ThreeDChargeGenerator threeDChargeGenerator;
    //private Rigidbody rb;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Init();
    }

    private void OnValidate()
    {
        Init();
    }

    void Init()
    {
        if (!threeDStrength)
        {
            threeDStrength = GetComponent<ThreeDStrength>();
        }
        if (!threeDChargeGenerator)
        {
            threeDChargeGenerator = GetComponent<ThreeDChargeGenerator>();
        }
        //if (!rb)
        //{
         //   rb = GetComponent <Rigidbody>();
        //}
    }

    public override Vector3 ForceUponReactor()
    {
        return threeDStrength.Strength *
               EMController.Instance.GetFieldValue(threeDChargeGenerator.GetClosePoint(), fieldReactingTo, associatedGenerator);
    }
}