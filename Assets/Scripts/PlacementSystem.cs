using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int m_selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private GridData floorData, furnitureData;

    private List<GameObject> placedGameObjects = new();
    [SerializeField]
    private PreviewSystem preview;

    private Vector3 lastDetectedPosition = Vector3Int.zero;

    void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition) {
            bool placementValidity = CheckPlacementValidity(gridPosition, m_selectedObjectIndex);
    
            mouseIndicator.transform.position = mousePosition;   
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        ObjectData selectedObject = database.objectsData[selectedObjectIndex];

        GridData selectedData = selectedObject.Id == 0 
            ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, selectedObject.Size);
    }

    public void StartPlacement(int Id)
    {
        m_selectedObjectIndex = database.objectsData.FindIndex(data => data.Id == Id);
        if (m_selectedObjectIndex < 0) {
            Debug.LogError($"No Id found {Id}");
            return;
        }

        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(
            database.objectsData[m_selectedObjectIndex].Prefab,
            database.objectsData[m_selectedObjectIndex].Size
        );
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnClicked += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, m_selectedObjectIndex);
        if (!placementValidity) return;

        GameObject newObject = Instantiate(database.objectsData[m_selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);
        GridData selectedData = database.objectsData[m_selectedObjectIndex].Id == 0 
            ? floorData : furnitureData;

        selectedData.AddObjectAt(gridPosition,
            database.objectsData[m_selectedObjectIndex].Size,
            database.objectsData[m_selectedObjectIndex].Id,
            placedGameObjects.Count - 1
        );

        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private void StopPlacement()
    {
        if (m_selectedObjectIndex < 0) return;

        gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnClicked -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }
}
