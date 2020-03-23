using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMP.Core
{

    //[RequireComponent(typeof(EMStrength))]

    public abstract class FieldGenerator : MonoBehaviour
    {
        
        //public MonopoleStrength eMStrength { get; set; }
        [SerializeField]
        public InteractionField generatedField;
        
        private Vector3 _cachedPosition;
        private bool _positionCachedThisFrame = false;
        
        public Vector3 Position
        {
            get
            {
                if (!_positionCachedThisFrame)
                {
                    _cachedPosition = transform.position;
                    _positionCachedThisFrame = true;
                }
                return _cachedPosition;
            }
        }

        // Start is called before the first frame update
        protected virtual void Start(){}

        protected void LateUpdate()
        {
            _positionCachedThisFrame = false;
        }

        public virtual void Register()
        {
            EMController.Instance.RegisterFieldGenerator(this);

        }

        public virtual void UnRegister()
        {
            EMController.Instance.UnRegisterFieldGenerator(this);
        }

        public abstract Vector3 FieldValue(Vector3 fieldPoint);

        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            UnRegister();
        }
    }
}