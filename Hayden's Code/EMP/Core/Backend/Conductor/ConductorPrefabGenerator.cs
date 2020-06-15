using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EMP.Core
{
    //can write this in one line, can also streamline the variables and what components are required for this.
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class ConductorPrefabGenerator : MonoBehaviour
    {
        private Vector3 target;
        private Vector3[] vertices;
        private MeshCollider collider;
        private float xSize, ySize, zSize;
        private MeshRenderer renderer;
        private TwoDChargeGenerator generator;
        //currently only has one variable for a negativeCharge prefab. Final version would have a positive charge variable as well.
        //  not yet implimented because I can't get just the negative charges spawning and working properly yet.
        public GameObject negativeCharge;
        // Start is called before the first frame update
        void Start()
        {

        }

        void Awake()
        {
            renderer = GetComponent<MeshRenderer>();
            collider = GetComponent<MeshCollider>();
            generator = GetComponent<TwoDChargeGenerator>();

            xSize = (int)renderer.bounds.size.x;
            ySize = (int)renderer.bounds.size.y;
            zSize = (int)renderer.bounds.size.z;
            GenerateGrid();
            GenerateParticles();
        }

        //Creates an empty cube of equally spaced points around an object using size variables defined in awake()
        //afterwards, "snaps" each point to the closest point on the game object this script is attached to.
        void GenerateGrid()
        {
            Vector3 startPos = renderer.transform.position;
            //generate empty cube list of vertices. Surrounds object in question.
            float step = 1f;
            int reset = 0;
            vertices = new Vector3[(int)((2 * (xSize) * (ySize)) + (2 * (zSize) * (ySize)) + (2 * (xSize) * (zSize)))/(3)];
            int i = 0;
            for (float y = 0; y <= ySize; y += step)
            {
                for (float z = 0; z <= zSize; z += step)
                {
                    for (float x = 0; x <= xSize; x += step)
                    {
                        if (x == 0 || x == xSize || y == 0 || y == ySize || z == 0 || z == zSize)
                        {
                            if(reset == 2)
                            {
                                vertices[i] = new Vector3(
                                    startPos.x - (xSize / 2) + (float)x,
                                    startPos.y - (ySize / 2) + (float)y,
                                    startPos.z - (zSize / 2) + (float)z);
                                i++;
                                reset = 0;
                            } else
                            {
                                reset++;
                            }
                        }

                    }
                }
            }

            //vertices[] is a list of vertices creating a box around an object.
            //snap vertices to object 
            //  moves each point ever so slightly away from the surface.
            Vector3[] tempVertices = vertices;
            for (int j = 0; j < tempVertices.Length; j++)
            {
                Vector3 closePoint = collider.ClosestPoint(tempVertices[j]);
                //vertices[j] = closePoint;
                vertices[j] = Vector3.MoveTowards(closePoint, collider.transform.position, -0.03f);
            }
        }

        void GenerateParticles()
        {
            //instantate negative charge at all points
            //rotate each point so y axis is faced away from the object, rather than up in world space
            for (int i = 0; i < vertices.Length; i++)
            {
                GameObject clone = Instantiate(negativeCharge, vertices[i], Quaternion.identity);
                clone.transform.SetParent(this.gameObject.transform);

                Vector3 normal = vertices[i] - collider.ClosestPoint(vertices[i]);

                clone.transform.rotation = Quaternion.FromToRotation(clone.transform.up, normal);
            }
        }

        //gizmo drawing to view grid on scene view.
        void OnDrawGizmos()
        {
            if (vertices == null)
            {
                return;
            }

            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
}