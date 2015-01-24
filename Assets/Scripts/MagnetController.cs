using UnityEngine;
using System.Collections;

public class MagnetController : MonoBehaviour {
    //public GameObject sign;

    public float quantityOfCharge = 1.0f;

    public float factor = 5.0f;

    private GameObject nearestMagnet = null;

	// Use this for initialization
	void Start () {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(Color.yellow, Color.red);
        lineRenderer.SetWidth(0.1f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void FixedUpdate()
    {
        DrawLineToNearestMagnet();
        AddInstanceForce();
    }

    public void OnQuantityChanged() {
        if (UIProgressBar.current != null)
        {
            quantityOfCharge = Mathf.RoundToInt(UIProgressBar.current.value * 2) - 1.0f;
        }
    }

    GameObject GetNearestMagnet()
    {
        GameObject[] magnets = GameObject.FindGameObjectsWithTag("Magnet");
        GameObject nearest = null;
        float min = float.MaxValue;
        for (int i = 0; i < magnets.Length; i++)
        {
            float distance = Vector3.Distance(magnets[i].transform.position, transform.position);
            if (distance < min && !Mathf.Approximately(distance, 0))
            {
                nearest = magnets[i];
                min = distance;
            }
        }
        return nearest;
    }

    void DrawLineToNearestMagnet()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        nearestMagnet = GetNearestMagnet();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, nearestMagnet.transform.position);
    }

    void AddInstanceForce()
    {
        Vector3 direction = nearestMagnet.transform.position - transform.position;
        if (direction.magnitude < 1f) return;
        float distance = Vector3.Distance(nearestMagnet.transform.position, transform.position);
        float force = -1.0f * nearestMagnet.GetComponent<MagnetController>().quantityOfCharge * quantityOfCharge / (distance * distance);
        direction.Normalize();
        Vector2 direction2d = new Vector2(direction.x,direction.y);
        rigidbody2D.AddForce(direction2d * force * factor);
    }
}
