Shader "Shader Forge/waltz_DengGuang" {
	Properties {
		_TintColor ("Color", Vector) = (0.5,0.5,0.5,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_Tex01 ("Tex01", 2D) = "white" {}
		_Tex01_U ("Tex01_U", Float) = 0.01
		_Tex01_V ("Tex01_V", Float) = -0.05
		_tex01_s ("tex01_s", Float) = 1
		[MaterialToggle] _F_kaiguan ("F_kaiguan", Float) = 1
		_Fresnel ("Fresnel", Float) = 0.5
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
}