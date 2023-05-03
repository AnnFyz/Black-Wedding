// Use this script to get runtime access to the lighting box to controll effects
/// <summary>
/// example:
/// 
/// // Update bloom effect .
/// void Start ()
/// {
///   	GameObject.FindObjectOfType<LB_LightingBoxHelper> ().Update_Bloom (true, 1f, 0.5f, Color.white);
/// }
/// </summary>
using UnityEngine;   
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

#if UNITY_EDITOR
using UnityEditor;
#endif

#region Emum Types
public enum FogColorType
{
	SkyColor,CustomColor
}
public enum LB_ExposureMode
{
	Automatic,Fixed
}

public enum CameraMode
{
	Single,All,Custom
}
public enum WindowMode
{
	Part1,Part2,Part3,
	Finish
}
public enum AmbientLight
{
	HDRI,
	PhysicallyBased,
	None
}
public enum LightingMode
{
	FullyRealtime,
    EnlightenRealtimeGI,
	BakedCPU, BakedGPU,
    RealtimeGIandBakedGI_CPU,
    RealtimeGIandBakedGI_GPU
}
public enum LightSettings
{
	Default,
	Realtime,
	Mixed,
	Baked
}
public enum MyColorSpace
{
	Linear,
	Gamma
}

public enum CustomFog
{
	Exponential,
	Linear,
	Volumetric
}
public enum LightsShadow
{
	OnlyDirectional,
	AllLights,
	Off
}
public enum LightProbeMode
{
	Blend,
	Proxy
}

public enum DOFQuality
{
	Low,Medium,High
}

public enum SSGIQuality
{
    Low, Medium, High
}

public enum AOType
{
	Low,Medium,High
}

public enum ColorMode
{
	ACES,Neutral,LUT
}

public enum AAMode
{
	TAA,FXAA,SMAA
}

public enum MotionBlurQuality
{
    Low, Medium,High
}
public enum SSRQuality
{
    Low, Medium, High
}
#endregion

public class LB_LightingBoxHelper : MonoBehaviour {

	public LB_LightingProfile mainLightingProfile;
	public VolumeProfile volumeProfileMain;

	#region Runtime Update

	Light sunLight;
	Camera mainCamera;
	LB_LightingBoxHelper helper;

	void Start()
	{
		if (!mainCamera) {
			if (GameObject.Find (mainLightingProfile.mainCameraName))
				mainCamera = GameObject.Find (mainLightingProfile.mainCameraName).GetComponent<Camera> ();
			else
				mainCamera = GameObject.FindObjectOfType<Camera> ();
		}

		Update_SunRuntime (mainLightingProfile);
		UpdatePostEffects (mainLightingProfile);
		UpdateSettings (mainLightingProfile);
	}

	void UpdatePostEffects(LB_LightingProfile profile)
	{
		if(!helper)
			helper = GameObject.Find("LightingBox_Helper").GetComponent<LB_LightingBoxHelper> ();

		if (!profile)
			return;

		helper.UpdateProfiles (mainCamera/*, profile.postProcessingProfile*/,profile.volumeProfile);

		// MotionBlur
		if (profile.MotionBlur_Enabled)
            helper.Update_MotionBlur(true, profile.blurIntensity, profile.blurMaxVelocity
                , profile.blurQuality);
        else
			helper.Update_MotionBlur (false, profile.blurIntensity, profile.blurMaxVelocity
                , profile.blurQuality);
       
        // Vignette
        helper.Update_Vignette (profile.Vignette_Enabled,profile.vignetteIntensity);


		// _ChromaticAberration
		helper.Update_ChromaticAberration(profile.Chromattic_Enabled,profile.CA_Intensity);

		helper.Update_Bloom(profile.Bloom_Enabled,profile.bIntensity,profile.bThreshould,profile.bColor,profile.dirtTexture,profile.dirtIntensity,profile.mobileOptimizedBloom,profile.bRotation);


		// Depth of Field
		helper.Update_DOF(profile.DOF_Enabled,profile.dofFocusDistance,profile.dofQuality,
            profile.dofMode);
       
        // AO
        if (profile.AO_Enabled)
			helper.Update_AO(mainCamera,true,profile.aoType,profile.aoRadius,profile.aoIntensity,profile.directStrenth,profile.aoTemporal, profile.ghostReducing);
		else
			helper.Update_AO(mainCamera,false,profile.aoType,profile.aoRadius,profile.aoIntensity,profile.directStrenth, profile.aoTemporal, profile.ghostReducing);


		// Color Grading
		helper.Update_ColorGrading(profile.meteringMode, profile.colorMode,profile.exposureIntensity,profile.contrastValue,profile.temp,profile.eyeKeyValue1, profile.eyeKeyValue2, profile.saturation,profile.colorGamma,profile.colorLift,profile.gamma,profile.lut, profile.lb_exposureMode, profile.compensation, profile.compensationFixed, profile.fixedExposure);


		// Snow
		//helper.Update_Snow(profile.snowAlbedo, profile.snowNormal, profile.snowIntensity);

		// Screen Space Reflections
		//helper.Update_SSR(mainCamera, profile.SSR_Enabled,profile.minSmoothness, profile.edgeDistance, profile.ssrReflectSky, profile.ssrQUality);
       


    }

