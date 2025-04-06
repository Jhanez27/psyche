using UnityEngine;

public class PathWispFollow : MonoBehaviour
{
    [SerializeField] Transform[] Points;
    [SerializeField] private float moveSpeed;
    private int pointsIndex;

    void Start()
    {
        transform.position = Points[pointsIndex].transform.position;
    }

    private void Update()
    {
        if(pointsIndex <= Points.Length -1)
        {
            transform.position = Vector2.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);

            if(transform.position == Points[pointsIndex].transform.position)
            {
                pointsIndex += 1;
            }

            /*
            if(pointsIndex == points.Length)
            {
                pointsIndex = 0;
            }
            */
        }
    }
}
