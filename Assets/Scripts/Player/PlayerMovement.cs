using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public CropManager cropManager;

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Interact.canceled+= OnInteract;

    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Interact.canceled -= OnInteract;

        inputActions.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    // TODO Make the facing dirction work it only takes what the player stands on now
    private void OnInteract(InputAction.CallbackContext context)
    {
        Vector3Int playerTilePos = cropManager.groundTilemap.WorldToCell(transform.position);
        if (cropManager.TillSoil(playerTilePos))
        {
            Debug.Log("Soil tilled at " + playerTilePos);
        }
        else
        {
            Debug.Log("Can't till this tile!");
        }

    }
}