	void UpdateSettings(LB_LightingProfile profile)
	{
		// Sun Light Update
		if (sunLight) {
			sunLight.color = profile.sunColor;
			sunLight.intensity = profile.sunIntensity;
			sunLight.bounceIntensity = profile.indirectIntensity;
		} else {
			Update_SunRuntime (profile);
		}

		// Update Ambient
		helper.Update_Ambient (profile.Ambient_Enabled,profile.ambientLight,profile.skyAmbientMode, profile.skyCube, profile.skyExposure,profile.hdrRotation,profile.skyTint,profile.groundColor,profile.tickness
			,profile.gradientTop,profile.gradientMiddle,profile.gradientBottom,profile.gradientDiffusion);

		// Global Fog
		/*helper.Update_GlobalFog(profile.Fog_Enabled, profile.baseHeight, profile.fogAttenDistance, profile.maxHeight, profile.maxDistance, profile.fogTint, profile.fogColorMode, profile.fogVolumetric, profile.volumetricColor
            , profile.vAnistropic);*/

        
	}

	void Update_SunRuntime(LB_LightingProfile profile)
	{
		if (profile.Sun_Enabled) {
			if (!RenderSettings.sun) {
				Light[] lights = GameObject.FindObjectsOfType<Light> ();
				foreach (Light l in lights) {
					if (l.type == LightType.Directional) {
						sunLight = l;

						if (profile.sunColor != Color.clear)
							profile.sunColor = l.color;
						else
							profile.sunColor = Color.white;

						//sunLight.shadowNormalBias = 0.05f;  
						sunLight.color = profile.sunColor;
						if (sunLight.bounceIntensity == 1f)
							sunLight.bounceIntensity = profile.indirectIntensity;
					}
				}
			} else {		
				sunLight = RenderSettings.sun;

				if (profile.sunColor != Color.clear)
					profile.sunColor = sunLight.color;
				else
					profile.sunColor = Color.white;

				//	sunLight.shadowNormalBias = 0.05f;  
				sunLight.color = profile.sunColor;
				if (sunLight.bounceIntensity == 1f)
					sunLight.bounceIntensity = profile.indirectIntensity;
			}
		}


	}

	#endregion

	public void Update_MainProfile(LB_LightingProfile profile,VolumeProfile volumeProfile)
	{
		if(profile)
			mainLightingProfile = profile;

		if(volumeProfile)
			volumeProfileMain = volumeProfile;             
	}

	public void UpdateProfiles(Camera mainCamera/*,PostProcessProfile profile*/,VolumeProfile volumeProfile)
	{
		if (!volumeProfile)
			return;
      
        if (GameObject.Find("Sky and Fog Volume"))
            GameObject.DestroyImmediate(GameObject.Find("Sky and Fog Volume"));

        if (!GameObject.Find ("Fog/Sky/HD Volume")) {            
            GameObject fogVolume = new GameObject ();
			fogVolume.name = "Fog/Sky/HD Volume";
			fogVolume.AddComponent<Volume> ();
			fogVolume.GetComponent<Volume> ().isGlobal = true;
			fogVolume.GetComponent<Volume> ().priority = 1f;
			if (volumeProfile)
				fogVolume.GetComponent<Volume> ().sharedProfile = volumeProfile;
        } else {
            if (volumeProfile)
            {
                GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile = volumeProfile;
            }
           

        }
    }

