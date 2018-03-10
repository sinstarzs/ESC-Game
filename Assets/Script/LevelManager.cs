using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance{set;get;}

	private int lives = 3;
	private int score = 0;

	public Transform spawnPosition;
	public Transform playerTransform;

	public Text starsText;
	public Text livesText;

	private void Awake()
	{
		Instance = this;
		starsText.text = "Current Stars: "+ score.ToString();
		livesText.text = "Lives Left: " + lives.ToString();
	}

	private void Update()
	{
		if (playerTransform.position.y < -20)	// player falls and dies
		{
			playerTransform.position = spawnPosition.position;
			lives--;
			livesText.text = "Lives Left: " + lives.ToString();
			Debug.Log ("Died.. Respawning..");
		}
	}

	public void CollectStar()
	{
		score++;
		starsText.text = "Current Stars: "+ score.ToString();
	}
}
