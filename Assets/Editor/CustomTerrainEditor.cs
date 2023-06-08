using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]
public class CustomTerrainEditor : Editor
{
    // fold outs -------
    private bool showRandom = false;
    private bool showLoadHeight = false;
    private bool showPerlin = false;
    private bool showMultiplePerlin = false;
    private bool showVoronoi = false;
    private bool showSplats = false;
    private bool showVegetation = false;

    // properties -------
    private SerializedProperty resetTerrain;
    private SerializedProperty randomHeightRange;
    private SerializedProperty heightMapScale;
    private SerializedProperty heightMapImage;
    private SerializedProperty perlinXScale;
    private SerializedProperty perlinYScale;
    private SerializedProperty perlinOffsetX;
    private SerializedProperty perlinOffsetY;
    private SerializedProperty perlinOctaves;
    private SerializedProperty perlinPersistance;
    private SerializedProperty perlinHeightScale;
    private SerializedProperty splatOffset;
    private SerializedProperty splatOffsetFactor;
    private SerializedProperty splatPerlinXScale;
    private SerializedProperty splatPerlinYScale;
    private SerializedProperty splatPerlinFactor;

    private GUITableState perlinParameterTable;
    private SerializedProperty perlinParameters;

    private GUITableState splatHeightTable;
    private SerializedProperty splatHeights;

    private GUITableState vegetationTable;
    private SerializedProperty vegetations;
    private SerializedProperty maxTrees;
    private SerializedProperty treeSpacing;

    private void OnEnable()
    {
        resetTerrain = serializedObject.FindProperty("resetTerrain");
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        heightMapScale = serializedObject.FindProperty("heightMapScale");
        heightMapImage = serializedObject.FindProperty("heightMapImage");
        perlinXScale = serializedObject.FindProperty("perlinXScale");
        perlinYScale = serializedObject.FindProperty("perlinYScale");
        perlinOffsetX = serializedObject.FindProperty("perlinOffsetX");
        perlinOffsetY = serializedObject.FindProperty("perlinOffsetY");
        perlinOctaves = serializedObject.FindProperty("perlinOctaves");
        perlinPersistance = serializedObject.FindProperty("perlinPersistance");
        perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");
        perlinParameterTable = new GUITableState("perlinParametersTable");
        perlinParameters = serializedObject.FindProperty("perlinParameters");
        splatHeightTable = new GUITableState("splatHeightTable");
        splatHeights = serializedObject.FindProperty("splatHeights");
        splatOffset = serializedObject.FindProperty("splatOffset");
        splatOffsetFactor = serializedObject.FindProperty("splatOffsetFactor");
        splatPerlinXScale = serializedObject.FindProperty("splatPerlinXScale");
        splatPerlinYScale = serializedObject.FindProperty("splatPerlinYScale");
        splatPerlinFactor = serializedObject.FindProperty("splatPerlinFactor");
        vegetationTable = new GUITableState("vegetationTable");
        vegetations = serializedObject.FindProperty("vegetations");
        maxTrees = serializedObject.FindProperty("maxTrees");
        treeSpacing = serializedObject.FindProperty("treeSpacing");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CustomTerrain terrain = (CustomTerrain) target;
        EditorGUILayout.PropertyField(resetTerrain);

        showRandom = EditorGUILayout.Foldout(showRandom, "Random");
        if (showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set heights between random values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHeightRange);
            if (GUILayout.Button("Random Heights"))
            {
                terrain.RandomTerrain();
            }   
        }

        showLoadHeight = EditorGUILayout.Foldout(showLoadHeight, "Load Heights");
        if(showLoadHeight)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("LoadHeights from texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(heightMapImage);
            EditorGUILayout.PropertyField(heightMapScale);
            if (GUILayout.Button("Load Texture"))
            {
                terrain.LoadTexture();
            }
        }

        showPerlin = EditorGUILayout.Foldout(showPerlin, "Perlin Noise");
        if (showPerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights Perlin Noise", EditorStyles.boldLabel);
            EditorGUILayout.Slider(perlinXScale, 0, 0.1f, new GUIContent("X Scale"));
            EditorGUILayout.Slider(perlinYScale, 0, 0.1f, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinOffsetX, 0, 10000, new GUIContent("X Offset"));
            EditorGUILayout.IntSlider(perlinOffsetY, 0, 10000, new GUIContent("Y Offset"));
            EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
            EditorGUILayout.Slider(perlinPersistance, 0.1f, 10, new GUIContent("Persistance"));
            EditorGUILayout.Slider(perlinHeightScale, 0, 1, new GUIContent("Height Scale"));
            if (GUILayout.Button("Create Perlin Terrain"))
            {
                terrain.Perlin();
            }
        }

        showMultiplePerlin = EditorGUILayout.Foldout(showMultiplePerlin, "Multiple Perlin Noise");
        if (showMultiplePerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Multiple Perlin Noise", EditorStyles.boldLabel);
            perlinParameterTable = GUITableLayout.DrawTable(perlinParameterTable, perlinParameters);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button("+"))
            {
                terrain.AddNewPerlin();
            }
            if (GUILayout.Button("-"))
            {
                terrain.RemovePerlin();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Apply Multiple Perlin"))
            {
                terrain.MultiplePerlinTerrain();
            }
        }

        showSplats = EditorGUILayout.Foldout(showSplats, "Splat Maps");
        if (showSplats)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Splat Maps", EditorStyles.boldLabel);
            splatHeightTable = GUITableLayout.DrawTable(splatHeightTable, splatHeights);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button("+"))
            {
                terrain.AddSplatHeight();
            }
            if (GUILayout.Button("-"))
            {
                terrain.RemoveSplatHeight();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(splatOffset);
            EditorGUILayout.PropertyField(splatOffsetFactor);
            EditorGUILayout.PropertyField(splatPerlinXScale);
            EditorGUILayout.PropertyField(splatPerlinYScale);
            EditorGUILayout.PropertyField(splatPerlinFactor);
            if (GUILayout.Button("Apply SplatMaps"))
            {
                terrain.SplatMaps();
            }
        }

        showVoronoi = EditorGUILayout.Foldout(showVoronoi, "Voronoi");
        if (showVoronoi)
        {
            if (GUILayout.Button("Voronoi"))
            {
                terrain.Voronoi();
            }
        }

        showVegetation = EditorGUILayout.Foldout(showVegetation, "Vegetation");
        if (showVegetation)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Vegetation", EditorStyles.boldLabel);
            EditorGUILayout.IntSlider(maxTrees, 1, 10000, "Maximum Trees");
            EditorGUILayout.IntSlider(treeSpacing, 2, 20, "Tree Spacing");
            vegetationTable = GUITableLayout.DrawTable(vegetationTable, vegetations);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button("+"))
            {
                terrain.AddNewVegetation();
            }
            if (GUILayout.Button("-"))
            {
                terrain.RemoveVegetation();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Apply Vegetation"))
            {
                terrain.ApplyVegetation();
            }

        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Reset Terrain"))
        {
            terrain.ResetTerrain();
        }

        serializedObject.ApplyModifiedProperties();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
