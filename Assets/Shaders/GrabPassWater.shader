// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GrabPassInvert"
{
	Properties{
		_DisplaceTex("displacement texture",2D) = "white"{}
	}
    SubShader
    {
        Tags { "Queue" = "Transparent" }

        GrabPass
        {
            "_BackgroundTexture"
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 disuv : TEXCOORD1;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_full v) {
                v2f o;
                o.disuv = v.texcoord;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }

            sampler2D _BackgroundTexture;
            sampler2D _DisplaceTex;

            half4 frag(v2f i) : SV_Target
            {
            	float2 animcoords = float2(i.disuv.x+_Time.x * 2,i.disuv.y+_Time.x * 2);
            	float2 disp = tex2D(_DisplaceTex,animcoords);
            	disp = ((disp * 2) - 1)*0.1;
            	float4 coords = i.grabPos+float4(disp,0,1);
            	float4 color = tex2D(_BackgroundTexture,UNITY_PROJ_COORD(coords));
            	return color;
            }
            ENDCG
        }

    }
}