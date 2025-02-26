using System;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, CellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int m_selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    void Start()
    {
        StopPlacement();   
    }

    void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;   

        CellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    public void StartPlacement(int Id)
    {
        m_selectedObjectIndex = database.objectsData.FindIndex(data => data.Id == Id);
        if (m_selectedObjectIndex < 0) {
            Debug.LogError($"No Id found {Id}");
            return;
        }

        gridVisualization.SetActive(true);
        CellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnClicked += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(database.objectsData[m_selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
    }

    private void StopPlacement()
    {
        if (m_selectedObjectIndex < 0) return;

        gridVisualization.SetActive(false);
        CellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnClicked -= StopPlacement;
    }
}
