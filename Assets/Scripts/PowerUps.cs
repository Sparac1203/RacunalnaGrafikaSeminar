using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        // Move the power-up to the left
        transform.position += speed * Time.deltaTime * Vector3.left;

        // Check if the power-up is out of the screen on the left
        if (IsOutOfScreen())
        {
            // Destroy the power-up when it goes beyond a certain position
            Destroy(gameObject);
        }
    }

    private bool IsOutOfScreen()
    {
        // Check if the power-up is out of the screen on the left
        float leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
        return transform.position.x < leftEdge;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Customize this part based on the interaction with the player or other game elements
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