	public void Update_MotionBlur(bool enabled,int blurIntensity,int blurMaxVelocity,
       MotionBlurQuality blurQuality )
	{
       
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
            TryGet<UnityEngine.Rendering.HighDefinition.MotionBlur>(out var mb);
     
        mb.active = enabled;

        mb.intensity.overrideState = true;
        mb.intensity.value = blurIntensity;

        mb.maximumVelocity.overrideState = true;
        mb.maximumVelocity.value = blurMaxVelocity;

        mb.quality.overrideState = true;
        if(blurQuality == MotionBlurQuality.Low)
          mb.quality.value = 0;
        if (blurQuality == MotionBlurQuality.Medium)
            mb.quality.value = 1;
        if (blurQuality == MotionBlurQuality.High)
            mb.quality.value = 2;

    }

    public void Update_Vignette(bool enabled, float intensity)
	{
       
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
            TryGet<UnityEngine.Rendering.HighDefinition.Vignette>(out var vi);
       
        vi.active = enabled;

		vi.intensity.overrideState = true;
		vi.intensity.value = intensity;

		vi.smoothness.overrideState = true;
		vi.smoothness.value = 1f;

		vi.roundness.overrideState = true;
		vi.roundness.value = 1f;

	}

	public void Update_ChromaticAberration(bool enabled,float intensity)
	{
      
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.ChromaticAberration>(out var ca);
        

        ca.intensity.overrideState = true;
		ca.intensity.value = intensity;

        ca.active = enabled;

    }

    public void Update_Bloom(bool enabled,float intensity,float threshold,Color color,Texture2D dirtTexture,float dirtIntensity,bool mobileOptimized,bool bRotation)
	{
		if(enabled)
		{
            
            GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.Bloom>(out var b);

            b.intensity.overrideState = true;
			b.intensity.value = intensity;
			b.threshold.overrideState = true;
			b.threshold.value = threshold;
            b.tint.overrideState = true;
           b.tint.value = color;

            b.anamorphic.overrideState = true;
            	b.anamorphic.value = bRotation;

            b.dirtTexture.overrideState = true;
			b.dirtTexture.value = dirtTexture;

			b.dirtIntensity.overrideState = true;
			b.dirtIntensity.value = dirtIntensity;

            b.active = enabled;

        }
        else
		{
          
            GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
               TryGet<UnityEngine.Rendering.HighDefinition.Bloom>(out var b);

            b.intensity.overrideState = true;
			b.intensity.value = intensity;
			b.threshold.overrideState = true;
			b.threshold.value = threshold;
			b.tint.overrideState = true;
			b.tint.value = color;

			b.dirtTexture.overrideState = true;
			b.dirtTexture.value = dirtTexture;

			b.dirtIntensity.overrideState = true;
			b.dirtIntensity.value = dirtIntensity;

			b.anamorphic.overrideState = true;
			b.anamorphic.value = bRotation;

            b.active = enabled;
        }
    }

	public void Update_DOF(bool dofEnabled,float dofFocusDistance,DOFQuality dofQuality,
        DepthOfFieldMode dofMode)
	{
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.DepthOfField>(out var dof);

		dof.active = dofEnabled;

        dof.focusDistance.overrideState = true;
        dof.focusDistance.value = dofFocusDistance;

        if (dofMode == DepthOfFieldMode.UsePhysicalCamera)
        {
            dof.focusMode.overrideState = true;
            dof.focusMode.value = DepthOfFieldMode.UsePhysicalCamera;
        }

        dof.quality.overrideState = true;
        if(dofQuality == DOFQuality.Low)
            dof.quality.value = 0;
        if (dofQuality == DOFQuality.Medium)
            dof.quality.value = 1;
        if (dofQuality == DOFQuality.High)
            dof.quality.value = 2;
        /*dof.aperture.overrideState = true;
               dof.aperture.value = 0.9f;

               dof.focalLength.overrideState = true;
               dof.focalLength.value = 2f;

               dof.focusDistance.overrideState = true;
               dof.focusDistance.value = dofDistance2;

               dof.kernelSize.overrideState = true;
               dof.kernelSize.value = KernelSize.Medium;
               */
    }

    public void Update_AA(Camera mainCamera ,AAMode aaMode, bool  enabled)
	{
		if (enabled) {
			if (aaMode == AAMode.TAA) {
                mainCamera.GetComponent<HDAdditionalCameraData>().antialiasing = HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing;
            }
			if (aaMode == AAMode.FXAA) {
                mainCamera.GetComponent<HDAdditionalCameraData>().antialiasing = HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing;
            }
            if (aaMode == AAMode.SMAA) {
                mainCamera.GetComponent<HDAdditionalCameraData>().antialiasing = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            }
        } else {
            mainCamera.GetComponent<HDAdditionalCameraData>().antialiasing = HDAdditionalCameraData.AntialiasingMode.None;

        }
    }

