using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;

[ExecuteInEditMode]
public class CustomTerrain : MonoBehaviour
{
    public Vector2 randomHeightRange = new Vector2(0, 0.1f);
    public Texture2D heightMapImage;
    public Vector3 heightMapScale = Vector3.one;

    public bool resetTerrain = true;

    // Single Perlin Noise -----------------
    public float perlinXScale = 0.01f;
    public float perlinYScale = 0.01f;
    public int perlinOffsetX = 0;
    public int perlinOffsetY = 0;
    public int perlinOctaves = 3;
    public float perlinPersistance = 8;
    public float perlinHeightScale = 0.09f;

    // Multiple Perlin Noises -------------------
    [System.Serializable]
    public class PerlinParameters
    {
        public float perlinXScale = 0.01f;
        public float perlinYScale = 0.01f;
        public int perlinOctaves = 3;
        public float perlinPersistance = 8;
        public float perlinHeightScale = 0.09f;
        public int perlinOffsetX = 0;
        public int perlinOffsetY = 0;
        public bool remove = false;
    }

    public List<PerlinParameters> perlinParameters = new List<PerlinParameters>()
    {
        new PerlinParameters()
    };

    // Splatmaps -----------------------
    [System.Serializable]
    public class SplatHeight
    {
        public Texture2D texture = null;
        public float minHeight = 0.1f;
        public float maxHeight = 0.2f;
        public float minSlope = 0;
        public float maxSlope = 1.5f;
        public Vector2 tileOffset = new Vector2(0,0);
        public Vector2 tileSize = new Vector2(50,50);
        public bool remove = false;
    }

    public List<SplatHeight> splatHeights = new List<SplatHeight>()
    {
        new SplatHeight()
    };

    public float splatOffset = 0.01f;
    public float splatOffsetFactor = 0.02f;
    public float splatPerlinXScale = 0.01f;
    public float splatPerlinYScale = 0.01f;
    public float splatPerlinFactor = 0.05f;

    // Vegetation --------------------
    [System.Serializable]
    public class Vegetation
    {
        public GameObject mesh;
        public float minHeight = 0.1f;
        public float maxHeight = 0.2f;
        public float minSlope = 0;
        public float maxSlope = 1;
        public bool remove = false;
    }

    public List<Vegetation> vegetations = new List<Vegetation>()
    {
        new Vegetation()
    };

    public int maxTrees = 5000;
    public int treeSpacing = 5;

    public Terrain terrain;
    public TerrainData terrainData;

    private float[,] GetHeightMap()
    {
        if (!resetTerrain)
        {
            return terrainData.GetHeights(0, 0,
            terrainData.heightmapResolution, terrainData.heightmapResolution);
        } else
        {
            return new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        }
    }

