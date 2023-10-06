using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeckgroundStars : MonoBehaviour
{
    [SerializeField] private int[] stars;
    private readonly float[] _starSpeed = { 2.0f, 3.0f, 1.5f };
    private float speed;

    private void Start()
    {
        speed = _starSpeed[Random.Range(0, 3)];
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovments();
    }

    void CalculateMovments()
    {
        float yBoundryDown = -6.5f;
        transform.Translate(Vector3.down * Time.deltaTime * speed);

        if (transform.position.y <= yBoundryDown)
            Destroy(this.gameObject);
    }
}
