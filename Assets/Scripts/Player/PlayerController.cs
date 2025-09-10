using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
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
    }

    private void UseTool()
    {
        GrowBlock block = null;
        block = FindFirstObjectByType<GrowBlock>();

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
