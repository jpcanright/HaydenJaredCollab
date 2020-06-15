using System.Collections;
using System.Collections.Generic;
using EMP.Core;
using UnityEngine;

//This doesnt work either

public class NegativeChargeReactor : FieldReactor
{
    public MonopoleStrength monopoleStrength;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Init();

        //raycast method
        //snap to object method
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
        //calc new move method
        //AdjustPosition();
        
        float distanceAwayFromSurface = .5f;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            this.gameObject.transform.position = hit.point + hit.normal * distanceAwayFromSurface;
            this.gameObject.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal);
        }

        return monopoleStrength.Strength *
               EMController.Instance.GetFieldValue(transform.position, fieldReactingTo, associatedGenerator);
    }

    //raycast down and see if I'm off of the surface
}
