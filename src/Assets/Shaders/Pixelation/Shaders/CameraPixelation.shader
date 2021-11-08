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

                // shit just won't work <TODO>
                float2 cameraOffset = mul(UNITY_MATRIX_MVP, float4(0, 0, 0, 1)).xy;
                cameraOffset *= float2(-0.5, 0.5) * _ScreenParams.xy;

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
                
                // <TODO> Maybe implement same depth corner-only pixelation thing for the DGPixelation?
                float d = sampleDepth(input.uv);
                depth = d == 0 ? sampleDepth(targetPixel) : d;

                half4 col = sampleColor(targetPixel);
                col.xyz = gradeColor(col.xyz, PIXELATION_COLOR_VARIATION);
                return col;
            }

			ENDHLSL
		}
        
	} 
	FallBack "Diffuse"
}