using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    public List<TestModuleSO> possibleModules;

    List<int> possibleIds = new List<int>();

    Dictionary<int, int[]> connections = new Dictionary<int, int[]>();

    int moduleWidth;
    int moduleDepth;

    [SerializeField] private int width = 5;
    [SerializeField] private int depth = 5;
    [SerializeField] float delay = 1.0f;

    private void Awake()
    {
        //SetUpWFC();
    }

    private void SetUpWFC()
    {
        moduleWidth = (int)possibleModules[0].prefab.transform.localScale.x;
        moduleDepth = (int)possibleModules[0].prefab.transform.localScale.z;
        possibleIds.Clear();
        foreach (TestModuleSO moduleSO in possibleModules)
        {
            connections[moduleSO.Id] = moduleSO.connections;
            possibleIds.Add(moduleSO.Id);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //RunWFC();
    }

    public void RunWFC()
    {
        ResetGrid();
        SetUpWFC();
        List<int>[,] grid = CreateRandomGrid(width, depth);
        WaveFunctionCollapse(grid, width, depth);
        StartCoroutine(GenerateGridVisualCoroutine(grid, width, depth));
    }
    public void RunWFCInEditor()
    {
        ResetGrid();
        SetUpWFC();
        List<int>[,] grid = CreateRandomGrid(width, depth);
        WaveFunctionCollapse(grid, width, depth);
        GenerateGridVisual(grid, width, depth);
    }


    public void ResetGrid()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("TestModule");
        foreach(GameObject cube in cubes)
        {
            DestroyImmediate(cube);
        }
    }

    private void GenerateGridVisual(List<int>[,] grid, int width, int depth)
    {
        for (int x = 0; x < width; x += moduleWidth)
        {
            for (int z = 0; z < depth; z += moduleDepth)
            {
                int id = grid[x, z][0];
                Instantiate(possibleModules[id].prefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }

    private IEnumerator GenerateGridVisualCoroutine(List<int>[,] grid, int width, int depth)
    {
        for (int x = 0; x < width; x+=moduleWidth)
        {
            for (int z = 0; z < depth; z+=moduleDepth)
            {
                int id = grid[x, z][0];
                Instantiate(possibleModules[id].prefab, new Vector3(x, 0, z), Quaternion.identity);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private List<int>[,] CreateRandomGrid(int width, int depth)
    {
        List<int>[,] grid = new List<int>[width, depth];
        for (int z = 0; z < depth; z += moduleDepth)
        {
            for (int x = 0; x < width; x+=moduleWidth)
            {
                grid[x, z] = new List<int>(possibleIds);
            }
        }
        return grid;
    }

    private MapLocation GetMinEntropyPosition(List<int>[,] grid, int width, int depth)
    {
        // Placeholder for list with minimum entropy
        List<int> minEntropyList = null;

        // Placeholder for position with minimum entropy
        MapLocation position = new MapLocation(0, 0);

        // Go through grid and find position with least entropy
        for (int z = 0; z < depth; z+=moduleDepth)
        {
            for (int x = 0; x < width; x+=moduleWidth)
            {
                List<int> currentEntry = grid[x, z];
                if ((minEntropyList == null || (currentEntry.Count < minEntropyList.Count)) && (currentEntry.Count > 1))
                {
                    minEntropyList = currentEntry;
                    position = new MapLocation(x, z);
                }
            }
        }
        return position;
    }

    private void WaveFunctionCollapse(List<int>[,] grid, int width, int depth) 
    {
        // Grab position with smallest entropy
        MapLocation positionToCollapse = GetMinEntropyPosition(grid, width, depth);

        // Set active list to be list at position of smallest entropy
        List<int> currentCell = grid[positionToCollapse.x, positionToCollapse.z];

        // Run loop while there is a cell with entropy greater than one
        while (currentCell.Count > 1)
        {
            // Pull a random value from available positions and check if value can be used
            int value = GetValidValue(grid, width, depth, positionToCollapse, currentCell);

            // Place it as sole item of the cell
            currentCell.Clear();
            currentCell.Add(value);

            CollapseNeighbors(positionToCollapse, grid);

            positionToCollapse = GetMinEntropyPosition(grid, width, depth);
            currentCell = grid[positionToCollapse.x, positionToCollapse.z];
        }
    }

    private int GetValidValue(List<int>[,] grid, int width, int depth, MapLocation positionToCollapse, List<int> currentCell)
    {
        int value = GetRandomIndexFromList(currentCell);

        List<MapLocation> neighbors = GetNeighbors(positionToCollapse);
        foreach (MapLocation neighbor in neighbors)
        {
            if (IsNeighborValid(neighbor, width, depth) && grid[neighbor.x, neighbor.z].Count == 1)
            {
                if (!connections[grid[neighbor.x, neighbor.z][0]].Contains(value))
                {
                    currentCell.Remove(value);
                    value = GetRandomIndexFromList(currentCell);
                }
            }
        }

        return value;
    }

    private void CollapseNeighbors(MapLocation currentPosition, List<int>[,] grid)
    {
        // Current possible values in the cell
        int[] possibleValues = grid[currentPosition.x, currentPosition.z].ToArray();

        // If cell could have and of the possible ids, get out
        if (possibleValues.Length == possibleIds.Count)
            return;

        // Find all the neighbors
        List<MapLocation> neighbors = GetNeighbors(currentPosition);

        // Go to every neighbor
        foreach(MapLocation neighbor in neighbors)
        {
            if (IsNeighborValid(neighbor, width, depth) && grid[neighbor.x, neighbor.z].Count > possibleValues.Length)
            {
                List<int> possibleNeighborConnection = new List<int>();
                foreach (int i in possibleValues)
                {
                    foreach (int j in connections[i])
                    {
                        if (grid[neighbor.x, neighbor.z].Contains(j) && !possibleNeighborConnection.Contains(j))
                        {
                            possibleNeighborConnection.Add(j);
                        }
                    }
                }
                grid[neighbor.x, neighbor.z] = possibleNeighborConnection;               
            }
        }
        foreach(MapLocation neighbor in neighbors)
        {
            if (IsNeighborValid(neighbor, width, depth) && grid[neighbor.x, neighbor.z].Count > possibleValues.Length)
            {
                CollapseNeighbors(neighbor, grid);
            }
        }
    }
    private bool IsNeighborValid(MapLocation neighbor, int width, int depth)
    {
        return neighbor.x >= 0 && neighbor.x < width && neighbor.z >= 0 && neighbor.z < depth;
    }
    private T GetRandomIndexFromList<T>(List<T> list)
    {
        int random = Random.Range(0, list.Count);
        return list[random];
    }

    private List<MapLocation> GetNeighbors(MapLocation currentPosition)
    {
        List<MapLocation> neighbors = new List<MapLocation>()
            {
                new MapLocation(currentPosition.x-1, currentPosition.z),
                new MapLocation(currentPosition.x+1, currentPosition.z),
                new MapLocation(currentPosition.x, currentPosition.z-1),
                new MapLocation(currentPosition.x, currentPosition.z+1),
            };
        return neighbors;
    }
    public class MapLocation
    {
        public int x;
        public int z;
        public MapLocation(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

    }
}
