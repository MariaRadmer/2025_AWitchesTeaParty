using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public InputActionReference moveInput, actionInput;
    public CropManager cropManager;
    public Animator animator;

    public float moveSpeed = 5f;

    private Rigidbody2D rb2D;

    public ToolType currentTool;

    public enum ToolType
    {
        plow,
        wateringcan,
        seeds,
        basket
    }

    public float toolWaitTime = .5f;
    private float toolWaitCounter;

    public Transform toolIndicator;
    public float toolRange = 3.0f;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        UIController.instance.SwitchTool((int)currentTool);
    }

    void Update()
    {

        if(toolWaitCounter > 0)
        {
            toolWaitCounter-= Time.deltaTime;
            rb2D.linearVelocity = Vector2.zero;

        } else
        {
            rb2D.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;

            if (rb2D.linearVelocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (rb2D.linearVelocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }



        bool hasSwitchedTool = false;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            Debug.Log("Pressed tab");
            currentTool++;

            if((int) currentTool >= 4)
            {
                currentTool = ToolType.plow;
            }
            hasSwitchedTool = true;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentTool = ToolType.plow;
            hasSwitchedTool = true;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringcan;
            hasSwitchedTool = true;
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;
            hasSwitchedTool = true;
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.basket;
            hasSwitchedTool = true;
        }

        if (hasSwitchedTool)
        {
            UIController.instance.SwitchTool((int) currentTool);
        }

        if (actionInput.action.WasPressedThisFrame())
        {
            UseTool();
        }

        animator.SetFloat("speed", rb2D.linearVelocity.magnitude);

        toolIndicator.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        toolIndicator.position = new Vector3(toolIndicator.position.x, toolIndicator.position.y, 0.0f);
        
        if(Vector3.Distance(toolIndicator.position, transform.position) > toolRange)
        {
            Vector2 diraction = toolIndicator.position - transform.position;
            diraction = diraction.normalized * toolRange;
            toolIndicator.position = transform.position + new Vector3(diraction.x,diraction.y,0f);
        }

        toolIndicator.position = new Vector3(Mathf.FloorToInt(toolIndicator.position.x) + 0.5f, Mathf.FloorToInt(toolIndicator.position.y) + 0.5f, 0f);
    }

    private void UseTool()
    {
       
        GrowBlock block = GridController.instance.GetBlock(toolIndicator.position.x - .5f, toolIndicator.position.y - .5f);
        toolWaitCounter = toolWaitTime;

        if(block != null )
        {
            switch(currentTool)
            {
                case ToolType.plow:
                    block.PloughSoil();
                    animator.SetTrigger("usePlough");
                    break;
                case ToolType.wateringcan:
                    animator.SetTrigger("useWateringcan");
                    block.WaterSoil();
                    break;
                case ToolType.seeds:
                    block.PlantCrop();
                    break;
                case ToolType.basket:
                    block.HarvestCrop();
                    break;
                default:
                    break;
            }
        }
    }
    

    // TODO Make the facing dirction work it only takes what the player stands on now
    private void OnInteract(InputAction.CallbackContext context)
    {
        Vector3Int playerTilePos = cropManager.groundTilemap.WorldToCell(transform.position);
        cropManager.tillOrWaterSoil(playerTilePos);

    }
}
