using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCtrl : MonoBehaviour
{
	[SerializeField]
	GameObject[] HIT;
	public void Set()
	{
		for (int i = 0; i < HIT.Length; i++)
		{
			HIT[i].gameObject.SetActive(true);
		}
	}
	public void Reset()
	{
		for (int i = 0; i < HIT.Length; i++)
		{
			HIT[i].gameObject.SetActive(false);
		}
	}
}
