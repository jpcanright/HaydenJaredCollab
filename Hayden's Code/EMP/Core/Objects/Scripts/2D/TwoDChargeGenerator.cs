using EMP.Core;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EMP.Core
{
    [RequireComponent(typeof(TwoDStrength))]
    public class TwoDChargeGenerator : FieldGenerator
    {
        public TwoDStrength twoDStrength;
        public MeshCollider collider;
        public Vector3 closePoint;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            twoDStrength = GetComponent<TwoDStrength>();

            if (!collider)
            {
                return;
            }
        }

        public void OnValidate()
        {
            twoDStrength = GetComponent<TwoDStrength>();
            collider = GetComponent<MeshCollider>();
        }

        public Vector3 GetClosePoint()
        {
            return closePoint;
        }

        public override Vector3 FieldValue(Vector3 fieldPoint)
        {
            //Finds the closest point on the generator to the field point, and returns a vector pointing straight at it.
            closePoint = collider.ClosestPoint(fieldPoint);
            Vector3 field = twoDStrength.Strength / Mathf.Pow(Vector3.Distance(closePoint, fieldPoint), 3) *
                (fieldPoint - closePoint);

            return field;
        }
    }
}