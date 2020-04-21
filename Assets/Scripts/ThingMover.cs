using UnityEngine;

public class ThingMover : MonoBehaviour
{
    private float moveSpeed = 1f;
    private float Timer = 0;
    private int currentNode;
    private Vector3 CurrentPositionHolder;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Timer += Time.deltaTime * moveSpeed;
        if(transform.position != CurrentPositionHolder)
        {
            transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, Timer);
        }
        else
        {
            if(currentNode < Linedrawer.points.Count - 1)
            {
                currentNode++;
                Timer = 0;
                startPosition = transform.position;
                CurrentPositionHolder = Linedrawer.points[currentNode].transform.position;
            }
            else if (currentNode >= Linedrawer.points.Count - 1)
            {
                currentNode = 0;
                Timer = 0;
                startPosition = transform.position;
                CurrentPositionHolder = Linedrawer.points[currentNode].transform.position;
            }
        }
    }

}
