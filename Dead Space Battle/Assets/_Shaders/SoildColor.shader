Shader "MANA/Simple/Color/SolidColor"
{
	Properties
	{
		_Color ( "Tint Color", COLOR ) = ( 1.0, 1.0, 1.0, 1.0 )
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
		#pragma exclude_renderers ps3 xbox360 flash
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		uniform fixed4 _Color;

		struct vertexInput
		{
			float4 vertex : POSITION;		// position in object coordinates
		};

		struct fragmentInput
		{
			float4 pos : SV_POSITION;
			float4 color : COLOR0;
		};

		fragmentInput vert( vertexInput v )
		{
			fragmentInput o;
			o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
			o.color = _Color;

			if ( v.vertex.y > 0.01f )
			{
				o.color = float4(0, 0, 1, 0.1f);
			}

			return o;
		}

		half4 frag( fragmentInput i ) : COLOR
		{
			return i.color;
		}

		ENDCG
		}
	}

	FallBack "Diffuse"
}