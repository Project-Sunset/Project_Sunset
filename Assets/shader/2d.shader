Shader "Unlit/2d"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ZP("Texture1", 2D) = "white" {}
		
    }
    SubShader
    {
        Tags {"Queue" = "Transparent"
		"RenderType" = "Transparent" }
        LOD 100
		Pass//正片叠底
		{

			Blend DstColor OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv_shadow:TEXCOORD1;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			float2 uv_shadow:TEXCOORD1;
			
		};

		sampler2D _ZP;
		float4 _ZP_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv_shadow = TRANSFORM_TEX(v.uv_shadow, _ZP);
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			fixed4 s_col = tex2D(_ZP, i.uv_shadow);
			s_col.rgb *= s_col.a;//这里乘上透明度使透明度对rgb生效
			return s_col;
		}
		ENDCG
		}

        Pass//正常显示
        {

			Blend One OneMinusSrcAlpha
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= col.a;//这里乘上透明度使透明度对rgb生效
                return col;
            }
            ENDCG
        }
    }
		
}
