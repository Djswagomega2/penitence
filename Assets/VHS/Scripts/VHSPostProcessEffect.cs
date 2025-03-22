using UnityEngine;
using UnityEngine.Video;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/2DGlitchEffect")]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(VideoPlayer))]
public class VHSPostProcessEffect : MonoBehaviour
{
    public Shader shader;
    public VideoClip VHSClip;

    private float _yScanline;
    private float _xScanline;
    private Material _material = null;
    private VideoPlayer _player;

    void Start()
    {
        // Initialize material and video player
        _material = new Material(shader);
        _player = GetComponent<VideoPlayer>();

        // Ensure VideoPlayer is set to loop and APIOnly render mode (we want it as a texture)
        _player.isLooping = true;
        _player.renderMode = VideoRenderMode.APIOnly;
        _player.audioOutputMode = VideoAudioOutputMode.None;
        _player.clip = VHSClip;
        _player.Play();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Pass video texture to shader
        _material.SetTexture("_VHSTex", _player.texture);

        // Add randomness to scanline effects
        _yScanline += Time.deltaTime * .3f;
        _xScanline -= Time.deltaTime * 0.1f;

        if (_yScanline >= .3f)
        {
            _yScanline = Random.Range(0, 3f);
        }
        if (_xScanline <= 0 || Random.value < 0.05)
        {
            _xScanline = Random.Range(.05f, .1f);
        }

        // Set scanline values in shader
        _material.SetFloat("_yScanline", _yScanline);
        _material.SetFloat("_xScanline", _xScanline);

        // Render the glitch effect
        Graphics.Blit(source, destination, _material);
    }

    protected void OnDisable()
    {
        // Clean up material when disabled
        if (_material)
        {
            DestroyImmediate(_material);
        }
    }
}
