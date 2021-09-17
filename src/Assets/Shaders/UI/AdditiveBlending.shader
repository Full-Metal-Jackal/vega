Shader "Unlit/AdditiveBlending"
{
	Properties
	{
		_MainTex("Base Map", 2D) = "white" {}
		// _Opacity("Opacity", Float) = 1
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
