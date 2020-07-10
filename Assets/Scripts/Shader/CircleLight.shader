Shader "Unlit/CircleLight"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Radius("Radius", float) = 2
		_Bright("Brightness", float) = 2
		_CenterX("x coordinate of center", float) = 0
		_CenterY("y coordinate of center", float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			GrabPass{"_Savior"}
			GrabPass{}
			LOD 100

			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				//float4 grabPos : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 clipPos : SV_POSITION;
				float4 worldPos : TEXCOORD1;
				float4 grabPos : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _GrabTexture;
			sampler2D _Savior;
			float4 _MainTex_ST;
			float _Radius;
			float _Bright;
			float _CenterX;
			float _CenterY;

			float distance;

			v2f vert(appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.clipPos = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.clipPos);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				distance = (i.worldPos.x - _CenterX) * (i.worldPos.x - _CenterX) + (i.worldPos.y - _CenterY) * (i.worldPos.y - _CenterY);

				//fixed4 col = tex2D(_MainTex, i.clipPos);
				fixed4 col = tex2Dproj(_Savior, i.grabPos);
				fixed4 col2 = tex2Dproj(_GrabTexture, i.grabPos);
				fixed4 result = fixed4(col.xyz * ((_Bright - 1)*(_Radius * _Radius - distance) / (_Radius * _Radius) + 1), 1);

				if (distance <= _Radius * _Radius)
				{
					if (result.x < col2.x)
						return fixed4(col2.xyz, 1);
					else
						return result;
				}
				else
					return fixed4(col2.xyz, 1);
			}
		ENDCG
			}
		}
		
}
