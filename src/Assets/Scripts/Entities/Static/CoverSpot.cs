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

	public bool isOccupied;
	public bool isDestroyed;
	public bool isSafe;  //Define if player can easily attack/see this cover spot
	public float radius = 2;
	private Mob currentUser;
	private Mob player;
	public LayerMask layerMask;

	private void Start()
	{
		player = PlayerController.Instance.Possessed;
	}

	private void Update()
	{
		player = PlayerController.Instance.Possessed;
		CheckSafety();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isOccupied)
		{
			if (other.transform.parent.TryGetComponent(out Mob mob) && mob.CanTakeCover)
				isOccupied = true;
			currentUser = mob;
		}	
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob))
		{
			if (currentUser == mob)
			{
				isOccupied = false;
				currentUser = null;
			}
		}
	}

	private void CheckSafety()
	{
		Vector3 dir = (player.transform.position - transform.position).normalized;
		float angle = Vector3.SignedAngle(dir, transform.forward, Vector3.up);
		if (Mathf.Abs(angle) < 90)  //Checking that the new point is not in the target's direction
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
		if (isSafe)
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
