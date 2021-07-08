using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
	private AudioSource playerAudio;
	public AudioClip jumpSound;
	public AudioClip crashSound;
	public float jumpForce;
	public float gravityModifier;
	public bool isOnGround = true;
	public bool gameOver = false;
	public bool gameStart = false;
	public bool doubleJumpUsed = false;
	public float doubleJumpForce;
	private Animator playerAnim;
	public bool doubleSpeed = false;
	public float addPerSecond = .01f;
	public ParticleSystem explosionParticle;
	public ParticleSystem dirtParticle;
	// Start is called before the first frame update
	void Start()
    {
        playerRb = GetComponent<Rigidbody>();
		playerAnim = GetComponent<Animator>();
		playerAudio = GetComponent<AudioSource>();
		Physics.gravity = new Vector3(0, -98F, 0);
	}

	// Update is called once per frame
	void Update()
	{
		playerAnim.speed += addPerSecond * Time.deltaTime;

		if (gameStart == true)
        {
			isOnGround = false;
			doubleJumpUsed = true;
		}

		if (gameOver == true)
        {
			isOnGround = false;
			doubleJumpUsed = true;
			dirtParticle.Stop();
		}

		if (SC_MobileControls.instance.GetMobileButtonDown("Jumping") && isOnGround)
        {
			playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			isOnGround = false;
			playerAnim.SetTrigger("Jump_trig");
			dirtParticle.Stop();
			playerAudio.PlayOneShot(jumpSound, 1.0f);

			doubleJumpUsed = false;
		}
		else if (SC_MobileControls.instance.GetMobileButtonDown("Jumping") && !isOnGround && !doubleJumpUsed)
        {
			doubleJumpUsed = true;
			playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
			playerAnim.Play("Running_Jump", 3, 0f);
			playerAudio.PlayOneShot(jumpSound, 1.0f);
		}

		if (SC_MobileControls.instance.GetMobileButton("Sprinting"))
        {
			doubleSpeed = true;
			playerAnim.SetFloat("Speed_Multiplier", 2.0f);
		}
		else if (doubleSpeed)
        {
			doubleSpeed = false;
			playerAnim.SetFloat("Speed_Multiplier", 1.0f);
		}

		if (SC_MobileControls.instance.GetMobileButtonDown("Reset"))
        {
			Restart();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isOnGround = true;
			dirtParticle.Play();
		}
		else if (collision.gameObject.CompareTag("Obstacle"))
        {
			gameOver = true;
			gameStart = true;
			Debug.Log("Game Over");
			playerAnim.SetBool("Death_b", true);
			playerAnim.SetInteger("DeathType_int", 1);
			explosionParticle.Play();
			dirtParticle.Stop();
			playerAudio.PlayOneShot(crashSound, 1.0f);
			Handheld.Vibrate();
		}
	}

	void Restart()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
