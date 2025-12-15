using UnityEngine;

public class Chaser : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform player;
    [SerializeField] private float baseSpeed = 4f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float rampDuration = 240f;

    private Rigidbody2D playerRigidBody;
    private float currentSpeed;
    private float elapsedTime;
    private bool isPlayerKnocked = false;
    private bool isPlayerCaught = false;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerRigidBody = player.GetComponent<Rigidbody2D>();
        }

        currentSpeed = baseSpeed;
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 offset = player.position - transform.position;
        if (offset.sqrMagnitude < 0.0001f) return;

        elapsedTime += Time.deltaTime;

        float time = Mathf.Clamp01(elapsedTime / rampDuration);
        currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, time);

        Vector3 direction = offset.normalized;
        transform.position += direction * currentSpeed * Time.deltaTime;

        spriteRenderer.flipX = direction.x > 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (playerRigidBody == null) return;

        if (GameManager.instance.State != GameState.End)
        {
            animator.SetTrigger("Attack");

            Debug.Log("Player attacked by the chaser!");
            GameManager.instance.ChangeState(GameState.End);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (playerRigidBody == null) return;
        if (!(isPlayerKnocked && playerRigidBody.linearVelocity.magnitude < 0.1f)) return;

        if (!isPlayerCaught)
        {
            Debug.Log("Player caught by the chaser!");
            animator.SetTrigger("Catch");
            isPlayerCaught = true;
        }            
    }

    public void AttackPlayer()
    {
        Vector2 knockbackDirection = (player.position - transform.position).normalized;

        knockbackDirection.y += 0.7f;
        knockbackDirection.Normalize();

        playerRigidBody.AddForce(knockbackDirection * 10f, ForceMode2D.Impulse);

        isPlayerKnocked = true;
    }
}
