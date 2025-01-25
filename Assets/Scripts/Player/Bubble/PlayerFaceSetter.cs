using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerFaceSetter : MonoBehaviour
{

	[SerializeField] private List<GameObject> _faces;

	private void Awake()
	{
		SetFace();
	}

	void SetFace()
	{
		int randIndex = Random.Range(0, _faces.Count);

		for (int i = 0; i < _faces.Count; i++)
		{
			if(randIndex == i)
			{
				_faces[i].SetActive(true);
				continue;
			}
			_faces[i].SetActive(false);
		}
	}
}