    public void Voronoi()
    {
        float[,] heightMap = GetHeightMap();
        float fallOff = 2f;
        float dropoff = 0.5f;
        Vector3 peak = new Vector3(256, 0.25f, 256)
            
            
            /*new Vector3(
            UnityEngine.Random.Range(0, terrainData.heightmapResolution),
            UnityEngine.Random.Range(0.0f, 1.0f),
            UnityEngine.Random.Range(0, terrainData.heightmapResolution));*/
;
        heightMap[(int)peak.x, (int)peak.z] = peak.y;

        Vector2 peakLocation = new Vector2(peak.x, peak.z);
        float maxDistance = Vector2.Distance(new Vector2(0,0), new Vector2(terrainData.heightmapResolution, terrainData.heightmapResolution));

        for (int y = 0; y <terrainData.heightmapResolution ; y++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                if(!(x == peak.x && y == peak.z))
                {
                    float distanceToPeak = Vector2.Distance(peakLocation, new Vector2 (x,y)) /maxDistance;
                    heightMap[x, y] = peak.y - distanceToPeak * fallOff - (float)Math.Pow(distanceToPeak, dropoff);
                    //heightMap[x, y] = peak.y - Mathf.Sin(distanceToPeak * 100) * 0.01f;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    public void Perlin()
    {
        float[,] heightMap = GetHeightMap();
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                heightMap[x, y] += Utils.fBM(
                    (x + perlinOffsetX) * perlinXScale,
                    (y + perlinOffsetY) * perlinYScale,
                    perlinOctaves,
                    perlinPersistance) * perlinHeightScale;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void MultiplePerlinTerrain()
    {
        float[,] heightMap = GetHeightMap();
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                foreach (PerlinParameters p in perlinParameters)
                {
                    heightMap[x,y] += Utils.fBM(
                        (x + p.perlinOffsetX) * p.perlinXScale,
                        (y + p.perlinOffsetY) * p.perlinYScale,
                        p.perlinOctaves,
                        p.perlinPersistance) * p.perlinHeightScale;
                }
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void AddNewPerlin()
    {
        perlinParameters.Add(new PerlinParameters());
    }

    public void RemovePerlin() 
    {
        List<PerlinParameters> keptPerlinParameters = new List<PerlinParameters>();
        for (int i = 0; i < perlinParameters.Count; i++)
        {
            if (!perlinParameters[i].remove)
            {
                keptPerlinParameters.Add(perlinParameters[i]);
            }
        }
        if (keptPerlinParameters.Count == 0)
        {
            keptPerlinParameters.Add(perlinParameters[0]);
        }
        perlinParameters = keptPerlinParameters;

    }

    public void AddSplatHeight()
    {
        splatHeights.Add(new SplatHeight());
    }

    public void RemoveSplatHeight()
    {
        List<SplatHeight> keptSplatHeights = new List<SplatHeight>();
        for (int i = 0; i < splatHeights.Count; i++)
        {
            if (!splatHeights[i].remove)
            {
                keptSplatHeights.Add(splatHeights[i]);
            }
        }
        if (keptSplatHeights.Count == 0)
        {
            keptSplatHeights.Add(splatHeights[0]);
        }
        splatHeights = keptSplatHeights;

    }

    private float GetSteepness(float[,] heightMap, int x, int y, int width, int height)
    {
        // Get current vertex point in the map
        float h = heightMap[x, y];

        // find the next vertices
        int nx = x + 1;
        int ny = y + 1;

        // if on the upper edge of the map, find gradient by going backward.
        if (nx > width - 1) nx = x - 1;
        if (ny > height - 1) ny = y - 1;

        // find values of neightbor
        float dx = heightMap[nx, y] - h;
        float dy = heightMap[x, ny] - h;

        // create the gradient
        Vector2 gradient = new Vector2(dx, dy);

        float steep = gradient.magnitude;

        return steep;
    }

    public void AddNewVegetation()
    {
        vegetations.Add(new Vegetation());

    }

    public void RemoveVegetation()
    {
        List<Vegetation> keptVegetations = new List<Vegetation>();
        for (int i = 0; i < vegetations.Count; i++)
        {
            if (!vegetations[i].remove)
            {
                keptVegetations.Add(vegetations[i]);
            }
        }
        if (keptVegetations.Count == 0)
        {
            keptVegetations.Add(vegetations[0]);
        }
        vegetations = keptVegetations;
    }

    public void ApplyVegetation()
    {
        TreePrototype[] newTreePrototypes;
        newTreePrototypes = new TreePrototype[vegetations.Count];
        int tIndex = 0;
        foreach (Vegetation t in vegetations)
        {
            newTreePrototypes[tIndex] = new TreePrototype();
            newTreePrototypes[tIndex].prefab = t.mesh;
            tIndex++;
        }
        terrainData.treePrototypes = newTreePrototypes;

        List<TreeInstance> allVegetation = new List<TreeInstance> ();
        Transform thisTransform = transform;
        for (int z = 0; z < terrainData.size.z; z+=treeSpacing)
        {
            for (int x = 0; x < terrainData.size.x; x += treeSpacing)
            {
                for (int tp = 0; tp < terrainData.treePrototypes.Length; tp++)
                {
                    float thisHeight = terrainData.GetHeight(x, z) / terrainData.size.y;
                    float thisHeightStart = vegetations[tp].minHeight;
                    float thisHeightStop = vegetations[tp].maxHeight;
                    if (thisHeight < thisHeightStart || thisHeight > thisHeightStop)
                        continue;
                    TreeInstance instance = new TreeInstance();
                    instance.position = new Vector3(
                        (x + UnityEngine.Random.Range(-5f, 5f)) / terrainData.size.x,
                        thisHeight,
                        (z + UnityEngine.Random.Range(-5f, 5f)) / terrainData.size.z);

                    Vector3 treeWorldPos = new Vector3(
                        instance.position.x * terrainData.size.x,
                        instance.position.y * terrainData.size.y,
                        instance.position.z * terrainData.size.z) + thisTransform.position;

                    RaycastHit hit;
                    int layerMask = 1 << terrainLayer;
                    if ((Physics.Raycast(treeWorldPos, -Vector3.up, out hit, 100, layerMask)) || (Physics.Raycast(treeWorldPos, Vector3.up, out hit, 100, layerMask)))
                    {
                        float treeHeight = (hit.point.y - thisTransform.position.y) / terrainData.size.y;
                        instance.position = new Vector3(instance.position.x, treeHeight, instance.position.z);
                        instance.rotation = UnityEngine.Random.Range(0, 360);
                        instance.prototypeIndex = tp;
                        instance.color = Color.white;
                        instance.lightmapColor = Color.white;
                        instance.heightScale = 0.95f;
                        instance.widthScale = 0.95f;

                        allVegetation.Add(instance);
                        if (allVegetation.Count >= maxTrees) goto TREESDONE;
                    }
                }
            }
        }
        TREESDONE:
            terrainData.treeInstances = allVegetation.ToArray();
    }
    
    public void SplatMaps()
    {
        TerrainLayer[] newTerrainLayers;
        newTerrainLayers = new TerrainLayer[splatHeights.Count];
        int spindex = 0;
        foreach (SplatHeight sh in splatHeights)
        {
            newTerrainLayers[spindex] = new TerrainLayer();
            newTerrainLayers[spindex].diffuseTexture = sh.texture;
            newTerrainLayers[spindex].tileOffset = sh.tileOffset;
            newTerrainLayers[spindex].tileSize = sh.tileSize;
            newTerrainLayers[spindex].diffuseTexture.Apply(true);
            spindex++;
        }
        terrainData.terrainLayers = newTerrainLayers;

        float[,] heightMap = terrainData.GetHeights(0, 0,
            terrainData.heightmapResolution, terrainData.heightmapResolution);
        float[,,] splatMapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float[] splat = new float[terrainData.alphamapLayers];
                for (int i = 0; i < splatHeights.Count; i++)
                {
                    float noise = Mathf.PerlinNoise(x * splatPerlinXScale, y * splatPerlinYScale) * splatPerlinFactor;
                    float offset = (splatOffset + noise) * splatOffsetFactor;
                    float thisHeightStart = splatHeights[i].minHeight - offset;
                    float thisHeightStop = splatHeights[i].maxHeight + offset;
                    float steepness = GetSteepness(
                        heightMap, x, y, 
                        terrainData.heightmapResolution, 
                        terrainData.heightmapResolution);
                    if ((heightMap[x, y] >= thisHeightStart && heightMap[x, y] <= thisHeightStop) && (steepness >= splatHeights[i].minSlope && steepness <= splatHeights[i].maxSlope))
                    {
                        splat[i] = 1;
                    }
                }
                NormalizeVector(splat);
                for (int j = 0; j < splatHeights.Count; j++)
                {
                    splatMapData[x, y, j] = splat[j];
                }
            }
        }
        terrainData.SetAlphamaps(0, 0, splatMapData);
    }

    private void NormalizeVector(float[] v)
    {
        float total = 0;
        for (int i = 0; i < v.Length; i++)
        {
            total += v[i];
        }

        for (int i = 0; i < v.Length; i++)
        {
            v[i] /= total;
        }
    }

    public void RandomTerrain()
    {
        float[,] heightMap = GetHeightMap();
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                heightMap[x, y] += UnityEngine.Random.Range(randomHeightRange.x, randomHeightRange.y);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void LoadTexture()
    {
        float[,] heightMap = GetHeightMap();
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < terrainData.heightmapResolution; z++)
            {
                heightMap[x, z] += heightMapImage.GetPixel((int)(x * heightMapScale.x), (int)(z * heightMapScale.z)).grayscale * heightMapScale.y;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void ResetTerrain()
    {
        float[,] heightMap;
        heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                heightMap[x, y] = 0;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    private void OnEnable()
    {
        Debug.Log("Initialising Terrain Data");
        terrain = this.GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;
    }

    public enum TagType { Tag = 0, Layer = 1}
    [SerializeField] int terrainLayer = -1;
    private void Awake()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        AddTag(tagsProp, "Terrain", TagType.Tag);
        AddTag(tagsProp, "Cloud", TagType.Tag);
        AddTag(tagsProp, "Shore", TagType.Tag);
        
        tagManager.ApplyModifiedProperties();

        SerializedProperty layerProp = tagManager.FindProperty("layers");
        terrainLayer = AddTag(layerProp, "Terrain", TagType.Layer);
        tagManager.ApplyModifiedProperties();

        gameObject.tag = "Terrain";
        gameObject.layer = terrainLayer;
    }

    private int AddTag(SerializedProperty tagsProp, string newTag, TagType tType)
    {
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag)) { found = true; return i; }
        }
        if (!found && tType == TagType.Tag)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
            newTagProp.stringValue = newTag;
        } else if (!found && tType == TagType.Layer)
        {
            for (int j = 6 ; j < tagsProp.arraySize; j++)
            {
                SerializedProperty newLayer = tagsProp.GetArrayElementAtIndex(j);
                if (newLayer.stringValue == "")
                {
                    newLayer.stringValue = newTag;
                    return j;
                }
            }
        }
        return -1;
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
