using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity = -12;
	public float jumpHeight = 1;
	[Range(0, 1)]
	public float airControlPercent;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;
	float velocityY;
	bool inJump;

	Animator animator;
	Transform cameraT;
	CharacterController controller;
	string lastAnimState;

	void Start()
	{
		animator = GetComponent<Animator>();
		cameraT = Camera.main.transform;
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		// input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector2 inputDir = input.normalized;
		bool running = Input.GetKey(KeyCode.LeftShift);

		Move(inputDir, running);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			inJump = true;
			Jump();
		}
		// animator
		float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);

		string animState;
		if (inputDir.magnitude == 0)
			animState = "Idle";
		else
			animState = running ? "Run" : "Walk";

		UpdateAnimationState(animState);
	}

	void Move(Vector2 inputDir, bool running)
	{
		if (inputDir != Vector2.zero)
		{
			float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}

		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		velocityY += Time.deltaTime * gravity;
		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move(velocity * Time.deltaTime);
		currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

		if (controller.isGrounded)
		{
			velocityY = 0;
			if (inJump)
			{
				inJump = false; // Now we have landed from jump, duh
				animator.CrossFadeInFixedTime("Landing", .1f);
			}
		}
	}

	void Jump()
	{
		if (controller.isGrounded)
		{
			float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;

			animator.CrossFadeInFixedTime(lastAnimState == "Idle" ? "Idle Jump" : "Running Jump", .1f);
		}
	}

	float GetModifiedSmoothTime(float smoothTime)
	{
		if (controller.isGrounded)
		{
			return smoothTime;
		}

		if (airControlPercent == 0)
		{
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

	void UpdateAnimationState(string animState)
	{
		if (lastAnimState != animState)
		{
			//animator.ResetTrigger(lastAnimState);
			animator.SetTrigger(animState);

			lastAnimState = animState;
			Debug.Log($"Animation State is now '{animState}'");
		}
	}

	void LedgeHit(Transform ledge)
	{
		Debug.Log($"Hit {ledge}");
		
		if (!controller.isGrounded || Input.GetKey(KeyCode.Space))
		{
			Debug.Log("Hanging on to ledge");
			//transform.Translate(ledge.position);
		}
	}
}