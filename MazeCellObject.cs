using UnityEngine;
using System.Collections.Generic;

public class MazeCellObject : MonoBehaviour
{
    [SerializeField] GameObject topWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject leftWall;

    public void Init (bool top, bool bottom, bool right, bool left)
    {
        topWall.SetActive (top);
        bottomWall.SetActive (bottom);
        rightWall.SetActive (right);
        leftWall.SetActive (left);
    }
}