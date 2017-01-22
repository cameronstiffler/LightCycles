using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float speed = 3;
	public GameObject model;
	public float tilt = 1;
	public float torque = 1;
	public float thrust = 2;
	public GameObject trail;
	private Vector3 startAngle;
	private Vector3 newAngle;
	private Vector3 currentAngle;
	private Vector3 trailSegmentStart;
	private Vector3 trailSegmentEnd;
	public GameObject trailOrigin;
	private int straightAway = 1;
	private int trailCounter = 0;
	private GameObject currentWall;
	private int wallRenderCounter = 0;

	void Start() {
		startAngle = model.transform.eulerAngles;
		currentAngle = startAngle;
		trail = Resources.Load ("prefabs/TrailWall") as GameObject;
		currentWall = trail;
		trailSegmentStart = trailOrigin.transform.position;
		trail.transform.rotation = transform.rotation;
		trail.transform.position = trailSegmentStart;
		//CreateTrailSegment ();
	}

	void Update()
	{
		var horiz = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var speed = Input.GetAxis("Vertical") * Time.deltaTime;
		transform.Rotate(0,horiz, 0);
		currentAngle = model.transform.eulerAngles;
		float x = Input.GetAxis("Horizontal") * 15.0f; // might be negative, just test it
		Vector3 euler = model.transform.localEulerAngles;
		euler.x = (euler.x > 180f) ? euler.x - 360f : euler.x;
		euler.x = Mathf.Lerp(euler.x, x, 2.0f * Time.deltaTime);
		model.transform.localEulerAngles = euler;
		float moveVertical = Input.GetAxis ("Vertical");
		GetComponent<Rigidbody>().position += transform.right * Time.deltaTime * (moveVertical+1.0f) * thrust * -1;

		float dist = Vector3.Distance(trailSegmentStart,trailOrigin.transform.position);
		currentWall.transform.localScale = new Vector3(dist, 3f, .1f);
		currentWall.transform.position = ((trailOrigin.transform.position - trailSegmentStart) * 0.5f)+trailSegmentStart;
		Debug.Log ("currentWall.name"+currentWall.name+"  currentWall.pos:" + currentWall.transform.position + "  currentWall.Scale.x:" + currentWall.transform.localScale.x);
		if (x !=0 && wallRenderCounter > 10) {
			CreateTrailSegment ();
			wallRenderCounter = 0;
		} 
		wallRenderCounter++;
	}

	void CreateTrailSegment()
	{
		trailSegmentStart = trailOrigin.transform.position;
		Instantiate (trail);
		currentWall = trail;
		trailCounter++;
		trail.name = "trail"+trailCounter;
		trail.transform.rotation = transform.rotation;
		//trail.transform.position = trailSegmentStart;
	}
}