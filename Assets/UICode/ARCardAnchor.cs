using UnityEngine;

public class ARCardAnchor : MonoBehaviour
{
    public GameObject chessboard;

    void Update()
    {
        if (chessboard != null)
        {
            chessboard.transform.position = transform.position;
            chessboard.transform.rotation = transform.rotation;
        }
    }
}
