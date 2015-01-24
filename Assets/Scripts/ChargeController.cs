using UnityEngine;
using System.Collections;

public class ChargeController : MonoBehaviour {

	public int quantityOfCharge = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		other.gameObject.GetComponent<MagnetController>().quantityOfCharge += quantityOfCharge;

        gameObject.SetActive(false);
    }
}
