using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMP.Core
{
    public class ThreeDStrength : EMStrength
    {
        //Defaults to 1.5 since a multi dimensional object conceptially is easier to understand as stronger 
        //  than a point charge, so default value is aimed to reflect that.
        //      3D object will sit between a 1D and 2D object at 1.5 default strength.
        [SerializeField]
        private float _strength = 1.5f;

        public float Strength
        {
            get => _strength;
            set => _strength = value;
        }

        //If a value is not provided in the inspector then set it to 1.5.  I could see this happening if object does not include EMStrength and it is generated at runtime.
        void Start()
        {
            if (_strength == 0)
            {
                _strength = 1.5f;
            }
        }
    }
}