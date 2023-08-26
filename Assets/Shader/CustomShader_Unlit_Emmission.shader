Shader "CustomShader/Unlit/Emmission" {
	Properties {
		_MainTexture ("MainTexture", 2D) = "white" {}
		_ScrollXSpeed ("X Scroll Speed", Range(-50, 50)) = 0
		_ScrollYSpeed ("Y Scroll Speed", Range(-50, 50)) = 0
		_MainColor ("MainColor", Vector) = (1,1,1,0)
		_Emmission ("Emmission", Float) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
}