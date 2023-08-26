Shader "CustomShader/Unlit/TransparentRim" {
	Properties {
		_RimColor ("Rim Color", Vector) = (0.5,0.5,0.5,0.5)
		_InnerColor ("Inner Color", Vector) = (0.5,0.5,0.5,0.5)
		_InnerColorPower ("Inner Color Power", Range(0, 1)) = 0.5
		_RimPower ("Rim Power", Range(0, 5)) = 2.5
		_AlphaPower ("Alpha Rim Power", Range(0, 8)) = 4
		_AllPower ("All Power", Range(0, 10)) = 1
		_Alpha ("Alpha", Range(0, 1)) = 1
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