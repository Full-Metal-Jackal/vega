using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calss representing a certain scenario.
/// Not supposed to be interscenic or static.
/// Needed to provide ability to call singletons' method during cutscenes.
/// </summary>
public class ScenarioController : MonoBehaviour
{
	/// <summary>
	/// Since Unity cannot handle properties, there's a setter for this one.
	/// Must not be used anywhere in code.
	/// </summary>
	/// <param name="isPlaying">New state of PlayingScene.</param>
	public void SetPlayingScene(bool isPlaying) =>
		Game.PlayingScene = isPlaying;

	public void SetTrackedMob(Mob mob) =>
		CameraController.Instance.SetTrackedMob(mob);

	public void AddPOI(GameObject poi) =>
		CameraController.Instance.AddPOI(poi);

	public void SetOnlyPOI(GameObject poi) =>
		CameraController.Instance.SetOnlyPOI(poi);

	public void ResetPOI() =>
		CameraController.Instance.ResetPOI();

	// <TODO> I'm not sure this one belongs here
	public void SetPlayer(Mob mob) =>
		PlayerController.Instance.PossessMob(mob);
}
