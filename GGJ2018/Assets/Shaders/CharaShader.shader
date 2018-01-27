Shader "Custom/OcclusionShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (RGB)", 2D) = "black" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_OccluTex ("Occlusion map", 2D) = "white" {}
		_OccluStrengh("Occlusion strengh", Range(0,1)) = 0.5
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Sickness("Sickness", Range(0, 1)) = 0.0
		_SpotColor ("Spot Color", Color) = (1,1,1,1)
		_SickColor ("Sickness Color", Color) = (1,1,1,1)
		_EyeLidColor ("Sickness Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MaskTex;
		sampler2D _NormalMap;
		sampler2D _OccluTex;

		half _OccluStrengh;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Sickness;
		fixed4 _SpotColor;
		fixed4 _SickColor;
		fixed4 _EyeLidColor;
		fixed4 _SickMask;


		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 m = tex2D (_MaskTex, IN.uv_MainTex) * _Color;
			fixed4 occlu = tex2D(_OccluTex, IN.uv_MainTex);


			//Pustules
			c = lerp(c, _SpotColor, saturate(m.b*10));

			//Maladie
			//Verdatrie
			_SickMask = lerp(0.0,_Sickness, saturate(m.g * 1.5));
			c= lerp(c, _SickColor, saturate(_SickMask));

			//Yeux Rouges
			c= lerp (c, _EyeLidColor, m.r * _Sickness);

			//Occlu
			o.Albedo = lerp(c.rgb, c.rgb * occlu.rgb, _OccluStrengh);
			o.Normal = UnpackNormal (tex2D (_NormalMap, IN.uv_NormalMap));
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
