using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    private Texture _texture;
    private Vector2 _offset;
    private Vector2 _tiling;
    private bool _trigger = false;
    private float _fader = 0f;
    private Material _material;

    public float BlendSpeed { get; set; } = 3f;

    public void Transition(Texture texture, Vector2 offset, Vector2 tiling)
    {
        _offset = offset;
        _tiling = tiling;
        _texture = texture;

        _material.SetTexture("_Texture2", _texture);
        _material.SetTextureOffset("_Texture2", _offset);
        _material.SetTextureScale("_Texture2", _tiling);

        _trigger = true;
    }

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        _material.SetFloat("_Blend", _fader);
    }

    private void Update()
    {
        if (!_trigger)
        {
            return;
        }

        _fader += BlendSpeed * Time.deltaTime;
        _material.SetFloat("_Blend", _fader);

        if (_fader >= 1f)
        {
            _trigger = false;
            _fader = 0f;

            _material.SetTexture("_MainTex", _texture);
            _material.SetTextureOffset("_MainTex", _offset);
            _material.SetTextureScale("_MainTex", _tiling);
            _material.SetFloat("_Blend", _fader);
        }
    }
}
