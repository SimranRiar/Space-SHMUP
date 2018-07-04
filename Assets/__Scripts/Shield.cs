using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    [Header("Set in inspector")]
	public float rotationsPerSecond = 0.1f;
	
    [Header("Set Dynamically")]
	public int levelShown = 0;

    Material mat;

	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	void Update () {
		int currlevel = Mathf.FloorToInt (Hero.S.shieldLevel);

		if (levelShown != currlevel) {
			levelShown = currlevel;
			mat.mainTextureOffset = new Vector2(0.2f*levelShown, 0f);
		}

		float rZ = (rotationsPerSecond * Time.time * 360) % 360f;
		transform.rotation = Quaternion.Euler (0, 0, rZ);

	}
}
