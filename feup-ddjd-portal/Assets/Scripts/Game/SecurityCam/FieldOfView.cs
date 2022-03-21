using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public LayerMask whatIsSolid;
    private Mesh mesh;
    public int triangleCount = 25;
    
    
    public float viewDistance = 5f; 
    public float initialAngle = -45f;
    public float rotateSpeed = 1f;
    private float maxAngle = -90;
    private float minAngle;
    private bool goingRight = true; 





    private void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        minAngle = maxAngle + 90;
    }

    void Update() {
        Vector3 relativePositionVertex = Vector3.zero;
        int rayCount = triangleCount;
        float fov = 90f;
        float angle = initialAngle;
        float angleIncrease = fov/rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount*3];
        vertices[0] = relativePositionVertex;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= rayCount; i++) {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, GetVectorFromAngle(angle), viewDistance, whatIsSolid);

            if (hitInfo.collider != null) {
                if (ContainsObjectWithName(hitInfo, "Player")) {
                    Debug.Log("PLAYER FOUND, EXTERMINATING");
                }
            }

            Vector3 vertex = relativePositionVertex + GetVectorFromAngle(angle) * viewDistance;
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex +=3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        RotateAngle();
    }


    private Vector3 GetVectorFromAngle(float angle){
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad),Mathf.Sin(angleRad));
    }

    private bool ContainsObjectWithName(RaycastHit2D raycastHit2D, string name){
        Debug.Log(raycastHit2D.collider.name);

         if (raycastHit2D.collider.name == name) return true;
        return false;
    }

    private void RotateAngle(){

        if(goingRight){
            if (initialAngle + rotateSpeed < minAngle) initialAngle += rotateSpeed;
            else if(initialAngle + rotateSpeed >= minAngle){
                initialAngle = minAngle;
                goingRight = false;
            } 
        }
        else{
            if (initialAngle - rotateSpeed > maxAngle) initialAngle -= rotateSpeed;
            else if(initialAngle - rotateSpeed <= maxAngle){
                initialAngle = maxAngle;
                goingRight = true; 
            } 
        }

        
    }
}
