using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSelect : MonoBehaviour
{
    public List<Item> inv = new List<Item>();
    #region Variables
    [Header("Main UI")]
    public bool showSelectMenu;
    public bool toggleTogglable;
    public float scrW, scrH;
    [Header("Resources")]
    public Texture2D radialTexture;
    public Texture2D slotTexture;
    [Range(0, 100)]
    public int circleScaleOffset;
    [Header("Icons")]
    public Vector2 iconSize;
    public bool showIcons, showBoxes, showBounds;
    [Range(0.1f, 1)]
    public float iconSizeNum;
    [Range(-360, 360)]
    public int radialRotation;
    [SerializeField]
    private float iconOffset;
    [Header("Mouse Settings")]
    public Vector2 mouse;
    public Vector2 input;
    private Vector2 circleCenter;
    [Header("Input Settings")]
    public float inputDist;
    public float inputAngle;
    public int keyIndex;
    public int mouseIndex;
    public int inputIndex;
    [Header("Sector Settings")]
    public Vector2[] slotPos;
    public Vector2[] boundPos;
    [Range(1, 8)]
    public int numOfSectors = 1;
    [Range(50, 300)]
    public float circleRadius;
    public float mouseDistance, sectorDegree, mouseAngles;
    public int sectorIndex = 0;
    public bool withinCircle;
    [Header("Misc")]
    private Rect debugWindow;
    #endregion
    #region Set Up Functions
    private Vector2 Scr(float x, float y)
    {
        Vector2 coord = new Vector2(scrW * x, scrH * y);
        return coord;
    }
    private Vector2[] BoundPositions(int slots)
    {
        Vector2[] boundPos = new Vector2[slots];
        float angle = 0 + radialRotation;
        for (int i = 0; i < boundPos.Length; i++)
        {
            boundPos[i].x = circleCenter.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            boundPos[i].y = circleCenter.y + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            angle += sectorDegree;

        }
        return boundPos;
    }
    private Vector2[] SlotPositions(int slots)
    {
        Vector2[] slotPos = new Vector2[slots];
        float angle = ((iconOffset / 2) * 2) + radialRotation;
        //We Change the for loop syntax because the current item does not sit on the correct slot
        //We got item 1 in 8th slot and 2 on 7th slot and so on
        //*Counter For loop syntax
        //for (int i = slots - 1; i >= 0; i--)
        for (int i = 0; i < slotPos.Length; i++)
        {
            slotPos[i].x = circleCenter.x + circleRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            slotPos[i].y = circleCenter.y + circleRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            angle += sectorDegree;

        }
        return slotPos;
    }
    void SetItemSlots(int slots, Vector2[] pos) // This is the slot for the actual item. *slots = numOfSectors
    {
        //We Change the for loop syntax because the current item does not sit on the correct slot
        //We got item 1 in 8th slot and 2 on 7th slot and so on
        //*Counter For loop syntax
        //for (int i = slots - 1; i >= 0; i--)
        int s = slots - 1;
        for (int i = 0; i < slots; i++)
        
        {
            GUI.DrawTexture(new Rect(pos[i].x - (scrW * iconSizeNum * 0.5f), pos[i].y - (scrH * iconSizeNum * 0.5f),
                                                 scrW * iconSizeNum, scrH * iconSizeNum), inv[s].Icon);
            s--;

        }
    }
    private int CheckCurrentSector(float angle)
    {
        float boundingAngle = 0;
        for (int i = 0; i < numOfSectors; i++)
        {
            boundingAngle += sectorDegree;
            if (angle < boundingAngle)
            {
                return i;
            }
        }
        return 0;
    }
    void CalculateMouseAngles()
    {
        mouse = Input.mousePosition;
        input.x = Input.GetAxis("Horizontal");
        input.y = -Input.GetAxis("Vertical");

        mouseDistance = Mathf.Sqrt(Mathf.Pow((mouse.x - circleCenter.x), 2) + Mathf.Pow((mouse.y - circleCenter.y), 2));

        inputDist = Vector2.Distance(Vector2.zero, input);

        withinCircle = mouseDistance <= circleRadius ? true : false;

        if (input.x != 0 || input.y != 0)
        {
            inputAngle = (Mathf.Atan2(-input.y, input.x) * 180 / Mathf.PI) + radialRotation;
        }
        else
        {
            mouseAngles = (Mathf.Atan2(mouse.y - circleCenter.y, mouse.x - circleCenter.x) * 180 / Mathf.PI) + radialRotation;
        }
        if (mouseAngles < 0)
        {
            mouseAngles += 360;
        }
        if (inputAngle < 0)
        {
            inputAngle += 360;
        }
        inputIndex = CheckCurrentSector(inputAngle);
        mouseIndex = CheckCurrentSector(mouseAngles);

        if (input.x != 0 || input.y != 0)
        {
            sectorIndex = inputIndex;
        }
        if (input.x == 0 && input.y == 0)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                sectorIndex = mouseIndex;
            }
        }
    }
    #endregion
    #region Unity Functions
    private void Start()
    {
        inv.Add(ItemData.CreateItem(0));
        inv.Add(ItemData.CreateItem(1));
        inv.Add(ItemData.CreateItem(2));
        inv.Add(ItemData.CreateItem(302));
        inv.Add(ItemData.CreateItem(401));
        inv.Add(ItemData.CreateItem(402));
        inv.Add(ItemData.CreateItem(502));
        inv.Add(ItemData.CreateItem(602));
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            scrW = Screen.width / 16;
            scrH = Screen.height / 9;
            circleCenter.x = Screen.width / 2;
            circleCenter.y = Screen.height / 2;
            showSelectMenu = true;
            showBounds = true;
            showIcons = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            showSelectMenu = false;
            showBounds = false;
            showIcons = false;
        }
    }
    #endregion
    private void OnGUI()
    {
        if (showSelectMenu)
        {
            CalculateMouseAngles();
            sectorDegree = 360 / numOfSectors;
            iconOffset = sectorDegree / 2;
            slotPos = SlotPositions(numOfSectors);
            boundPos = BoundPositions(numOfSectors);
            //Deadzone
            //             Already have access variable Scr ( scrW/H * value)
            GUI.Box(new Rect(Scr(7.5f, 4), Scr(1, 1)), "");
            //Circle
            GUI.DrawTexture(new Rect(circleCenter.x - circleRadius - (circleScaleOffset / 4),
                                     circleCenter.y - circleRadius - (circleScaleOffset / 4),
                                     (circleRadius * 2) + (circleScaleOffset / 2),
                                     (circleRadius * 2) + (circleScaleOffset / 2)), radialTexture);
        }
        if (showBoxes)
        {
            for (int i = 0; i < numOfSectors; i++)
            {
                GUI.DrawTexture(new Rect(slotPos[i].x - (scrW * iconSizeNum * 0.5f),
                                         slotPos[i].y - (scrH * iconSizeNum * 0.5f),
                                         scrW * iconSizeNum,
                                         scrH * iconSizeNum), slotTexture);
            }
        }
        if (showBounds)//The line that seperate the slots (numOfSector is the number of the line) or Equal to the number of item slots
        {
            for (int i = 0; i < numOfSectors; i++)
            {
                GUI.Box(new Rect(boundPos[i].x - (scrW * 0.1f * 0.5f),
                                 boundPos[i].y - (scrH * 0.1f * 0.5f),
                                 scrW * 0.1f, scrH * 0.1f), "");
            }
        }
        if (showIcons)
        {
            SetItemSlots(numOfSectors, slotPos);
        }
    }
}
