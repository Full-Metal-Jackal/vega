Shader "Unlit/AdditiveBlending"
{
	Properties
	{
		_MainTex("Base Map", 2D) = "white" {}
		// _Opacity("Opacity", Float) = 1

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }
		Pass
		{
			Blend One One
			// _MainTex.a *= _BloomOpacity

			SetTexture[_MainTex] { combine texture }
		}
	}
}