	public void Update_Snow(Texture2D albedo, Texture2D normal, float intensity)
	{
		/*Shader.SetGlobalTexture("_SnowAlbedo", albedo);
		Shader.SetGlobalTexture("SnowAlbedo", albedo);
		Shader.SetGlobalTexture("_SnowNormal", normal);
		Shader.SetGlobalTexture("SnowNormal", normal);
		Shader.SetGlobalFloat("_SnowIntensity", intensity);
		Shader.SetGlobalFloat("SnowIntensity", intensity);*/
	}

	public void Update_AO(Camera mainCamera ,bool enabled,AOType aoType,float aoRadius,float aoIntensity,float directStrenth,bool temporal, float ghostReducing)
	{

       
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.AmbientOcclusion>(out var ao);

        if (enabled) {
            ao.active = true;

            if (aoType == AOType.Low)
            {
                ao.quality.overrideState = true;
                ao.quality.value = 0;
            }
            if (aoType == AOType.Medium)
            {
                ao.quality.overrideState = true;
                ao.quality.value = 1;
            }
            if (aoType == AOType.High)
            {
                ao.quality.overrideState = true;
                ao.quality.value = 2;
            }

            ao.radius.overrideState = true;
			ao.radius.value = aoRadius;

            ao.intensity.overrideState = true;
			ao.intensity.value = aoIntensity;

            ao.directLightingStrength.overrideState = true;
            ao.directLightingStrength.value = directStrenth;

            ao.temporalAccumulation.overrideState = true;
            ao.temporalAccumulation.value = temporal;

            ao.ghostingReduction.overrideState = true;
            ao.ghostingReduction.value = ghostReducing;
        
		} else {
			ao.active = false;
		}
	}

	public void Update_ColorGrading(MeteringMode meteringMode, ColorMode colorMode,float exposureIntensity,float contrastValue,float temp, float eyeKeyValue1, float eyeKeyValue2
        , float saturation,Color colorGamma,Color colorLift,float gamma,Texture lut, LB_ExposureMode exposureModeLB,float compensation,float compensationFixed, float fixedExposure)
	{
		
        #region Tonemmaper
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
          TryGet<UnityEngine.Rendering.HighDefinition.Tonemapping>(out var toneMap);

        if (colorMode == ColorMode.ACES)
        {
            toneMap.active = true;
            toneMap.mode.overrideState = true;
            toneMap.mode.value = TonemappingMode.ACES;

        }
        if (colorMode == ColorMode.Neutral)
        {
            toneMap.active = true;
            toneMap.mode.overrideState = true;
            toneMap.mode.value = TonemappingMode.Neutral;

        }
        #endregion

        #region ColorAdjustments

        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
        TryGet<UnityEngine.Rendering.HighDefinition.ColorAdjustments>(out var colorAdj);
        
        colorAdj.saturation.overrideState = true;
        colorAdj.saturation.value = saturation * 100;


        colorAdj.postExposure.overrideState = true;
        colorAdj.postExposure.value = exposureIntensity;

        colorAdj.contrast.overrideState = true;
        colorAdj.contrast.value = contrastValue * 100;
        colorAdj.active = true;

        #endregion

        #region WhiteBalance

        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
        TryGet<UnityEngine.Rendering.HighDefinition.WhiteBalance>(out var wBalance);

        wBalance.temperature.overrideState = true;
        wBalance.temperature.value = temp;
        wBalance.active = true;
        #endregion

        #region LiftGammaGain

        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
             TryGet<UnityEngine.Rendering.HighDefinition.LiftGammaGain>(out var gammaLiftGain);

        if (colorMode == ColorMode.LUT) {
            toneMap.active = true;
            toneMap.mode.value = TonemappingMode.External;

            if (lut != null) {
                toneMap.lutTexture.overrideState = true;
                toneMap.lutTexture.value = lut;
			}
		} else {

            gammaLiftGain.lift.overrideState = true;
            gammaLiftGain.lift.value = new Vector4(colorLift.r, colorLift.g, colorLift.b, 0);

            gammaLiftGain.gamma.overrideState = true;
            gammaLiftGain.gamma.value = new Vector4(colorGamma.r, colorGamma.g, colorGamma.b, gamma);

            gammaLiftGain.gain.overrideState = false;
            gammaLiftGain.gain.value = new Vector4(gammaLiftGain.gain.value.x, gammaLiftGain.gain.value.y, gammaLiftGain.gain.value.z, 0);

            #endregion

            #region Expoture Automattic / Fixed

            GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
             TryGet<UnityEngine.Rendering.HighDefinition.Exposure>(out var autoExpoture);

			if (exposureModeLB == LB_ExposureMode.Automatic)
			{
				autoExpoture.active = true;

				autoExpoture.mode.overrideState = true;
				autoExpoture.mode.value = ExposureMode.Automatic;

				autoExpoture.compensation.overrideState = true;
				autoExpoture.compensation.value = compensation;

				autoExpoture.limitMin.overrideState = true;
				autoExpoture.limitMin.value = eyeKeyValue1;

				autoExpoture.limitMax.overrideState = true;
				autoExpoture.limitMax.value = eyeKeyValue2;

				autoExpoture.meteringMode.overrideState = true;
				autoExpoture.meteringMode.value = meteringMode;
			}
			if (exposureModeLB == LB_ExposureMode.Fixed)
			{
				autoExpoture.active = true;

				autoExpoture.mode.overrideState = true;
				autoExpoture.mode.value = ExposureMode.Fixed;

				autoExpoture.fixedExposure.overrideState = true;
				autoExpoture.fixedExposure.value = fixedExposure;

				autoExpoture.compensation.overrideState = true;
				autoExpoture.compensation.value = compensationFixed;
			}
			#endregion


		}
    }
   
