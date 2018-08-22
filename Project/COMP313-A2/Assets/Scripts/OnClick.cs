using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClick : MonoBehaviour
{

	public float maxDistance;
	public Transform player;
	public UnityEvent click;
	Transform button;

    void Start()
    {
        button = GetComponent<Transform>();
    }

    void OnMouseDown()
    {
		if(Vector3.Distance(button.position, player.position) <= maxDistance)
		{
			click.Invoke();
		}
    }
}
