Shader "Pixelation/CameraPixelation"
{
	Properties 
	{
	    [HideInInspector] _MainTex ("BaseMap", 2D) = "white" {}
	    [HideInInspector] _CameraRotation ("CameraRotation", Vector) = (1, 0, 0, 0)
	    [HideInInspector] _RotatedCamPos ("RotatedCameraPosition", Vector) = (0, 0, 0, 0)
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
            float4 _CameraRotation;
            float3 _RotatedCamPos;

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

            // from https://gist.github.com/patricknelson/f4dcaedda9eea5f5cf2c359f68aa35fd
            float4 multQuat(float4 q1, float4 q2) {
                return float4(
                    q1.w * q2.x + q1.x * q2.w + q1.z * q2.y - q1.y * q2.z,
                    q1.w * q2.y + q1.y * q2.w + q1.x * q2.z - q1.z * q2.x,
                    q1.w * q2.z + q1.z * q2.w + q1.y * q2.x - q1.x * q2.y,
                    q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
                );
            }
            // from https://gist.github.com/patricknelson/f4dcaedda9eea5f5cf2c359f68aa35fd
            float3 rotateVector(float4 quat, float3 vec) {
                float4 qv = multQuat(
                    quat,
                    float4(vec, 0.0)
                );
                return multQuat(
                    qv,
                    float4(-quat.x, -quat.y, -quat.z, quat.w)
                ).xyz;
            }

            half4 frag(Varyings input, out float depth : SV_Depth) : SV_Target 
            {
                int pixelSize = PIXELATION_PIXEL_SIZE;

                float4 pos = float4(1, 1, 1, 1);

                // pos = float4(_WorldSpaceCameraPos, 1);
                // pos.xyz = rotateVector(
                //     _CameraRotation,
                //     pos
                // );
                pos.xyz = _RotatedCamPos; 
                pos.y *= -1;
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

			ENDCG
		}
        
	} 
	FallBack "Diffuse"
}