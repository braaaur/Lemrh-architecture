using UnityEngine;
using Lemrh;

public class PlayeroMovementController : MonoBehaviour
{
    [Header("Attachments")]
    [SerializeField] Rigidbody2D attachedRigidBody;

	[Header("Input")]
	[SerializeField] LemFloatHybridEvent movementInput;

	[Header("Properties")]
    [SerializeField] Vector2 jumpForceVector = new Vector2(0f, 3f);
	[SerializeField] float movementForceScale = 1f;

	//inner
	private Vector2 movementForceVector = new Vector2();

	private void OnEnable()
	{
		movementInput.RegisterDelegate(MovementInputChanged);
	}

	private void OnDisable()
	{
		movementInput.UnregisterDelegate(MovementInputChanged);
	}

	private void FixedUpdate()
	{
		DoMovement();
	}


	//called from event (via Inspector)
	public void JumpInput()
    {
        TryJump();
	}

	private void MovementInputChanged(bool isDebug)
	{
		if (isDebug)
		{
			Debug.Log("MovementInputChanged() in PlayerController.cs on object '" + gameObject.name + "'");
		}

		movementForceVector.x = movementInput.FloatValue * movementForceScale;
	}

	//jumping
	private void TryJump()
	{
		attachedRigidBody.AddForce(jumpForceVector, ForceMode2D.Impulse); //?
	}

	private void DoMovement()
	{
		attachedRigidBody.AddForce(movementForceVector, ForceMode2D.Force);
	}
}
