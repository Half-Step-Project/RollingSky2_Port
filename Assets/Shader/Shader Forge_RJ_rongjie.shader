Shader "Shader Forge/RJ_rongjie" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		_MainTex_speed ("MainTex_speed", Vector) = (0,0,0,0)
		[HDR] _TintColor ("Color", Vector) = (0.5,0.5,0.5,1)
		_Tex_Intensity ("Tex_Intensity", Range(1, 10)) = 1
		_Dissolve ("Dissolve", Range(0, 2)) = 0
		_Mask ("Mask", 2D) = "white" {}
		[MaterialToggle] _kaiguan ("kaiguan", Float) = 0
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ShaderForgeMaterialInspector"
}