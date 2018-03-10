using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance{set;get;}

	private int lives = 3;
	private int score = 0;

	public Transform spawnPosition;
	public Transform p1Transform;

	public Text starsText;
	public Text livesText;

	// hitpoints
	public Image CurrentHP;
	public Text	TextHP;
	private float hitpoint = 100;
	private float maxHP = 100;

	private void Awake()
	{
		Instance = this;
		starsText.text = "Current Stars: "+ score.ToString();
		livesText.text = "Lives Left: " + lives.ToString();
	}

	private void Update()
	{
		if (p1Transform.position.y < -20 || hitpoint <= 0)	// player1 falls and dies
		{
			Death ();
		}
	}

	public void CollectStar()
	{
		score++;
		starsText.text = "Current Stars: "+ score.ToString();
	}

	private void UpdateHealthBar()
	{
		float ratio = hitpoint / maxHP;
		CurrentHP.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		TextHP.text = (ratio * 100).ToString("0") + "%";
	}

	public void TakeDamage(float damage)
	{
		if (hitpoint > 0) {
			hitpoint -= damage;
			UpdateHealthBar ();
			Debug.Log ("take dmg");
		}
	}
		
	public void Death()
	{
		p1Transform.position = spawnPosition.position;
		lives--;
		livesText.text = "Lives Left: " + lives.ToString();
		Debug.Log ("Died.. Respawning..");
		hitpoint = maxHP;
		UpdateHealthBar ();
	}

	public void Finish(){
		if (score >= 3) {
			Debug.Log ("Level Complete!");
		}
	}
}