    public void Update_SSR(Camera mainCamera ,bool enabled,float minSmoothness, float edgeDistance
        , bool ssrReflectSky, SSRQuality ssrQUality)
	{
		/*
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
    TryGet<UnityEngine.Rendering.HighDefinition.ScreenSpaceReflection>(out var ssr);

        ssr.enabled.overrideState = true;
		ssr.enabled.value = enabled;

		ssr.minSmoothness = minSmoothness;

        ssr.screenFadeDistance.overrideState = true;
        ssr.screenFadeDistance.value = edgeDistance;

        ssr.reflectSky.overrideState = true;
		ssr.reflectSky.value = ssrReflectSky;

		ssr.quality.overrideState = true;
        if(ssrQUality == SSRQuality.Low)
		    ssr.quality.value = 0;
        if (ssrQUality == SSRQuality.Medium)
            ssr.quality.value = 1;
        if (ssrQUality == SSRQuality.High)
            ssr.quality.value = 2;*/
    }

    public void Update_SSGI(Camera mainCamera, bool enabled, SSGIQuality ssgiQuality)
    {

        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
    TryGet<UnityEngine.Rendering.HighDefinition.GlobalIllumination>(out var ssgi);

        ssgi.active = enabled;

        ssgi.quality.overrideState = true;
        ssgi.enable.overrideState = true;
        ssgi.enable.value = enabled;

        if (ssgiQuality == SSGIQuality.Low)
           ssgi.quality.value = 0;
        if (ssgiQuality == SSGIQuality.Medium)
            ssgi.quality.value = 1;
        if (ssgiQuality == SSGIQuality.High)
            ssgi.quality.value = 2;
    }


    public void Update_MicroShadowing(bool enabled, float opacity)
	{
		
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
    TryGet<UnityEngine.Rendering.HighDefinition.MicroShadowing>(out var mShadow);

        mShadow.enable.overrideState = true;
		mShadow.opacity.overrideState = true;
		mShadow.enable.value  = enabled;
		mShadow.opacity.value =  opacity;

	}
		
	public void Update_LightingMode(bool enabled, LightingMode lightingMode,float indirectDiffuse, float  indirectSpecular,float probeIntensity)
	{
		if (enabled) {
			#if UNITY_EDITOR
			if (lightingMode == LightingMode.EnlightenRealtimeGI) {
				Lightmapping.realtimeGI = true;
				Lightmapping.bakedGI = false;
				LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.Enlighten;
			}
			if (lightingMode == LightingMode.BakedCPU) {
				Lightmapping.realtimeGI = false;
				Lightmapping.bakedGI = true;
				LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveCPU;
			}
            if (lightingMode == LightingMode.BakedGPU)
            {
                Lightmapping.realtimeGI = false;
                Lightmapping.bakedGI = true;
                LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
            }
            if (lightingMode == LightingMode.FullyRealtime) {
				Lightmapping.realtimeGI = false;
				Lightmapping.bakedGI = false;
			}
			if (lightingMode == LightingMode.RealtimeGIandBakedGI_CPU) {
				Lightmapping.realtimeGI = true;
				Lightmapping.bakedGI = true;
				LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveCPU;
			}
            if (lightingMode == LightingMode.RealtimeGIandBakedGI_GPU)
            {
                Lightmapping.realtimeGI = true;
                Lightmapping.bakedGI = true;
                LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
            }
            
#endif

            GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
        TryGet<UnityEngine.Rendering.HighDefinition.IndirectLightingController>(out var inController);


            inController.indirectDiffuseLightingMultiplier.overrideState = true;
			inController.reflectionLightingMultiplier.overrideState = true;
            inController.reflectionProbeIntensityMultiplier.overrideState = true;
            inController.indirectDiffuseLightingMultiplier.value  = indirectDiffuse;
			inController.reflectionLightingMultiplier.value =  indirectSpecular;
            inController.reflectionProbeIntensityMultiplier.value = probeIntensity;

        }
    }

