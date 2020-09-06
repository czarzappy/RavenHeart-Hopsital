Shader "Unlit/WavyStripes"
{
    Properties {
		_Color1 ("Color 1", Color) = (0,0,0,1)
		_Color2 ("Color 2", Color) = (1,1,1,1)
		_Tiling ("Tiling", Range(1, 500)) = 10
		_Direction ("Direction", Range(0, 1)) = 0
		_WarpScale ("Warp Scale", Range(0, 1)) = 0
		_WarpTiling ("Warp Tiling", Range(1, 10)) = 1
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
		Tags { "RenderType"="Opaque" }
		
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
			
            #include "UnityUI.cginc"

			fixed4 _Color1;
			fixed4 _Color2;
			int _Tiling;
			float _Direction;
			float _WarpScale;
			float _WarpTiling;
			
            float4 _ClipRect;
            float _Speed;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color    : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPosition : TEXCOORD1;
				float4  color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				const float PI = 3.14159;
				
				
	            float2 startPos = i.uv;
	            float2 dilatedPos = startPos + (_Time.xy * _Speed);

				float2 pos;
				pos.x = lerp(dilatedPos.x, dilatedPos.y, _Direction);
				pos.y = lerp(dilatedPos.y, 1 - dilatedPos.x, _Direction);

				pos.x += sin(pos.y * _WarpTiling * PI * 2 * (_Time.x / 100)) * _WarpScale;
				pos.x *= _Tiling;

				fixed value = floor(frac(pos.x) + 0.5);
				
				fixed4 output_color = lerp(_Color1, _Color2, value);
				
				output_color = output_color * i.color;
                
                output_color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                
				return output_color;
			}
			ENDCG
		}
	}
}