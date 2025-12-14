using UnityEngine;

public class Chaser : MonoBehaviour
{
    public Transform player;
    public float baseSpeed = 4f;
    public float maxSpeed = 10f;
    public float rampDuration = 120f;

    private float currentSpeed;
    private float elapsedTime;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        currentSpeed = baseSpeed;
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (player != null)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / rampDuration);
            currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, t);

            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * currentSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player caught by the enemy!");
            GameManager.instance.ChangeState(GameState.End);
        }
    }
}
