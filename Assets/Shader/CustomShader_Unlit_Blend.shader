Shader "CustomShader/Unlit/Blend" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_DiffusePower ("Diffuse Power", Float) = 1
		_SpecularColor ("Specular Color", Vector) = (1,1,1,1)
		_SpecularPower ("Specular Power", Float) = 1
		_Gloss ("Gloss", Float) = 8
		_FakeLightColor ("Fake Light Color", Vector) = (0.765,0.765,0.765,1)
		_FakeLightDir ("FakeLightDir", Vector) = (-0.43,0.766,-0.478,0)
		_EmissionColor ("Emmission Color", Vector) = (1,1,1,1)
		_Emmission ("Emmission", Float) = 0
		[Space(20)] _MainTex ("Main Texture", 2D) = "white" {}
		_SubTex ("Sub Texture", 2D) = "white" {}
		_BlendTex ("Blend Texture", 2D) = "white" {}
		_BlendCutOff ("Blend Cut Off", Range(0, 1)) = 0
		_BlendPower ("Blend Power", Float) = 3
		_Blend ("Blend", Range(0, 1)) = 0
		[Space(20)] _ModelHeight ("Model Heith", Float) = 1.06
		_CenterOffset ("Center Offset", Float) = 0.53
		[Toggle(ENABLE_CUTOFF)] _EnableCutOff ("Enable CutOff", Float) = 0
		[Toggle(ENABLE_REVERSE)] _EnableReverse ("Enable Reverse", Float) = 0
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