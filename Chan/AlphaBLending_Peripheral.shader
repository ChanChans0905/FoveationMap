Shader "Custom/AlphaBlending_Peripheral" {
    Properties {

    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // Including necessary Unity shader libraries
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float Resolution_X;
            float Resolution_Y;
            float4 UserGazePoint;
            float FoveaRegionSize;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Sample the texture color
                fixed4 TextureColor = tex2D(_MainTex, i.uv);
                Resolution_X = 7680.0;
                Resolution_Y = 4320.0;

                float PixelCoor_X = i.worldPos.x;
                float PixelCoor_Y = i.worldPos.y;
                float FRS_Left = UserGazePoint.x - FoveaRegionSize;
                float FRS_Right = UserGazePoint.x + FoveaRegionSize;
                float FRS_Up = UserGazePoint.y + FoveaRegionSize;
                float FRS_Down = UserGazePoint.y - FoveaRegionSize;

                
                // Square Region
                bool FoveaRegion = PixelCoor_X < FRS_Right && PixelCoor_X > FRS_Left && PixelCoor_Y < FRS_Up && PixelCoor_Y > FRS_Down;
                bool BlendRegion_1 = PixelCoor_X < FRS_Right + 0.5 && PixelCoor_X > FRS_Left - 0.5 && PixelCoor_Y < FRS_Up + 0.5 && PixelCoor_Y > FRS_Down - 0.5; 
                bool BlendRegion_2 = PixelCoor_X < FRS_Right + 1.0 && PixelCoor_X > FRS_Left - 1.0 && PixelCoor_Y < FRS_Up + 1.0 && PixelCoor_Y > FRS_Down - 1.0; 
                bool BlendRegion_3 = PixelCoor_X < FRS_Right + 1.5 && PixelCoor_X > FRS_Left - 1.5 && PixelCoor_Y < FRS_Up + 1.5 && PixelCoor_Y > FRS_Down - 1.5; 
            
                
                if(FoveaRegion)
                {
                    col = tex2D(_MainTex,uv);
                }

                if(!FoveaRegion)
                {
                    if(BlendRegion_3)
                    {
                        TextureColor.a = 0.75;
                        //col = float4(1,1,1,1);
                        
                    }
                    if(BlendRegion_2)
                    {
                        TextureColor.a = 0.5;
                        //col = float4(0,1,1,1);
                    }
                    if(BlendRegion_1)
                    {
                        TextureColor.a = 0.25;
                        //col = float4(1,0,1,1);
                    }
                    if(!BlendRegion_3) // Peripheral Region
                    {
                        TextureColor.a = 1.0;
                        //col = float4(1,0,0,1);
                    }
                }
                
                return TextureColor; // Return the modified color
            }
            ENDCG
        }
    }
}
