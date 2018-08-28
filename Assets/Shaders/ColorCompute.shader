Shader "Custom/ColorCompute" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	    _Texture2("Albedo (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Pass{
		CGPROGRAM

#include "UnityCG.cginc"  
#pragma vertex vert_img  
#pragma fragment frag  

		uniform sampler2D _MainTex;
	    uniform sampler2D _Texture2;

		struct appdata {
			float4 vertex: POSITION;
			float2 texcoord:TEXCOORD0;
			float2 texcoord2:TEXCOORD1;
			fixed4 color : COLOR;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			half2 uv : TEXCOORD0;
			half2 uv2: TEXCOORD1;
			fixed4 color : COLOR;
		};


	//去色
	fixed4 DelColor(fixed4 _color) {
		fixed c = _color.r + _color.g + _color.b;
		c /= 3;
		return fixed4(c,c,c,1.0f);
	}
	


	//曝光
	fixed4 Exposure(fixed4 _color,fixed force) {
		fixed r = min(1,max(0,_color.r * pow(2,force)));
		fixed g = min(1,max(0,_color.g * pow(2,force)));
		fixed b = min(1,max(0,_color.b * pow(2,force)));

		return fixed4(r,g,b,1.0f);
	}
	//颜色加深
	fixed4 ColorPlus(fixed4 _color) {
		fixed r = 1 - (1 - _color.r) / _color.r;
		fixed g = 1 - (1 - _color.g) / _color.g;
		fixed b = 1 - (1 - _color.b) / _color.b;

		return fixed4(r,g,b,1.0f);
	}

	//颜色减淡
	fixed4 ColorMinus(fixed4 _color) {
		fixed r = _color.r + pow(_color.r,2) / (1 - _color.r);
		fixed g = _color.g + pow(_color.g,2) / (1 - _color.g);
		fixed b = _color.b + pow(_color.b,2) / (1 - _color.b);

		return fixed4(r,g,b,1.0f);
	}


	//滤色
	fixed4 Screen(fixed4 _color) {
		fixed r = 1 - (pow((1 - _color.r),2));
		fixed g = 1 - (pow((1 - _color.g),2));
		fixed b = 1 - (pow((1 - _color.b),2));
		return fixed4(r,g,b,1.0f);
	}

	//正片叠底
	fixed4 Muitiply(fixed4 _color) {
		fixed r = pow(_color.r,2);
		fixed g = pow(_color.g,2);
		fixed b = pow(_color.b,2);
		return fixed4(r,g,b,1.0f);
	}
	//强光
	fixed4 ForceLight(fixed4 _color) {
		fixed r = 1 - pow((1 - _color.r),2) / 0.5f;
		fixed g = 1 - pow((1 - _color.g),2) / 0.5f;
		fixed b = 1 - pow((1 - _color.b),2) / 0.5f;
		if (_color.r < 0.5f) r = pow(_color.r,2) / 0.5f;
		if (_color.g < 0.5f) g = pow(_color.g,2) / 0.5f;
		if (_color.b < 0.5f) b = pow(_color.b,2) / 0.5f;
		return fixed4(r,g,b,1.0f);
	}
	//高光
	fixed4 HighLight(fixed4 _color, fixed4 _color2) {
		fixed r = _color.r + (_color.r*(2*_color2.r - 1)) / (2 * (1 - _color2.r));
		fixed g = _color.g + (_color.g*(2*_color2.g - 1)) / (2 * (1 - _color2.g));
		fixed b = _color.r + (_color.b*(2*_color2.b - 1)) / (2 * (1 - _color2.b));
		if (_color2.r < 0.5f) r = _color.r - ((1 - _color.r)*(1 - 2 * _color2.r)) / (2 * _color2.r);
		if (_color2.g < 0.5f) g = _color.g - ((1 - _color.g)*(1 - 2 * _color2.g)) / (2 * _color2.g);
		if (_color2.b < 0.5f) r = _color.b - ((1 - _color.b)*(1 - 2 * _color2.b)) / (2 * _color2.b);
		return fixed4(r, g, b, 1.0f);
	}
	//叠加测试
	fixed4 Add(fixed4 _color, fixed4 _color2) {
		fixed r = _color.r*_color2.r;
		fixed g = _color.g*_color2.g;
		fixed b = _color.b*_color2.b;
		return fixed4(r, g, b, 1.0f);
	}




	float4 frag(v2f_img o) : COLOR
	{
		fixed4 _color = tex2D(_MainTex, o.uv);
	    fixed4 _color2 = tex2D(_Texture2, o.uv);

		//_color = DelColor(_color);

		//_color = ColorPlus(_color);
		//_color = ColorMinus(_color);
		//_color = Screen(_color);
		//_color = Muitiply(_color);
		//_color = ForceLight(_color);
		//_color = HighLight(_color ,_color2);
		_color = Add(_color, _color2);
		return _color;
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}

