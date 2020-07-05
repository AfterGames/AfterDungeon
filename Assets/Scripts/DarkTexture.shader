Shader "Unlit/DarkTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Bright("Brightness", float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

		GrabPass{"_BackgroundTexture"}
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
                float4 grabPos : TEXCOORD0;
                float4 clipPos : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _BackgroundTexture;
            float4 _MainTex_ST;
			float _Bright;

            v2f vert (appdata v)
            {
                v2f o;
                o.clipPos = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.clipPos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = tex2Dproj(_BackgroundTexture , i.grabPos);
                // apply fog
				col.r = col.r*_Bright;
				col.g = col.g*_Bright;
				col.b = col.b*_Bright;
                return fixed4(col.xyz,1);
            }
            ENDCG
        }
    }
}
