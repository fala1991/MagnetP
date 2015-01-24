Shader "Custom/TexAnimator" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Intensity("intensity of the texture", Range(0,0.99)) = 0

		_TexWidth("Sheet Width", float) = 1.0
		_TexHeight("Sheet Height", float) = 1.0

		_CellWidth ("Cell Width", float) = 0.0
		_CellHeight ("Cell Height", float) = 0.0

		_Speed ("Speed", Range(0.01,32)) = 12
	}
	SubShader {
		Tags { 
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"IgnoreProjector"="True"
		}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed _Intensity;
		fixed _Speed;
		float _TexWidth;
		float _TexHeight;

		float _CellHeight;
		float _CellWidth;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			
			float cellUVPerX = _CellWidth / _TexWidth;
			float cellUVPerY = _CellHeight / _TexHeight;
			float rowAmount = _TexWidth / _CellWidth;
			float coloumAmount = _TexHeight / _CellHeight;
			float xVal = fmod(_Time.y * _Speed, rowAmount);
			xVal = ceil(xVal);

			float yVal = _Intensity / cellUVPerY;
			yVal = floor(yVal);

			float2 spriteUV = IN.uv_MainTex;
			float xValue = spriteUV.x;
			float yValue = spriteUV.y;

			xValue += cellUVPerX * xVal * rowAmount;
			xValue *= cellUVPerX;

			yValue += cellUVPerY * yVal * coloumAmount;
			yValue *= cellUVPerY;

			spriteUV = float2(xValue,yValue);
			half4 c = tex2D (_MainTex, spriteUV);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
