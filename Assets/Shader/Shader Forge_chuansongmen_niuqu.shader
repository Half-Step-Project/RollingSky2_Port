Shader "Shader Forge/chuansongmen_niuqu" {
	Properties {
		_Color ("Color", Vector) = (0.5,0.5,0.5,1)
		_Main_strength ("Main_strength", Float) = 1
		_Main_tex ("Main_tex", 2D) = "white" {}
		_tex ("tex", 2D) = "white" {}
		_UV ("UV", Vector) = (0.15,0.15,0,0)
		_twistingstrength ("twisting strength", Range(0, 1)) = 0.08547009
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
}