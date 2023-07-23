using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCtrl : MonoBehaviour
{
	[SerializeField]
	SphereCollider HIT1;
	[SerializeField]
	SphereCollider HIT2;
	[SerializeField]
	CapsuleCollider HIT3;

	public void Set()
	{
		HIT1.enabled = true;
		HIT2.enabled = true;
		HIT3.enabled = true;
		HIT1.gameObject.SetActive(true);
		HIT2.gameObject.SetActive(true);
		HIT3.gameObject.SetActive(true);
	}
	public void Reset()
	{
		HIT1.enabled = false;
		HIT2.enabled = false;
		HIT3.enabled = false;
		HIT1.gameObject.SetActive(false);
		HIT2.gameObject.SetActive(false);
		HIT3.gameObject.SetActive(false);
	}
}
