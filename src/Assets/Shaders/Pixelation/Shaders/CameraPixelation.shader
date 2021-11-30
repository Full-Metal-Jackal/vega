Shader "Pixelation/CameraPixelation"
{
	Properties 
	{
	    [HideInInspector] _MainTex ("BaseMap", 2D) = "white" {}
	}
	SubShader 
	{
		Tags
		{
		    "RenderType"="Opaque"
		}
		LOD 200
		
		Pass
		{
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On

            CGPROGRAM
            
			#pragma vertex vert
			#pragma fragment frag

            #include "UnityCG.cginc"
            #include "PixelationDefines.hlsl"

            sampler2D _MainTex;
            sampler2D _CameraPixelationDepthTexture;

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv        : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                output.vertex = UnityObjectToClipPos(input.positionOS.xyz);
                output.uv = input.uv;

                return output;
            }
            
            half4 sampleColor(float2 uv)
            {
                return tex2D(_MainTex, uv);
            }
            float sampleDepth(float2 uv)
            {
                return tex2D(_CameraPixelationDepthTexture, uv);
            }

            half4 frag(Varyings input, out float depth : SV_Depth) : SV_Target 
            {
                int pixelSize = PIXELATION_PIXEL_SIZE;
                pixelSize = 1;

                float4 pos = float4(1, 1, 1, 1);

                // pos = mul(unity_WorldToObject, pos);
                // pos = UnityObjectToClipPos(pos);
                
                // The Jedi way
                // /*
                // pos = float4(-_WorldSpaceCameraPos, 1);
                pos = mul(unity_WorldToObject, pos);
                pos = mul(unity_CameraProjection, pos);
                // */

                // The API projection way
                /*
                pos = float4(_WorldSpaceCameraPos, 1);
                pos = mul(UNITY_MATRIX_P, pos);
                pos.xy *= float2(-0.5, 0.5);
                // */
                
                // pos = ComputeScreenPos(pos);
                // pos.xy *= _ScreenParams.xy;

                float2 cameraOffset = pos.xy;
                cameraOffset *= _ScreenParams.xy * float2(0.5, 0.5);

                float2 offset = -pixelSize * 0.5f;
                offset += fmod(
                    floor(input.vertex.xy + cameraOffset),
                    pixelSize
                );
                offset /= _ScreenParams.xy;

                float2 targetPixel = input.uv - offset;
                
                // <TODO> Maybe implement same depth corner-only pixelation thing for the DGPixelation?
                float d = sampleDepth(input.uv);
                depth = d == 0 ? sampleDepth(targetPixel) : d;

                half4 col = sampleColor(targetPixel);
                col.xyz = gradeColor(col.xyz, PIXELATION_COLOR_VARIATION);

                // Pixelation offset testing
                float2 gridOffset = fmod(floor(input.vertex.xy + cameraOffset), 32);
                gridOffset /= _ScreenParams.xy;
                if (gridOffset.x * gridOffset.y == 0)
                    return float4(0, 1, 0, 1);

                return col;
            }

			ENDCG
		}
        
	} 
	FallBack "Diffuse"
}