using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveInput;
    public CropManager cropManager;
    public Animator animator;

    public float moveSpeed = 5f;

    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidbody2D.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;

        if (rigidbody2D.linearVelocity.x<0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(rigidbody2D.linearVelocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }

        animator.SetFloat("speed", rigidbody2D.linearVelocity.magnitude);
    }


    // TODO Make the facing dirction work it only takes what the player stands on now
    private void OnInteract(InputAction.CallbackContext context)
    {
        Vector3Int playerTilePos = cropManager.groundTilemap.WorldToCell(transform.position);
        cropManager.tillOrWaterSoil(playerTilePos);

    }
}
