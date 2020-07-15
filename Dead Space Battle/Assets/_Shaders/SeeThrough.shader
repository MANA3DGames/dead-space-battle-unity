// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "MANA/SeeThrough"
{
	Properties
	{
		_Color ( "Main Color", COLOR ) = ( 1.0, 1.0, 1.0, 1.0 )
		_CursorPos ( "Cursor Position", Vector ) = ( 0, 0, 0, 0 )
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
				uniform fixed4 _CursorPos;

				struct vertexInput
				{
					float4 vertex : POSITION;
				};

				struct fragmentInput
				{
					float4 pos : SV_POSITION;
					float4 color : COLOR0;
				};


				fragmentInput vert( vertexInput v )
				{
					fragmentInput f;

					f.pos = mul( UNITY_MATRIX_MVP, v.vertex );
					f.color = _Color;


					float4 modelMatrix = mul( unity_ObjectToWorld, v.vertex );
					if ( modelMatrix.x >= _CursorPos.x - 1.0 && modelMatrix.x <= _CursorPos.x + 1.0 &&
						 modelMatrix.y >= _CursorPos.y - 1.0 && modelMatrix.y <= _CursorPos.y + 1.0 &&
						 modelMatrix.z >= _CursorPos.z - 1.0 && modelMatrix.z <= _CursorPos.z + 1.0 )
						f.color = float4( 0, 0, 0, 0 );


					return f;
				}

				half4 frag( fragmentInput f ) : COLOR
				{
					return f.color;
				}
			ENDCG
		}
	}

	Fallback "Diffuse"
}