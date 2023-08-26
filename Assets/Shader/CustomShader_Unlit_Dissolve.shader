Shader "CustomShader/Unlit/Dissolve" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("MainTex", 2D) = "white" {}
		_ColorR ("MainTex R Color", Vector) = (1,1,1,0.5)
		_ColorB ("MainTex B Color", Vector) = (0,0,0,1)
		_VLength ("V(UV)Length", Float) = 10
		_Progress ("Progress", Range(0, 1)) = 0
		_Range ("Range", Range(0, 1)) = 0.2
		[Header(Dissolve)] _DissolveMap ("DissolveMap", 2D) = "white" {}
		_DissolveRange ("Dissolve Range", Range(0, 1)) = 0.05
		_DissolveEdgeOutterColor ("Dissolve Outter Edge Color", Vector) = (1,1,1,0.1)
		_DissolveEdgeInnerColor ("Dissolve Inner Edge Color", Vector) = (1,1,1,0.1)
		[Header(FadeOut)] _FadeOutRange ("FadeOut Range", Float) = 1
		[Header(Brightness)] _BrightnessOffset ("Brightness Offset", Range(0, 1)) = 1
		[Enum(Positive, 2, Negitive, 0)] _ZPositiveDir ("ForwardDirection", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
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