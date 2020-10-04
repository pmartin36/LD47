using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	public LayerMask GrabbableMask;
	public LayerMask ExitMask;

	public LayerMask TileMask;
	public LayerMask RealTileMask;

	private RealTile CurrentTile;

	private Animator anim;
	private Coroutine activeRoutine;

	private Vector3 originalPosition;
	private Quaternion originalRotation;
	private Vector3 originalScale;

	public const float ActionTime = 0.5f;

	public bool ReachedExit { get; set; }

	private Grabbable Grabbable;
	private bool HasKey => Grabbable != null;

	private Player Dupe;
	private bool IsDupe;

	private SpriteRenderer miniKey;
	public SpriteRenderer sr;
	public float miniKeyPercent = 0;
	private float miniKeyActual = 0;

	private Vector3 stretchDirection = Vector2.up;
	private Vector3 orthogonalStretchDirection => stretchDirection.Rotate(90).Abs();
	public float stretchAmount;

	[Header("Sounds")]
	private AudioSource audioSource;
	public AudioClip MoveClip;
	public AudioClip KeyGetClip;
	public AudioClip GrabFailClip;

	public void Start() {
		miniKey = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(k => k.gameObject != sr.gameObject);
		sr.transform.localScale = Vector3.one;

		originalPosition = transform.position;
		originalRotation = transform.rotation;
		originalScale = sr.transform.localScale;

		if (!IsDupe) {
			audioSource = GetComponent<AudioSource>();
			anim = GetComponent<Animator>();
			GameManager.Instance.LevelManager.Player = this;
			CurrentTile = Physics2D.OverlapBox(this.transform.position, Vector2.one * 0.9f, 0, RealTileMask).GetComponent<RealTile>();
		}
	}

	public void LateUpdate() {
		sr.transform.localScale = originalScale 
			+ stretchAmount * stretchDirection * 0.25f
			- stretchAmount * orthogonalStretchDirection * 0.1f;

		float mod = HasKey ? 1 : 0;
		miniKeyActual = Mathf.Max(miniKeyActual, miniKeyPercent) * mod;
		miniKey.color = mod * Colors.Black;
		miniKey.transform.localScale = Vector3.Lerp(Vector3.one * 0.7f, Vector3.one * 0.3f, miniKeyActual);
		miniKey.transform.localEulerAngles = Vector3.forward * -23f * miniKeyActual;
		miniKey.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up * 0.05f, miniKeyActual);
	}

	public Coroutine Move(Direction d) {
		RaycastHit2D tile = Physics2D.RaycastAll(this.transform.position, d.Value, 1f, TileMask)
			.FirstOrDefault(t => t.collider != null && t.collider.gameObject != CurrentTile.gameObject);
		if(tile.collider != null) {
			Tile t = tile.collider.GetComponent<Tile>();
			if(t is RealTile) {
				activeRoutine = StartCoroutine(WalkAction(d));
			}
			else if(t is WrapTile) {
				// shoot a ray further ahead ot make sure this is, in fact, an edge
				RaycastHit2D fTile = Physics2D.RaycastAll(this.transform.position, d.Value, 15f, RealTileMask ^ TileMask)
					.FirstOrDefault(hit => hit.collider != null && hit.collider.gameObject != t.gameObject);
				if(fTile.collider == null) {
					Vector3 currentPosition = transform.position;
					Dupe = Instantiate(this, this.transform.position, this.transform.rotation, null);
					Dupe.IsDupe = true;
					Dupe.anim = Dupe.GetComponent<Animator>();
					StartCoroutine(Dupe.WalkAction(d));

					RaycastHit2D otherSideWrapTileRay = Physics2D.RaycastAll(this.transform.position, -d.Value, 15, RealTileMask ^ TileMask).Last();
					if(otherSideWrapTileRay.collider != null) {
						if(Mathf.Abs(d.Value.x) > Mathf.Abs(d.Value.y)) {
							int x = (int)otherSideWrapTileRay.transform.position.x; // intentionally truncate
							transform.position = new Vector2(x, currentPosition.y);
						}
						else {
							int y = (int)otherSideWrapTileRay.transform.position.y;
							transform.position = new Vector2(currentPosition.x, y);
						}

						activeRoutine = StartCoroutine(WalkAction(d));
					}
				}
			}

			audioSource.pitch = 0.7f + 0.2f * Random.value;
			audioSource.clip = MoveClip;
			audioSource.Play();
		}
		else {
			activeRoutine = StartCoroutine(NoopAction());
		}
		return activeRoutine;
	}

	public Coroutine Grab() {
		Collider2D grabbable = Physics2D.OverlapBox(this.transform.position, Vector2.one * 0.9f, 0, GrabbableMask);
		if(grabbable != null) {
			Grabbable = grabbable.gameObject.GetComponent<Grabbable>();
			Grabbable.gameObject.SetActive(false);
			audioSource.clip = KeyGetClip;
		}
		else {
			audioSource.clip = GrabFailClip;
		}
		audioSource.pitch = 1f;
		audioSource.Play();
		activeRoutine = StartCoroutine(GrabAction());
		return activeRoutine;
	}

	public Coroutine Exit() {
		Collider2D exit = Physics2D.OverlapBox(this.transform.position, Vector2.one * 0.9f, 0, ExitMask);
		if(exit != null) {
			Exit e = exit.gameObject.GetComponent<Exit>();
			if( !e.Locked || (e.Locked && HasKey) ) {
				ReachedExit = true;
			}
		}
		activeRoutine = StartCoroutine(ExitAction());
		return activeRoutine;
	}

	public void Reset() {
		transform.position = originalPosition;
		transform.rotation = originalRotation;
		sr.transform.localScale = originalScale;

		stretchAmount = 0f;
		stretchDirection = Vector2.up;
		anim.SetBool("Moving", false);
		anim.SetBool("Grabbing", false);
		
		CurrentTile = Physics2D.OverlapBox(originalPosition, Vector2.one * 0.9f, 0, RealTileMask).GetComponent<RealTile>();

		if (HasKey) {
			miniKeyActual = 0f;
			Grabbable.Reset();
			Grabbable = null;
			miniKey.color = Color.clear;
		}

		if(activeRoutine != null)
			StopCoroutine(activeRoutine);

		DestroyDupe();
	}

	private void DestroyDupe() {
		if (Dupe != null) {
			Dupe.gameObject.Destroy();
			Dupe = null;
		}
	}

	private IEnumerator WalkAction(Direction d) {
		var startPosition = transform.position;
		var position = transform.position + (Vector3)d.Value;

		anim.SetBool("Moving", true);
		stretchDirection = d.Value;

		float t = 0f;
		while(t < ActionTime) {
			transform.position = Vector2.Lerp(startPosition, position, t / ActionTime);
			t += Time.deltaTime;
			yield return null;
		}
		transform.position = position;
		if(!IsDupe) {
			var tile = Physics2D.OverlapBox(this.transform.position, Vector2.one * 0.9f, 0, RealTileMask);
			if(tile != null) {
				CurrentTile = tile.GetComponent<RealTile>();
			}
			anim.SetBool("Moving", false);
			stretchDirection = Vector2.up;
			DestroyDupe();
		}

	}

	private IEnumerator GrabAction() {
		stretchDirection = Vector2.up;
		anim.SetBool("Grabbing", true);
		yield return new WaitForSeconds(ActionTime);
		anim.SetBool("Grabbing", false);
	}

	private IEnumerator ExitAction() {
		yield return new WaitForSeconds(ActionTime);
	}

	private IEnumerator NoopAction() {
		yield return new WaitForSeconds(ActionTime);
	}
}
