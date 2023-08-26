Shader "CustomShader/Unlit/EmissionStar" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_EmissionTex ("Emission Tex", 2D) = "white" {}
		_EmissionScale ("Emssion Scale", Range(0.011, 3)) = 1
		_EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		_EmissionPower ("Emission Power", Range(1, 10)) = 1
		[Toggle(ENABLE_CURVED_WORLD)] _EnableCurvedWorld ("Enable Curved World", Float) = 1
		[Toggle(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 1
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