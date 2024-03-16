Shader "CanvasShader"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing


            #include "UnityCG.cginc"
            #define UNITY_INDIRECT_DRAW_ARGS IndirectDrawArgs
            #include "UnityIndirect.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR0;
            };

            StructuredBuffer<int> _Triangles;
            StructuredBuffer<float3> _Positions;
            StructuredBuffer<int> _Indices;
            uniform uint _BaseVertexIndex;
            uniform int _EdgeLength;
            uniform float4x4 _ObjectToWorld;

            v2f vert (uint svVertexID: SV_VertexID, uint svInstanceID : SV_InstanceID)
            {
                InitIndirectDrawArgs(0);
                v2f o;
                uint cmdID = GetCommandID(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);
                float3 pos = _Positions[_Triangles[GetIndirectVertexID(svVertexID)] + _BaseVertexIndex];
                int index = _Indices[(int)instanceID];

                int x = index%_EdgeLength;
                int y = index/_EdgeLength;
                float4 wpos = mul(_ObjectToWorld, float4(pos + float3(x, y, 0.0f), 1.0f));
                o.vertex = mul(UNITY_MATRIX_VP, wpos);
                o.color = float4(1,1,1,1);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
