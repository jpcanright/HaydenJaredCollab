using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMP.Core
{
    public class TwoDStrength : EMStrength
    {
        [SerializeField]
        //Defaults to 2 since a multi dimensional object conceptially is easier to understand as stronger 
        //  than a point charge, so default value is aimed to reflect that.
        //      infinate 2D object will be strongest at default of 2
        private float _strength = 2;

        public float Strength
        {
            get => _strength;
            set => _strength = value;
        }

        //If a value is not provided in the inspector then set it to 2.  I could see this happening if object does not include EMStrength and it is generated at runtime.
        void Start()
        {
            if (_strength == 0)
            {
                _strength = 2;
            }
        }
    }
}