	public void Update_Shadows(LightsShadow lightsShadow)
	{

		if (lightsShadow == LightsShadow.OnlyDirectional)
		{

			Light[] lights = GameObject.FindObjectsOfType<Light>();

			foreach (Light l in lights)
			{
				if (l.type == LightType.Directional)
					l.GetComponent<HDAdditionalLightData>().EnableShadows(true);

				if (l.type == LightType.Spot || l.type == LightType.Area
					|| l.type == LightType.Disc || l.type == LightType.Point
					|| l.type == LightType.Rectangle)
					l.GetComponent<HDAdditionalLightData>().EnableShadows(false);
			}
		}

		if (lightsShadow == LightsShadow.AllLights)
		{

			Light[] lights = GameObject.FindObjectsOfType<Light>();

			foreach (Light l in lights)
			{
				if (l.type == LightType.Directional)
					l.GetComponent<HDAdditionalLightData>().EnableShadows(true);

				if (l.type == LightType.Spot || l.type == LightType.Area
					|| l.type == LightType.Disc || l.type == LightType.Point
					|| l.type == LightType.Rectangle)
					l.GetComponent<HDAdditionalLightData>().EnableShadows(true);
			}
		}

		if (lightsShadow == LightsShadow.Off)
		{

			Light[] lights = GameObject.FindObjectsOfType<Light>();

			foreach (Light l in lights)
			{
				if (l.type == LightType.Directional)
					l.GetComponent<HDAdditionalLightData>().EnableShadows(false);

				if (l.type == LightType.Spot || l.type == LightType.Area
					|| l.type == LightType.Disc || l.type == LightType.Point
					|| l.type == LightType.Rectangle)
					l.GetComponent<HDAdditionalLightData>().EnableShadows(false);
			}
		}

	}


	public void Update_HDSHadows(bool enabled ,int cascadeCount,float distance,float split1,float split2,float split3)
	{
		if(enabled)
		{
			
            GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
          TryGet<UnityEngine.Rendering.HighDefinition.HDShadowSettings>(out var hdShadows);


            hdShadows.active = enabled;

            hdShadows.cascadeShadowSplitCount.overrideState = true;
			hdShadows.cascadeShadowSplitCount.value  =cascadeCount;

			hdShadows.maxShadowDistance.overrideState = true;
			hdShadows.maxShadowDistance.value  =  distance;

			hdShadows.cascadeShadowSplit0.overrideState = true;
			hdShadows.cascadeShadowSplit0.value  = split1;

			hdShadows.cascadeShadowSplit1.overrideState = true;
			hdShadows.cascadeShadowSplit1.value =  split2;

			hdShadows.cascadeShadowSplit2.overrideState = true;
			hdShadows.cascadeShadowSplit2.value =  split3;
		}
	}

