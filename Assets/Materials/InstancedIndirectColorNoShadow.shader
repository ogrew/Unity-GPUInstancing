Shader "Custom/InstancedIndirectColorNoShadow"
{
    SubShader 
    {
        CGINCLUDE
            #include "UnityCG.cginc"
            struct MeshProperties {
                float4x4 mat;
                float4 color;
            };
            StructuredBuffer<MeshProperties> _Properties;
        ENDCG

        Pass 
        {
            Tags { "RenderType" = "Opaque" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct v2f {
                float4 pos          : SV_POSITION;
                fixed4 color        : COLOR;
            };

            v2f vert(appdata_full i, uint instanceID : SV_InstanceID) {
                MeshProperties props = _Properties[instanceID];

                v2f o;
                float4 pp = mul(props.mat, i.vertex);
                o.pos = UnityObjectToClipPos(pp);
                o.color = props.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return i.color;
            }
            ENDCG
        }
    }
}