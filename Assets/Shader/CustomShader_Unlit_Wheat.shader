Shader "CustomShader/Unlit/Wheat" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,1)
		_RootColor ("Root Color", Vector) = (1,1,1,1)
		_WindColor ("Shadow Color", Vector) = (0.1,0.1,0.1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_WindTex ("Wind Tex", 2D) = "white" {}
		_WindTexCutOff ("WindTex CutOff", Range(0, 1)) = 0.4
		_WindTexPower ("WindTex Power", Range(0, 10)) = 1
		_EdgeLength ("Edge Length", Range(0.0002, 2)) = 0.25
		_EdgeFade ("Edge Fade", Range(0, 1)) = 0.4
		_Cutoff ("Alpha cutoff", Range(0, 1)) = 0.35
		_EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		_Emission ("Emission", Float) = 0.5
		_GravityPower ("Gravity Power", Range(0, 10)) = 1
		_Range ("Gravity Range", Range(1, 10)) = 7
		_GravityOffsetY ("Gravity Offet Y", Range(0, 1)) = 0.7
		_WindPeriod ("WindPeriod", Range(0, 2)) = 1.7
		_WindPower ("WindPower", Range(0, 1)) = 0.2
		_WindScale ("WindScale", Range(1, 10)) = 5.4
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}