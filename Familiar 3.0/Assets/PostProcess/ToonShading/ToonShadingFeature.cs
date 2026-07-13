using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public sealed class ToonShadingFeature : ScriptableRendererFeature
{
    public Shader shader;
    public RenderPassEvent passEvent = RenderPassEvent.BeforeRenderingPostProcessing;

    ToonShadingPass toonPass;
    Material material;

    public override void Create()
    {
        if (shader == null) return;
        material = CoreUtils.CreateEngineMaterial(shader);
        toonPass = new ToonShadingPass(material, passEvent);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material == null || toonPass == null) return;
        if (renderingData.cameraData.cameraType == CameraType.Preview) return;

        VolumeStack stack = VolumeManager.instance.stack;
        ToonShadingVolume comp = stack.GetComponent<ToonShadingVolume>();
        if (comp == null || !comp.IsActive()) return;

        toonPass.Setup(comp);
        renderer.EnqueuePass(toonPass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(material);
    }

    sealed class ToonShadingPass : ScriptableRenderPass
    {
        //Params that are editable form the inspector
        static readonly int s_LightTint = Shader.PropertyToID("lightTint");
        static readonly int s_DarkTint = Shader.PropertyToID("darkTint");
        static readonly int s_Threshold = Shader.PropertyToID("threshold");
        static readonly int s_Contrast = Shader.PropertyToID("contrast");
        static readonly int s_LightBlend = Shader.PropertyToID("lightBlendAmountst");
        static readonly int s_DarkBlend = Shader.PropertyToID("darkBlendAmount");
        static readonly int s_BlendIntensity = Shader.PropertyToID("blendIntensity");
        static readonly int s_UseCustomDepth = Shader.PropertyToID("useCustomDepth");

        const string k_PassName = "Toon Shading PP";

        readonly Material material;
        ToonShadingVolume postProcessVolume;

        public ToonShadingPass(Material otherMaterial, RenderPassEvent evet)
        {
            material = otherMaterial;
            renderPassEvent = evet;
            ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);
        }

        public void Setup(ToonShadingVolume volume) => postProcessVolume = volume;

        class PassData
        {
            public Material material;
            public TextureHandle source;
        }

        //Render callback func
        private static void callback(PassData data, RasterGraphContext context)
        {
            //Actual rendering command
            Blitter.BlitTexture(context.cmd, data.source, new Vector4(1, 1, 0, 0), data.material, 0);
        }

        //Tells unity what we need to do/use for the shader
        //actual render command is in the callback function above
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (material == null || postProcessVolume == null) return;

            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

            material.SetColor(s_LightTint, postProcessVolume.lightTint.value);
            material.SetColor(s_DarkTint, postProcessVolume.darkTint.value);
            material.SetFloat(s_Threshold, postProcessVolume.threshold.value);
            material.SetFloat(s_Contrast, postProcessVolume.contrast.value);
            material.SetFloat(s_LightBlend, postProcessVolume.lightBlendAmount.value);
            material.SetFloat(s_DarkBlend, postProcessVolume.darkBlendAmount.value);
            material.SetFloat(s_BlendIntensity, postProcessVolume.blendIntensity.value);
            material.SetFloat(s_UseCustomDepth, postProcessVolume.useCustomDepth.value ? 1f : 0f);

            TextureHandle source = resourceData.activeColorTexture;

            TextureDesc destDesc = renderGraph.GetTextureDesc(source);
            destDesc.name = "TempTexture";
            destDesc.clearBuffer = false;
            TextureHandle destination = renderGraph.CreateTexture(destDesc);

            //Main render pass
            using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>(k_PassName, out PassData passData))
            {
                passData.material = material;
                passData.source = source;

                builder.UseTexture(source, AccessFlags.Read);
                builder.SetRenderAttachment(destination, 0, AccessFlags.Write);

                if (resourceData.activeDepthTexture.IsValid())
                {
                    builder.UseTexture(resourceData.activeDepthTexture, AccessFlags.Read);
                }
                builder.SetRenderFunc<PassData>(callback);
            }
            resourceData.cameraColor = destination;
        }
    }
}