Shader "Custom/Cel Surface Shader Terrain" {

	Properties {
		//_varName("Label", type) = defaultValue;
		//Types: 2D, Cube, Color, Range, Float, Int, Vector
		[HideInInspector] _MainTex("Albedo (RGBA)", 2D) = "white" {}
		_Color("Color", Color) = (1.0,1.0,1.0,1.0)
		_EdgeColor("Edge Color", Color) = (0.0,0.0,0.0,1.0)
		_UnlitVolume("Unlit Volume", Range(-1, 1)) = 0.1
		_Brightness("Brightness", Float) = 1.0
		_LightingLevels("Lighting Levels", Int) = 6

		_FlatNormal("Flat Normal", Vector) = (0.0, 1.0, 0.0, 1.0)
		//The required Dot product between Flat Normal and surface normal to be considered flat.
		_RequiredFlatness("Required Flatness", Range(0, 1)) = 0.9
		_MinFlatBrightness("Minimum Flat Brightness", Range(0, 1)) = 0.2
	}

	SubShader {
		//Tells Unity what we're about to do. In this case we're about to render an opaque texture.
		Tags {
			"Queue" = "Geometry"
			"RenderType" = "Opaque"			
		}

		//Tell Unity we are about to use the CGProgramming language to do stuff.
		CGPROGRAM

		//Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf CelShadingForward keepalpha
		//Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			//There's a built in list of possible variables. Use to link and scroll near the bottom of the page.
			//https://docs.unity3d.com/Manual/SL-SurfaceShaders.html
			//The input structure Input generally has any texture coordinates needed by the shader.
			//Texture coordinates must be named “uv” followed by texture name (or start it with “uv2” to use second texture coordinate set).
			float2 uv_MainTex; //Grab the uv of MainTex and store it here in the first uv thingy.
			float2 uv2_MainTex; //Grab the 2nd uv of MainTex and store it here in the first uv thingy.

			float4 color : COLOR;
			INTERNAL_DATA float3 viewDir;
			INTERNAL_DATA float3 worldNormal;
		};

		//Define our properties.
		sampler2D _MainTex; //Our texture.
		fixed4 _Color;
		fixed4 _EdgeColor;
		float _UnlitVolume;
		float _Brightness;
		int _LightingLevels;

		half visibility;
		fixed4 _FlatNormal;
		float _RequiredFlatness;
		float _MinFlatBrightness;		
		half flatness;

		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten) {
			half4 c;
			if (visibility >= _UnlitVolume) {
				half NdotL = dot(s.Normal, lightDir); //How much light is shining on the surface.
				half brightness = brightness = (floor(NdotL * _LightingLevels) / _LightingLevels);

				if (flatness >= _RequiredFlatness) {
					brightness = max(_MinFlatBrightness, brightness);
				}
			
				c.rgb = s.Albedo * _LightColor0.rgb * (brightness * atten * 2.0) * _Brightness;
				c.a = s.Alpha;
			} else {
				c.rgb = s.Albedo;
				c.a = s.Alpha;
			}

			return c;
		}

		//This is our output.
		void surf (Input IN, inout SurfaceOutput o) {
			//Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			//fixed4 c = _Color;
			o.Alpha = c.a;	

			//Is this surface flat?
			fixed3 flatNormal = _FlatNormal.xyz * _FlatNormal.w;
			flatness = dot(flatNormal, IN.worldNormal);
			visibility = dot(IN.viewDir, o.Normal);
			
			//If the surface is flat or visible,
			if (flatness >= _RequiredFlatness || visibility >= _UnlitVolume) {
				o.Albedo = c.rgb * _Color;
			} else {
				o.Albedo = _EdgeColor * _LightColor0;
			}
		}
		ENDCG
	}

	FallBack "Diffuse" //What shader to use if the above subshader(s) doesn't work.

}