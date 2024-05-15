Shader "Custom/AlphaBlending_Fovea" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        // Render with transparency
        Tags {"Render Queue" = "Transparency"  "RenderType"="Transparency" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // Including necessary Unity shader libraries
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 UserGazePoint;
            float FoveaRegionSize;

            v2f vert (appdata v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {

                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                
                // Sample the texture color
                fixed4 TextureColor = tex2D(_MainTex, i.uv);

                float UV_X = i.worldPos.x;
                float UV_Y = i.worldPos.y;
                float FR_L = UserGazePoint.x - FoveaRegionSize;
                float FR_R = UserGazePoint.x + FoveaRegionSize;
                float FR_U = UserGazePoint.y + FoveaRegionSize;
                float FR_D = UserGazePoint.y - FoveaRegionSize;

                float BR_1 = FoveaRegionSize * 0.01;
                float BR_2 = FoveaRegionSize * 0.02;
                float BR_3 = FoveaRegionSize * 0.03;
                float FRL = FoveaRegionSize * 0.02;
                
                // Square Region
                bool FoveaRegion = UV_X < FR_R && UV_X > FR_L && UV_Y < FR_U && UV_Y > FR_D;

                // Define blend regions
                bool BlendRegion_1 = UV_X < FR_R - BR_1 && UV_X > FR_L + BR_1 && UV_Y < FR_U - BR_1 && UV_Y > FR_D + BR_1; 
                bool BlendRegion_2 = UV_X < FR_R - BR_2 && UV_X > FR_L + BR_2 && UV_Y < FR_U - BR_2 && UV_Y > FR_D + BR_2; 
                bool BlendRegion_3 = UV_X < FR_R - BR_3 && UV_X > FR_L + BR_3 && UV_Y < FR_U - BR_3 && UV_Y > FR_D + BR_3; 
                bool FoveaRegionLine = UV_X < FR_R + FRL && UV_X > FR_L - FRL && UV_Y < FR_U + FRL && UV_Y > FR_D - FRL; 

                if(FoveaRegion)
                {
                    TextureColor.a = 0.25;

                    if(BlendRegion_1)
                        TextureColor.a = 0.5;             

                    if(BlendRegion_2)
                        TextureColor.a = 0.75;

                    if(BlendRegion_3)
                        TextureColor.a = 1;
                }
            // else if(FoveaRegionLine) // fovea region boundary
            //       TextureColor = (0,0,0,1);
                else if(!FoveaRegion) // Peripheral Region
                    TextureColor.a = 0.0;
                
                return TextureColor; // Return the modified color
            }
            ENDCG
        }
    }
}
