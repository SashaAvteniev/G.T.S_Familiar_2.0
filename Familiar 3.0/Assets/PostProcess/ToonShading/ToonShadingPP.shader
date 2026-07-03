Shader "Custom/PP_ToonShading"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    TEXTURE2D(_CameraColorTexture);
    SAMPLER(sampler_CameraColorTexture);

    TEXTURE2D(_CameraOpaqueTexture);
    SAMPLER(sampler_CameraOpaqueTexture);

    TEXTURE2D(_CameraDepthTexture);
    SAMPLER(sampler_CameraDepthTexture);

    TEXTURE2D(_CustomDepthTexture);
    SAMPLER(sampler_CustomDepthTexture);

    float4 lightTint;       
    float4 darkTint;        
    float  threshold;       
    float  contrast;        
    float  lightBlendAmountst;
    float  darkBlendAmount; 
    float  blendIntensity;    
    float  useCustomDepth;  

    // UE5 destaturation values
    static const float3 LUM = float3(0.212639, 0.715169, 0.072192);
    float ToonLuminance(float3 c) { return dot(c, LUM); }

    //UE5 contrast function (cheap ver)
    float CheapContrast(float x, float contrast)
    {
        return saturate((x - 0.5h) * (contrast + 1.0h) + 0.5h);
    }

    // Pixel shader (fragment)
    half4 Pixel(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;

        // Post process input 0 from UE
        half4 sceneColor = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv);

        // Scene base color
        half4 baseColor = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv);

        float lumScene = ToonLuminance(sceneColor.rgb);
        float lumBase  = max(ToonLuminance(baseColor.rgb), 0.0001); //This way no /0
        float lightRatio = saturate(lumScene / lumBase);

        float contrastedRatio = CheapContrast(lightRatio, contrast);

        // In-light color
        float4 lightTintRGBA = float4(lightTint.rgb, 0.0);
        half3  litColor     = (baseColor.rgb + lightBlendAmountst * sceneColor.rgb)
                               * lightTintRGBA.rgb;

        // Shadow color
        float4 darkTintRGBA  = float4(darkTint.rgb, 0.0);
        half3  darkcolor    = (baseColor.rgb + darkBlendAmount * sceneColor.rgb)
                               * darkTintRGBA.rgb;

        half3 tooncolor = lerp(darkcolor, litColor, contrastedRatio);
        half3 blended = tooncolor * (baseColor.rgb * blendIntensity);

        // Future proffing that allows us to change mark specific assets to only have the shader affect if need be
        float sceneDepth  = SAMPLE_TEXTURE2D(_CameraDepthTexture,
                                              sampler_CameraDepthTexture, uv).r;
        float customDepth = SAMPLE_TEXTURE2D(_CustomDepthTexture,
                                              sampler_CustomDepthTexture, uv).r;

        float depthMask = step(customDepth, sceneDepth);   // 1 if scene is behind custom object

        // use scene color if outside cusom depth
        half3 maskedResult = lerp(blended, sceneColor.rgb, useCustomDepth * depthMask);

        return half4(maskedResult, sceneColor.a);
    }

    ENDHLSL

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off ZTest Always Cull Off Blend Off

        Pass
        {
            Name "ToonShadingPP"
            HLSLPROGRAM
            #pragma vertex   Vert      // in Blit.hlsl
            #pragma fragment Pixel
            ENDHLSL
        }
    }
}
