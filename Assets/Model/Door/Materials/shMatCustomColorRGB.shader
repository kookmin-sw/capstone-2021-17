Shader "Custom/shMatCustomColorRGB" {
	Properties {
		_cRed ("ColorRed", Color) = (1,0,0,1)
		_cGreen ("ColorGreen", Color) = (0,1,0,1)
		_cBlue ("ColorBlue", Color) = (0,0,1,1)
		_cDirty ("ColorDirty", Color) = (0.5,0.5,0.5,1)
		_cLight ("ColorLight", Color) = (0,0,0,1)
		_cBorderDamaged ("ColorBorderDamaged", Color) = (0.5,0.5,0.5,1)

		_gRedMk ("Glossiness Red Mask", Range(0,1)) = 0.5
		_gGreenMk ("Glossiness Green Mask", Range(0,1)) = 0.5
		_gBlueMk ("Glossiness Blue Mask", Range(0,1)) = 0.5

		_mRedMk ("Metallic Red Mask", Range(0,1)) = 0.0
		_mGreenMk ("Metallic Green Mask", Range(0,1)) = 0.0
		_mBlueMk ("Metallic Blue Mask", Range(0,1)) = 0.0

		_iAO ("Ambient Occlusion", Range(0,2)) = 1.0
		_gAO ("Glossiness Ambient Occlusion", Range(0,1)) = 0.0
		_iLight ("Intensity Light", Range(0,10)) = 0.0
		_iBorderDamaged ("Border Damaged", Range(0,2)) = 0.0
		_gBorderDamaged("Glossiness Border Damaged", Range(0,1)) = 0.0
		_iDirty ("Dirty", Range(0,2)) = 0.0
		_gDirty ("Glossiness Dirty", Range(0,1)) = 0.0
		_iNormalMap ("Intensity Normal Map", Range(0,2)) = 1.0

		_MaskTex ("Mask Texture (RGB)", 2D) = "black" {}
		_MaskEm ("Mask Emission (RGB)", 2D) = "black" {}
		_CompTex ("Composite Dirty/AO/Damaged Texture (RGB)", 2D) = "white" {}		
		_NormalTex ("Normal Map", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MaskTex, _CompTex, _NormalTex, _MaskEm;

		struct Input {
			float2 uv_MaskTex;
		};

		half _gRedMk, _gGreenMk, _gBlueMk, _mRedMk, _mGreenMk, _mBlueMk;
		half _iAO, _iBorderDamaged, _iDirty, _gAO, _gBorderDamaged, _gDirty, _iNormalMap, _iLight;
		fixed4 _cRed, _cGreen, _cBlue, _cDirty, _cLight,_cBorderDamaged;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 _cMk = tex2D (_MaskTex, IN.uv_MaskTex);
			fixed4 _cCp = tex2D (_CompTex, IN.uv_MaskTex);
			fixed4 _mEm = tex2D (_MaskEm, IN.uv_MaskTex);


			// Final Color
			fixed4 _finalColor;
			_finalColor = lerp(float4(0, 0, 0, 1), _cRed, _cMk.r);
			_finalColor = lerp(_finalColor, _cGreen, _cMk.g);
			_finalColor = lerp(_finalColor, _cBlue, _cMk.b);
			_finalColor = lerp(_finalColor, _cDirty, lerp(0.0, _cCp.b, _iDirty));
			_finalColor = lerp(_finalColor, _cBorderDamaged, lerp(0.0, _cCp.g, _iBorderDamaged));

			_finalColor *= lerp(float4(1, 1, 1, 1), float4(_cCp.r, _cCp.r, _cCp.r, 1), _iAO);			

			_finalColor = lerp(_finalColor, _cLight, lerp(0.0, _iLight, _mEm.r));
			o.Albedo = _finalColor.rgb;


			// Normal Map
			fixed3 _n = UnpackNormal(tex2D(_NormalTex, IN.uv_MaskTex)).rgb;
			_n.x *= _iNormalMap;
			_n.y *= _iNormalMap;
			o.Normal = normalize(_n);


			// Final Glossiness
			half _finalGlossiness;
			_finalGlossiness = lerp(0.0, _gRedMk, _cMk.r);
			_finalGlossiness = lerp(_finalGlossiness, _gGreenMk, _cMk.g);
			_finalGlossiness = lerp(_finalGlossiness, _gBlueMk, _cMk.b);

			_finalGlossiness = lerp(_finalGlossiness, _gAO, lerp(0.0, 1-_cCp.r, _iAO));
			_finalGlossiness = lerp(_finalGlossiness, _gBorderDamaged, lerp(0.0, _cCp.g, _iBorderDamaged));
			_finalGlossiness = lerp(_finalGlossiness, _gDirty, lerp(0.0, _cCp.b, _iDirty));

			o.Smoothness = _finalGlossiness;


			//Final Metallic
			half _finalMetallic;
			_finalMetallic = lerp(0.0, _mRedMk, _cMk.r);
			_finalMetallic = lerp(_finalMetallic, _mGreenMk, _cMk.g);
			_finalMetallic = lerp(_finalMetallic, _mBlueMk, _cMk.b);

			o.Metallic = _finalMetallic;


			//Alpha
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

