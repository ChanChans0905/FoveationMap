Shader "Unlit/Screen_FFR"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Resolution_X("Resolution_X", Float) = 3840
        _Resolution_Y("Resolution_Y", Float) = 2160
        _DownscaleFactor("Downscale Factor", Range(0,3)) = 1.0
        _GazePoint("Gaze Point", Vector) = (0,0,10,0)
        _FoveaRegionRadius("Fovea Region Radius", Float) = 5
        _GroupSize("Group Size", Range(0.01,1)) = 1.0
        _Sigma("Sigma", Range(0.1,5.0)) = 1.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                
                // get world position of pixel
                float3 worldPos : TEXCOORD1;
            };

            // Variables
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _DownscaleFactor;
            float _Resolution_X;
            float _Resolution_Y; 
            float4 _GazePoint;
            float _FoveaRegionRadius;
            float _GroupSize;
            float _Sigma;
            float _GaussianRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float Gaussian(float x, float y, float _Sigma) {
                float coeff = 1.0 / (2.0 * 3.14 * _Sigma * _Sigma);
                float expon = -(x*x + y*y) / (2.0 * _Sigma * _Sigma);
                return coeff * exp(expon);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 col = float4(0,0,0,0);
                float2 gridSize;

                float totalWeight = 0.0;
                int _GaussianRange=5;

                // // Fovea region : Sphere
                // float DistanceVar = distance(i.worldPos, _GazePoint.xyz);

                // if(DistanceVar < _FoveaRegionRadius + 0.1 || DistanceVar > _FoveaRegionRadius - 0.1)
                // {
                //     for (int x = -_GaussianRange; x <= _GaussianRange; x++) {
                //         for (int y = -_GaussianRange; y <= _GaussianRange; y++) {
                //             float2 offset = float2(x, y) / float2(3840.0, 2160.0) ;
                //             float weight = Gaussian(x, y , _Sigma);
                //             col += tex2D(_MainTex, uv + offset) * weight;
                //             totalWeight += weight;
                //         }
                //     }
                //     col /= totalWeight; // 정규화를 통한 최종 색상 계산
                // }

                // if(DistanceVar > _FoveaRegionRadius + 0.1)
                // {
                //                     // 정의된 영역에 따라 UV 좌표를 조정
                // float2 gridSize = float2(3840.0, 2160.0);
                // gridSize *= _GroupSize;
                // float2 gridPosition = floor(i.uv * gridSize) / gridSize;
                // float2 gridStep = 1.0/gridSize;
                                                
                // // // 해당 영역의 가장 왼쪽 위 픽셀 색상을 샘플링
                // // float4 color = tex2D(_MainTex, gridPosition);
                // // return color;
                
                // // 네 모서리 픽셀의 색상을 샘플링
                // float2 topLeft = gridPosition;
                // float2 topRight = gridPosition + float2(gridStep.x, 0);
                // float2 bottomLeft = gridPosition + float2(0, gridStep.y);
                // float2 bottomRight = gridPosition + gridStep;

                // float4 colorTopLeft = tex2D(_MainTex, topLeft);
                // float4 colorTopRight = tex2D(_MainTex, topRight);
                // float4 colorBottomLeft = tex2D(_MainTex, bottomLeft);
                // float4 colorBottomRight = tex2D(_MainTex, bottomRight);
                
                // // 평균 색상 계산
                // float4 averageColor = (colorTopLeft + colorTopRight + colorBottomLeft + colorBottomRight) / 4.0;
                
                // return averageColor;
                // }
                
                // Fovea region : Square
                // float2 GridUV = i.uv * float2(8.0, 8.0);
                // float2 GridPosition = floor(GridUV);
                // int GridNumber = int(GridPosition.y) * 8 + int(GridPosition.x) + 1;
                // bool FoveaRegion_Square = (GridNumber == 19 || GridNumber == 20 || GridNumber == 21 || GridNumber == 22
                //                         || GridNumber == 27 || GridNumber == 28 || GridNumber == 29 || GridNumber == 30
                //                         || GridNumber == 35 || GridNumber == 36 || GridNumber == 37 || GridNumber == 38
                //                         || GridNumber == 43 || GridNumber == 44 || GridNumber == 45 || GridNumber == 46);
                // bool GaussianRegion = (GridNumber == 10 || GridNumber == 11 || GridNumber == 12 || GridNumber == 13
                //                     || GridNumber == 14 || GridNumber == 15 || GridNumber == 18 || GridNumber == 23
                //                     || GridNumber == 26 || GridNumber == 31 || GridNumber == 34 || GridNumber == 39
                //                     || GridNumber == 42 || GridNumber == 47 || GridNumber == 50 || GridNumber == 51
                //                     || GridNumber == 52 || GridNumber == 53 || GridNumber == 54 || GridNumber == 55);
                // bool RestRegion = (GridNumber == 1 || GridNumber == 2 || GridNumber == 3 || GridNumber == 4
                //                 || GridNumber == 5 || GridNumber == 6 || GridNumber == 7 || GridNumber == 8
                //                 || GridNumber == 9 || GridNumber == 16 || GridNumber == 17 || GridNumber == 24
                //                 || GridNumber == 25 || GridNumber == 32 || GridNumber == 33 || GridNumber == 40
                //                 || GridNumber == 41 || GridNumber == 48 || GridNumber == 49 || GridNumber == 56
                //                 || GridNumber == 57 || GridNumber == 58 || GridNumber == 59 || GridNumber == 60
                //                 || GridNumber == 61 || GridNumber == 62 || GridNumber == 63 || GridNumber == 64);

                // Region 구분
                bool FoveaRegion = uv.x > 0.25 && uv.x < 0.75 && uv.y > 0.25 && uv.y < 0.75;
                bool PeripheralRegion = uv.x > 0.2 && uv.x < 0.8 && uv.y > 0.2 && uv.y < 0.8;
                bool BlendRegion_1 = uv.x > 0.24 && uv.x < 0.76 && uv.y > 0.24 && uv.y < 0.76;
                bool BlendRegion_2 = uv.x > 0.23 && uv.x < 0.77 && uv.y > 0.23 && uv.y < 0.77;
                bool BlendRegion_3 = uv.x > 0.22 && uv.x < 0.78 && uv.y > 0.22 && uv.y < 0.78;
                bool BlendRegion_4 = uv.x > 0.21 && uv.x < 0.79 && uv.y > 0.21 && uv.y < 0.79;

                if(FoveaRegion)
                {
                    col = tex2D(_MainTex,uv);
                }

                if(!FoveaRegion)
                {
                    if(!BlendRegion_1)
                    {
                        gridSize = float2(3840.0, 2160.0);
                        gridSize *= _GroupSize*2;
                    }
                    if(!BlendRegion_2)
                    {
                        gridSize = float2(3840.0, 2160.0);
                        gridSize *= _GroupSize*1.75;
                    }
                    if(!BlendRegion_3)
                    {
                        gridSize = float2(3840.0, 2160.0);
                        gridSize *= _GroupSize*1.5;
                    }
                    if(!BlendRegion_4)
                    {
                        gridSize = float2(3840.0, 2160.0);
                        gridSize *= _GroupSize*1.25;
                    }
                    if(!PeripheralRegion)
                    {
                        gridSize = float2(3840.0, 2160.0);
                        gridSize *= _GroupSize;
                    }

                    float2 gridPosition = floor(i.uv * gridSize) / gridSize;
                    float2 gridStep = 1.0/gridSize;
                                                    
                    // 네 모서리 픽셀의 색상을 샘플링
                    float2 topLeft = gridPosition;
                    float2 topRight = gridPosition + float2(gridStep.x, 0);
                    float2 bottomLeft = gridPosition + float2(0, gridStep.y);
                    float2 bottomRight = gridPosition + gridStep;
    
                    float4 colorTopLeft = tex2D(_MainTex, topLeft);
                    float4 colorTopRight = tex2D(_MainTex, topRight);
                    float4 colorBottomLeft = tex2D(_MainTex, bottomLeft);
                    float4 colorBottomRight = tex2D(_MainTex, bottomRight);
                    
                    // 평균 색상 계산
                    float4 averageColor = (colorTopLeft + colorTopRight + colorBottomLeft + colorBottomRight) / 4.0;
                    
                    return averageColor;
                }

                // if(isInBlendRegion)
                // {
                //     for (int x = -_GaussianRange; x <= _GaussianRange; x++) {
                //         for (int y = -_GaussianRange; y <= _GaussianRange; y++) {
                //             float2 offset = float2(x, y) / float2(3840.0, 2160.0) ;
                //             float weight = Gaussian(x, y , _Sigma);
                //             col += tex2D(_MainTex, uv + offset) * weight;
                //             totalWeight += weight;
                //         }
                //     }
                //     col /= totalWeight; // 정규화를 통한 최종 색상 계산 
                // }



                // if(GaussianRegion)
                // {
                //     for (int x = -_GaussianRange; x <= _GaussianRange; x++) {
                //         for (int y = -_GaussianRange; y <= _GaussianRange; y++) {
                //             float2 offset = float2(x, y) / float2(3840.0, 2160.0) ;
                //             float weight = Gaussian(x, y , _Sigma);
                //             col += tex2D(_MainTex, uv + offset) * weight;
                //             totalWeight += weight;
                //         }
                //     }
                //     col /= totalWeight; // 정규화를 통한 최종 색상 계산 
                // }
                                
                // if(RestRegion){
                // //     // uv.x *= _Resolution_X * _DownscaleFactor;
                // //     // uv.x = floor(uv.x) / _Resolution_X / _DownscaleFactor;
                // //     // uv.y *= _Resolution_Y * _DownscaleFactor;
                // //     // uv.y = floor(uv.y) / _Resolution_Y / _DownscaleFactor;

                // // 정의된 영역에 따라 UV 좌표를 조정
                // float2 gridSize = float2(3840.0, 2160.0);
                // gridSize *= _GroupSize;
                // float2 gridPosition = floor(i.uv * gridSize) / gridSize;
                // float2 gridStep = 1.0/gridSize;
                                                
                // // // 해당 영역의 가장 왼쪽 위 픽셀 색상을 샘플링
                // // float4 color = tex2D(_MainTex, gridPosition);
                // // return color;
                
                // // 네 모서리 픽셀의 색상을 샘플링
                // float2 topLeft = gridPosition;
                // float2 topRight = gridPosition + float2(gridStep.x, 0);
                // float2 bottomLeft = gridPosition + float2(0, gridStep.y);
                // float2 bottomRight = gridPosition + gridStep;

                // float4 colorTopLeft = tex2D(_MainTex, topLeft);
                // float4 colorTopRight = tex2D(_MainTex, topRight);
                // float4 colorBottomLeft = tex2D(_MainTex, bottomLeft);
                // float4 colorBottomRight = tex2D(_MainTex, bottomRight);
                
                // // 평균 색상 계산
                // float4 averageColor = (colorTopLeft + colorTopRight + colorBottomLeft + colorBottomRight) / 4.0;
                
                // return averageColor;
                // }

                // if(FoveaRegion_Square)
                // {
                //     col = tex2D(_MainTex,uv);
                // }

                //col = tex2D(_MainTex, uv);  
                return col;
            }
            ENDCG
        }
    }
}
