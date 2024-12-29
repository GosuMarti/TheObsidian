Shader "Custom/FresnelBlink"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        // Fresnel-related variables
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelStrength ("Fresnel Strength", Range(0,1)) = 0.5

        // Blink speed control
        _BlinkSpeed ("Blink Speed", Range(0.1, 10)) = 0.5
        _IsLightOn ("Is Light On", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Fresnel-related variables
        half _FresnelStrength;
        fixed4 _FresnelColor;
        half _BlinkSpeed;
        float _IsLightOn;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            // If the light is turned on, skip the Fresnel effect
            if (_IsLightOn < 0.5)
            {
                // Fresnel effect
                half fresnelFactor = 1.0 - dot(normalize(IN.viewDir), IN.worldNormal);
                fresnelFactor = pow(fresnelFactor, _FresnelStrength);

                // Make it blink based on time and blink speed
                half blink = abs(sin(_Time.y * (6.28318 / 5)));
                fresnelFactor *= blink;

                // Apply Fresnel color to emission channel
                o.Emission = _FresnelColor.rgb * fresnelFactor;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}