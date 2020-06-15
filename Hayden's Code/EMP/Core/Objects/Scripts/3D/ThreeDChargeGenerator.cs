using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace EMP.Core
{
    [RequireComponent(typeof(ThreeDStrength))]
    [RequireComponent(typeof(MeshCollider))]
    public class ThreeDChargeGenerator : FieldGenerator
    {
        public ThreeDStrength threeDStrength;
        public MeshCollider collider;
        public Vector3 closePoint;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            threeDStrength = GetComponent<ThreeDStrength>();

            if (!collider)
            {
                return;
            }
        }

        public void OnValidate()
        {
            threeDStrength = GetComponent<ThreeDStrength>();
            collider = GetComponent<MeshCollider>();
        }

        public Vector3 GetClosePoint()
        {
            return closePoint;
        }

        public override Vector3 FieldValue(Vector3 fieldPoint)
        {
            //scales between close point and center of mass of object, further away the more the center takes over
            closePoint = collider.ClosestPoint(fieldPoint);
            Vector3 field = 2 * Vector3.Normalize(fieldPoint - closePoint) / (Vector3.Distance(closePoint, fieldPoint));

            Vector3 fieldCenter = (Vector3.Distance(closePoint, fieldPoint)) * Vector3.Normalize(fieldPoint - gameObject.transform.position);

            Vector3 finalField = threeDStrength.Strength * (field + fieldCenter) / (3 * Mathf.Pow(Vector3.Distance(fieldPoint, closePoint), 3));

            
            return finalField;
        }
    }
}