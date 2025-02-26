using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelList : MonoBehaviour
{
    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private Button buttonPrefab;

    [SerializeField]
    private PlacementSystem placementSystem;
    [SerializeField]
    private RectTransform contentPanel;

    private Button[] m_buttonList;

    void Start()
    {
        if (database == null || buttonPrefab == null || placementSystem == null || contentPanel == null) return;
        int totalData = database.objectsData.Count;
        m_buttonList = new Button[totalData];

        for (int i = 0; i < totalData; i++)
        {
            m_buttonList[i] = Instantiate(buttonPrefab, contentPanel.transform);
            m_buttonList[i].GetComponentInChildren<TMP_Text>().text = database.objectsData[i].Name;

            int currentIndex = i;
            m_buttonList[i].onClick.AddListener(() => {
                Debug.Log("Button clicked with index: " + currentIndex);
                placementSystem.StartPlacement(database.objectsData[currentIndex].Id);
            });
        }
    }
}
