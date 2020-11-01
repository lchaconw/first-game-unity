using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstDown : MonoBehaviour
{
	
	public Vector3 posStart;
	

	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	private void Teletransportar()
	{
		transform.localPosition = new Vector3(posStart.x, posStart.y, posStart.z + 8);


	}

	private void OnCollisionEnter(Collision player)
	{
			if (player.gameObject.CompareTag("Player"))
		{
				Invoke("Teletransportar", 1.4f);
				
				
		}
		
	}


}
