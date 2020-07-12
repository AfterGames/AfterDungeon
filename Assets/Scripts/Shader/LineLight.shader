Shader "Unlit/LineLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Thickness("Thickness", Range(0,1)) = 0.15
		_Proportion("Proportion", Range(0,1)) = 0.5
		_Brightness("Brightness", float) = 2
		//_Alpha("Alpha", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		GrabPass{}
        LOD 100

        Pass
        {
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
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 grabPos : TEXCOORD1;
            };

            sampler2D _MainTex;
			sampler2D _GrabTexture;
            float4 _MainTex_ST;
			float _Proportion;
			half _Thickness;
			float _Brightness;
			//float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = tex2Dproj(_GrabTexture, i.grabPos);
                
			if (i.uv.x < _Proportion)
				return col;
			else
			{
				if (0.5 - _Thickness < i.uv.y && i.uv.y < 0.5 + _Thickness)
				{	
					float multi = _Brightness;
					//float multi = 1 + (_Thickness * _Thickness - (i.uv.y - 0.5) * (i.uv.y - 0.5)) / (_Thickness * _Thickness) * (_Brightness - 1);
					//float multi = 1 + (((i.uv.y - 0.5) * (i.uv.y - 0.5) - 2 * sqrt((i.uv.y - 0.5) * (i.uv.y - 0.5)) * _Thickness) / (_Thickness * _Thickness) + 1) * (_Brightness - 1);
					return fixed4(col.xyz * multi, 1);					
				}
				else
					return col;
			}

            }
            ENDCG
        }
    }
}
