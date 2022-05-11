Shader "Custom/InstancedColor"
{
    SubShader 
    {
        CGINCLUDE
        #include "UnityCG.cginc"
        ENDCG

        Pass 
        {
            Tags { "RenderType" = "Opaque" "LightMode"="ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            struct v2f {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
            }; 

            float4 _Colors[1023];   // Max instanced batch size.

            v2f vert(appdata_full i, uint instanceID: SV_InstanceID) {
                UNITY_SETUP_INSTANCE_ID(i);

                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.color = _Colors[instanceID];
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return i.color;
            }
            ENDCG
        }

        Pass
        {
            Tags { "LightMode"="ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert_shadow
            #pragma fragment frag_shadow
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing

            struct v2f {
                V2F_SHADOW_CASTER;
            };

            v2f vert_shadow(appdata_base v)
            {
                UNITY_SETUP_INSTANCE_ID(v);

                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag_shadow(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}