Shader "Custom/AlphaBlending_Peripheral" {
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
