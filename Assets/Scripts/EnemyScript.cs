using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
	
    public GameObject Bullet;
    public Transform shotSpawn;
    public Transform shotSpawn2;
	public short localSpeed;

    public AudioClip enemyExplosion;

//	private WaveAI waveAI;
    private float nextFire;
//    private GameController gameController;

	//initialise all variables and start spawning animation
    void Start()
    {
		Random.seed = System.DateTime.Now.Millisecond;
//		waveAI = transform.parent.GetComponent<WaveAI>();
//		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

 /*   void Update()
    {
		Random.seed = System.DateTime.Now.Millisecond;
		waveAI.CheckDelete();
		waveAI.DoMovement();
    }

	public void Movement()
	{
        transform.LookAt(waveAI.player.transform.position);
	} */
	

    public void Shoot()
    {
        if (Time.time > nextFire)
        {
//			nextFire = Time.time + waveAI.fireRate + Random.Range(0, 2);
			Instantiate(Bullet, shotSpawn.position, shotSpawn.rotation);
			Instantiate(Bullet, shotSpawn2.position, shotSpawn2.rotation);
        }
    }


	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Hit1");
		if (other.gameObject.tag == "PlayerBullet")
		{
            SoundManager.instance.RandomizeSfx(enemyExplosion);
            Destroy(this.gameObject);
			Destroy(other.gameObject);
//            gameController.AddScore(waveAI.scoreValue);
        }
	}
}
