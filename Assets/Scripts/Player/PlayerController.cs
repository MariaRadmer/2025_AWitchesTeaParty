using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveInput, actionInput;
    public CropManager cropManager;
    public Animator animator;

    public float moveSpeed = 5f;

    private Rigidbody2D rigidbody2D;

    public ToolType currentTool;

    public enum ToolType
    {
        plow,
        wateringcan,
        seeds,
        basket
    }

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidbody2D.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;

        if (rigidbody2D.linearVelocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rigidbody2D.linearVelocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            Debug.Log("Pressed tab");
            currentTool++;

            if((int) currentTool >= 4)
            {
                currentTool = ToolType.plow;
            }
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentTool = ToolType.plow;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringcan;
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.basket;
        }


        if (actionInput.action.WasPressedThisFrame())
        {
            UseTool();
        }

        animator.SetFloat("speed", rigidbody2D.linearVelocity.magnitude);
    }

    private void UseTool()
    {
        GrowBlock block = null;
        block = FindFirstObjectByType<GrowBlock>();

        //block.PloughSoil();


    }
    

    // TODO Make the facing dirction work it only takes what the player stands on now
    private void OnInteract(InputAction.CallbackContext context)
    {
        Vector3Int playerTilePos = cropManager.groundTilemap.WorldToCell(transform.position);
        cropManager.tillOrWaterSoil(playerTilePos);

    }
}
