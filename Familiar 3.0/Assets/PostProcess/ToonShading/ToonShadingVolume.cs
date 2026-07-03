using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("Custom/Toon Shading")]
public sealed class ToonShadingVolume : VolumeComponent, IPostProcessComponent
{
    // UE5 post process volume params
    public ColorParameter lightTint = new ColorParameter(Color.white, true, false, true);
    public ColorParameter darkTint  = new ColorParameter(new Color(0.281f, 0.276f, 0.291f), true, false, true);
    public ClampedFloatParameter threshold = new ClampedFloatParameter(0.5f, 0f, 1f);
    public ClampedFloatParameter contrast  = new ClampedFloatParameter(1f, 0f, 10f);
    public ClampedFloatParameter lightBlendAmount = new ClampedFloatParameter(1f, 0f, 4f);
    public ClampedFloatParameter darkBlendAmount  = new ClampedFloatParameter(1f, 0f, 4f);
    public ClampedFloatParameter blendIntensity = new ClampedFloatParameter(1f, 0f, 4f);
    public BoolParameter useCustomDepth = new BoolParameter(false);

    public bool IsActive() => active;
    public bool IsTileCompatible() => false;
}
