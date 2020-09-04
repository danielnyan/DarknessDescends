using UnityEngine;
using System.Collections;

public class SetSortingLayer : MonoBehaviour
{

    public string sortingLayerName;        // The name of the sorting layer .
    public int sortingOrder;            //The sorting order

    void Start()
    {
        // Set the sorting layer and order.
        GetComponent<Renderer>().sortingLayerName = sortingLayerName;
        GetComponent<Renderer>().sortingOrder = sortingOrder;
    }
}