using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    
    [Header("Boundary Settings")]
    [SerializeField] private float leftBound = -10f;  // Minimale X-Position
    [SerializeField] private float rightBound = 10f; // Maximale X-Position

    void Update()
    {
        // Eingabe lesen
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Neue Position berechnen
        Vector3 movement = Vector3.right * horizontalInput * movementSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;
        
        // X-Position auf die Grenzen beschränken
        newPosition.x = Mathf.Clamp(newPosition.x, leftBound, rightBound);
        
        // Position aktualisieren
        transform.position = newPosition;
    }
}