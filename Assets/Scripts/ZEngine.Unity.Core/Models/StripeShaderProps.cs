using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Models
{
    [Serializable]
    public struct StripeShaderProps
    {
        public Color Color1;
        public Color Color2;
        public float Tiling;
        public float Direction;
        public float Speed;


        public void UpdateMat(Material material)
        {
            material.SetColor("_Color1", Color1);
            material.SetColor("_Color2", Color2);
            material.SetFloat("_Tiling", Tiling);
            material.SetFloat("_Direction", Direction);
            material.SetFloat("_Speed", Speed);
        }
    }
}