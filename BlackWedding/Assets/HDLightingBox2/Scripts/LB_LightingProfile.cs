
// AliyerEdon@gmail.com/
// Lighting Box is an "paid" asset. Don't share it for free

using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[CreateAssetMenu(fileName = "Lighting Data", menuName = "Lighting Profile", order = 1)]
public class LB_LightingProfile : ScriptableObject {

	[Header("Camera")]
	public string mainCameraName = "Main Camera";

	public VolumeProfile volumeProfile;

	public string objectName = "LB_LightingProfile";
	[Header("Profiles")]
	//public PostProcessProfile postProcessingProfile;

	[Header("Snow")]
	public Texture2D snowAlbedo;
	public Texture2D snowNormal;
	public float snowIntensity = 0;

	[Header("Global")]
	public  LightingMode lightingMode = LightingMode.EnlightenRealtimeGI;
	public  float bakedResolution = 10f;
	public  LightSettings lightSettings = LightSettings.Mixed;
	public MyColorSpace colorSpace = MyColorSpace.Linear;

	[Header("Environment")]
	public  AmbientLight ambientLight = AmbientLight.HDRI;
    public SkyAmbientMode skyAmbientMode;
	public  Cubemap skyCube;
	public  float skyExposure;
	public  float hdrRotation;
	[ColorUsageAttribute(true,true)]
	public  Color skyTint;
	[ColorUsageAttribute(true,true)]
	public  Color groundColor;
	public  float tickness;
	// gradient sky
	[ColorUsageAttribute(true,true)]
	public  Color gradientTop = Color.blue;
	[ColorUsageAttribute(true,true)]
	public  Color gradientMiddle = new Color(0.3f,0.7f,1f);
	[ColorUsageAttribute(true,true)]
	public  Color gradientBottom = Color.white;
	public float gradientDiffusion = 1f;

	[Header("Sun")]
	public  Color sunColor = Color.white;
	public float sunIntensity = 2.1f;
	public float indirectIntensity = 1.43f;

	[Header("Fog")]
    public float baseHeight;
    public float fogAttenDistance;
    public float maxHeight;
    public float maxDistance;
    public Color fogTint;
    public FogColorMode fogColorMode;
    public bool fogVolumetric;
    public Color volumetricColor;
    public float vAnistropic;


    [Header("Bloom")]
	public float bIntensity = 0.73f;
	public float bThreshould = 0.5f;
	public Color bColor = Color.white;
	public Texture2D dirtTexture;
	public float dirtIntensity;
	public bool mobileOptimizedBloom = false;
	public bool bRotation;

	[Header("Indirect Controller")]
	public float indirectDiffuse = 0.3f;
	public float indirectSpecular = 0.3f;
    public float probeIntensity = 0.3f;
    
    [Header("Micro  Shadowing")]
	public bool microEnabled = false;
	public float microOpacity = 1f;

	[Header("AO")]
	public AOType aoType = AOType.High;
	public float aoRadius = 0.3f;
	public float aoIntensity = 1f;
	public bool temporal = false;
    public float directStrenth = 1f;
    public int aoQuality;
    public bool aoTemporal;
    public float ghostReducing;

    [Header("Other")]
	public LightProbeMode lightProbesMode;
	public bool automaticLightmap = false;
	public LightsShadow lightsShadow = LightsShadow.AllLights;

	[Header("Depth of Field")]
    public float dofFocusDistance = 10f;
    public DOFQuality dofQuality = DOFQuality.Low;
    public DepthOfFieldMode dofMode = DepthOfFieldMode.UsePhysicalCamera;

	[Header("Color settings")]
	public LB_ExposureMode lb_exposureMode = LB_ExposureMode.Fixed;
	public float compensation = 0;
	public float compensationFixed = 0;

	public float fixedExposure = 0;
	public float exposureIntensity = 1.43f;
	public float contrastValue = 30f;
	public float temp = 0;
	public ColorMode colorMode = ColorMode.ACES;
	public float saturation = 0;
	public float gamma = 0;
	public Color colorGamma = Color.black;
	public Color colorLift = Color.black;
	public Texture lut;

	[Header("Effects")]
	public float vignetteIntensity = 0.1f;
	public float CA_Intensity = 0.1f;

	[Header("Unity SSR")]
	public float minSmoothness = 0;
	public float edgeDistance = 0;
    public bool ssrReflectSky = true;
    public SSRQuality ssrQUality = SSRQuality.High;

	public float eyeKeyValue1  =   -4f;
    public float eyeKeyValue2 = 1f;
	public MeteringMode meteringMode;

	[Header("AA")]
	public AAMode aaMode;


	[Header("HD Render Pipeline")]
	// Sky box HDR
	CubemapParameter hdrSkyBox;

	[Header("HD Shadows")]
	public  int CascadeCount = 4;
	public float distance = 500f;
	public float split1 = 0.05f;
	public float split2 = 0.15f;
	public float split3 = 0.3f;

    [Header("Motion Blur")]
    public int blurIntensity = 10;
    public int blurMaxVelocity = 100;
    public MotionBlurQuality blurQuality = MotionBlurQuality.Medium;

    [Header("Screen Space GI")]
    public SSGIQuality ssgiQuality;

    [Header("Enabled Options")]
	public bool Ambient_Enabled = true;
	public bool Scene_Enabled = true;
	public bool Sun_Enabled = true;
	public bool VL_Enabled = false;
	public bool Fog_Enabled = false;
	public bool AutoFocus_Enabled = false;
	public bool DOF_Enabled = true;
	public bool Bloom_Enabled = false;
	public bool AA_Enabled = true;
	public bool AO_Enabled = false;
	public bool MotionBlur_Enabled = true;
	public bool Vignette_Enabled = true;
	public bool Chromattic_Enabled = true;
	public bool SSR_Enabled = false;
	public bool HD_Enabled = false;
	public bool Micro_Enabled = false;
    public bool SSGI_Enabled = true;

    public bool ambientState = false;
    public bool ssgiState = false;
    public bool sunState = false;
	public bool lightSettingsState = false;
	public bool cameraState = false;
	public bool profileState = false;
	public bool buildState = false;
	public bool fogState = false;
	public bool dofState = false;
	public bool autoFocusState =  false;
	public bool colorState = false;
	public bool bloomState = false;
	public bool aaState = false;
	public bool aoState = false;
	public bool motionBlurState = false;
	public bool vignetteState = false;
	public bool chromatticState = false;
	public bool ssrState;
	public bool hdState;
	public bool MicroState;
	public bool OptionsState = true;
	public bool LightingBoxState = true;
	public bool snowState = false;

}