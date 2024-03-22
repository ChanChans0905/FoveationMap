Shader "Custom/AlphaBlending_Fovea" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        // Render with transparency
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
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
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Sample the texture color
                fixed4 texColor = tex2D(_MainTex, i.uv);

                
                // Square Region
                bool FoveaRegion = Coor_X < XXX && Coor_X > XXX2 && Coor_Y < YYY && Coor_Y > YYY2;
                bool BlendRegion_1 = Coor_X < XXX + 0.5 && Coor_X > XXX2 - 0.5 && Coor_Y < YYY + 0.5 && Coor_Y > YYY2 - 0.5; 
                bool BlendRegion_2 = Coor_X < XXX + 1.0 && Coor_X > XXX2 - 1.0 && Coor_Y < YYY + 1.0 && Coor_Y > YYY2 - 1.0; 
                bool BlendRegion_3 = Coor_X < XXX + 1.5 && Coor_X > XXX2 - 1.5 && Coor_Y < YYY + 1.5 && Coor_Y > YYY2 - 1.5; 
                bool BlendRegion_4 = Coor_X < XXX + 2.0 && Coor_X > XXX2 - 2.0 && Coor_Y < YYY + 2.0 && Coor_Y > YYY2 - 2.0; 

                
                if(FoveaRegion)
                {
                    col = tex2D(_MainTex,uv);
                }

                if(!FoveaRegion)
                {
                    if(BlendRegion_4)
                    {
                        gridSize = float2(_Resolution_X, _Resolution_Y);
                        gridSize *= _GroupSize*1.25;
                        //col = float4(1,1,1,1);
                        
                    }
                    if(BlendRegion_3)
                    {
                        gridSize = float2(_Resolution_X, _Resolution_Y);
                        gridSize *= _GroupSize*1.5;
                        //col = float4(0,1,1,1);
                    }
                    if(BlendRegion_2)
                    {
                        gridSize = float2(_Resolution_X, _Resolution_Y);
                        gridSize *= _GroupSize*1.75;
                        //col = float4(1,0,1,1);
                    }
                    if(BlendRegion_1)
                    {
                        gridSize = float2(_Resolution_X, _Resolution_Y);
                        gridSize *= _GroupSize*2.0;
                        //col = float4(1,1,0,1);
                    }

                    // Square Region
                    if(!BlendRegion_4) // Peripheral Region
                    {
                        gridSize = float2(_Resolution_X, _Resolution_Y);
                        gridSize *= _GroupSize;
                        //col = float4(1,0,0,1);
                    }

                
                // Calculate distance from the center of the UV map
                float dis = distance(i.uv, float2(0.5, 0.5));
                
                // Check the distance to adjust transparency
                if (dis < 0.2) {
                    // Adjust only the alpha value of the texture color to make it semi-transparent
                    texColor.a = 0.5;
                } 
                // Else, keep the original alpha value of the texture
                
                return texColor; // Return the modified color
            }
            ENDCG
        }
    }
}
