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

                float PixelCoor_X = i.worldPos.x;
                float PixelCoor_Y = i.worldPos.y;
                float FRS_Left = UserGazePoint.x - FoveaRegionSize;
                float FRS_Right = UserGazePoint.x + FoveaRegionSize;
                float FRS_Up = UserGazePoint.y + FoveaRegionSize;
                float FRS_Down = UserGazePoint.y - FoveaRegionSize;
                
                // Square Region
                bool FoveaRegion = PixelCoor_X < FRS_Right && PixelCoor_X > FRS_Left && PixelCoor_Y < FRS_Up && PixelCoor_Y > FRS_Down;
                bool BlendRegion_1 = PixelCoor_X < FRS_Right + FoveaRegionSize*2/100 && PixelCoor_X > FRS_Left - FoveaRegionSize*2/100 && PixelCoor_Y < FRS_Up + FoveaRegionSize*2/100 && PixelCoor_Y > FRS_Down - FoveaRegionSize*2/100; 
                bool BlendRegion_2 = PixelCoor_X < FRS_Right + FoveaRegionSize*4/100 && PixelCoor_X > FRS_Left - FoveaRegionSize*4/100 && PixelCoor_Y < FRS_Up + FoveaRegionSize*4/100 && PixelCoor_Y > FRS_Down - FoveaRegionSize*4/100; 
                bool BlendRegion_3 = PixelCoor_X < FRS_Right + FoveaRegionSize*6/100 && PixelCoor_X > FRS_Left - FoveaRegionSize*6/100 && PixelCoor_Y < FRS_Up + FoveaRegionSize*6/100 && PixelCoor_Y > FRS_Down - FoveaRegionSize*6/100; 

                if(!FoveaRegion)
                {
                    if(BlendRegion_3)
                        TextureColor.a = 0.25;             

                    if(BlendRegion_2)
                        TextureColor.a = 0.5;

                    if(BlendRegion_1)
                        TextureColor.a = 0.75;

                    if(!BlendRegion_3) // Peripheral Region
                        TextureColor.a = 0.0;
                }
                
                return TextureColor; // Return the modified color
            }
            ENDCG
        }
    }
}
