using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoscoFightScenario : MonoBehaviour
{
	[field : SerializeField]
	private int startStageNumber;
	[field: SerializeField]
	private int numberOfDroneWaves;
	[field: SerializeField]
	private Mob boss;
	[field: SerializeField]
	private Mob bossStage3;

	public int dronesLeft;

	private int currentStageNumber;
	private int wavesLeft;

	private void Start()
	{
		currentStageNumber = startStageNumber;
		wavesLeft = numberOfDroneWaves;
	}

	private void Update()
    {
		switch (currentStageNumber)
		{
		case 0:
			ManageFirstStage();
			break;
		case 1:
			ManageSecondStage();
			break;
		case 2:
			ManageThirdStage();
			break;
		case 3:
			print("BOSCO DEFEATED!");
			return;
		}
    }

	private void ManageFirstStage()
	{
		if (boss.Health < boss.MaxHealth * 0.3f)
			currentStageNumber = 1;
	}

	/*
	 * Спавнит дронов пока не будет уничтожено n волн
	 */
	private void ManageSecondStage()
	{
		if (wavesLeft > 0 && dronesLeft <= 0)
		{
			wavesLeft--;
			//SpawnWave(); //Найти где
			//dronesLeft = Сколько дронов заспавнилось
		}
	}

	private void ManageThirdStage()
	{

	}
}
