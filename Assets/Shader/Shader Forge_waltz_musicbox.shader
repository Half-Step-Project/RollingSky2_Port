Shader "Shader Forge/waltz_musicbox" {
	Properties {
		_tex_size01 ("tex_size01", Float) = -0.1
		_edgetex1_speed ("edgetex1_speed", Float) = 0.3
		_Tex01 ("Tex01", 2D) = "white" {}
		_tex_size02 ("tex_size02", Float) = -0.05
		_edgetex2_speed ("edgetex2_speed", Float) = 0.2
		_Tex02 ("Tex02", 2D) = "white" {}
		_field_size ("field_size", Range(0, 10)) = 7
		_field_edge_bright ("field_edge_bright", Float) = 0.6
		_field_edge_color ("field_edge_color", Vector) = (1,0.5,1,0.5)
		_buttom_color ("buttom_color", Vector) = (0,0.1,0.5,1)
		_top_color ("top_color", Vector) = (0.8,0.7,0.5,1)
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
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
}