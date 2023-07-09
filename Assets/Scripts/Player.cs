using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoSingleton<Player>
{
    private CharacterController character;
    private Vector3 direction;

    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;

    // void Start()
    // {
    //     character = GetComponent<CharacterController>();
    // }

    // void OnEnable()
    // {
    //     direction = Vector3.zero;
    // }

    // void Update()
    // {
    //     direction += Vector3.down * gravity * Time.deltaTime;

    //     if (character.isGrounded)
    //     {
    //         direction = Vector3.down;

    //         if (Input.GetButton("Jump"))
    //         {
    //             direction = Vector3.up * jumpForce;
    //         }
    //     }

    //     character.Move(direction * Time.deltaTime);
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            FindObjectOfType<GameManager>().GameOver();
        }
    }

}