	public void Update_Ambient(bool enabled,AmbientLight ambientMode,SkyAmbientMode skyAmbientMode, Cubemap skyCube,float skyExposure,float hdrRotation,
		Color skyTint,Color groundColor,float tickness,Color gradientTop,Color gradientMiddle,Color gradientBottom,float gradientDiffusion)
	{
		if (enabled)
		{
			
            GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
          TryGet<UnityEngine.Rendering.HighDefinition.VisualEnvironment>(out var enviro);

            enviro.skyAmbientMode.overrideState = true;
            enviro.skyAmbientMode.value = skyAmbientMode;

            if (ambientMode == AmbientLight.HDRI) {

				enviro.skyType.overrideState = true;
				enviro.skyType.value = (int)SkyType.HDRI;

				try{
				 GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
          TryGet<UnityEngine.Rendering.HighDefinition.HDRISky>(out var hdrSkyBox);

                    hdrSkyBox.exposure.overrideState = true;
				hdrSkyBox.exposure.value = skyExposure;

				hdrSkyBox.hdriSky.overrideState = true;
				hdrSkyBox.hdriSky.value = skyCube;
				
				hdrSkyBox.rotation.overrideState = true;
					hdrSkyBox.rotation.value = hdrRotation;
				}
				catch{}				
			}
			/*if (ambientMode == AmbientLight.Procedural) {
				enviro.skyType.overrideState = true;
				enviro.skyType.value = (int)SkyType.Procedural;

                GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
          TryGet<UnityEngine.Rendering.HighDefinition.ProceduralSky>(out var pSky);

                pSky.skyTint.overrideState = true;
				pSky.skyTint.value = skyTint;

				pSky.groundColor.overrideState = true;
				pSky.groundColor.value = groundColor;

				pSky.atmosphereThickness.overrideState = true;
				pSky.atmosphereThickness.value = tickness;

				pSky.exposure.overrideState = true;
				pSky.exposure.value = skyExposure;
			}*/
			/*if (ambientMode == AmbientLight.Gradient) {

				enviro.skyType.overrideState = true;
				enviro.skyType.value = (int)SkyType.Gradient;

                GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
          TryGet<UnityEngine.Rendering.HighDefinition.GradientSky>(out var gSky);

                gSky.top.overrideState = true;
				gSky.top.value = gradientTop;

				gSky.middle.overrideState = true;
				gSky.middle.value = gradientMiddle;

				gSky.bottom.overrideState = true;
				gSky.bottom.value =  gradientBottom;

				gSky.gradientDiffusion.overrideState = true;
				gSky.gradientDiffusion.value = gradientDiffusion;
			}*/
			if (ambientMode == AmbientLight.None) {



				enviro.skyType.overrideState = true;
				enviro.skyType.value = 0;

			}
            if (ambientMode == AmbientLight.PhysicallyBased)
            {
                enviro.skyType.overrideState = true;
                enviro.skyType.value = (int)SkyType.PhysicallyBased;

                GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
          TryGet<UnityEngine.Rendering.HighDefinition.PhysicallyBasedSky>(out var pbSky);

                pbSky.active = true;
                pbSky.exposure.overrideState = true;
                pbSky.exposure.value = skyExposure;
            }


        }
    }

