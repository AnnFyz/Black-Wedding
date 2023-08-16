
// AliyerEdon@gmail.com/
// Lighting Box is an "paid" asset. Don't share it for free

#if UNITY_EDITOR
using UnityEngine;   
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;   
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[ExecuteInEditMode]
public class LB_LightingBox : EditorWindow
{
#region Variables

	public WindowMode winMode = WindowMode.Part1;
	public LB_LightingBoxHelper helper;
	//public bool webGL_Mobile = false;

	// AA
	public AAMode aaMode;

	// AO
	public AOType aoType;
	public float aoRadius;
	public float aoIntensity = 1f;
	public int aoQuality ;
    public bool temporal = false;
    public float directStrenth = 1f;
    public bool aoTemporal;
    public float ghostReducing;

    // Bloom
    public float bIntensity = 1f;
	public float bThreshould = 0.5f;
	public Color bColor = Color.white;
	public Texture2D dirtTexture;
	public float dirtIntensity;
	public bool mobileOptimizedBloom;
	public bool bRotation;

	// indirect 
	public float indirectDiffuse = 0.3f;
	public float indirectSpecular = 0.3f;
    public float probeIntensity = 0.3f;
    
    // Micro  Shadowing   
    public bool microEnabled = false;
	public float microOpacity = 1f;

	public bool visualize;

    // Color settings
    public LB_ExposureMode lb_exposureMode = LB_ExposureMode.Fixed;
    public float compensation = 0;
   	public float compensationFixed = 0;
 public float fixedExposure = 0;
    public MeteringMode meteringMode;
    public float exposureIntensity = 1.43f;

	public float contrastValue = 30f;
	public float temp = 0;
	public float eyeKeyValue1 = -4f;
    public float eyeKeyValue2 = 1f;
    public ColorMode colorMode;
	public float gamma = 0;
	public Color colorGamma = Color.black;
	public Color colorLift = Color.black;
	public float saturation = 0;
	public Texture lut;

    // SSR
    public float minSmoothness = 0;
    public float edgeDistance = 0;
    public bool ssrReflectSky = true;
    public SSRQuality ssrQUality = SSRQuality.High;

    // Screen Space GI
    public SSGIQuality ssgiQuality;

    // Vignette
    public float vignetteIntensity = 0.1f;
	public float CA_Intensity = 0.1f;
	public bool mobileOptimizedChromattic;

	// Profiles
	public LB_LightingProfile LB_LightingProfile;
	//public PostProcessProfile postProcessingProfile;
	public VolumeProfile volumeProfileMain;


	public LightingMode lightingMode;
	public AmbientLight ambientLight;
    public SkyAmbientMode skyAmbientMode;
    public LightSettings lightSettings;
	public LightProbeMode lightprobeMode;

	// Depth of Field 
    public float dofFocusDistance = 10f;
    public DOFQuality dofQuality = DOFQuality.Low;
    public DepthOfFieldMode dofMode = DepthOfFieldMode.UsePhysicalCamera;

    // Sky and Sun
    public Light sunLight;
	public Color sunColor = Color.white;
	public float sunIntensity = 2.1f;
	public float indirectIntensity = 1.43f;
	Cubemap skyCube;
	float skyExposure;
	float hdrRotation;
	Color skyTint;
	Color groundColor;
	float tickness;
	// gradient sky
	// gradient sky
	public  Color gradientTop = Color.blue;
	public  Color gradientMiddle = new Color(0.3f,0.7f,1f);
	public  Color gradientBottom = Color.white;
	public float gradientDiffusion = 1f;

	public bool autoMode;
	public MyColorSpace colorSpace;

    // Fog
    public float baseHeight;
    public float fogAttenDistance;
    public float maxHeight;
    public float maxDistance;
    public Color fogTint;
    public FogColorMode fogColorMode;
    public bool fogVolumetric;
    public Color volumetricColor;
    public float vAnistropic;

    // shadows
    public LightsShadow psShadow;
	public float bakedResolution = 10f;
	public bool helpBox;

	// Private variabled
	Color redColor;
	bool lightError;
	bool lightChecked;
	GUIStyle myFoldoutStyle;
	bool showLogs;
	// Display window elements (Lighting Box)   
	Vector2 scrollPos = Vector2.zero;

	// Camera
	public Camera mainCamera;

	[Header("HD Shadows")]
	public  int CascadeCount = 4;
	public float distance = 500f;
	public float split1 = 0.05f;
	public float split2 = 0.15f;
	public float split3 = 0.3f;
	public bool HD_Enabled = false;
	public bool hdState;

	public bool MicroState;
	public bool Micro_Enabled = false;

    [Header("Motion Blur")]
    public int blurIntensity = 10;
    public int blurMaxVelocity = 100;
    public MotionBlurQuality blurQuality = MotionBlurQuality.Medium;

    // Snow
    public Texture2D snowAlbedo;
    public Texture2D snowNormal;
    public float snowIntensity = 0;


#endregion

#region Init()
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/HD Lighting Box 2 Lit %E")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
	////	LB_LightingBox window = (LB_LightingBox)EditorWindow.GetWindow(typeof(LB_LightingBox));
		System.Type inspectorType = System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
		LB_LightingBox window = (LB_LightingBox)EditorWindow.GetWindow<LB_LightingBox>("Lighting Box 2", true, new System.Type[] {inspectorType} );

		window.Show();
		window.autoRepaintOnSceneChange = true;
		window.maxSize = new Vector2 (1000f, 1000f);
		window.minSize = new Vector2 (387f, 1000f);
        
    }
#endregion

#region Options
	// Internal Usage
	public bool LightingBoxState = true,OptionsState = true;
	public bool ambientState = true;
	public bool sunState = true;
	public bool lightSettingsState = true;
	public bool cameraState = true;
	public bool profileState = true;
	public bool buildState = true;
	public bool fogState = true;
	public bool dofState = true;
	public bool colorState = true;
	public bool bloomState = true;
	public bool aaState = true;
	public bool aoState = true;
	public bool motionBlurState = true;
	public bool vignetteState = true;
	public bool chromatticState = true;
	public bool ssrState = true;
    public bool ssgiState = false;
    public bool snowState = true;

    // Effects enabled
    public bool Ambient_Enabled = true;
	public bool Scene_Enabled = true;
	public bool Sun_Enabled = true;
	public bool Fog_Enabled = false;
	public bool DOF_Enabled = true;
	public bool Bloom_Enabled = false;
	public bool AA_Enabled = true;
	public bool AO_Enabled = false;
	public bool MotionBlur_Enabled = true;
	public bool Vignette_Enabled = true;
	public bool Chromattic_Enabled = true;
	public bool SSR_Enabled = false;
    public bool SSGI_Enabled = true;

    Texture2D arrowOn,arrowOff;

