using UnityEngine;
using System.Collections;

public class MagnetController : MonoBehaviour {
    //public GameObject sign;

    public float quantityOfCharge = 1.0f;

    public float factor = 5.0f;

    private GameObject nearestMagnet = null;

    private GameObject strongestMagnet = null;

    private Vector2 strongestForce = Vector2.zero;

	// Use this for initialization
	void Start () {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(Color.red, Color.red);
        lineRenderer.SetWidth(0.1f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void FixedUpdate()
    {
        strongestMagnet = GetStrongestMagnet();
        if (strongestMagnet == null) {
            strongestForce = Vector2.zero;
            return;
        }
        DrawLineToStrongestMagnet();
        rigidbody2D.AddForce(strongestForce);
    }

    public void OnQuantityChanged() {
        if (UIProgressBar.current != null)
        {
            quantityOfCharge = Mathf.RoundToInt(UIProgressBar.current.value * 2) - 1.0f;
        }
    }


    GameObject GetStrongestMagnet() {
        GameObject[] magnets = GameObject.FindGameObjectsWithTag("Magnet");
        GameObject strongest = null;
        float max = float.MinValue;
        for (int i = 0; i < magnets.Length; i++) {
            Vector2 force = ComputeMagentForce(magnets[i].transform.position, magnets[i].GetComponent<MagnetController>().quantityOfCharge);
            if(!Mathf.Approximately(0.0f,force.magnitude) && force.magnitude > max){
                strongest = magnets[i];
                max = force.magnitude;
                strongestForce = force;
            }
        }
        return strongest;
    }

    
    void DrawLineToStrongestMagnet()
    {
        if (strongestMagnet == null) return;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, strongestMagnet.transform.position);
    }

    Vector2 ComputeMagentForce(Vector3 targetPositon, float targetQuantity) {

        Vector3 direction = transform.position - targetPositon;
        float distance = direction.magnitude;
        float force = targetQuantity * quantityOfCharge * factor / (distance * distance);
        direction.Normalize();
        direction *= force;
        return new Vector2(direction.x, direction.y);
    }
    /*
     * 
     * 
    void DrawLineToNearestMagnet()
    {
        if (nearestMagnet == null) return;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, nearestMagnet.transform.position);
    }
     * 
     * 
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
     * 
     * 
    void AddInstanceForce()
    {
        if (nearestMagnet == null) return;
        Vector3 direction = nearestMagnet.transform.position - transform.position;
        if (direction.magnitude < 1f) return;
        float distance = Vector3.Distance(nearestMagnet.transform.position, transform.position);
        float force = -1.0f * nearestMagnet.GetComponent<MagnetController>().quantityOfCharge * quantityOfCharge / (distance * distance);
        direction.Normalize();
        Vector2 direction2d = new Vector2(direction.x,direction.y);
        rigidbody2D.AddForce(direction2d * force * factor);
    }*/
}
