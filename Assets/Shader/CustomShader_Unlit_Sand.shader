Shader "CustomShader/Unlit/Sand" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_UVSpeedX ("Main Tex X Speed", Range(-10, 10)) = 0
		_UVSubSpeedX ("sub Tex X Speed", Range(-10, 10)) = 0
		_Blend ("Blend Num", Range(0, 1)) = 0
		[Space(10)] [Header(Glitter)] _GlitterTex ("Glitter Noise Map ", 2D) = "white" {}
		_Glitterness ("Glitterness ", Float) = 1
		_GlitterRange ("Glitter Range ", Float) = 1
		_GlitterColor ("Glitter Color ", Vector) = (1,1,1,1)
		_GlitterPower ("Glitter Power", Float) = 1
		_GlitterCutOff (" Glitter CutOff", Float) = 0
		_UVGlitterSpeedX ("Glitter Tex X Speed", Range(-10, 10)) = 0
		[Space(10)] [Header(NormalMap)] _DetailBumpMap ("Detail Bump Map ", 2D) = "white" {}
		_DetailBumpScale ("Detail Bump Scale ", Range(0, 2)) = 1
		_UVDetailSpeedX ("Detail Bump Tex X Speed", Range(-10, 10)) = 0
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