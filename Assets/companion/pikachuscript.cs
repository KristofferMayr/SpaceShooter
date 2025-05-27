using UnityEngine;

public class CompanionPetController : MonoBehaviour
{
    public float moveRadius = 1.5f; // Wie weit das Pet vom Zentrum wegfliegen darf
    public float moveSpeed = 1.0f;  // Bewegungsgeschwindigkeit
    public float waitTime = 2.0f;   // Wartezeit zwischen Bewegungen

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float waitCounter;
    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition;
        waitCounter = waitTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            // An Ziel angekommen → warten
            animator.SetBool("isMoving", false);
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0f)
            {
                // Neues Ziel wählen
                float offsetX = Random.Range(-moveRadius, moveRadius);
                float offsetY = Random.Range(-moveRadius, moveRadius);
                targetPosition = startPosition + new Vector3(offsetX, offsetY, 0f);
                waitCounter = waitTime;
            }
        }
        else
        {
            // Bewegen
            animator.SetBool("isMoving", true);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
