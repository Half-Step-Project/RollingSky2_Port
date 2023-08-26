Shader "CustomShader/Unlit/DissolveSimple" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_ColorInstensity ("Color Instensity", Range(0, 10)) = 1
		_MainTex ("MainTex", 2D) = "white" {}
		[Header(Dissolve)] _MaskTex ("Dissolve Map", 2D) = "white" {}
		[KeywordEnum(Default, Distance, Height, UV2Mask)] _Type ("Trigger Type", Float) = 0
		_DissolveEdgeOutterColor ("Dissolve Outter Edge Color", Vector) = (1,1,1,0.1)
		_DissolveEdgeInnerColor ("Dissolve Inner Edge Color", Vector) = (1,1,1,0.1)
		_Progress ("Progress", Range(0, 1)) = 0
		[Space(4)] [Header(DistanceMode)] _StartOffset ("Start Offset", Float) = 5
		_EndOffset ("End Offset", Float) = 0
		[Space(4)] [Header(HeightMode)] _modelY ("ModelY", Float) = 13
		_modelHeight ("ModelHeight", Float) = 1.3
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
	Fallback "Diffuse"
}