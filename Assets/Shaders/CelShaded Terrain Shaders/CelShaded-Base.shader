Shader "Hidden/Custom/TerrainEngine/Splatmap/CelShaded-Base" {
	Properties {
		_MainTex ("Base (RGB) Smoothness (A)", 2D) = "white" {}
		_MetallicTex ("Metallic (R)", 2D) = "white" {}

		// used in fallback on old cards
		_Color ("Main Color", Color) = (1,1,1,1)

		//Cel Shading Stuff
		_Brightness("Brightness", Float) = 1.0
		_LightingLevels("Lighting Levels", Int) = 6
	}

	SubShader {
		Tags {
			"RenderType" = "Opaque"
			"Queue" = "Geometry-100"
		}
		LOD 200

		CGPROGRAM
		#pragma surface surf CelShadedTerrain fullforwardshadows
		#pragma target 3.0
		// needs more than 8 texcoords
		#pragma exclude_renderers gles
		#include "UnityPBSLighting.cginc"

		struct Input {
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		sampler2D _MetallicTex;
		
		//Cel Shading Stuff
		float _Brightness;
		int _LightingLevels;

		half4 LightingCelShadedTerrain(SurfaceOutputStandard s, half3 lightDir, half atten) {
			half4 c;
			half NdotL = dot(s.Normal, lightDir); //How much light is shining on the surface.
			half brightness = (floor(NdotL * _LightingLevels) / _LightingLevels);
			c.rgb = s.Albedo * _LightColor0.rgb * (brightness * atten * 2.0) * _Brightness;
			c.a = s.Alpha;

			return c;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = 1;
			o.Smoothness = c.a;
			o.Metallic = tex2D (_MetallicTex, IN.uv_MainTex).r;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
