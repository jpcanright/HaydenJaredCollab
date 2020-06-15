using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using EMP.Core;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class TwoDStrengthTimesFieldReactor : FieldReactor
{
    public TwoDStrength twoDStrength;
    public TwoDChargeGenerator twoDChargeGenerator;

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
        if (!twoDStrength)
        {
            twoDStrength = GetComponent<TwoDStrength>();
        }
        if (!twoDChargeGenerator)
        {
            twoDChargeGenerator = GetComponent<TwoDChargeGenerator>();
        }
    }

    public override Vector3 ForceUponReactor()
    {
        return twoDStrength.Strength *
               EMController.Instance.GetFieldValue(twoDChargeGenerator.GetClosePoint(), fieldReactingTo, associatedGenerator);
    }
}