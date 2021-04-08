using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Every "drag and drop" item must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static public DragAndDropItem draggedItem;                                      // Item that is dragged now
    static public GameObject icon;                                                  // Icon of dragged item
    static public DragAndDropCell sourceCell;                                       // From this cell dragged item is

    public delegate void DragEvent(DragAndDropItem item);
    static public event DragEvent OnItemDragStartEvent;                             // Drag start event
    static public event DragEvent OnItemDragEndEvent;                               // Drag end event

    

    /// <summary>
    /// This item is dragged
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        sourceCell = GetComponentInParent<DragAndDropCell>();                       // Remember source cell

        draggedItem = this;                                                         // Set as dragged 


        icon = new GameObject("Icon");                                              // Create object for item's icon
        Image image = icon.AddComponent<Image>();
        image.sprite = GetComponent<Image>().sprite;
        image.raycastTarget = false;                                                // Disable icon's raycast for correct drop handling
        RectTransform iconRect = icon.GetComponent<RectTransform>();
        // Set icon's dimensions
        iconRect.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x,
                                            GetComponent<RectTransform>().sizeDelta.y);
        Canvas canvas = GetComponentInParent<Canvas>();                             // Get parent canvas
        if (canvas != null)
        {
            // Display on top of all GUI (in parent canvas)
            icon.transform.SetParent(canvas.transform, true);                       // Set canvas as parent
            icon.transform.SetAsLastSibling();                                      // Set as last child in canvas transform
        }
        if (OnItemDragStartEvent != null)
        {
            OnItemDragStartEvent(this);                                             // Notify all about item drag start
        }
    }

    /// <summary>
    /// Every frame on this item drag
    /// </summary>
    /// <param name="data"></param>
    public void OnDrag(PointerEventData data)
    {
        if (icon != null)
        {
            icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor
        }
    }

    /// <summary>
    /// This item is dropped
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        bool isCheck = false;

        if (icon.transform.position.x <= 1100)
        {
            if (this.GetComponentInParent<DragAndDropCell>().cellType == DragAndDropCell.CellType.UnlimitedSource)
            {
                Debug.Log("Cell이 UnlimitedSource이므로 삭제되지 않습니다.");
            }
            else
            {
                Debug.Log("Cell과 item이 삭제되었습니다.");

                // remarkBlock을 제외하고 하이라이트를 꺼야한다.
                GameObject h_block = sourceCell.transform.GetChild(0).gameObject;
                if (h_block.name != "RemarkBlock")
                {
                    GameObject Cavas = GameObject.Find("Canvas");
                    Cavas.GetComponent<HighLightManager>().setCurrentHighLight(false);
                }


                sourceCell.RemoveItem();
                isCheck = true;
                Destroy(sourceCell.gameObject);
                NotifyCellPadding(true);
            }
        }


        if (icon != null)
        {
            Destroy(icon);                                                          // Destroy icon on item drop
        }

        MakeVisible(true);                                                          // Make item visible in cell

        if (OnItemDragEndEvent != null)
        {
            OnItemDragEndEvent(this);                                               // Notify all cells about item drag end
        }

        if (isCheck == true)
            sourceCell.SetBackgroundState(false);

        draggedItem = null;
        icon = null;
        sourceCell = null;

        // Debug.Log("item in");
    }

    /// <summary>
    /// Enable item's raycast
    /// </summary>
    /// <param name="condition"> true - enable, false - disable </param>
    public void MakeRaycast(bool condition)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = condition;
        }
    }

    private IEnumerator NotifyCellPadding(bool isTrue)
    {
        while (isTrue != true)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("checkThePadding", isTrue);
    }

    /// <summary>
    /// Enable item's visibility
    /// </summary>
    /// <param name="condition"> true - enable, false - disable </param>
    public void MakeVisible(bool condition)
    {
        GetComponent<Image>().enabled = condition;
    }


    // GridLayoutGroup 
    GridLayoutGroup glg;
    GameObject cell;
    GameObject Content;
    Vector2 s;

    void Update()
    {
        if (inventoryCheck())
        {
            if (this.name == "RemarkBlock")
            {
                s = new Vector2(400, 80);
                glg = cell.transform.GetComponent<GridLayoutGroup>();
                glg.cellSize = s;
            }
            else
            {
                s = new Vector2(200, 80);
                glg = cell.transform.GetComponent<GridLayoutGroup>();
                glg.cellSize = s;
            }
        }
    }

    // taskinventory에 있을경우 glg 값을 조정한다.
    public bool inventoryCheck()
    {
        if (this.transform.parent.gameObject != null && this.transform.parent.gameObject.name != "Canvas")
        {
            cell = this.transform.parent.gameObject;

            if (cell.transform.parent.gameObject != null)
            {
                Content = cell.transform.parent.gameObject;

                if (Content.name == "Content")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        else
            return false;
    }
}
