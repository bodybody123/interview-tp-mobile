using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(
        Vector3Int gridPosition,
        Vector2Int objectSize,
        int id,
        int placedObjectsIndex
    ) {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, id, placedObjectsIndex);

        foreach (var pos in positionToOccupy) {
            if(placedObjects.ContainsKey(pos)) {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }

            placedObjects[pos] = data; 
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0;y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }

        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize) {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, objectSize);
        foreach (var pos in positionToOccupy) {
            if (placedObjects.ContainsKey(pos)) return false;
        }

        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition)) return -1;

        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPosition)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData {
    public List<Vector3Int> occupiedPosition;

    public int Id { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPosition, int id, int placedObjectsIndex) {
        this.occupiedPosition = occupiedPosition;
        Id = id;
        PlacedObjectIndex = placedObjectsIndex;
    }
}
