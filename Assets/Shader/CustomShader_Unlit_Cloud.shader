Shader "CustomShader/Unlit/Cloud" {
	Properties {
		_Color ("Color Tint", Vector) = (1,1,1,1)
		_EdgeLength ("Edge Length", Range(0.0002, 5)) = 0.25
		_EdgeFade ("Edge Fade", Range(0, 1)) = 0.4
		_EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		_Emission ("Emission", Float) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
}