using UnityEngine;
using ZEngine.Unity.Core.Extensions;

namespace ZEngine.Unity.Core.Factories
{
    public static class VertFactory
    {
        public static Vector3[] GenerateQuadVertices(float width, float height)
        {
            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(0, 0, 0),
                new Vector3(width, 0, 0),
                new Vector3(0, height, 0),
                new Vector3(width, height, 0)
            };

            return vertices;
        }
        
        /**
         *  C -> D
         *     \
         *  A -> B
         */
        
        public static Vector3[] GenerateLineVertices(Vector3 start, Vector3 end, float thickness)
        {
            Vector3 dir = VectorExt.GetNormalDirection(start, end);

            Vector3 leftDir = dir.GetLeft();
            Vector3 rightDir = dir.GetRight();

            float halfThick = thickness / 2;

            Vector3[] vertices = new Vector3[4]
            {
                start + (rightDir * halfThick),
                end + (rightDir * halfThick),
                start + (leftDir * halfThick),
                end + (leftDir * halfThick)
            };

            return vertices;
        }
    }

    public static class TriFactory
    {
        public static int[] GenerateQuadTris()
        {
            int[] tris = new int[6]
            {
                // lower left triangle
                0, 2, 1,
                // upper right triangle
                2, 3, 1
            };

            return tris;
        }
    }

    public static class NormalFactory
    {
        public static Vector3[] GenerateQuadNormals()
        {
            Vector3[] normals = new Vector3[4]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };

            return normals;
        }
    }

    public static class UVFactory
    {
        public static Vector2[] GenerateQuadUVs()
        {
            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

            return uv;
        }
        
        public static Vector2[] GenerateVertUVs(Vector3[] verts)
        {

            Vector2[] uv = new Vector2[verts.Length];

            for (int idx = 0; idx < verts.Length; idx++)
            {
                uv[idx] = new Vector2(verts[idx].x, verts[idx].y);
            }

            return uv;
        }
    }
    
    public static class MeshFactory
    {
        public static Mesh GenerateMesh(Vector3 start, Vector3 end)
        {
            Mesh mesh = new Mesh();

            mesh.vertices = VertFactory.GenerateQuadVertices(end.y - start.y, end.x - start.x);

            mesh.triangles = TriFactory.GenerateQuadTris();
            mesh.normals = NormalFactory.GenerateQuadNormals();

            mesh.uv = UVFactory.GenerateQuadUVs();
            
            return mesh;
        }
        
        public static Mesh GenerateLine(Vector3 start, Vector3 end, float thickness)
        {
            Mesh mesh = new Mesh
            {
                vertices = VertFactory.GenerateLineVertices(start, end, thickness),
                triangles = TriFactory.GenerateQuadTris(),
                normals = NormalFactory.GenerateQuadNormals(),
                uv = UVFactory.GenerateQuadUVs()
            };

            return mesh;
        }
        
        public static Mesh GenerateMesh(Vector3[] vertices)
        {
            Mesh mesh = new Mesh
            {
                vertices = vertices,
                triangles = TriFactory.GenerateQuadTris(),
                normals = NormalFactory.GenerateQuadNormals(),
                uv = UVFactory.GenerateQuadUVs()
            };

            return mesh;
        }
    }
}