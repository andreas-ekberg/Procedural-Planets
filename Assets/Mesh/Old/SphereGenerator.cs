using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    public List<Polygon> polygon = new List<Polygon>();
    public List<Vector3> vertices = new List<Vector3>();
    public int[] triangles;

    public float radius = 1.0f;
    public int divisions = 2;

    public void createShape()
    {
        vertices.Add(new Vector3(-0.5f, 0, -0.5f).normalized * radius);
        vertices.Add(new Vector3(-0.5f, 0, 0.5f).normalized * radius);
        vertices.Add(new Vector3(0.5f, 0, 0.5f).normalized * radius);
        vertices.Add(new Vector3(0.5f, 0, -0.5f).normalized * radius);
        vertices.Add(new Vector3(0, 1, 0).normalized * radius);
        vertices.Add(new Vector3(0, -1, 0).normalized * radius);

        polygon.Add(new Polygon(4, 0, 1));
        /*  polygon.Add(new Polygon(4, 1, 2));
         polygon.Add(new Polygon(4, 2, 3));
         polygon.Add(new Polygon(4, 3, 0));
         polygon.Add(new Polygon(0, 5, 1));
         polygon.Add(new Polygon(1, 5, 2));
         polygon.Add(new Polygon(2, 5, 3));
         polygon.Add(new Polygon(3, 5, 0)); */

        updateTriangle();
        divideUpTriangle();
    }

    void divideUpTriangle()
    {
        Dictionary<int, Vector3> edges = new Dictionary<int, Vector3>();
        //indexCache.Clear();

        foreach (Polygon pol in polygon)
        {
            int A = pol.triangle[0];
            int B = pol.triangle[1];
            int C = pol.triangle[2];

            //findPointsBetweenVertex(indexCache, A, B);
            //findPointsBetweenVertex(indexCache, B, C);
            // findPointsBetweenVertex(indexCache, C, A);
            Vector3 vertexA = vertices[A];
            Vector3 vertexB = vertices[B];
            Vector3 vertexC = vertices[C];

            Vector3 VectorAB = (vertexB - vertexA);
            Vector3 VectorBC = (vertexC - vertexB);

            float pointStep = 1 / (float)divisions;
            for (int row = 1; row <= divisions; row++)
            {
                Vector3 rowPoint = vertexA + VectorAB * pointStep * row;
                vertices.Add(rowPoint);
                edges.Add(vertices.Count, rowPoint);

                for (int column = 1; column <= row; column++)
                {
                    Vector3 columnPoint = rowPoint + VectorBC * pointStep * column;
                    vertices.Add(columnPoint);
                    if (column == row)
                    {
                        edges.Add(vertices.Count, columnPoint);
                    }
                }


            }







        }
        updateTriangle();
        Debug.Log(vertices.Count);


    }


    private void findPointsBetweenVertex(List<cacheMemory> cache, int indexA, int indexB)
    {
        int smallerIndex = Mathf.Min(indexA, indexB);
        int biggerIndex = Mathf.Max(indexA, indexB);

        for (int i = 0; i < cache.Count; i++)
        {
            //Debug.Log(cache[i].smallerIndex);
            if (cache[i].smallerIndex == smallerIndex && cache[i].biggerIndex == biggerIndex)
            {
                return;
            }
        }
        Vector3 vertexA = vertices[indexA];
        Vector3 vertexB = vertices[indexB];

        for (int i = 1; i < divisions; i++)
        {
            float pointStep = (float)i / (float)divisions;
            Debug.Log(pointStep);
            Vector3 point = vertexA + (vertexB - vertexA) * pointStep;
            //Vector3 point = Vector3.Lerp(A, B, pointStep).normalized;

            vertices.Add(point);

        }

        cacheMemory mem = new cacheMemory(vertices.Count, smallerIndex, biggerIndex);
        cache.Add(mem);

    }
    void updateTriangle()
    {
        triangles = new int[polygon.Count * 3];
        int triangleCount = 0;
        foreach (Polygon pol in polygon)
        {
            for (int i = 0; i < pol.triangle.Count; i++)
            {
                //Debug.Log(pol.triangle.Count);

                triangles[triangleCount * 3 + i] = pol.triangle[i];
            }
            triangleCount++;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        foreach (Vector3 vertex in vertices)
        {
            // Draw a small black sphere at each vertex position
            Gizmos.DrawSphere(transform.position + vertex, 0.05f);
        }
    }
}
