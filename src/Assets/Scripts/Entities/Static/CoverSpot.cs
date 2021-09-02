using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSpot : MonoBehaviour
{
	// Объект являющийся укрытием. Представляет собой "Щит" простреливаемый с внутренней стороны, но не со внешней.
	// Может быть уничтожено гранатой
	// Досутпен если: 1. Перекрывает обзор игрока
	//                2. Не занят другим Мобом
	//                3. Ближайший
	// Взаимодействет с Навмешем, создает/обязан находиться в зоне Cover.
	// 

	public bool IsOccupied => currentUser;
	public bool isSafe;  //Define if player can easily attack/see this cover spot
	public float radius = 2;
	public Mob currentUser;
	private Mob player;

	private void Update()
	{
		player = PlayerController.Instance.Possessed;
		CheckSafety();
	}

	private void OnTriggerEnter(Collider other)
	{
	
		if (!IsOccupied)
		{
			if (other.transform.parent.TryGetComponent(out Mob mob) && mob.CanTakeCover)
			{
				currentUser = mob;
				print(currentUser + " Entered");
			}	
		}	
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob))
		{
			if (currentUser == mob)
			{
				AI.AIManager ai = currentUser.transform.GetComponentInChildren<AI.AIManager>();
				if (ai != null)
				{
					print(currentUser + " Exited");
					ai.currentCover = null;
				}
				currentUser = null;
			}
		}
	}

	private void CheckSafety()
	{
		Vector3 dir = (player.transform.position - transform.position).normalized;
		float angle = Vector3.SignedAngle(dir, transform.forward, Vector3.up);
		if (Mathf.Abs(angle) < 90)
		{
			isSafe = false;
		}
		else
		{
			isSafe = true;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!IsOccupied && isSafe)
		{
			Gizmos.color = Color.green;
		}
		else
		{
			Gizmos.color = Color.red;
		}
		
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
