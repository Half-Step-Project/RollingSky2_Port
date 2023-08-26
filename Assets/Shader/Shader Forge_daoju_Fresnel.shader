Shader "Shader Forge/daoju_Fresnel" {
	Properties {
		_Main_tex_color ("Main_tex_color", Vector) = (0.5,0.5,0.5,1)
		_Main_tex ("Main_tex", 2D) = "white" {}
		_Fre_Color ("Fre_Color", Vector) = (1,1,1,1)
		_Fre_qiangdu ("Fre_qiangdu", Range(0, 5)) = 1.338552
		_Fre_fanwei ("Fre_fanwei", Range(0, 5)) = 0.6439628
		_niuqu_tex ("niuqu_tex", 2D) = "white" {}
		_niuqu_qiangdu ("niuqu_qiangdu", Float) = 0
		_UV ("UV", Vector) = (0,0,0,0)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
}