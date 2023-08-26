Shader "CustomShader/Unlit/MatCap" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
		_MatCap ("MatCap", 2D) = "white" {}
		_ReflectionColor ("Reflection Color", Vector) = (1,1,1,1)
		_ReflectionPower ("Reflection Power", Range(0, 5)) = 1
		_Reflectivity ("Reflectivity", Range(0, 2)) = 0.5
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