using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour {

    public float scroll_Speed = 0.1f;

    private MeshRenderer mesh_Renderer;

    void Awake()
    {
        mesh_Renderer = GetComponent<MeshRenderer>();
    }

	// Update is called once per frame
	void Update ()
    {
        float y = Time.time * scroll_Speed;

        Vector2 offset = new Vector2(0, y);

        mesh_Renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
	}
}
