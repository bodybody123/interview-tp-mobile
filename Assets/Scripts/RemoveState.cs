using System;
using Unity.Android.Gradle;
using UnityEngine;

public class RemoveState : IBuildingState
{
    private int m_selectedObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData, furnitureData;
    ObjectPlacer objectPlacer;
    SoundFeedback soundFeedback;

    public RemoveState(
        Grid grid,
        PreviewSystem previewSystem,
        GridData floorData,
        GridData furnitureData,
        ObjectPlacer objectPlacer,
        SoundFeedback soundFeedback
    ) {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.soundFeedback = soundFeedback;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;

        if (!furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one)) {
            selectedData = furnitureData;
        } else if (!floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)) {
            selectedData = floorData;
        }

        if (selectedData == null) {
        } else {
            soundFeedback.PlaySound(SoundType.Remove);
            m_selectedObjectIndex = selectedData.GetRepresentationIndex(gridPosition);

            if (m_selectedObjectIndex == -1) return;

            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(m_selectedObjectIndex);
        }

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
            floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
