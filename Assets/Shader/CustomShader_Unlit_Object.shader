Shader "CustomShader/Unlit/Object" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Vector) = (1,1,1,1)
		_EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		_Emmission ("Emission Power", Float) = 0
		[Toggle(ENABLE_CURVED_WORLD)] _EnableCurvedWorld ("Enable Curvedworld", Float) = 0
		[Toggle(ENABLE_FOG)] _EnableFog ("Enable Fog", Float) = 0
		[Toggle(ENABLE_FAKE_LIGHTING)] _FakeLight ("Enable Fake Light", Float) = 0
		_FakeLightColor ("Fake Light Color", Vector) = (0.765,0.765,0.765,1)
		_FakeLightDir ("Fake Light Dir", Vector) = (-0.43,0.766,-0.478,0)
		_AmbientColorPower ("Ambient Color Power", Float) = 1
		[Toggle(ENABLE_DIFFUSE)] _EnableDiffuse ("Enable Diffuse", Float) = 0
		_DiffusePower ("Diffuse Power", Float) = 1
		[Toggle(ENABLE_SPECULAR)] _EnableSpecular ("Enable Specular", Float) = 0
		_SpecularColor ("Specular Color", Vector) = (1,1,1,1)
		_SpecularPower ("Specular Power", Float) = 1
		_Gloss ("Specular Gloss", Float) = 8
		[Toggle(ENABLE_FRESNEL)] _EnableFresnel ("Enable Fresnel", Float) = 0
		_FresnelColor ("Fresnel Color", Vector) = (1,1,1,1)
		_FresnelPower ("Fresnel Power", Float) = 1
		_FresnelScale ("Fresnel Scale", Float) = 0.03
		[Toggle(ENABLE_RIM)] _EnableRim ("Enable Rim", Float) = 0
		_RimColor ("Rim Color", Vector) = (0.5,0.5,0.5,0.5)
		_RimInnerColor ("Rim Inner Color", Vector) = (0.5,0.5,0.5,0.5)
		_RimInnerColorPower ("Rim Inner Color Power", Range(0, 1)) = 0.5
		_RimPower ("Rim Power", Range(0, 5)) = 2.5
		_RimAlphaPower ("Rim Alpha Power", Range(0, 8)) = 4
		_RimAllPower ("Rim All Power", Range(0, 10)) = 1
		_RimAlpha ("Rim Alpha", Range(0, 1)) = 1
		[Toggle(ENABLE_VERTEX_COLOR)] _EnableVertexColor ("Enable Vertex Color", Float) = 0
		[Enum(Opaque, 0, Transparent, 1)] _Mode ("Render Type", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
		[Enum(UnityEngine.Rendering.BlendMode)] [HideInInspector] _SrcBlend ("Src Blend Mode", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] [HideInInspector] _DstBlend ("Dst Blend Mode", Float) = 1
		[Enum(Off, 0, On, 1)] _ZWrite ("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] [HideInInspector] _ZTest ("ZTest", Float) = 0
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
	//CustomEditor "ObjectEditor"
}