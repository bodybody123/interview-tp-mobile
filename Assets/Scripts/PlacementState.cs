using UnityEngine;

public class PlacementState : IBuildingState
{
    private int m_selectedObjectIndex = -1;
    int id;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData floorData, furnitureData;
    ObjectPlacer objectPlacer;
    SoundFeedback soundFeedback;

    public PlacementState(int iD,
                        Grid grid,
                        PreviewSystem previewSystem,
                        ObjectsDatabaseSO database,
                        GridData floorData,
                        GridData furnitureData,
                        ObjectPlacer objectPlacer,
                        SoundFeedback soundFeedback
    )
    {
        id = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.soundFeedback = soundFeedback;

        m_selectedObjectIndex = database.objectsData.FindIndex(data => data.Id == id);
        if (m_selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[m_selectedObjectIndex].Prefab,
                database.objectsData[m_selectedObjectIndex].Size
            );
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, m_selectedObjectIndex);
        if (!placementValidity) {
            soundFeedback.PlaySound(SoundType.WrongPlacement);
            return;
        }
        soundFeedback.PlaySound(SoundType.Place);

        int index = objectPlacer.PlacedObject(
            database.objectsData[m_selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition)
        );

        GridData selectedData = database.objectsData[m_selectedObjectIndex].Id == 0
            ? floorData : furnitureData;

        selectedData.AddObjectAt(gridPosition,
            database.objectsData[m_selectedObjectIndex].Size,
            database.objectsData[m_selectedObjectIndex].Id,
            index
        );

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        ObjectData selectedObject = database.objectsData[selectedObjectIndex];

        GridData selectedData = selectedObject.Id == 0
            ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, selectedObject.Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, m_selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
