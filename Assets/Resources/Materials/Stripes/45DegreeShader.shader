Shader "Unlit/45DegreeShader"
{
    Properties {
        MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (0,0,0,1)
        _Color2 ("Color 2", Color) = (1,1,1,1)
        _Tiling ("Tiling", Range(-100, 100)) = 10
	    _Speed ("Speed", Range(0, 10)) = 1
	    
	    // required for UI.Mask
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
         _ColorMask ("Color Mask", Float) = 15
    }
     
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		ZWrite Off
		
		// required for UI.Mask
         Stencil
         {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp] 
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
         }
        ColorMask [_ColorMask]
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma multi_compile_fog
			
            #include "UnityUI.cginc"
			
            fixed4 _Color1;
            fixed4 _Color2;
            int _Tiling;
            float _Direction;
            float _Speed;
            
            float4 _ClipRect;
            sampler2D MainTex;
            uniform float4 MainTex_ST;
            
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color    : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPosition : TEXCOORD1;
				fixed4  color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(o.worldPosition);
				o.uv = v.uv;
                //o.uv = TRANSFORM_TEX(v.uv, MainTex);
				o.color = v.color;
				return o;
			}

			//fixed4 frag (v2f i) : SV_Target
            //{
              //  fixed4 color;
                //color.rg = i.uv;
                //return color;
            //}
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(MainTex, i.uv);
                return col;
                
	            float2 startPos = i.uv;
	         
	            float2 dilatedPos = startPos + (_Time.xy * _Speed);
	            
	            float pos = lerp(dilatedPos.x, dilatedPos.y, .5) * _Tiling;
	            
                fixed value = floor(frac(pos) + 0.5);
                
                fixed4 output_color = lerp(_Color1, _Color2, value);
                
                output_color *= i.color;
                
                output_color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                
                return output_color;
            }
			ENDCG
		}
	}
}