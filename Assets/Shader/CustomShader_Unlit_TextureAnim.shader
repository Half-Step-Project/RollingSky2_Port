Shader "CustomShader/Unlit/TextureAnim" {
	Properties {
		[Header(TextureSheetAnimation)] _Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_Row ("Row", Range(1, 10)) = 3
		_Col ("Col", Range(1, 10)) = 3
		_Speed ("Speed", Range(0, 100)) = 8
		[KeywordEnum(Default, Alpha)] _Animation_Type ("Texture Animation Type", Float) = 0
		_TextureSheetST ("Texture Animation Tilling(xy) Offset(zw)", Vector) = (1,1,0,0)
		[Header(Emission)] _EmissionColor ("Emission Color(Alpha is intensity)", Vector) = (1,1,1,1)
		[Space(10)] [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
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
}