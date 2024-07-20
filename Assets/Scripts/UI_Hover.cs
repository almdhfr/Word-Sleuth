using UnityEngine;

public class UI_Hover : MonoBehaviour
{
    private float length = 0.1f; 
    private float offset;

    void Start()
    {
        offset = Random.Range(0f, length);
    }

    private void Update()
    {
        float newY = Mathf.PingPong(Time.time * length + offset, length) - (length / 2);
        transform.position = new Vector3(transform.position.x, newY + transform.position.y, transform.position.z);
    }
}
