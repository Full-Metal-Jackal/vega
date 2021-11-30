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

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#include "Assets/Shaders/Pixelation/Shaders/PixelationDefines.hlsl"
            
			#pragma vertex vert
			#pragma fragment frag

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_CameraPixelationDepthTexture);

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv        : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.uv = input.uv;

                return output;
            }
            
            half4 sampleColor(float2 uv)
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
            }
            float sampleDepth(float2 uv)
            {
                return SAMPLE_TEXTURE2D(_CameraPixelationDepthTexture, sampler_MainTex, uv).x;
            }

            half4 frag(Varyings input, out float depth : SV_Depth) : SV_Target 
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                int pixelSize = PIXELATION_PIXEL_SIZE;
                pixelSize = 1;

                float4 pos = float4(0, 0, 0, 1);
                pos = float4(_WorldSpaceCameraPos, 1);
                pos = mul(unity_WorldToObject, pos);
                // pos = mul(unity_ObjectToWorld, pos);
                // pos = mul(UNITY_MATRIX_MVP, pos);
                pos = mul(unity_CameraProjection, pos);
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

			ENDHLSL
		}
        
	} 
	FallBack "Diffuse"
}