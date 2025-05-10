Shader "Hidden/VHSPostProcessEffect"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _VHSTex("VHS Texture", 2D) = "white" {}
    }
        SubShader
    {
        Pass
        {
            // Disable depth, since we don't need it for 2D
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform sampler2D _VHSTex;

            // Scanline variables
            float _yScanline;
            float _xScanline;

            // Random function for glitch effect
            float rand(float3 co)
            {
                return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
            }

            // Fragment shader
            fixed4 frag(v2f_img i) : COLOR
            {
                fixed4 vhs = tex2D(_VHSTex, i.uv);

            // Apply scanline effects on y and x axes
            float dx = 1 - abs(i.uv.y - _xScanline);
            float dy = 1 - abs(i.uv.y - _yScanline);

            dy = ((int)(dy * 15)) / 15.0; // Adjust the intensity of the effect
            i.uv.x += dy * 0.025 + rand(float3(dy, dy, dy)).r / 500.0;

            // Apply bleeding effect based on color channels
            float bleed = tex2D(_MainTex, i.uv + float2(0.01, 0)).r;
            bleed += tex2D(_MainTex, i.uv + float2(0.02, 0)).r;
            bleed += tex2D(_MainTex, i.uv + float2(0.01, 0.01)).r;
            bleed += tex2D(_MainTex, i.uv + float2(0.02, 0.02)).r;
            bleed /= 6.0;

            if (bleed > 0.1)
            {
                vhs += fixed4(bleed * _xScanline, 0, 0, 0);
            }

            // Add randomness to the video texture
            float x = ((int)(i.uv.x * 320)) / 320.0;
            float y = ((int)(i.uv.y * 240)) / 240.0;

            fixed4 c = tex2D(_MainTex, i.uv);
            c -= rand(float3(x, y, _xScanline)) * _xScanline / 5.0;

            // Return the combined result
            return c + vhs;
        }
        ENDCG
    }
    }
        Fallback off
}
