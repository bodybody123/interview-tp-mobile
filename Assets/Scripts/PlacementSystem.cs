using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    private GridData floorData, furnitureData;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3 lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    [SerializeField]
    private SoundFeedback soundFeedback;

    void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
    }

    void Update()
    {
        if (buildingState == null) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition) {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

    public void StartPlacement(int Id)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(
            Id,
            grid,
            preview,
            database,
            floorData,
            furnitureData,
            objectPlacer,
            soundFeedback
        );

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnClicked += StopPlacement;
    }

    public void StartRemoving() {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemoveState(grid, preview, floorData, furnitureData, objectPlacer, soundFeedback);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnClicked += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    private void StopPlacement()
    {
        if (buildingState == null) return;
        
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnClicked -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
}
