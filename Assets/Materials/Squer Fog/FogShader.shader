Shader "Unlit/FogShader"
{
    Properties
    {
        _GridSize("GridSize",Float) = 10
        _FogRate("Fog rate",Float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
                float fogAmmount : dr;
            };

            float _GridSize;
            float _FogRate;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float4 pos = mul(unity_ObjectToWorld,v.vertex);
                float2 delta = abs(pos.xz) - _GridSize;
                float maxDelta = max(delta.x, delta.y);
                
                if (maxDelta > _FogRate)
                    o.fogAmmount = 1;
                else if (maxDelta > 0) 
                        o.fogAmmount = maxDelta / _FogRate;
                else
                    o.fogAmmount = 0;
                return o;
            }

            float4 _MainColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = float4(0,0,0, i.fogAmmount);
                return col;
            }
            ENDCG
        }
    }
}
