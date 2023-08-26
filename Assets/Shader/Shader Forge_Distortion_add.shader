Shader "Shader Forge/Distortion_add" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		_TintColor ("Color", Vector) = (0.5,0.5,0.5,1)
		_niuqu_tex ("niuqu_tex", 2D) = "white" {}
		_rotator_speed ("rotator_speed", Float) = 0
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