#endregion

	void NewSceneInit()
	{
		if (EditorSceneManager.GetActiveScene ().name == "") 
		{
			LB_LightingProfile = Resources.Load ("DefaultSettings")as LB_LightingProfile;
			helper.Update_MainProfile(LB_LightingProfile,volumeProfileMain);

			OnLoad ();
			currentScene = EditorSceneManager.GetActiveScene ().name;

		} 
		else
		{
			if (System.String.IsNullOrEmpty (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name))) 
			{
				LB_LightingProfile = Resources.Load ("DefaultSettings")as LB_LightingProfile;
				helper.Update_MainProfile(LB_LightingProfile,volumeProfileMain);

			} else 
			{
				LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name), typeof(LB_LightingProfile));
				helper.Update_MainProfile(LB_LightingProfile,volumeProfileMain);

			}

			OnLoad ();   
			currentScene = EditorSceneManager.GetActiveScene ().name;

		}


	}

	// Load and apply default settings when a new scene opened
	void OnNewSceneOpened()
	{
		NewSceneInit ();
	}

	void OnDisable()
	{
		EditorApplication.hierarchyWindowChanged -= OnNewSceneOpened;
	}

	void OnEnable()
	{
		arrowOn = Resources.Load ("arrowOn") as Texture2D;
		arrowOff = Resources.Load ("arrowOff") as Texture2D;

		if (!GameObject.Find ("LightingBox_Helper")) 
		{
			GameObject helperObject = new GameObject ("LightingBox_Helper");
			helperObject.AddComponent<LB_LightingBoxHelper> ();
			helper = helperObject.GetComponent<LB_LightingBoxHelper> ();
		}

		EditorApplication.hierarchyWindowChanged += OnNewSceneOpened;

		currentScene = EditorSceneManager.GetActiveScene().name;

		if (System.String.IsNullOrEmpty (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name)))
			LB_LightingProfile = Resources.Load ("DefaultSettings")as LB_LightingProfile;
		else
			LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name), typeof(LB_LightingProfile));

		OnLoad ();

	}

    void OnGUI()
    {

#region Styles
        GUIStyle redStyle = new GUIStyle(EditorStyles.label);
        redStyle.alignment = TextAnchor.MiddleLeft;
        redStyle.normal.textColor = Color.red;

        GUIStyle blueStyle = new GUIStyle(EditorStyles.label);
        blueStyle.alignment = TextAnchor.MiddleLeft;
        blueStyle.normal.textColor = Color.blue;


        GUIStyle stateButton = new GUIStyle();
        stateButton = "Label";
        stateButton.alignment = TextAnchor.MiddleLeft;
        stateButton.fontStyle = FontStyle.Bold;

#endregion

#region GUI start implementation
        Undo.RecordObject(this, "lb");

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos,
            false,
            false,
            GUILayout.Width(this.position.width),
            GUILayout.Height(this.position.height));

        EditorGUILayout.Space();

        GUILayout.Label("HDRP Lighting Box 2 Lit v1.6.1 (june 2021) Unity: 2020.3.12 lts", EditorStyles.helpBox);


        EditorGUILayout.BeginHorizontal();

        if (!helpBox) {
            if (GUILayout.Button("Show Help",  GUILayout.Width(177), GUILayout.Height(24f))) {
                helpBox = !helpBox;
            }
        } else {
            if (GUILayout.Button("Hide Help",  GUILayout.Width(177), GUILayout.Height(24f))) {
                helpBox = !helpBox;
            }
        }
        if (GUILayout.Button("Refresh",  GUILayout.Width(179), GUILayout.Height(24f))) {
            UpdateSettings();
            UpdatePostEffects();
        }

        EditorGUILayout.EndHorizontal();

        if (EditorPrefs.GetInt("RateLBH") != 3) {

            if (GUILayout.Button("Rate Lighting Box")) {
                EditorPrefs.SetInt("RateLBH", 3);
                Application.OpenURL("http://u3d.as/24UT");
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();


#endregion

#region Tabs
        EditorGUILayout.BeginHorizontal();
        //----------------------------------------------
        if (winMode == WindowMode.Part1)
            GUI.backgroundColor = Color.green;
        else
            GUI.backgroundColor = Color.white;
        //----------------------------------------------
        if (GUILayout.Button("Scene",  GUILayout.Width(87), GUILayout.Height(43)))
            winMode = WindowMode.Part1;
        //----------------------------------------------
        if (winMode == WindowMode.Part2)
            GUI.backgroundColor = Color.green;
        else
            GUI.backgroundColor = Color.white;
        //----------------------------------------------
        if (GUILayout.Button("Effect",  GUILayout.Width(87), GUILayout.Height(43)))
            winMode = WindowMode.Part2;
        //----------------------------------------------
        if (winMode == WindowMode.Part3)
            GUI.backgroundColor = Color.green;
        else
            GUI.backgroundColor = Color.white;
        //----------------------------------------------
        if (GUILayout.Button("Color",  GUILayout.Width(87), GUILayout.Height(43)))
            winMode = WindowMode.Part3;
        //----------------------------------------------
        if (winMode == WindowMode.Finish)
            GUI.backgroundColor = Color.green;
        else
            GUI.backgroundColor = Color.white;
        //----------------------------------------------
        if (GUILayout.Button("Screen",  GUILayout.Width(87), GUILayout.Height(43)))
            winMode = WindowMode.Finish;
        //----------------------------------------------
        GUI.backgroundColor = Color.white;
        //----------------------------------------------//----------------------------------------------//----------------------------------------------

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
#endregion

#region Toolbar

        EditorGUILayout.BeginHorizontal();
        if (LightingBoxState)
        {
            if (GUILayout.Button("Effects is On",  GUILayout.Width(177), GUILayout.Height(24))) {
                helper.Toggle_Effects();

                LightingBoxState = !LightingBoxState;

                if (LB_LightingProfile)
                    LB_LightingProfile.LightingBoxState = LightingBoxState;
            }
        } else {
            if (GUILayout.Button("Effects is Off",  GUILayout.Width(177), GUILayout.Height(24))) {
                helper.Toggle_Effects();
                LightingBoxState = !LightingBoxState;

                if (LB_LightingProfile)
                    LB_LightingProfile.LightingBoxState = LightingBoxState;
            }
        }
        if (OptionsState)
        {
            if (GUILayout.Button("Expand All",  GUILayout.Width(179), GUILayout.Height(24))) {
                ambientState = sunState = lightSettingsState = true;
                cameraState = profileState = buildState = true;
                fogState = true;
                dofState = colorState = true;
                bloomState = aaState = aoState = true;
                motionBlurState = vignetteState = chromatticState = true;
                ssrState = ssgiState = true;
                ssrState = snowState = true;
                ssrState = hdState = true;
                ssrState = MicroState = true;


                OptionsState = !OptionsState;


                if (LB_LightingProfile)
                {
                    LB_LightingProfile.ambientState = ambientState;
                    LB_LightingProfile.sunState = sunState;
                    LB_LightingProfile.lightSettingsState = lightSettingsState;
                    LB_LightingProfile.cameraState = cameraState;
                    LB_LightingProfile.profileState = profileState;
                    LB_LightingProfile.buildState = buildState;
                    LB_LightingProfile.fogState = fogState;
                    LB_LightingProfile.dofState = dofState;
                    LB_LightingProfile.colorState = colorState;
                    LB_LightingProfile.bloomState = bloomState;
                    LB_LightingProfile.aaState = aaState;
                    LB_LightingProfile.aoState = aoState;
                    LB_LightingProfile.motionBlurState = motionBlurState;
                    LB_LightingProfile.vignetteState = vignetteState;
                    LB_LightingProfile.chromatticState = chromatticState;
                    LB_LightingProfile.ssrState = ssrState;
                    LB_LightingProfile.ssgiState = ssgiState;
                    LB_LightingProfile.snowState = snowState;                    LB_LightingProfile.snowState = snowState;
                    LB_LightingProfile.hdState = hdState;
                    LB_LightingProfile.MicroState = MicroState;

                    LB_LightingProfile.OptionsState = OptionsState;
                    EditorUtility.SetDirty(LB_LightingProfile);
                }

            }
        }
        else
        {
            if (GUILayout.Button("Close All",  GUILayout.Width(179), GUILayout.Height(24))) {

                ambientState = sunState = lightSettingsState = false;
                cameraState = profileState = buildState = false;
                fogState = false;
                dofState = colorState = false;
                bloomState = aaState = aoState = false;
                motionBlurState = vignetteState = chromatticState = false;
                ssrState = ssgiState = false;
                ssrState = snowState = false;
                ssrState = hdState = false;
                ssrState = MicroState = false;
                OptionsState = !OptionsState;

                if (LB_LightingProfile)
                {
                    LB_LightingProfile.ambientState = ambientState;
                    LB_LightingProfile.sunState = sunState;
                    LB_LightingProfile.lightSettingsState = lightSettingsState;
                    LB_LightingProfile.cameraState = cameraState;
                    LB_LightingProfile.profileState = profileState;
                    LB_LightingProfile.buildState = buildState;
                    LB_LightingProfile.fogState = fogState;
                    LB_LightingProfile.dofState = dofState;
                    LB_LightingProfile.colorState = colorState;
                    LB_LightingProfile.bloomState = bloomState;
                    LB_LightingProfile.aaState = aaState;
                    LB_LightingProfile.aoState = aoState;
                    LB_LightingProfile.motionBlurState = motionBlurState;
                    LB_LightingProfile.vignetteState = vignetteState;
                    LB_LightingProfile.chromatticState = chromatticState;
                    LB_LightingProfile.ssrState = ssrState;
                    LB_LightingProfile.ssgiState = ssgiState; 
                    LB_LightingProfile.OptionsState = OptionsState;
                    EditorUtility.SetDirty(LB_LightingProfile);
                }

            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
#endregion

        if (winMode == WindowMode.Part1) {

#region Toggle Settings


#endregion

#region Profiles

            //-----------Profile----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            if (profileState)
                GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
            else
                GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

            var profileStateRef = profileState;

            if (GUILayout.Button("Profile", stateButton, GUILayout.Width(300), GUILayout.Height(17f))) {
                profileState = !profileState;
            }

            if (profileStateRef != profileState)
            {

                if (LB_LightingProfile)
                {
                    LB_LightingProfile.profileState = profileState;
                    EditorUtility.SetDirty(LB_LightingProfile);
                }

            }
            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
            //---------------------------------------------------------------------------------------

            if (profileState) {
                if (helpBox)
                    EditorGUILayout.HelpBox("1. LB_LightingBox settings profile   2.HD Pipeline Profile", MessageType.Info);

                var lightingProfileRef = LB_LightingProfile;
                //	var postProcessingProfileRef = postProcessingProfile;

                EditorGUILayout.BeginHorizontal();
                LB_LightingProfile = EditorGUILayout.ObjectField("Lighting Profile", LB_LightingProfile, typeof(LB_LightingProfile), true) as LB_LightingProfile;

                if (GUILayout.Button("New", GUILayout.Width(43), GUILayout.Height(17))) {

                    if (EditorSceneManager.GetActiveScene().name == "")
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

                    string path = EditorUtility.SaveFilePanelInProject("Save As ...", "Lighting_Profile_" + EditorSceneManager.GetActiveScene().name, "asset", "");

                    if (path != "")
                    {
                        LB_LightingProfile = new LB_LightingProfile();

                        AssetDatabase.CreateAsset(LB_LightingProfile, path);
                        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Resources.Load("DefaultSettings_LB")), path);
                        LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath(path, typeof(LB_LightingProfile));
                        helper.Update_MainProfile(LB_LightingProfile, volumeProfileMain);

                        AssetDatabase.Refresh();
                        /*
						string path2 = System.IO.Path.GetDirectoryName(path) + "/Post_Profile_"+EditorSceneManager.GetActiveScene ().name+".asset";
						// Create new post processing stack 2 profile
						postProcessingProfile = new PostProcessProfile ();
						AssetDatabase.CreateAsset (postProcessingProfile, path2);
						AssetDatabase.CopyAsset (AssetDatabase.GetAssetPath (Resources.Load ("Default_Post_Profile")), path2);
						postProcessingProfile = (PostProcessProfile)AssetDatabase.LoadAssetAtPath (path2, typeof(PostProcessProfile));
						LB_LightingProfile.postProcessingProfile = postProcessingProfile;
                       
						AssetDatabase.Refresh ();
						    */
                        string path3 = System.IO.Path.GetDirectoryName(path) + "/Volume_Profile_" + EditorSceneManager.GetActiveScene().name + ".asset";
                        // Create new HD Pipeline profile
                        volumeProfileMain = new VolumeProfile();
                        AssetDatabase.CreateAsset(volumeProfileMain, path3);
                        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Resources.Load("Default_Volume_Profile")), path3);
                        volumeProfileMain = (VolumeProfile)AssetDatabase.LoadAssetAtPath(path3, typeof(VolumeProfile));
                        LB_LightingProfile.volumeProfile = volumeProfileMain;

                        AssetDatabase.Refresh();
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                if (lightingProfileRef != LB_LightingProfile) {

                    helper.Update_MainProfile(LB_LightingProfile, volumeProfileMain);
                    OnLoad();
                    EditorPrefs.SetString(EditorSceneManager.GetActiveScene().name, AssetDatabase.GetAssetPath(LB_LightingProfile));

                    if (LB_LightingProfile)
                        EditorUtility.SetDirty(LB_LightingProfile);
                }
                /*
				if (postProcessingProfileRef != postProcessingProfile)
				{
					if (LB_LightingProfile)
					{
						LB_LightingProfile.postProcessingProfile = postProcessingProfile;
						EditorUtility.SetDirty (LB_LightingProfile);
					}
                    
					UpdatePostEffects ();

				}
				*/



                if (helpBox)
                    EditorGUILayout.HelpBox("Which camera should has effects", MessageType.Info);

                EditorGUILayout.BeginHorizontal();
                var mainCameraRef = mainCamera;

                mainCamera = EditorGUILayout.ObjectField("Target Camera", mainCamera, typeof(Camera), true) as Camera;
                if (GUILayout.Button("Save", GUILayout.Width(43), GUILayout.Height(17)))
                {
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.mainCameraName = mainCamera.name;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (mainCameraRef != mainCamera)
                {
                    UpdatePostEffects();
                    UpdateSettings();

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.mainCameraName = mainCamera.name;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }

                /*var webGL_MobileRef = webGL_Mobile;

				webGL_Mobile = EditorGUILayout.Toggle ("WebGL 2.0 Target", webGL_Mobile);

				if (webGL_MobileRef != webGL_Mobile) {
					if (LB_LightingProfile)
					{
						LB_LightingProfile.webGL_Mobile = webGL_Mobile;
						EditorUtility.SetDirty (LB_LightingProfile);
					}
				}
                */
                EditorGUILayout.Space();
                EditorGUILayout.Space();

            }

#endregion

#region Ambient

            //-----------Ambient----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            if (ambientState)
                GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
            else
                GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

            var Ambient_EnabledRef = Ambient_Enabled;
            var ambientStateRef = ambientState;

            if (GUILayout.Button("Ambient", stateButton, GUILayout.Width(300), GUILayout.Height(17f))) {
                ambientState = !ambientState;
            }

            if (ambientStateRef != ambientState)
            {
                if (LB_LightingProfile)
                {
                    LB_LightingProfile.ambientState = ambientState;
                    EditorUtility.SetDirty(LB_LightingProfile);
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
            //---------------------------------------------------------------------------------------


            if (ambientState) {

                Ambient_Enabled = EditorGUILayout.Toggle("Enabled", Ambient_Enabled);
                EditorGUILayout.Space();

                if (helpBox)
                    EditorGUILayout.HelpBox("Set ambient lighting source as HDRI(cubemap), Gradient or None", MessageType.Info);

                var ambientLightRef = ambientLight;
                var skyCubeRef = skyCube;
                var skyExposureRef = skyExposure;
                var hdrRotationRef = hdrRotation;
                var groundColorRef = groundColor;
                var skyTintRef = skyTint;
                var ticknessRef = tickness;
                var skyAmbientModeRef = skyAmbientMode;

                var gradientTopRef = gradientTop;
                var gradientMiddleRef = gradientMiddle;
                var gradientBottomRef = gradientBottom;
                var gradientDiffusionRef = gradientDiffusion;
                
                // choose ambient lighting mode   (color or skybox)
                ambientLight = (AmbientLight)EditorGUILayout.EnumPopup("Ambient Source", ambientLight, GUILayout.Width(343));
                skyAmbientMode = (SkyAmbientMode)EditorGUILayout.EnumPopup("Ambient Mode", skyAmbientMode, GUILayout.Width(343));
                
                if (ambientLight == AmbientLight.HDRI)
                {
                    skyCube = EditorGUILayout.ObjectField("Sky Cube", skyCube, typeof(Cubemap), true) as Cubemap;
                    skyExposure = EditorGUILayout.Slider("Exposure", skyExposure, 0, 17);
                    hdrRotation = EditorGUILayout.Slider("Rotation", hdrRotation, 0, 360);
                }
                if (ambientLight == AmbientLight.PhysicallyBased)
                {
                    skyExposure = EditorGUILayout.Slider("Exposure", skyExposure, 0, 5f);
                }
               /* if (ambientLight == AmbientLight.Procedural)
                {
                    skyExposure = EditorGUILayout.Slider("Exposure", skyExposure, 0, 5f);
                    skyTint = EditorGUILayout.ColorField(new GUIContent("Sky Tint"), skyTint, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);
                    groundColor = EditorGUILayout.ColorField(new GUIContent("Ground Color"), groundColor, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);
                    tickness = EditorGUILayout.Slider("Atmosphere Thickness", tickness, 0, 5f);

                }*/
               /* if (ambientLight == AmbientLight.Gradient)
                {
                    gradientTop = EditorGUILayout.ColorField(new GUIContent("Top"), gradientTop, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);
                    gradientMiddle = EditorGUILayout.ColorField(new GUIContent("Middle"), gradientMiddle, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);
                    gradientBottom = EditorGUILayout.ColorField(new GUIContent("Bottom"), gradientBottom, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);
                    gradientDiffusion = EditorGUILayout.Slider("Diffusion", gradientDiffusion, -3f, 3f);

                }*/
                if (ambientLightRef != ambientLight || skyCubeRef != skyCube
                    || skyTintRef != skyTint || skyExposureRef != skyExposure
                    || hdrRotationRef != hdrRotation || groundColorRef != groundColor
                    || Ambient_EnabledRef != Ambient_Enabled || ticknessRef != tickness
                    || gradientTopRef != gradientTop || gradientMiddleRef != gradientMiddle || gradientBottomRef != gradientBottom
                    || gradientDiffusionRef != gradientDiffusion || skyAmbientModeRef != skyAmbientMode)
                {
                    helper.Update_Ambient(Ambient_Enabled, ambientLight, skyAmbientMode, skyCube, skyExposure, hdrRotation, skyTint, groundColor, tickness
                        , gradientTop, gradientMiddle, gradientBottom, gradientDiffusion);

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.skyCube = skyCube;
                        LB_LightingProfile.ambientLight = ambientLight;
                        LB_LightingProfile.skyExposure = skyExposure;
                        LB_LightingProfile.hdrRotation = hdrRotation;
                        LB_LightingProfile.tickness = tickness;
                        LB_LightingProfile.groundColor = groundColor;
                        LB_LightingProfile.skyTint = skyTint;
                        LB_LightingProfile.skyAmbientMode = skyAmbientMode;
                        
                        LB_LightingProfile.gradientTop = gradientTop;
                        LB_LightingProfile.gradientMiddle = gradientMiddle;
                        LB_LightingProfile.gradientBottom = gradientBottom;
                        LB_LightingProfile.gradientDiffusion = gradientDiffusion;

                        LB_LightingProfile.Ambient_Enabled = Ambient_Enabled;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }
                //----------------------------------------------------------------------
            }
#endregion

#region Sun Light
            //-----------Sun----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            if (sunState)
                GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
            else
                GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

            var sunStateRef = sunState;

            if (GUILayout.Button("Sun", stateButton, GUILayout.Width(300), GUILayout.Height(17f))) {
                sunState = !sunState;
            }

            if (sunStateRef != sunState)
            {
                if (LB_LightingProfile)
                {
                    LB_LightingProfile.sunState = sunState;
                    EditorUtility.SetDirty(LB_LightingProfile);
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
            //---------------------------------------------------------------------------------------


            if (sunState) {
                if (helpBox)
                    EditorGUILayout.HelpBox("Sun /  Moon light settings", MessageType.Info);

                var Sun_EnabledRef = Sun_Enabled;

                Sun_Enabled = EditorGUILayout.Toggle("Enabled", Sun_Enabled);

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                sunLight = EditorGUILayout.ObjectField("Sun Light", sunLight, typeof(Light), true) as Light;
                if (!sunLight) {
                    if (GUILayout.Button("Find"))
                        Update_Sun();
                }
                EditorGUILayout.EndHorizontal();
                var sunColorRef = sunColor;

                sunColor = EditorGUILayout.ColorField("Color", sunColor);

                var sunIntensityRef = sunIntensity;
                var indirectIntensityRef = indirectIntensity;

                sunIntensity = EditorGUILayout.Slider("Intenity", sunIntensity, 0, 300000);
                indirectIntensity = EditorGUILayout.Slider("Indirect Intensity", indirectIntensity, 0, 4f);

                if (Sun_EnabledRef != Sun_Enabled)
                {
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.Sun_Enabled = Sun_Enabled;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }

                if (Sun_Enabled)
                {

                    if (sunColorRef != sunColor || Sun_EnabledRef != Sun_Enabled) {

                        if (sunLight)
                            sunLight.color = sunColor;
                        else
                            Update_Sun();
                        if (LB_LightingProfile)
                        {
                            LB_LightingProfile.sunColor = sunColor;
                            EditorUtility.SetDirty(LB_LightingProfile);
                        }
                    }

                    if (sunIntensityRef != sunIntensity || indirectIntensityRef != indirectIntensity
                        || Sun_EnabledRef != Sun_Enabled) {

                        if (sunLight) {
                            sunLight.GetComponent<HDAdditionalLightData>()
                                .intensity = sunIntensity;
                            sunLight.bounceIntensity = indirectIntensity;
                        } else
                            Update_Sun();
                        if (LB_LightingProfile) {
                            LB_LightingProfile.sunIntensity = sunIntensity;
                            LB_LightingProfile.indirectIntensity = indirectIntensity;
                            LB_LightingProfile.Sun_Enabled = Sun_Enabled;
                        }
                        if (LB_LightingProfile)
                        {
                            LB_LightingProfile.sunState = sunState;
                            EditorUtility.SetDirty(LB_LightingProfile);
                        }
                    }
                }
                if (GUILayout.Button("Open Reference"))
                    Application.OpenURL("https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@10.3/manual/Physical-Light-Units.html");

                    }
#endregion

#region Lighting Mode


            //-----------Light Settings----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            if (lightSettingsState)
                GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
            else
                GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

            var lightSettingsStateRef = lightSettingsState;
            var Scene_EnabledRef = Scene_Enabled;

            if (GUILayout.Button("Scene", stateButton, GUILayout.Width(300), GUILayout.Height(17f))) {
                lightSettingsState = !lightSettingsState;
            }

            if (lightSettingsStateRef != lightSettingsState)
            {
                if (LB_LightingProfile)
                {
                    LB_LightingProfile.lightSettingsState = lightSettingsState;
                    LB_LightingProfile.Scene_Enabled = Scene_Enabled;
                    EditorUtility.SetDirty(LB_LightingProfile);
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
            //---------------------------------------------------------------------------------------


            if (lightSettingsState) {
                if (helpBox)
                    EditorGUILayout.HelpBox("Fully realtime without GI, Enlighten Realtime GI or Baked Progressive Lightmapper or both of them(realtime and baked)", MessageType.Info);

                var lightingModeRef = lightingMode;

                var indirectDiffuseRef = indirectDiffuse;
                var indirectSpecularRef = indirectSpecular;
                var probeIntensityRef = probeIntensity;

                Scene_Enabled = EditorGUILayout.Toggle("Enabled", Scene_Enabled);
                EditorGUILayout.Space();

                // Choose lighting mode (realtime GI or baked GI)
                lightingMode = (LightingMode)EditorGUILayout.EnumPopup("Lighting Mode", lightingMode, GUILayout.Width(343));

                if (lightingMode == LightingMode.BakedCPU ||
                    lightingMode == LightingMode.BakedGPU) {
                    EditorGUILayout.Space();

                    if (helpBox)
                        EditorGUILayout.HelpBox("Baked lightmapping resolution. Higher value needs more RAM and longer bake time. Check task manager about RAM usage during bake time", MessageType.Info);

                    // Baked lightmapping resolution   
                    bakedResolution = EditorGUILayout.FloatField("Baked Resolution", bakedResolution);
                    LightmapEditorSettings.bakeResolution = bakedResolution;
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.bakedResolution = bakedResolution;
                        LB_LightingProfile.Scene_Enabled = Scene_Enabled;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }

                }
                EditorGUILayout.Space();
                indirectDiffuse = EditorGUILayout.Slider("Indirect Diffuse", indirectDiffuse, 0, 10);
                indirectSpecular = EditorGUILayout.Slider("Indirect Reflection", indirectSpecular, 0, 5);
                probeIntensity = EditorGUILayout.Slider("Indirect Probe", probeIntensity, 0, 5);
                
                EditorGUILayout.Space();

                if (lightingModeRef != lightingMode || Scene_EnabledRef != Scene_Enabled || indirectDiffuseRef != indirectDiffuse
                    || indirectSpecularRef != indirectSpecular || probeIntensityRef != probeIntensity) {
                    //----------------------------------------------------------------------
                    // Update Lighting Mode
                    helper.Update_LightingMode(Scene_Enabled, lightingMode, indirectDiffuse, indirectSpecular, probeIntensity);
                    //----------------------------------------------------------------------
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.lightingMode = lightingMode;
                        LB_LightingProfile.indirectDiffuse = indirectDiffuse;
                        LB_LightingProfile.indirectSpecular = indirectSpecular;
                        LB_LightingProfile.probeIntensity = probeIntensity;                        
                        LB_LightingProfile.Scene_Enabled = Scene_Enabled;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }
#endregion

#region Light Types
                EditorGUILayout.Space();

                if (helpBox)
                    EditorGUILayout.HelpBox("Changing the type of all light sources (Realtime,Baked,Mixed)", MessageType.Info);

                var lightSettingsRef = lightSettings;

                // Change file lightmapping type mixed,realtime baked
                lightSettings = (LightSettings)EditorGUILayout.EnumPopup("Lights Type", lightSettings, GUILayout.Width(343));

                //----------------------------------------------------------------------
                // Light Types
                if (lightSettingsRef != lightSettings) {

                    helper.Update_LightSettings(Scene_Enabled, lightSettings);

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.lightSettings = lightSettings;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }
                //----------------------------------------------------------------------
                #endregion

                #region Light Shadows Settings
                EditorGUILayout.Space();

                if (helpBox)
                    EditorGUILayout.HelpBox("Activate shadows for point and spot lights   ", MessageType.Info);

                var pshadRef = psShadow;
                // Choose hard shadows state on off for spot and point lights
                psShadow = (LightsShadow)EditorGUILayout.EnumPopup("Enable Shadows", psShadow, GUILayout.Width(343));

                if (pshadRef != psShadow)
                {

                    // Shadows
                    helper.Update_Shadows(psShadow);

                    //----------------------------------------------------------------------
                    if (LB_LightingProfile)
                        LB_LightingProfile.lightsShadow = psShadow;
                    if (LB_LightingProfile)
                        EditorUtility.SetDirty(LB_LightingProfile);
                }
                #endregion

                #region Light Probes
                EditorGUILayout.Space();

                if (helpBox)
                    EditorGUILayout.HelpBox("Adjust light probes settings for non-static objects, Blend mode is more optimized", MessageType.Info);

                var lightprobeModeRef = lightprobeMode;

                lightprobeMode = (LightProbeMode)EditorGUILayout.EnumPopup("Light Probes", lightprobeMode, GUILayout.Width(343));

                if (lightprobeModeRef != lightprobeMode) {

                    // Light Probes
                    helper.Update_LightProbes(Scene_Enabled, lightprobeMode);

                    //----------------------------------------------------------------------
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.lightProbesMode = lightprobeMode;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }
            }
#endregion

#region Buttons
            //-----------Buttons----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            if (buildState)
                GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
            else
                GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

            var buildStateRef = buildState;

            if (GUILayout.Button("Build", stateButton, GUILayout.Width(300), GUILayout.Height(17f))) {
                buildState = !buildState;
            }

            if (buildStateRef != buildState)
            {
                if (LB_LightingProfile)
                    LB_LightingProfile.buildState = buildState;
                if (LB_LightingProfile)
                    EditorUtility.SetDirty(LB_LightingProfile);
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
            //---------------------------------------------------------------------------------------

            if (buildState) {
                var automodeRef = autoMode;

                if (helpBox)
                    EditorGUILayout.HelpBox("Automatic lightmap baking", MessageType.Info);


                autoMode = EditorGUILayout.Toggle("Auto Mode", autoMode);

                if (automodeRef != autoMode) {
                    // Auto Mode
                    if (autoMode)
                        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
                    else
                        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
                    //----------------------------------------------------------------------
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.automaticLightmap = autoMode;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }

                // Start bake
                if (!Lightmapping.isRunning) {

                    if (helpBox)
                        EditorGUILayout.HelpBox("Bake lightmap", MessageType.Info);

                    if (GUILayout.Button("Bake")) {
                        if (!Lightmapping.isRunning) {
                            Lightmapping.BakeAsync();
                        }
                    }

                    if (helpBox)
                        EditorGUILayout.HelpBox("Clear lightmap data", MessageType.Info);

                    if (GUILayout.Button("Clear")) {
                        Lightmapping.Clear();
                    }
                } else {

                    if (helpBox)
                        EditorGUILayout.HelpBox("Cancel baked lightmap data", MessageType.Info);

                    if (GUILayout.Button("Cancel")) {
                        if (Lightmapping.isRunning) {
                            Lightmapping.Cancel();
                        }
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (helpBox)
                    EditorGUILayout.HelpBox("Open unity Lighting Settings window", MessageType.Info);

                if (GUILayout.Button("Lighting Window")) {

                    EditorApplication.ExecuteMenuItem("Window/Rendering/Lighting");
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                if (GUILayout.Button("Open Material Converter"))
                {

                    EditorApplication.ExecuteMenuItem("Window/Batch Material Converter");
                }

                EditorGUILayout.Space();
                EditorGUILayout.Space();


                if (GUILayout.Button("Add Camera Move Script")) {
                    if (!mainCamera.GetComponent<LB_CameraMove>())
                        mainCamera.gameObject.AddComponent<LB_CameraMove>();
                }


                EditorGUILayout.Space();
                EditorGUILayout.Space();
                #endregion



            }
        }

            if (winMode == WindowMode.Part2)
            {

#region Global Fog

#region Fog

                //-----------Fog----------------------------------------------------------------------------
                GUILayout.BeginVertical("Box");

                var Fog_EnabledRef = Fog_Enabled;

                EditorGUILayout.BeginHorizontal();

                if (fogState)
                    GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
                else
                    GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

                Fog_Enabled = EditorGUILayout.Toggle("", Fog_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));



                if (Fog_EnabledRef != Fog_Enabled)
                {
               
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.Fog_Enabled = Fog_Enabled;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }



                var fogStateRef = fogState;

                if (GUILayout.Button("Fog", stateButton, GUILayout.Width(300), GUILayout.Height(17f))) {
                    fogState = !fogState;
                }

                if (fogStateRef != fogState)
                {
                    if (LB_LightingProfile)
                        LB_LightingProfile.fogState = fogState;
                    if (LB_LightingProfile)
                        EditorUtility.SetDirty(LB_LightingProfile);
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                //---------------------------------------------------------------------------------------


                if (fogState) {
                    if (helpBox)
                        EditorGUILayout.HelpBox("Set Fog Mode Normal or Volumetric ", MessageType.Info);

                EditorGUILayout.HelpBox("Fog and Volumetric light isn't available for Lit version", MessageType.Warning);

                //----------------------------------------------------------------------
            }

            #endregion

            #endregion

            #region Depth of Field 2    

            //-----------Depth of Field----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

                var DOF_EnabledRef = DOF_Enabled;

                EditorGUILayout.BeginHorizontal();

                if (dofState)
                    GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
                else
                    GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

                DOF_Enabled = EditorGUILayout.Toggle("", DOF_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

                var dofStateRef = dofState;

                if (GUILayout.Button("Depth of Field", stateButton, GUILayout.Width(300), GUILayout.Height(17f))) {
                    dofState = !dofState;
                }

                if (dofStateRef != dofState)
                {
                    if (LB_LightingProfile)
                        LB_LightingProfile.dofState = dofState;
                    if (LB_LightingProfile)
                        EditorUtility.SetDirty(LB_LightingProfile);
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                //---------------------------------------------------------------------------------------

                if (dofState)
                {
                    if (helpBox)
                        EditorGUILayout.HelpBox("Activate Depth Of Field for the camera", MessageType.Info);
                }
                var dofFocusDistanceRef = dofFocusDistance;
                var dofQualityRef = dofQuality;
                var dofModeRef = dofMode;

                if (dofState)
                {
                    if(dofMode != DepthOfFieldMode.UsePhysicalCamera)
                       EditorGUILayout.LabelField("Only Physical Camrea mode is supported for now");

                    dofMode = (DepthOfFieldMode)EditorGUILayout.EnumPopup("Mode", dofMode);
                    dofFocusDistance = (float)EditorGUILayout.Slider("Focus Distance", dofFocusDistance, 0.5f, 50f);
                    dofQuality = (DOFQuality)EditorGUILayout.EnumPopup("Focus Distance", dofQuality);
                }

                if (DOF_EnabledRef != DOF_Enabled || dofFocusDistanceRef != dofFocusDistance ||
                     dofQualityRef != dofQuality || dofModeRef != dofMode) {

			    helper.Update_DOF(DOF_Enabled, dofFocusDistance, dofQuality, dofMode);

			    //----------------------------------------------------------------------
			    if (LB_LightingProfile)
			    {
					    LB_LightingProfile.DOF_Enabled = DOF_Enabled;
					    LB_LightingProfile.dofFocusDistance = dofFocusDistance; 
                        LB_LightingProfile.dofQuality = dofQuality;
                        LB_LightingProfile.dofMode = dofMode;
                        EditorUtility.SetDirty (LB_LightingProfile);
			    }
		    }
			
#endregion

#region Bloom

				//-----------Bloom----------------------------------------------------------------------------
				GUILayout.BeginVertical ("Box");

				var Bloom_EnabledRef = Bloom_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(bloomState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
				Bloom_Enabled = EditorGUILayout.Toggle("",Bloom_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var bloomStateRef = bloomState;

				if (GUILayout.Button ("Bloom", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
					bloomState = !bloomState;
				}

			if(bloomStateRef != bloomState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.bloomState = bloomState;			
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

				EditorGUILayout.EndHorizontal ();

				GUILayout.EndVertical ();
				//---------------------------------------------------------------------------------------
				if(bloomState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox ("Activate Bloom for the camera", MessageType.Info);
				}
				var bIntensityRef = bIntensity;
				var bThreshouldRef = bThreshould;
				var bColorRef = bColor;
			var dirtTextureRef = dirtTexture;
			var dirtIntensityRef = dirtIntensity;
			var mobileOptimizedBloomRef = mobileOptimizedBloom;
			var bRotationRef = bRotation;
				if (bloomState)
				{
					bIntensity = (float)EditorGUILayout.Slider ("Intensity", bIntensity, 0, 1f);
					bThreshould = (float)EditorGUILayout.Slider ("Threshould", bThreshould, 1f, 2f);
				    bRotation = EditorGUILayout.Toggle ("Anamorphic", bRotation);

					bColor = (Color)EditorGUILayout.ColorField ("Color", bColor);
					//mobileOptimizedBloom = EditorGUILayout.Toggle("Mobile Optimized",mobileOptimizedBloom);
					EditorGUILayout.Space();

				dirtTexture = EditorGUILayout.ObjectField ("Dirt Texture", dirtTexture, typeof(Texture2D), true) as Texture2D;
				dirtIntensity = (float)EditorGUILayout.Slider ("Dirt Intensity", dirtIntensity, 0, 10f);
				}

			if (dirtTextureRef != dirtTexture || dirtIntensityRef != dirtIntensity || Bloom_EnabledRef != Bloom_Enabled || bIntensityRef != bIntensity || bColorRef != bColor || bThreshouldRef != bThreshould || bIntensityRef != bIntensity
				|| mobileOptimizedBloomRef != mobileOptimizedBloom  || bRotationRef != bRotation) {


				helper.Update_Bloom(Bloom_Enabled,bIntensity,bThreshould,bColor,dirtTexture,dirtIntensity,mobileOptimizedBloom,bRotation);


					//----------------------------------------------------------------------

				if (LB_LightingProfile)
				{
						LB_LightingProfile.Bloom_Enabled = Bloom_Enabled;
					LB_LightingProfile.bIntensity = bIntensity;
					LB_LightingProfile.bRotation = bRotation;
					LB_LightingProfile.bThreshould = bThreshould;
						LB_LightingProfile.mobileOptimizedBloom = mobileOptimizedBloom;						
						LB_LightingProfile.bColor = bColor;		
						LB_LightingProfile.dirtTexture = dirtTexture;		
						LB_LightingProfile.dirtIntensity = dirtIntensity;	
						EditorUtility.SetDirty (LB_LightingProfile);
				}
				}

            #endregion
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

        }

        if (winMode == WindowMode.Part3) {

#region Color Grading

			//-----------Color Grading----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");

			EditorGUILayout.BeginHorizontal ();

			if(colorState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			

			var colorStateRef = colorState;

			if (GUILayout.Button ("Color Grading", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				colorState = !colorState;
			}

			if(colorStateRef != colorState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.colorState = colorState;				
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------

			if(colorState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox ("Color grading settings", MessageType.Info);
			}
            var meteringModeRef = meteringMode;
            var colorModeRef = colorMode;
            var compensationRef = compensation;
            var compensationFixedRef = compensationFixed;

            var fixedExposureRef = fixedExposure;
            var lb_exposureModeRef = lb_exposureMode;
            var exposureIntensityRef = exposureIntensity;
				var contrastValueRef = contrastValue;
				var tempRef = temp;
				var eyeKeyValue1Ref = eyeKeyValue1;
            var eyeKeyValue2Ref = eyeKeyValue2;
            var gammaRef = gamma;
				var colorGammaRef = colorGamma;
				var colorLiftRef = colorLift;
				var saturationRef = saturation;
				var lutRef = lut;

			if(colorState)
			{
				/*if (!webGL_Mobile)
					colorMode = (ColorMode)EditorGUILayout.EnumPopup ("Mode", colorMode, GUILayout.Width (343));
				*/
				if(colorMode == ColorMode.LUT)
				{
					lut = EditorGUILayout.ObjectField ("LUT Texture   ", lut, typeof(Texture), true) as Texture;


				}
				else
				{
                    EditorGUILayout.LabelField("Auto / Fixed Exposure :");
                    lb_exposureMode = (LB_ExposureMode)EditorGUILayout.EnumPopup("Exposure Mode", lb_exposureMode, GUILayout.Width(343));
                    if(lb_exposureMode == LB_ExposureMode.Automatic)
                     compensation = (float)EditorGUILayout.Slider("Auto Compensation", compensation, 0, 10);
                    else
                     compensationFixed = (float)EditorGUILayout.Slider("Fixed Compensation", compensationFixed, 0, 10);


                    if (lb_exposureMode == LB_ExposureMode.Automatic)
                    { 
                        eyeKeyValue1 = (float)EditorGUILayout.Slider("Min Exposure", eyeKeyValue1, 0, -10);
                        eyeKeyValue2 = (float)EditorGUILayout.Slider("Max Exposure", eyeKeyValue2, 0, 14);
                        meteringMode = (MeteringMode)EditorGUILayout.EnumPopup("Metering Mode", meteringMode, GUILayout.Width(343));
                    }
                    if (lb_exposureMode == LB_ExposureMode.Fixed)
                    {
                        fixedExposure = (float)EditorGUILayout.Slider("Fixed Exposure", fixedExposure, -5, 15);
                    }
                    EditorGUILayout.Space();

                    exposureIntensity = (float)EditorGUILayout.Slider ("Post Exposure", exposureIntensity, 0, 3f);
					contrastValue = (float)EditorGUILayout.Slider ("Contrast", contrastValue, 0, 1f);
					saturation = (float)EditorGUILayout.Slider ("Saturation", saturation, -1f, 0.3f);
					temp = (float)EditorGUILayout.Slider ("Temperature", temp, 0, 100f);


				
						EditorGUILayout.Space();
						
					colorGamma = (Color)EditorGUILayout.ColorField ("Gamma Color", colorGamma);
					colorLift = (Color)EditorGUILayout.ColorField ("Lift Color", colorLift);
					EditorGUILayout.Space();

					gamma = (float)EditorGUILayout.Slider ("Gamma", gamma, -1f, 1f);
				}
			}

            if (meteringModeRef != meteringMode || exposureIntensityRef != exposureIntensity || contrastValueRef != contrastValue || tempRef != temp || eyeKeyValue1Ref != eyeKeyValue1 || eyeKeyValue2Ref != eyeKeyValue2
                  || colorModeRef != colorMode || gammaRef != gamma || colorGammaRef != colorGamma || colorLiftRef != colorLift || saturationRef != saturation
				|| lutRef != lut || compensationRef != compensation || compensationFixedRef != compensationFixed
                || fixedExposureRef != fixedExposure || lb_exposureModeRef != lb_exposureMode) {


                helper.Update_ColorGrading(meteringMode, colorMode, exposureIntensity, contrastValue, temp, eyeKeyValue1, eyeKeyValue2, saturation, colorGamma, colorLift, gamma, lut, lb_exposureMode, compensation,compensationFixed, fixedExposure);

                //----------------------------------------------------------------------
                if (LB_LightingProfile)
				{
                    LB_LightingProfile.compensation = compensation;
                                        LB_LightingProfile.fixedExposure = fixedExposure;
LB_LightingProfile.compensationFixed = compensationFixed;
                    LB_LightingProfile.lb_exposureMode = lb_exposureMode;
                    LB_LightingProfile.exposureIntensity = exposureIntensity;
						LB_LightingProfile.lut = lut;
						LB_LightingProfile.contrastValue = contrastValue;
						LB_LightingProfile.temp = temp;
						LB_LightingProfile.eyeKeyValue1 = eyeKeyValue1;
                    LB_LightingProfile.eyeKeyValue2 = eyeKeyValue2;
                    LB_LightingProfile.meteringMode = meteringMode;
                    LB_LightingProfile.colorMode = colorMode;
						LB_LightingProfile.colorLift = colorLift;
						LB_LightingProfile.colorGamma = colorGamma;
						LB_LightingProfile.gamma = gamma;
						LB_LightingProfile.saturation = saturation;
						EditorUtility.SetDirty (LB_LightingProfile);
				}
			}

#endregion

#region Snow
                //-----------Snow----------------------------------------------------------------------------
                GUILayout.BeginVertical("Box");


                EditorGUILayout.BeginHorizontal();

                if (snowState)
                    GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
                else
                    GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));


                var snowStateRef = snowState;

                if (GUILayout.Button("Snow", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
                {
                    snowState = !snowState;
                }

                if (snowStateRef != snowState)
                {
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.snowState = snowState;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                //---------------------------------------------------------------------------------------

                if (snowState)
                {
                    if (helpBox)
                        EditorGUILayout.HelpBox("Conrol lighting box  Snow shader settings", MessageType.Info);
                }

                var snowAlbedoRef = snowAlbedo;
                var snowNormalRef = snowNormal;
                var snowIntensityRef = snowIntensity;

                if (snowState)
                {
                   EditorGUILayout.HelpBox("Snow effect and shaders isn't available for Lit version", MessageType.Warning);
                }

            #endregion

            #region HD Shadows

            //-----------HD Shadows----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

                var HD_EnabledRef = HD_Enabled;

                EditorGUILayout.BeginHorizontal();

                if (hdState)
                    GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
                else
                    GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));


                var hdStateRef = hdState;

                if (GUILayout.Button("HD Shadows", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
                {
                    hdState = !hdState;
                }

                if (hdStateRef != hdState)
                {
                    if (LB_LightingProfile)
                        LB_LightingProfile.hdState = hdState;
                    if (LB_LightingProfile)
                        EditorUtility.SetDirty(LB_LightingProfile);
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                //---------------------------------------------------------------------------------------

                if (hdState)
                {
                    if (helpBox)
                        EditorGUILayout.HelpBox("Activate HD Shadows effect for your camera", MessageType.Info);
                }

                var CascadeCountRef = CascadeCount;
                var distanceRef = distance;
                var split1Ref = split1;
                var split2Ref = split2;
                var split3Ref = split3;

                if (hdState)
                {
                    HD_Enabled = EditorGUILayout.Toggle("Enabled", HD_Enabled);

                    distance = EditorGUILayout.Slider("Max Distance", distance, 0, 10000f);

                    CascadeCount = EditorGUILayout.IntSlider("Cascade Count", CascadeCount, 1, 4);
                    if (CascadeCount >= 2)
                    {
                        split1 = EditorGUILayout.Slider("Split 1", split1, 0, 1f);
                    }
                    if (CascadeCount >= 3)
                    {
                        split2 = EditorGUILayout.Slider("Split 2", split2, 0, 1f);
                    }
                    if (CascadeCount >= 4)
                    {
                        split3 = EditorGUILayout.Slider("Split 3", split3, 0, 1f);
                    }
                }

                if (HD_EnabledRef != HD_Enabled || CascadeCountRef != CascadeCount || distanceRef != distance || split1Ref != split1
                    || split2Ref != split2 || split3Ref != split3)
                {
                    helper.Update_HDSHadows(HD_Enabled, CascadeCount, distance, split1, split2, split3);

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.HD_Enabled = HD_Enabled;
                        LB_LightingProfile.CascadeCount = CascadeCount;
                        LB_LightingProfile.distance = distance;
                        LB_LightingProfile.split1 = split1;
                        LB_LightingProfile.split2 = split2;
                        LB_LightingProfile.split3 = split3;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }

#endregion

#region Micro  Shadowing  

                //-----------Micro Shadowing----------------------------------------------------------------------------
                GUILayout.BeginVertical("Box");

                var Micro_EnabledRef = Micro_Enabled;

                EditorGUILayout.BeginHorizontal();

                if (MicroState)
                    GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
                else
                    GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));


                var MicroStateRef = MicroState;

                if (GUILayout.Button("Micro Shadowing", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
                {
                    MicroState = !MicroState;
                }

                if (MicroStateRef != MicroState)
                {
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.MicroState = MicroState;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                //---------------------------------------------------------------------------------------

                if (MicroState)
                {
                    if (helpBox)
                        EditorGUILayout.HelpBox("Activate Micro Shadowing effect for your camera", MessageType.Info);
                }

                var microEnabledRef = microEnabled;
                var microOpacityRef = microOpacity;


                if (MicroState)
                {
                    Micro_Enabled = EditorGUILayout.Toggle("Enabled", Micro_Enabled);

                    microOpacity = EditorGUILayout.Slider("Opacity", microOpacity, 0, 1f);

                }

                if (Micro_EnabledRef != Micro_Enabled || microOpacityRef != microOpacity)
                {
                    helper.Update_MicroShadowing(Micro_Enabled, microOpacity);

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.Micro_Enabled = Micro_Enabled;
                        LB_LightingProfile.microOpacity = microOpacity;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }

            #endregion
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

        }

        if (winMode == WindowMode.Finish) {

#region Anti Aliasing

			//-----------Anti Aliasing----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");

			var AA_EnabledRef = AA_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(aaState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
			AA_Enabled = EditorGUILayout.Toggle("",AA_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var aaStateRef = aaState;

			if (GUILayout.Button ("Anti Aliasing", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				aaState = !aaState;
			}

			if(aaStateRef != aaState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.aaState = aaState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------

			var aaModeRef = aaMode;

			if(aaState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox ("Activate Antialiasing for the camera", MessageType.Info);

				aaMode = (AAMode)EditorGUILayout.EnumPopup ("Anti Aliasing", aaMode, GUILayout.Width (343));
			}
			if (aaModeRef != aaMode  || AA_EnabledRef != AA_Enabled) {

					helper.Update_AA (mainCamera, aaMode, AA_Enabled);

					//----------------------------------------------------------------------
				if (LB_LightingProfile)
				{
					LB_LightingProfile.aaMode = aaMode;
					LB_LightingProfile.AA_Enabled = AA_Enabled;
				}

				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);


				}

#endregion

#region AO

			//-----------Ambient Occlusion----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");

			var AO_EnabledRef = AO_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(aoState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
			AO_Enabled = EditorGUILayout.Toggle("",AO_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var aoStateRef = aoState;

			if (GUILayout.Button ("Ambient Occlusion", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				aoState = !aoState;
			}

			if(aoStateRef != aoState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.aoState = aoState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------

			if(aoState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox ("Activate AO for the camera", MessageType.Info);
			}
				var aoIntensityRef = aoIntensity;
				var aoTypeRef = aoType;
				var aoRadiusRef = aoRadius;
				var aoQualityRef = aoQuality;
            var temporalRef = temporal;
            var directStrenthRef = directStrenth;
            var aoTemporalRef = aoTemporal;
            var ghostReducingRef = ghostReducing;



				if(aoState)
				{
					aoType = (AOType)EditorGUILayout.EnumPopup ("Quality", aoType, GUILayout.Width (343));
				}
				
				if(aoState)
				{
						aoIntensity = (float)EditorGUILayout.Slider ("Intensity", aoIntensity, 0, 2f);
                        aoRadius = (float)EditorGUILayout.Slider("Radius", aoRadius, 0, 4.3f);
                        directStrenth = (float)EditorGUILayout.Slider("Direct Strenth", directStrenth, 0, 4.3f);
                    	aoTemporal = (bool)EditorGUILayout.Toggle ("AO Temporal", aoTemporal);
                        ghostReducing = (float)EditorGUILayout.Slider("Ghost Reducing", ghostReducing, 0, 4.3f);

                }

            if (AO_EnabledRef != AO_Enabled || aoIntensityRef != aoIntensity || temporalRef != temporal
               || ghostReducingRef != ghostReducing  || aoTemporalRef != aoTemporal || aoTypeRef != aoType || aoRadiusRef != aoRadius || directStrenthRef != directStrenth || aoQualityRef != aoQuality) {

					if (AO_Enabled)
						helper.Update_AO (mainCamera, true, aoType, aoRadius, aoIntensity, directStrenth, aoTemporal, ghostReducing);
					if (!AO_Enabled)
						helper.Update_AO (mainCamera, false, aoType, aoRadius, aoIntensity, directStrenth, aoTemporal, ghostReducing);


                //----------------------------------------------------------------------
                if (LB_LightingProfile)
				{

                    LB_LightingProfile.AO_Enabled = AO_Enabled;
						LB_LightingProfile.aoIntensity = aoIntensity;
						LB_LightingProfile.temporal = temporal;
						LB_LightingProfile.directStrenth = directStrenth;
						LB_LightingProfile.aoRadius = aoRadius;
						LB_LightingProfile.aoType = aoType;
						LB_LightingProfile.aoQuality = aoQuality;
                    LB_LightingProfile.ghostReducing = ghostReducing;
                    LB_LightingProfile.aoTemporal = aoTemporal;
                    EditorUtility.SetDirty (LB_LightingProfile);
				}
			}

#endregion

#region Vignette


				//-----------Vignette----------------------------------------------------------------------------
				GUILayout.BeginVertical ("Box");

				var Vignette_EnabledRef = Vignette_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(vignetteState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
				Vignette_Enabled = EditorGUILayout.Toggle("",Vignette_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var vignetteStateRef = vignetteState;

			if (GUILayout.Button ("Vignette", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
					vignetteState = !vignetteState;
				}

			if(vignetteStateRef != vignetteState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.vignetteState = vignetteState;	
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

				EditorGUILayout.EndHorizontal ();

				GUILayout.EndVertical ();
				//---------------------------------------------------------------------------------------

				if(vignetteState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox ("Activate Vignette effect for your camera", MessageType.Info);				
				}

				var vignetteIntensityRef = vignetteIntensity;

				if(vignetteState)
					vignetteIntensity = EditorGUILayout.Slider("Intensity",vignetteIntensity,0,0.3f);


				if(Vignette_EnabledRef != Vignette_Enabled || vignetteIntensityRef != vignetteIntensity)
				{
					helper.Update_Vignette(Vignette_Enabled,vignetteIntensity);
				}

				if (LB_LightingProfile)
			{
					LB_LightingProfile.Vignette_Enabled = Vignette_Enabled;
					LB_LightingProfile.vignetteIntensity = vignetteIntensity;
					EditorUtility.SetDirty (LB_LightingProfile);
			}

#endregion

#region Motion Blur


				//-----------Motion Blur----------------------------------------------------------------------------
				GUILayout.BeginVertical ("Box");

				var MotionBlur_EnabledRef = MotionBlur_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(motionBlurState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
				MotionBlur_Enabled = EditorGUILayout.Toggle("",MotionBlur_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var motionBlurStateRef = motionBlurState;
            var blurQualityRef = blurQuality;
            var blurIntensityRef = blurIntensity;
            var blurMaxVelocityRef = blurMaxVelocity;

            if (GUILayout.Button ("Motion Blur", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
					motionBlurState = !motionBlurState;
				}
           



            if (motionBlurStateRef != motionBlurState)
			{
                if (LB_LightingProfile)
                    LB_LightingProfile.motionBlurState = motionBlurState; 
                if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

				EditorGUILayout.EndHorizontal ();

				GUILayout.EndVertical ();
				//---------------------------------------------------------------------------------------

				if(motionBlurState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox ("Activate Motion Blur effect for your camera", MessageType.Info);				
				}


            if (motionBlurState)
            {
                blurQuality = (MotionBlurQuality)EditorGUILayout.EnumPopup("Quality", blurQuality, GUILayout.Width(343));
                blurIntensity = EditorGUILayout.IntSlider("Intensity", blurIntensity, 0, 30);
                blurMaxVelocity = EditorGUILayout.IntSlider("Max Velocity", blurMaxVelocity, 0, 200);
            }

            if (MotionBlur_EnabledRef != MotionBlur_Enabled || blurQualityRef != blurQuality
                || blurIntensityRef != blurIntensity || blurMaxVelocityRef != blurMaxVelocity)
				{
                helper.Update_MotionBlur(MotionBlur_Enabled, blurIntensity, blurMaxVelocity
                , blurQuality);
            }

            if (LB_LightingProfile)
            {
                LB_LightingProfile.MotionBlur_Enabled = MotionBlur_Enabled;

                LB_LightingProfile.blurIntensity = blurIntensity;
                LB_LightingProfile.blurMaxVelocity = blurMaxVelocity;
                LB_LightingProfile.blurQuality = blurQuality;

                EditorUtility.SetDirty(LB_LightingProfile);

            }

#endregion

#region Chromattic Aberration


				//-----------Chromattic Aberration----------------------------------------------------------------------------
				GUILayout.BeginVertical ("Box");

				var Chromattic_EnabledRef = Chromattic_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(chromatticState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
				Chromattic_Enabled = EditorGUILayout.Toggle("",Chromattic_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var chromatticStateRef = chromatticState;

			if (GUILayout.Button ("Chromattic Aberration", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
					chromatticState = !chromatticState;
				}

			if(chromatticStateRef != chromatticState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.chromatticState = chromatticState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

				EditorGUILayout.EndHorizontal ();

				GUILayout.EndVertical ();
				//---------------------------------------------------------------------------------------

				if(chromatticState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox ("Activate Chromattic Aberration effect for your camera", MessageType.Info);				
				}

				var CA_IntensityRef = CA_Intensity;
				var mobileOptimizedChromatticRef = mobileOptimizedChromattic;

			if(chromatticState)
			{
				CA_Intensity = EditorGUILayout.Slider("Intensity", CA_Intensity, 0,1f ) ;
				//mobileOptimizedChromattic = EditorGUILayout.Toggle("Mobile Optimized",mobileOptimizedChromattic);
			}

				if(Chromattic_EnabledRef != Chromattic_Enabled || CA_IntensityRef != CA_Intensity 
				|| mobileOptimizedChromatticRef != mobileOptimizedChromattic)
				{
				helper.Update_ChromaticAberration(Chromattic_Enabled,CA_Intensity);
					}

			if (LB_LightingProfile)
			{
				LB_LightingProfile.Chromattic_Enabled = Chromattic_Enabled;
				LB_LightingProfile.CA_Intensity = CA_Intensity;
				EditorUtility.SetDirty (LB_LightingProfile);
			}

#endregion
            
#region Screen Space Reflections


				//-----------Screen Space Reflections----------------------------------------------------------------------------
				GUILayout.BeginVertical ("Box");

				var SSR_EnabledRef = SSR_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(ssrState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
				SSR_Enabled = EditorGUILayout.Toggle("",SSR_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var ssrStateRef = ssrState;

			if (GUILayout.Button ("Screen Space Reflections", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
					ssrState = !ssrState ;
				}

			if(ssrStateRef != ssrState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.ssrState = ssrState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

				EditorGUILayout.EndHorizontal ();

				GUILayout.EndVertical ();
				//---------------------------------------------------------------------------------------

				if(ssrState )
				{
					if (helpBox)
						EditorGUILayout.HelpBox ("Activate Screen Space Reflections effect for your camera", MessageType.Info);				
				}

			var minSmoothnessRef = minSmoothness;
			var edgeDistanceRef = edgeDistance;
            var ssrReflectSkyRef = ssrReflectSky;
            var ssrQUalityRef = ssrQUality;


			if(ssrState)
			{
                EditorGUILayout.HelpBox("SSR isn't available for Lit version", MessageType.Warning);

            }

#endregion

#region Screen Space GI


                //-----------Screen Space Global illumination----------------------------------------------------------------------------
                GUILayout.BeginVertical("Box");

                var SSGI_EnabledRef = SSGI_Enabled;

                EditorGUILayout.BeginHorizontal();

                if (ssgiState)
                    GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
                else
                    GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

                SSGI_Enabled = EditorGUILayout.Toggle("", SSGI_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

                var ssgiStateRef = ssgiState;

                if (GUILayout.Button("Screen Space GI", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
                {
                    ssgiState = !ssgiState;
                }

                if (ssgiStateRef != ssgiState)
                {
                    if (LB_LightingProfile)
                        LB_LightingProfile.ssgiState = ssgiState;
                    if (LB_LightingProfile)
                        EditorUtility.SetDirty(LB_LightingProfile);
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.EndVertical();
                //---------------------------------------------------------------------------------------

                if (ssgiState)
                {
                    if (helpBox)
                        EditorGUILayout.HelpBox("Activate Screen Space Globl illumination effect for your camera", MessageType.Info);
                }

                var ssgiQualityRef = ssgiQuality;


                if (ssgiState)
                {
                    ssgiQuality = (SSGIQuality)EditorGUILayout.EnumPopup("Quality", ssgiQuality, GUILayout.Width(343));
                

                }

                if (SSGI_EnabledRef != SSGI_Enabled || ssgiQualityRef != ssgiQuality)
                {
                    helper.Update_SSGI(mainCamera, SSGI_Enabled, ssgiQuality);

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.SSGI_Enabled = SSGI_Enabled;
                        LB_LightingProfile.ssgiQuality = ssgiQuality;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Buy the full version"))
                 Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/hdrp-lighting-box-2-nextgen-lighting-solution-180283");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            #endregion

            #region Check for updates

            /*if (GUILayout.Button ("Check for updates")) {

                EditorApplication.ExecuteMenuItem ("Assets/Lighting Box Updates");
            }*/

            EditorGUILayout.Space ();
				    EditorGUILayout.Space ();
				    EditorGUILayout.Space ();
				    EditorGUILayout.Space ();

#endregion

			}
			EditorGUILayout.EndScrollView ();
		}

#region Update Settings
		void UpdateSettings()
		{
		
			// Sun Light Update
			if (sunLight) {
				sunLight.color = sunColor;
            sunLight.GetComponent<HDAdditionalLightData>()
                                            .intensity = sunIntensity;
            sunLight.bounceIntensity = indirectIntensity;
			} else {
				Update_Sun ();
			}
                              

            // Update Lighting Mode
        helper.Update_LightingMode (Scene_Enabled,lightingMode,indirectDiffuse,indirectSpecular, probeIntensity);

			// Update Ambient
		helper.Update_Ambient (Ambient_Enabled,ambientLight, skyAmbientMode, skyCube, skyExposure,hdrRotation,skyTint,groundColor,tickness
			,gradientTop,gradientMiddle,gradientBottom,gradientDiffusion);
			// Lights settings
		helper.Update_LightSettings (Scene_Enabled,lightSettings);

			// Light Probes
		helper.Update_LightProbes(Scene_Enabled,lightprobeMode);

			// Auto Mode
			helper.Update_AutoMode(autoMode);

			// Global Fog
		helper.Update_GlobalFog(Fog_Enabled, baseHeight, fogAttenDistance, maxHeight, maxDistance, fogTint, fogColorMode, fogVolumetric, volumetricColor
            , vAnistropic);
        
        helper.Update_HDSHadows(HD_Enabled, CascadeCount,distance,split1,split2,split3);
		helper.Update_MicroShadowing(Micro_Enabled, microOpacity);

		}
#endregion

#region On Load
		// load saved data based on project and scene name
		void OnLoad()
		{

		if (!mainCamera) {
			if (GameObject.Find (LB_LightingProfile.mainCameraName))
				mainCamera = GameObject.Find (LB_LightingProfile.mainCameraName).GetComponent<Camera> ();
			else
				mainCamera = GameObject.FindObjectOfType<Camera> ();
		}
		
		if (!GameObject.Find ("LightingBox_Helper")){
			GameObject helperObject = new GameObject ("LightingBox_Helper");
			helperObject.AddComponent<LB_LightingBoxHelper> ();
			helper = helperObject.GetComponent<LB_LightingBoxHelper> ();
		}


		if (LB_LightingProfile) {

			lightingMode = LB_LightingProfile.lightingMode;
            ambientLight = LB_LightingProfile.ambientLight;
            skyAmbientMode = LB_LightingProfile.skyAmbientMode;
            lightSettings = LB_LightingProfile.lightSettings;
			sunColor = LB_LightingProfile.sunColor;
            
            indirectDiffuse = LB_LightingProfile.indirectDiffuse;
			indirectSpecular = LB_LightingProfile.indirectSpecular;
            probeIntensity = LB_LightingProfile.probeIntensity;
            

                       // Color Space
                       colorSpace = LB_LightingProfile.colorSpace;

			lightprobeMode = LB_LightingProfile.lightProbesMode;

            // Fog
            baseHeight = LB_LightingProfile.baseHeight;
            fogAttenDistance = LB_LightingProfile.fogAttenDistance;
            maxHeight = LB_LightingProfile.maxHeight;
            maxDistance = LB_LightingProfile.maxDistance;
            fogTint = LB_LightingProfile.fogTint;
            fogColorMode = LB_LightingProfile.fogColorMode;
            fogVolumetric = LB_LightingProfile.fogVolumetric;
            volumetricColor = LB_LightingProfile.volumetricColor;
            vAnistropic = LB_LightingProfile.vAnistropic;

            
            // Depth of Field
            dofFocusDistance = LB_LightingProfile.dofFocusDistance;
            dofQuality = LB_LightingProfile.dofQuality;
            dofMode = LB_LightingProfile.dofMode;

            // AA
            aaMode = LB_LightingProfile.aaMode;



            var aoIntensityRef = aoIntensity;
            var aoTypeRef = aoType;
            var aoRadiusRef = aoRadius;
            var aoQualityRef = aoQuality;
            var temporalRef = temporal;
            var directStrenthRef = directStrenth;
            var aoTemporalRef = aoTemporal;
            var ghostReducingRef = ghostReducing;


            // AO
            aoIntensity = LB_LightingProfile.aoIntensity;
            temporal = LB_LightingProfile.temporal;
            directStrenth = LB_LightingProfile.directStrenth;
			aoRadius = LB_LightingProfile.aoRadius;
			aoType = LB_LightingProfile.aoType;
			aoQuality = LB_LightingProfile.aoQuality;
            aoTemporal = LB_LightingProfile.aoTemporal;
            ghostReducing = LB_LightingProfile.ghostReducing;

            // Bloom
            bIntensity = LB_LightingProfile.bIntensity;
			bColor = LB_LightingProfile.bColor;
			bThreshould = LB_LightingProfile.bThreshould;
			dirtTexture = LB_LightingProfile.dirtTexture;
			dirtIntensity = LB_LightingProfile.dirtIntensity;
			mobileOptimizedBloom = LB_LightingProfile.mobileOptimizedBloom;
			bRotation = LB_LightingProfile.bRotation;

            // Color Grading
            compensation = LB_LightingProfile.compensation;
            compensationFixed = LB_LightingProfile.compensationFixed;
            fixedExposure = LB_LightingProfile.fixedExposure;
            lb_exposureMode = LB_LightingProfile.lb_exposureMode;

            exposureIntensity = LB_LightingProfile.exposureIntensity;
			contrastValue = LB_LightingProfile.contrastValue;
			temp = LB_LightingProfile.temp;
			eyeKeyValue1 = LB_LightingProfile.eyeKeyValue1;
            eyeKeyValue2 = LB_LightingProfile.eyeKeyValue2;
            meteringMode = LB_LightingProfile.meteringMode;
            colorMode = LB_LightingProfile.colorMode;
			colorGamma = LB_LightingProfile.colorGamma;
			colorLift = LB_LightingProfile.colorLift;
			gamma = LB_LightingProfile.gamma;
			saturation = LB_LightingProfile.saturation;
			lut = LB_LightingProfile.lut;

            // Snow
            snowAlbedo = LB_LightingProfile.snowAlbedo;
            snowNormal = LB_LightingProfile.snowNormal;
            snowIntensity = LB_LightingProfile.snowIntensity;


            // Motion Blur
            blurIntensity = LB_LightingProfile.blurIntensity;
            blurMaxVelocity = LB_LightingProfile.blurMaxVelocity;
            blurQuality = LB_LightingProfile.blurQuality;

            // Effects
            MotionBlur_Enabled = LB_LightingProfile.MotionBlur_Enabled;
			Vignette_Enabled = LB_LightingProfile.Vignette_Enabled;
			vignetteIntensity = LB_LightingProfile.vignetteIntensity;
			Chromattic_Enabled = LB_LightingProfile.Chromattic_Enabled;
			CA_Intensity = LB_LightingProfile.CA_Intensity;

			// HD SHadows
			CascadeCount = LB_LightingProfile.CascadeCount;
			distance = LB_LightingProfile.distance;
			split1 = LB_LightingProfile.split1;
			split2 = LB_LightingProfile.split2;
			split3 = LB_LightingProfile.split3;
			microOpacity = LB_LightingProfile.microOpacity;
            HD_Enabled = LB_LightingProfile.HD_Enabled;

            // SSR
            SSR_Enabled = LB_LightingProfile.SSR_Enabled;
            minSmoothness = LB_LightingProfile.minSmoothness;
            edgeDistance = LB_LightingProfile.edgeDistance;
            ssrReflectSky = LB_LightingProfile.ssrReflectSky;

            // SSGI
            SSGI_Enabled = LB_LightingProfile.SSGI_Enabled;
            ssgiQuality = LB_LightingProfile.ssgiQuality;

            // Lightmap
            bakedResolution = LB_LightingProfile.bakedResolution;
			sunIntensity = LB_LightingProfile.sunIntensity;
			indirectIntensity = LB_LightingProfile.indirectIntensity;

			skyCube = LB_LightingProfile.skyCube;
            ambientLight = LB_LightingProfile.ambientLight;
            skyAmbientMode = LB_LightingProfile.skyAmbientMode;
            skyExposure = LB_LightingProfile.skyExposure;
			hdrRotation = LB_LightingProfile.hdrRotation;
			groundColor = LB_LightingProfile.groundColor;
			skyTint = LB_LightingProfile.skyTint;
			tickness = LB_LightingProfile.tickness;
            
            gradientTop = LB_LightingProfile.gradientTop;
			gradientMiddle = LB_LightingProfile.gradientMiddle;
			gradientBottom = LB_LightingProfile.gradientBottom;
			gradientDiffusion = LB_LightingProfile.gradientDiffusion;

			// Auto lightmap
			autoMode = LB_LightingProfile.automaticLightmap;

			// WebGL
		//	webGL_Mobile = LB_LightingProfile.webGL_Mobile;

			Ambient_Enabled = LB_LightingProfile.Ambient_Enabled;
			Scene_Enabled = LB_LightingProfile.Scene_Enabled;
			Sun_Enabled = LB_LightingProfile.Sun_Enabled;
			Fog_Enabled = LB_LightingProfile.Fog_Enabled;
			DOF_Enabled = LB_LightingProfile.DOF_Enabled;
			Bloom_Enabled = LB_LightingProfile.Bloom_Enabled;
			AA_Enabled = LB_LightingProfile.AA_Enabled;
			AO_Enabled = LB_LightingProfile.AO_Enabled;
			Micro_Enabled = LB_LightingProfile.Micro_Enabled;

            buildState = LB_LightingProfile.buildState;
			profileState = LB_LightingProfile.profileState;
			cameraState = LB_LightingProfile.cameraState;
			lightSettingsState = LB_LightingProfile.lightSettingsState;
			sunState = LB_LightingProfile.sunState;
			ambientState = LB_LightingProfile.ambientState;
			ssrState = LB_LightingProfile.ssrState;
			hdState = LB_LightingProfile.hdState;
			MicroState = LB_LightingProfile.MicroState;
            ssgiState = LB_LightingProfile.ssgiState;


            chromatticState = LB_LightingProfile.chromatticState;
			vignetteState = LB_LightingProfile.vignetteState;
			motionBlurState = LB_LightingProfile.motionBlurState;
			aoState = LB_LightingProfile.aoState;
			aaState = LB_LightingProfile.aaState;
			bloomState = LB_LightingProfile.bloomState;
			colorState = LB_LightingProfile.colorState;
			dofState = LB_LightingProfile.dofState;
			fogState = LB_LightingProfile.fogState;
			OptionsState = LB_LightingProfile.OptionsState;
			LightingBoxState = LB_LightingProfile.LightingBoxState;

			mainCamera.allowHDR = false;
			mainCamera.allowMSAA = false;

			/*if (LB_LightingProfile.postProcessingProfile)
				postProcessingProfile = LB_LightingProfile.postProcessingProfile;*/
		}

			UpdatePostEffects ();

			UpdateSettings ();

			Update_Sun();

	}
#endregion

#region Update Post Effects Settings

		public void UpdatePostEffects()
		{

			if(!helper)
				helper = GameObject.Find("LightingBox_Helper").GetComponent<LB_LightingBoxHelper> ();

		//	if (!postProcessingProfile)
				//return;

			helper.UpdateProfiles (mainCamera,/* postProcessingProfile,*/volumeProfileMain);

        // MotionBlur
        if (MotionBlur_Enabled)
        {
            helper.Update_MotionBlur(true, blurIntensity, blurMaxVelocity
                , blurQuality);
        }
        else
        {
            helper.Update_MotionBlur(false, blurIntensity, blurMaxVelocity
                            , blurQuality);
        }

			// Vignette
			helper.Update_Vignette (Vignette_Enabled,vignetteIntensity);


		// _ChromaticAberration
		helper.Update_ChromaticAberration(Chromattic_Enabled,CA_Intensity);

		helper.Update_Bloom(Bloom_Enabled,bIntensity,bThreshould,bColor,dirtTexture,dirtIntensity,mobileOptimizedBloom,bRotation);

			// Depth of Field 12
			helper.Update_DOF(DOF_Enabled,dofFocusDistance,dofQuality, dofMode);

			// AO
		if (AO_Enabled)
				helper.Update_AO(mainCamera,true,aoType,aoRadius,aoIntensity, directStrenth, aoTemporal, ghostReducing);
			else
				helper.Update_AO(mainCamera,false,aoType,aoRadius,aoIntensity, directStrenth, aoTemporal, ghostReducing);


        // Color Grading
        helper.Update_ColorGrading(meteringMode, colorMode, exposureIntensity, contrastValue, temp, eyeKeyValue1, eyeKeyValue2, saturation, colorGamma, colorLift, gamma, lut, lb_exposureMode, compensation,compensationFixed, fixedExposure);

        ////-----------------------------------------------------------------------------
        /// 
        // Screen Space Reflections
        helper.Update_SSR(mainCamera, SSR_Enabled, minSmoothness, edgeDistance, ssrReflectSky, ssrQUality);

        helper.Update_SSGI(mainCamera, SSGI_Enabled, ssgiQuality);

    }
#endregion

#region Scene Delegate

    string currentScene;    
		void SceneChanging ()
	{
		if (currentScene != EditorSceneManager.GetActiveScene ().name) {
			if (System.String.IsNullOrEmpty (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name)))
				LB_LightingProfile = Resources.Load ("DefaultSettings")as LB_LightingProfile;
			else
				LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name), typeof(LB_LightingProfile));

			helper.Update_MainProfile(LB_LightingProfile,volumeProfileMain);

			OnLoad ();
			currentScene = EditorSceneManager.GetActiveScene ().name;
		}

	}
#endregion

#region Sun Light
		public void Update_Sun()
		{
		if (Sun_Enabled) {
				Light[] lights = GameObject.FindObjectsOfType<Light> ();
				foreach (Light l in lights) {
					if (l.type == LightType.Directional) {
						sunLight = l;

						if (sunColor != Color.clear)
							sunColor = l.color;
						else
							sunColor = Color.white;

                    //sunLight.shadowNormalBias = 0.05f;  
                    sunLight.GetComponent<HDAdditionalLightData>()
                        .color = sunColor;

                        if (sunLight.bounceIntensity == 1f)
							sunLight.bounceIntensity = indirectIntensity;
					}
				}
		}
	}

#endregion

#region On Download Completed
		void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
	{
		if (e.Error != null)
			Debug.Log (e.Error);
	}
#endregion
}
#endif