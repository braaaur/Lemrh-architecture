using UnityEngine;
using UnityEngine.SceneManagement;
using Lemrh;
using System.Runtime.InteropServices;

public class GameController : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] LemIntHybridEvent addPoints;
	[SerializeField] LemIntHybridEvent currentPoints;
	[SerializeField] LemStringHybridEvent currentPointsText;
	[SerializeField] LemBoolHybridEvent isLevelWon;

	private void OnEnable()
	{
		addPoints.RegisterDelegate(AddPointsChanged);
		isLevelWon.RegisterDelegate(IsLevelWonChanged);

		ResetLevel();
	}

	private void OnDisable()
	{
		addPoints.UnregisterDelegate(AddPointsChanged);
		isLevelWon.UnregisterDelegate(IsLevelWonChanged);
	}

	//called by event
	public void GameOver()
    {
        ResetCurrentScene();
    }

	//called by event
	public void GameWon()
	{
		isLevelWon.BoolValue = true;
	}

	private void ResetCurrentScene()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void AddPointsChanged(bool isDebug)
	{
		if (isDebug)
		{
			Debug.Log("AddPointsChanged() in GameController.cs - addPoints.IntValue = " + addPoints.IntValue.ToString());
		}

		CurrentPoints += addPoints.IntValue;
	}

	private void IsLevelWonChanged(bool isDebug)
	{
		if (isLevelWon.BoolValue)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	private void ResetLevel()
	{
		CurrentPoints = 0;
		isLevelWon.BoolValue = false;
	}

	private int CurrentPoints
	{
		set
		{
			currentPoints.IntValue = value;

			currentPointsText.StringValue = currentPoints.IntValue.ToString();
		}
		get
		{
			return currentPoints.IntValue;
		}
	}
}
