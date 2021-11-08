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
            SAMPLER(sampler_CameraPixelationDepthTexture);

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
                return SAMPLE_TEXTURE2D(_CameraPixelationDepthTexture, sampler_CameraPixelationDepthTexture, uv).x;
            }

            half4 frag(Varyings input, out float depth : SV_Depth) : SV_Target 
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                int pixelSize = PIXELATION_PIXEL_SIZE;

                // float4 cameraOffset = mul(
                //     UNITY_MATRIX_VP,
                //     float4(_WorldSpaceCameraPos, 1)
                // );

                // shit just won't work <TODO>

                // float2 cameraOffset = mul(
                //     // UNITY_MATRIX_V,
                //     // UNITY_MATRIX_P,
                //     // UNITY_MATRIX_VP,
                //     // UNITY_MATRIX_MVP,

                //     // UNITY_MATRIX_VP,

                //     // unity_CameraProjection,
                //     // float4(_WorldSpaceCameraPos, 1)
                //     float4(0, 0, 0, 1)
                // );
                float2 cameraOffset = mul(unity_CameraInvProjection, float4(0, 0, 0, 1)).xy;
                // cameraOffset *= half2(0.0f, -0.5f);
                cameraOffset *= _ScreenParams;

                float2 offset = -pixelSize * 0.5f;
                offset.x += fmod(
                    floor(input.vertex.x + cameraOffset.x),
                    pixelSize
                );
                offset.y += fmod(
                    floor(input.vertex.y + cameraOffset.y),
                    pixelSize
                );
                offset /= _ScreenParams.xy;

                float2 targetPixel = input.uv - offset;
                depth = sampleDepth(targetPixel);
                half4 col = sampleColor(targetPixel);

                col.xyz = gradeColor(col.xyz, PIXELATION_COLOR_VARIATION);
                return col;
            }

			ENDHLSL
		}
        
	} 
	FallBack "Diffuse"
}