	#if UNITY_EDITOR
	public void Update_LightSettings(bool enabled, LightSettings lightSettings)
	{
		if(enabled)
		{
			if (lightSettings == LightSettings.Baked) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					SerializedObject serialLightSource = new SerializedObject(l);
					SerializedProperty SerialProperty  = serialLightSource.FindProperty("m_Lightmapping");
					SerialProperty.intValue = 2;
					serialLightSource.ApplyModifiedProperties ();
				}
			} 
			if (lightSettings == LightSettings.Realtime) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					SerializedObject serialLightSource = new SerializedObject(l);
					SerializedProperty SerialProperty  = serialLightSource.FindProperty("m_Lightmapping");
					SerialProperty.intValue = 4;
					serialLightSource.ApplyModifiedProperties ();
				}
			}
			if (lightSettings == LightSettings.Mixed) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					SerializedObject serialLightSource = new SerializedObject(l);
					SerializedProperty SerialProperty  = serialLightSource.FindProperty("m_Lightmapping");
					SerialProperty.intValue = 1;
					serialLightSource.ApplyModifiedProperties ();
				}

			}
		}
	}

	public void Update_AutoMode(bool enabled)
	{
		if(enabled)
			Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
		else
			Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;

        
	}

    public void Update_LightProbes(bool enabled, LightProbeMode lightProbesMode)
	{
		if (enabled) {
			if (lightProbesMode == LightProbeMode.Blend) {

				MeshRenderer[] renderers = GameObject.FindObjectsOfType<MeshRenderer> ();

				foreach (MeshRenderer mr in renderers) {
					if (!mr.gameObject.isStatic) {
						if (mr.gameObject.GetComponent<LightProbeProxyVolume> ()) {
							if (Application.isPlaying)
								Destroy (mr.gameObject.GetComponent<LightProbeProxyVolume> ());
							else
								DestroyImmediate (mr.gameObject.GetComponent<LightProbeProxyVolume> ());
						}
						mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
					}
				}
			}
			if (lightProbesMode == LightProbeMode.Proxy) {

				MeshRenderer[] renderers = GameObject.FindObjectsOfType<MeshRenderer> ();

				foreach (MeshRenderer mr in renderers) {

					if (!mr.gameObject.isStatic) {
						if (!mr.gameObject.GetComponent<LightProbeProxyVolume> ())
							mr.gameObject.AddComponent<LightProbeProxyVolume> ();
						mr.gameObject.GetComponent<LightProbeProxyVolume> ().resolutionMode = LightProbeProxyVolume.ResolutionMode.Custom;
						mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.UseProxyVolume;
					}
				}
			}
		}
	}

	#endif

	public void Update_GlobalFog(bool fogEnabled,
		float baseHeight,float fogAttenDistance,float maxHeight,float maxDistance  ,Color fogTint,FogColorMode colorMode,bool fogVolumetric
		,Color volumetricColor,float vAnistropic)
	{
		/*
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.Fog>(out var vFog);

        vFog.active = fogEnabled;
        vFog.enabled.overrideState = true;
        vFog.enabled.value = fogEnabled;

        vFog.baseHeight.overrideState = true;
        vFog.baseHeight.value = baseHeight;

        vFog.meanFreePath.overrideState = true;
        vFog.meanFreePath.value = fogAttenDistance;

        vFog.maxFogDistance.overrideState = true;
        vFog.maxFogDistance.value = maxDistance;
        vFog.maximumHeight.overrideState = true;
        vFog.maximumHeight.value = maxHeight;
        vFog.tint.overrideState = true;
        vFog.tint.value = fogTint;

        vFog.anisotropy.overrideState = true;
        vFog.anisotropy.value = fogAttenDistance;
        vFog.colorMode.overrideState = true;
        vFog.colorMode.value = FogColorMode.SkyColor;
        vFog.enableVolumetricFog.overrideState = true;
        vFog.enableVolumetricFog.value = fogVolumetric;
        vFog.albedo.overrideState = true;
        vFog.albedo.value = volumetricColor;
        vFog.anisotropy.overrideState = true;
        vFog.anisotropy.value = 0;
		*/
    }

    public void Update_Sun(bool enabled,Light sunLight,Color sunColor,float indirectIntensity)
	{
		if (enabled) {
			if (!RenderSettings.sun) {
				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					if (l.type == LightType.Directional) {
						sunLight = l;

						if (sunColor != Color.clear)
							sunColor = sunLight.color;
						else
							sunColor = Color.white;

						//	sunLight.shadowNormalBias = 0.05f;  
						sunLight.color = sunColor;
						if (sunLight.bounceIntensity == 1f)
							sunLight.bounceIntensity = indirectIntensity;
					}
				}
			} else {
				sunLight = RenderSettings.sun;

				if (sunColor != Color.clear)
					sunColor = sunLight.color;
				else
					sunColor = Color.white;

				//sunLight.shadowNormalBias = 0.05f;  
				sunLight.color = sunColor;
				if (sunLight.bounceIntensity == 1f)
					sunLight.bounceIntensity = indirectIntensity;
			}
		}
	}

	bool effectsIsOn = true;

	public void Toggle_Effects()
	{
		effectsIsOn = !effectsIsOn;
		GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().enabled = effectsIsOn;
	   /* 
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.Bloom>(out var b);
        b.active = effectsIsOn;
        
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.MotionBlur>(out var mb);
        mb.active = effectsIsOn;
        
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.ChromaticAberration>(out var ca);
        ca.active = effectsIsOn;
        
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.Fog>(out var fFog);
        fFog.active = effectsIsOn;
        fFog.enabled.value = effectsIsOn;

        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.HDShadowSettings>(out var hdSH);
        hdSH.active = effectsIsOn;
        
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.MicroShadowing>(out var mSh);
        mSh.active = effectsIsOn;
        mSh.enable.value = effectsIsOn;

        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.ScreenSpaceReflection>(out var ssR);
        ssR.active = effectsIsOn;
        ssR.enabled.value = effectsIsOn;

        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
           TryGet<UnityEngine.Rendering.HighDefinition.AmbientOcclusion>(out var aO);
        aO.active = effectsIsOn;
        
        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
          TryGet<UnityEngine.Rendering.HighDefinition.Vignette>(out var vI);
        vI.active = effectsIsOn;

        GameObject.Find("Fog/Sky/HD Volume").GetComponent<Volume>().sharedProfile.
         TryGet<UnityEngine.Rendering.HighDefinition.GlobalIllumination>(out var ssgi);
        ssgi.active = effectsIsOn;
        ssgi.enable.value = effectsIsOn;*/
	}


}
