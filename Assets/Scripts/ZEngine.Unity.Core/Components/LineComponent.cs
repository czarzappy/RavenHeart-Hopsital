using System.Linq;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Factories;
using ZEngine.Unity.Core.Models;

namespace ZEngine.Unity.Core.Components
{
    public class LineComponent : MonoBehaviour
    {
        public static readonly ResourcePath RESOURCE_PATH = new ResourcePath("UI", "LINE");

        public MeshRenderer MeshRenderer;
        public MeshFilter MeshFilter;

        public PolygonCollider2D PolygonCollider2D;

        public Vector3[] Verts;

        public void DisablePhysics()
        {
            ZBug.Info("LINE", "Reset");
            PolygonCollider2D.enabled = false;
            // MeshFilter.mesh = null;
        }

        public void EnablePhysics()
        {
            PolygonCollider2D.enabled = true;
        }

        public void Init(Vector3 startPos, Vector3 currentPos, float thickness,
            Color startColor = default, 
            Color endColor = default)
        {
            Verts = VertFactory.GenerateLineVertices(startPos, currentPos, thickness);

            if (PolygonCollider2D != null)
            {
                PolygonCollider2D.points = Verts.QuadVertsToPolyPoints().ToVec2();
            }

            if (startColor != default || endColor != default)
            {
                var material = MeshRenderer.material;
                
                this.gameObject.InitDurationTickZAttr<ColorFadeZAttr, ColorFadeZAttr.InitData>(
                    new ColorFadeZAttr.InitData
                    {
                        SourceType = ColorFadeZAttr.SourceType.MATERIAL,
                        Material = material,
                        
                        BlendType = ColorFadeZAttr.BlendType.START_END,
                        StartColor = startColor == default ? material.color : startColor,
                        EndColor = endColor == default ? material.color : endColor,
                    },
                    .5f);
            }

            // bounds.Intersects();
                
                
            // BoxCollider2D;

            bool tryOptimize = false;
            if (tryOptimize)
            {
                Mesh mesh = MeshFilter.mesh;
                if (mesh == null)
                {
                    mesh = MeshFactory.GenerateMesh(Verts);;
                    MeshFilter.mesh = mesh;
                }
                else
                {
                    mesh.vertices = Verts;
                    mesh.RecalculateBounds();
                    // MeshFilter.mesh = mesh;
                }
            }
            else
            {
                MeshFilter.mesh = MeshFactory.GenerateMesh(Verts);
                // MeshFilter.mesh = MeshFactory.GenerateMesh(startPos, transform.position, BandageThickness);
            }
            // initialLookDirection = RollerDisplay.rotation.eulerAngles;
        }
    }
}