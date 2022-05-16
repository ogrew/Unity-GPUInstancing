Shader "Custom/InstancedColorNoShadow"
{
    SubShader 
    {
        CGINCLUDE
        #include "UnityCG.cginc"
        ENDCG

        Pass 
        {
            Tags { "RenderType" = "Opaque"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            struct v2f {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
            }; 

            float4 _Colors[1023];   // Max instanced batch size.

            v2f vert(appdata_full v, uint instanceID: SV_InstanceID) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v)
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = _Colors[instanceID];
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return i.color;
            }
            ENDCG
        }
    }
}