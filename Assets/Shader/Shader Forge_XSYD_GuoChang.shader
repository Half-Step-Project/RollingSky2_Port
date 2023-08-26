Shader "Shader Forge/XSYD_GuoChang" {
	Properties {
		[HDR] _TintColor ("Color", Vector) = (0.5,0.5,0.5,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_MainTex_u_speed ("MainTex_u_speed", Float) = 0.2
		_MainTex_v_speed ("MainTex_v_speed", Float) = 0
		_Noise ("Noise", 2D) = "white" {}
		_Noise_u_speed ("Noise_u_speed", Float) = 0
		_Noise_v_speed ("Noise_v_speed", Float) = 0
		_Noise_qiangdu ("Noise_qiangdu", Range(0, 1)) = 0
		_Noise_Mask ("Noise_Mask", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
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