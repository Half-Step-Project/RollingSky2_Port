Shader "CustomShader/Unlit/EmissionTile" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
		_EmissionColor ("Emmission Color", Vector) = (1,1,1,1)
		_Emmission ("Emmission", Float) = 0
		_EmmissionMultiplier ("Emmission Multiplier", Float) = -1
		_EmmissionRange ("EmmissionRange", Range(-10, 10)) = 2.5
		_EmissionDistance ("Emission Distance", Range(0, 30)) = 3
		[KeywordEnum(Circle, Forward, None, Keyboard, Lightmap)] _Shape ("Trigger Shape", Float) = 0
		[Toggle] _VertexColor ("Use Vertex Color", Float) = 0
		[Toggle(ENABLE_FRESNEL)] _EnableFresnel ("Enable Fresnel", Float) = 0
		_FresnelColor ("Fresnel Color", Vector) = (1,1,1,1)
		_FresnelPower ("Fresnel Power", Float) = 1
		_FresnelScale ("Fresnel Scale", Float) = 0.03
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