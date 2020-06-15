using System.Collections;
using System.Collections.Generic;
using EMP.Core;
using UnityEngine;

//This script doesnt work at all.

public class ConductorChargeStrengthTimesFieldReactor : FieldReactor
{

    public MonopoleStrength monopoleStrength;

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
        if (!monopoleStrength)
        {
            monopoleStrength = GetComponent<MonopoleStrength>();
        }
    }

    public override Vector3 ForceUponReactor()
    {
        Vector3 field = monopoleStrength.Strength *
               EMController.Instance.GetFieldValue(transform.position, fieldReactingTo, associatedGenerator);

        RaycastHit hit;
        float distanceAwayFromSurface = .5f;

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal);
            transform.position = (hit.normal + hit.point) * distanceAwayFromSurface;
            return field + hit.normal;
        } else
        {
            return field;
        }
    }
}
