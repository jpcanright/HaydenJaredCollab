using System.Collections;
using System.Collections.Generic;
using EMP.Core;
using UnityEngine;

public class GlobalConstantFieldGenerator : FieldGenerator
{
    public Vector3 fieldValue = Vector3.zero;

    public override Vector3 FieldValue(Vector3 position)
    {
        return fieldValue;
    }
}
