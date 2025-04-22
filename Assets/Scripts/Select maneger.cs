using UnityEngine;
using System.Collections.Generic;

public class UnitSelectionManager : MonoBehaviour
{
    public LayerMask unitLayer;
    public RectTransform selectionBox;
    private Vector2 startPos;
    private List<SelectableUnit> selectedUnits = new List<SelectableUnit>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectUnitsInBox();
            selectionBox.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                int count = selectedUnits.Count;
                int rowLength = Mathf.CeilToInt(Mathf.Sqrt(count)); // кількість в ряд
                float spacing = 2.5f; // відстань між юнітами

                for (int i = 0; i < count; i++)
                {
                    int row = i / rowLength;
                    int col = i % rowLength;

                    Vector3 offset = new Vector3(col * spacing, 0, row * spacing);
                    Vector3 targetPos = hit.point + offset;

                    selectedUnits[i].MoveTo(targetPos);
                }
            }
        }

    }

    void UpdateSelectionBox(Vector2 currentMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = currentMousePos.x - startPos.x;
        float height = currentMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    void SelectUnitsInBox()
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            foreach (var unit in selectedUnits)
                unit.isSelected = false;

            selectedUnits.Clear();
        }

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach (var unit in FindObjectsOfType<SelectableUnit>())
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                unit.isSelected = true;
                if (!selectedUnits.Contains(unit))
                    selectedUnits.Add(unit);
            }
        }
    }

}
