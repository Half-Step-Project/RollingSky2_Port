Shader "Shader Forge/wenliniuqu_caihongshan" {
	Properties {
		_TintColor ("Main_Color", Vector) = (0.5,0.5,0.5,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_MainTex_UV ("MainTex_UV", Vector) = (0,0,0,0)
		_Add_tex_color ("Add_tex_color", Vector) = (0.5,0.5,0.5,1)
		_Add_tex ("Add_tex", 2D) = "white" {}
		_Add_tex_v ("Add_tex_v", Float) = 0
		_Add_tex_uv ("Add_tex_uv", Vector) = (0,0,0,0)
		_niuqu_tex ("niuqu_tex", 2D) = "white" {}
		_niuqu_tex_v ("niuqu_tex_v", Float) = 0
		_niuqu_tex_uv ("niuqu_tex_uv", Vector) = (0,0,0,0)
		_Fre_color ("Fre_color", Vector) = (0.5,0.5,0.5,1)
		_Fre_qiangdu ("Fre_qiangdu", Range(0, 5)) = 0
		_Fre_fanwei ("Fre_fanwei", Range(0, 5)) = 0
		_zhezhao ("zhezhao", 2D) = "white" {}
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