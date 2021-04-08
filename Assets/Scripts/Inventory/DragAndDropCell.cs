using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Every item's cell must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class DragAndDropCell : MonoBehaviour, IDropHandler
{
    bool isCellOnOff = false;

    public enum CellType
    {
        Swap,                                                               // Items will be swapped between cells
        DropOnly,                                                           // Item will be dropped into cell
        DragOnly,                                                           // Item will be dragged from this cell
        UnlimitedSource,                                                    // Item will be cloned and dragged from this cell
        gameBlock
    }
    public CellType cellType = CellType.Swap;                               // Special type of this cell

    public struct DropDescriptor                                            // Struct with info about item's drop event
    {
        public DragAndDropCell sourceCell;                                  // From this cell item was dragged
        public DragAndDropCell destinationCell;                             // Into this cell item was dropped
        public DragAndDropItem item;                                        // dropped item
    }

    public Color empty = new Color();                                       // Sprite color for empty cell
    public Color full = new Color();                                        // Sprite color for filled cell

    void OnEnable()
    {
        DragAndDropItem.OnItemDragStartEvent += OnAnyItemDragStart;         // Handle any item drag start
        DragAndDropItem.OnItemDragEndEvent += OnAnyItemDragEnd;             // Handle any item drag end
    }

    void OnDisable()
    {
        DragAndDropItem.OnItemDragStartEvent -= OnAnyItemDragStart;
        DragAndDropItem.OnItemDragEndEvent -= OnAnyItemDragEnd;
    }


    void Start()
    {
        SetBackgroundState(GetComponentInChildren<DragAndDropItem>() == null ? false : true);
    }

    /// <summary>
    /// On any item drag start need to disable all items raycast for correct drop operation
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragStart(DragAndDropItem item)
    {
        DragAndDropItem myItem = GetComponentInChildren<DragAndDropItem>(); // Get item from current cell
        if (myItem != null)
        {
            myItem.MakeRaycast(false);                                      // Disable item's raycast for correct drop handling
            if (myItem == item)                                             // If item dragged from this cell
            {
                // Check cell's type
                switch (cellType)
                {
                    case CellType.DropOnly:
                        DragAndDropItem.icon.SetActive(false);              // Item will not be dropped
                        break;
                    case CellType.UnlimitedSource:
                        // Nothing to do
                        break;
                    default:
                        MakeVisibleNumberBlock(myItem, false);

                        item.MakeVisible(false);                            // Hide item in cell till dragging\
                        

                        SetBackgroundState(false);
                        break;
                }
            }
        }


    }

    /// <summary>
    /// On any item drag end enable all items raycast
    /// </summary>
    /// <param name="item"> dragged item </param>
    private void OnAnyItemDragEnd(DragAndDropItem item)
    {
        DragAndDropItem myItem = GetComponentInChildren<DragAndDropItem>(); // Get item from current cell
        if (myItem != null)
        {
            if (myItem == item)
            {
                SetBackgroundState(true);

                //Debug.Log("set true");
            }
            myItem.MakeRaycast(true);                                       // Enable item's raycast
            MakeVisibleNumberBlock(myItem, true);


        }
        else
        {
            SetBackgroundState(false);
        }
    }

    /// <summary>
    /// Item is dropped in this cell
    /// </summary>
    /// <param name="data"></param>
    public void OnDrop(PointerEventData data)
    {
        // Debug.Log(this.gameObject.name);
        if (DragAndDropItem.icon != null)
        {
            if (DragAndDropItem.icon.activeSelf == true)                    // If icon inactive do not need to drop item in cell
            {
                DragAndDropItem item = DragAndDropItem.draggedItem;
                DragAndDropCell sourceCell = DragAndDropItem.sourceCell;
                DropDescriptor desc = new DropDescriptor();
                if ((item != null) && (sourceCell != this) && (isCellOnOff == true))
                {
                    switch (sourceCell.cellType)                            // Check source cell's type
                    {
                        case CellType.UnlimitedSource:
                            string itemName = item.name;
                            item = Instantiate(item);                       // Clone item from source cell
                            item.name = itemName;

                            break;
                        default:
                            // Nothing to do
                            break;
                    }
                    switch (cellType)                                       // Check this cell's type
                    {
                        case CellType.gameBlock:
                            switch (sourceCell.cellType)
                            {
                                case CellType.gameBlock:
                                    LowerItems(sourceCell, this);

                                    break;

                                case CellType.UnlimitedSource:
                                    LowerItems(item.gameObject, this);

                                    // for문일시 밑에 end 박스를 추가한다.
                                    if (item.name == "ForBlock")
                                    {
                                        GameObject endItem = createEndBlock();
                                        DragAndDropCell temp = nextCell(this);

                                        LowerItems(endItem, temp);

                                    }

                                    if (item.name == "IfBlock")
                                    {
                                        GameObject endItem = createIfEndBlock();
                                        DragAndDropCell temp = nextCell(this);

                                        LowerItems(endItem, temp);
                                    }

                                    // if this items have NumberBlock;
                                    AutoClickNumberBlock();

                                    if (item.name != "RemarkBlock")
                                    {
                                        // 넘버블록이 있을경우 넘버블록이 클릭되면서 하이라이트가 2번 클릭되어 꺼지게 된다
                                        int childNumber = item.transform.childCount;   // Block의 자식객체가 있으면 number블록이 있는것으로 간주


                                        if (childNumber == 0)       // number불록이 없을경우만 클릭되게 한다.
                                        {
                                            item.GetComponent<BlockHighLightNotify>().onClick();
                                        }
                                    }
                                    else    // remark 블록일시 이미지를 생성한다.
                                    {
                                        GameObject canvas = GameObject.Find("Canvas");
                                        canvas.GetComponent<DrawManager>().CreateRemarkImage(item.gameObject);
                                    }

                                    break;
                            }
                            break;

                        case CellType.Swap:
                            DragAndDropItem currentItem = GetComponentInChildren<DragAndDropItem>();
                            switch (sourceCell.cellType)
                            {
                                case CellType.Swap:
                                    SwapItems(sourceCell, this);            // Swap items between cells
                                    // Fill event descriptor
                                    desc.item = item;
                                    desc.sourceCell = sourceCell;
                                    desc.destinationCell = this;
                                    // Send message with DragAndDrop info to parents GameObjects
                                    //StartCoroutine(NotifyOnDragEnd(desc));
                                    if (currentItem != null)
                                    {
                                        // Fill event descriptor
                                        desc.item = currentItem;
                                        desc.sourceCell = this;
                                        desc.destinationCell = sourceCell;
                                        // Send message with DragAndDrop info to parents GameObjects
                                        //StartCoroutine(NotifyOnDragEnd(desc));
                                    }
                                    break;
                                default:
                                    PlaceItem(item.gameObject);             // Place dropped item in this cell
                                    // Fill event descriptor
                                    desc.item = item;
                                    desc.sourceCell = sourceCell;
                                    desc.destinationCell = this;
                                    // Send message with DragAndDrop info to parents GameObjects
                                   // StartCoroutine(NotifyOnDragEnd(desc));
                                    break;
                            }
                            break;
                        case CellType.DropOnly:
                            PlaceItem(item.gameObject);                     // Place dropped item in this cell
                            desc.item = item;
                            desc.sourceCell = sourceCell;
                            desc.destinationCell = this;
                            // Send message with DragAndDrop info to parents GameObjects
                            //StartCoroutine(NotifyOnDragEnd(desc));

                            break;
                        default:
                            // Nothing to do
                            break;
                    }


                }
                if (item.GetComponentInParent<DragAndDropCell>() == null)   // If item have no cell after drop
                {
                    //if(item.transform.position.x <= 1000)
                    //Debug.Log("dddd");
                    Destroy(item.gameObject);                               // Destroy it

                }
            }
        }
    }

    /// <summary>
    /// Change cell's sprite color on item put/remove
    /// </summary>
    /// <param name="condition"> true - filled, false - empty </param>
    public void SetBackgroundState(bool condition)
    {
        GetComponent<Image>().color = condition ? full : empty;
    }

    /// <summary>
    /// Delete item from this cell
    /// </summary>
    public void RemoveItem()
    {
        foreach (DragAndDropItem item in GetComponentsInChildren<DragAndDropItem>())
        {
            Destroy(item.gameObject);
        }
        SetBackgroundState(false);
    }

    /// <summary>
    /// Put new item in this cell
    /// </summary>
    /// <param name="itemObj"> New item's object with DragAndDropItem script </param>
    public void PlaceItem(GameObject itemObj)
    {
        RemoveItem();                                                       // Remove current item from this cell
        if (itemObj != null)
        {
            itemObj.transform.SetParent(transform, false);
            itemObj.transform.localPosition = Vector3.zero;
            DragAndDropItem item = itemObj.GetComponent<DragAndDropItem>();
            if (item != null)
            {
                item.MakeRaycast(true);
            }
            SetBackgroundState(true);
        }
    }

    /// <summary>
    /// Get item from this cell
    /// </summary>
    /// <returns> Item </returns>
    public DragAndDropItem GetItem()
    {
        return GetComponentInChildren<DragAndDropItem>();
    }

    /// <summary>
    /// get next sibiling cell
    /// </summary>
    public DragAndDropCell nextCell(DragAndDropCell cell)
    {
        int index = cell.transform.GetSiblingIndex();
        DragAndDropCell nCell = null;
        int childCount = 0;

        childCount = cell.transform.parent.childCount;

        if (childCount >= index + 1)
            nCell = cell.transform.parent.GetChild(index + 1).GetComponent<DragAndDropCell>();

        //Debug.Log("childCount : " + childCount);
        return nCell;
    }

    /// <summary>
    /// Cell위에 item이 올라올경우 null이 아니면 현재 item을 밑으로 내린다.
    /// </summary>
    public void LowerItems(DragAndDropCell firstCell, DragAndDropCell secondCell)
    {
        DragAndDropCell lowerCell = null;

        if (secondCell != null)
            lowerCell = nextCell(secondCell);


        DragAndDropItem firstItem = null;
        DragAndDropItem secondItem = null;
        DragAndDropItem lowerItem = null;

        if (firstCell != null)
            firstItem = firstCell.GetItem();

        if (secondCell != null)
            secondItem = secondCell.GetItem();

        if (lowerCell != null)
            lowerItem = lowerCell.GetItem();

        if ((secondCell != null) && (lowerCell != null))
        {

            if (firstItem != null)
            {
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
                //Debug.Log("secondCell : "+secondCell.name);
                secondCell.SetBackgroundState(true);
            }

            if (secondItem != null)
            {
                secondItem.transform.SetParent(lowerCell.transform, false);
                secondItem.transform.localPosition = Vector3.zero;
                //Debug.Log("lowerCell : " + lowerCell.name);
                lowerCell.SetBackgroundState(true);

                if (lowerItem != null)
                {// 재귀 함수
                    if (firstItem != lowerItem)
                    {
                        LowerItems(lowerCell, nextCell(lowerCell));
                    }
                }
            }

        }

        if ((secondCell != null) && (lowerCell == null))
        {
            //Debug.Log("jijijij");
            if (firstItem != null)
            {
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
                //Debug.Log("secondCell : "+secondCell.name);
                secondCell.SetBackgroundState(true);
            }
        }
    }

    public void LowerItems(GameObject firstItem, DragAndDropCell secondCell)
    {
        DragAndDropCell lowerCell = nextCell(secondCell);
        DragAndDropItem secondItem = secondCell.GetItem();
        DragAndDropItem lowerItem = lowerCell.GetItem();

        if ((secondCell != null) && (lowerCell != null))
        {
            if (firstItem != null)
            {
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
                DragAndDropItem temp = firstItem.GetComponent<DragAndDropItem>();
                if (temp != null)
                {
                    temp.MakeRaycast(true);
                }

                secondCell.SetBackgroundState(true);
            }

            if (secondItem != null)
            {
                secondItem.transform.SetParent(lowerCell.transform, false);
                secondItem.transform.localPosition = Vector3.zero;
                lowerCell.SetBackgroundState(true);

                if (lowerItem != null)
                {// 재귀 함수
                    LowerItems(lowerCell, nextCell(lowerCell));
                }
            }
        }

        if ((secondCell != null) && (lowerCell == null))
        {
            if (firstItem != null)
            {
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
                DragAndDropItem temp = firstItem.GetComponent<DragAndDropItem>();
                if (temp != null)
                {
                    temp.MakeRaycast(true);
                }

                secondCell.SetBackgroundState(true);
            }
        }
    }

    /// <summary>
    /// Swap items between to cells
    /// </summary>
    /// <param name="firstCell"> Cell </param>
    /// <param name="secondCell"> Cell </param>
    public void SwapItems(DragAndDropCell firstCell, DragAndDropCell secondCell)
    {
        if ((firstCell != null) && (secondCell != null))
        {
            DragAndDropItem firstItem = firstCell.GetItem();                // Get item from first cell
            DragAndDropItem secondItem = secondCell.GetItem();              // Get item from second cell
            if (firstItem != null)
            {
                // Place first item into second cell
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
                secondCell.SetBackgroundState(true);
            }
            if (secondItem != null)
            {
                // Place second item into first cell
                secondItem.transform.SetParent(firstCell.transform, false);
                secondItem.transform.localPosition = Vector3.zero;
                firstCell.SetBackgroundState(true);
            }
        }
    }

    /// <summary>
    /// for문이 taskManager에 들어오면 밑에 end 블록을 생성한다.
    /// </summary>
    private GameObject createEndBlock()
    {
        GameObject endBlock = GameObject.Find("EndBlock");
        //GameObject end = new GameObject();
        endBlock = Instantiate(endBlock) as GameObject;
        endBlock.name = "EndBlock";

        return endBlock;
    }

    private GameObject createIfEndBlock()
    {
        GameObject endBlock = GameObject.Find("IfEndBlock");
        //GameObject end = new GameObject();
        endBlock = Instantiate(endBlock) as GameObject;
        endBlock.name = "IfEndBlock";

        return endBlock;
    }

    // end 블록이 생성됨을 알려준다.
    private IEnumerator NotifyEndBlock(bool isTrue)
    {
        while (isTrue != true)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SendMessageUpwards("createDynamicCell", isTrue);
    }

    //private IEnumerator NotifyOnDragEnd(DropDescriptor desc)
    //{
    //    // Wait end of drag operation
    //    while (DragAndDropItem.draggedItem != null)
    //    {
    //        yield return new WaitForEndOfFrame();
    //    }
    //    // Send message with DragAndDrop info to parents GameObjects
    //    //gameObject.SendMessageUpwards("OnItemPlace", desc, SendMessageOptions.DontRequireReceiver);
    //}

    public void MakeVisibleNumberBlock(DragAndDropItem item, bool condition)
    {
        if (item.transform.childCount == 1 || item.transform.childCount == 2)
        {
            GameObject BlockMode = item.transform.GetChild(0).gameObject;

            int num = item.transform.childCount;
            if (num == 1)
            {
                // Debug.Log(myItem.transform.GetChild(0).name);
                BlockMode.transform.GetChild(0).GetComponent<Image>().enabled = condition;
                item.transform.GetChild(0).GetComponent<Image>().enabled = condition;
            }
            if (num == 2)
            {// if블록
                BlockMode.transform.GetChild(0).GetComponent<Image>().enabled = condition;
                item.transform.GetChild(0).GetComponent<Image>().enabled = condition;
                item.transform.GetChild(1).GetComponent<Image>().enabled = condition;
            }
        }

        //GameObject BlockMode = item.transform.GetChild(0).gameObject;
        //GameObject numberBlock = BlockMode.transform.GetChild(0).GetComponent<GameObject>();

        
    }

    //public void getMsg(string s)
    //{
    //    Debug.Log(s);
    //}

    public void AutoClickNumberBlock()
    {
        // numberBlock이 자식 객체로 있을 경우 드레그 함으로써 클릭이 되게 해야한다. 
        GameObject Block, BlockMode;
        BlockNotify NumberBlock;
        if (this.transform.childCount >= 1)
        {
            Block = this.transform.GetChild(0).gameObject;
            if (Block.transform.childCount >= 1)
            {
                BlockMode = Block.transform.GetChild(0).gameObject;
                if (BlockMode.transform.childCount == 1)
                {
                    NumberBlock = BlockMode.transform.GetChild(0).GetComponent<BlockNotify>();

                    NumberBlock.OnClickNotify();
                    ModeManager temp = GameObject.Find("ModeBox").GetComponent<ModeManager>();
                    temp.getButtonPosition(NumberBlock.transform.position);
                }
            }
        }
    }

    public void getCellOnOff(bool isTrue)
    {
        isCellOnOff = isTrue;
        GameObject g = this.gameObject;
        g.SetActive(isTrue);
    }
}
