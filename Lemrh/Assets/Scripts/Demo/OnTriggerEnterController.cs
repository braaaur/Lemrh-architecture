using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterController : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField] private string validTag = "Player";
	[SerializeField] private bool deactivateAfterTrigger;

	[Header("Output")]
	[SerializeField] private UnityEvent playerHit;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("OnTriggerEnter2D( " + collision.gameObject.name + ")");

		if (collision.CompareTag(validTag))
		{
			playerHit.Invoke();

			if (deactivateAfterTrigger)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
