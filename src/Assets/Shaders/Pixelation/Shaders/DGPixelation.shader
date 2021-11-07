Shader "Pixelation/DGPixelation"
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

            TEXTURE2D(_DitheredDepthTexture);
            SAMPLER(sampler_DitheredDepthTexture);

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
                return SAMPLE_TEXTURE2D(_DitheredDepthTexture, sampler_DitheredDepthTexture, uv).x;
            }

            half4 frag(Varyings i, out float depth : SV_Depth) : SV_Target 
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                half4 col = half4(1, 0, 1, 0);
                float nearestDepth = 0;

                int searchOffset = PIXELATION_PIXEL_SIZE * 0.5f;
				for (int v = -searchOffset; v <= searchOffset; v++)
				{
					for (int u = -searchOffset; u <= searchOffset; u++)
					{
                        float2 pixel = i.uv + float2(u, v) / _ScreenParams.xy;
                        float d = sampleDepth(pixel);

                        bool nearer = d > nearestDepth;
                        col = nearer ? sampleColor(pixel) : col;
                        nearestDepth = nearer ? d : nearestDepth;
					}
				}

                depth = nearestDepth;
                return col;
            }

			ENDHLSL
		}
        
	} 
	FallBack "Diffuse"
}