using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public LayerMask unitLayer;
    public Camera cam;
    public List<Squad> selectedSquads = new List<Squad>();

    private Vector2 dragStart;
    private bool isDragging = false;

    [Header("UI")]
    public RectTransform selectionBox;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStart = Input.mousePosition;
            isDragging = true;
            selectionBox.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateSelectionBox(dragStart, Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionBox.gameObject.SetActive(false);
            isDragging = false;

            if ((Input.mousePosition - (Vector3)dragStart).sqrMagnitude < 5f)
            {
                ClickSelect();
            }
            else
            {
                BoxSelect(dragStart, Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (var squad in selectedSquads)
                {
                    squad.MoveTo(hit.point);
                }
            }
        }
    }

    void ClickSelect()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, unitLayer))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null && unit.squad != null)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }

                if (!selectedSquads.Contains(unit.squad))
                {
                    selectedSquads.Add(unit.squad);
                    unit.squad.SetSelected(true);
                }
            }
        }
        else
        {
            DeselectAll();
        }
    }

    void BoxSelect(Vector2 screenStart, Vector2 screenEnd)
    {
        Vector2 min = Vector2.Min(screenStart, screenEnd);
        Vector2 max = Vector2.Max(screenStart, screenEnd);
        Rect selectionRect = new Rect(min, max - min);

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            DeselectAll();
        }

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);
            if (selectionRect.Contains(screenPos, true))
            {
                if (!selectedSquads.Contains(unit.squad))
                {
                    selectedSquads.Add(unit.squad);
                    unit.squad.SetSelected(true);
                }
            }
        }
    }

    void DeselectAll()
    {
        foreach (var squad in selectedSquads)
        {
            squad.SetSelected(false);
        }
        selectedSquads.Clear();
    }

    void UpdateSelectionBox(Vector2 screenStart, Vector2 screenEnd)
    {
        RectTransform canvasRect = selectionBox.parent as RectTransform;

        Vector2 localStart;
        Vector2 localEnd;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenStart, cam, out localStart);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenEnd, cam, out localEnd);

        Vector2 size = localEnd - localStart;

        Vector2 pivot = new Vector2(
            size.x >= 0 ? 0 : 1,
            size.y >= 0 ? 0 : 1
        );

        selectionBox.pivot = pivot;

        Vector2 anchoredPos =
            size.x >= 0 && size.y >= 0 ? localStart :
            size.x < 0 && size.y >= 0 ? new Vector2(localEnd.x, localStart.y) :
            size.x >= 0 && size.y < 0 ? new Vector2(localStart.x, localEnd.y) :
            localEnd;

        selectionBox.anchoredPosition = anchoredPos;
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
    }
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }


}
