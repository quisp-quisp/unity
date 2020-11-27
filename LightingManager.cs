using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light _sun;
    [SerializeField] private LightingPreset _preset;
    [SerializeField, Range(0, 24)] private float _timeOfDay;
    [SerializeField] private float _secondsInDay = 24;
    [SerializeField] Material _skybox;
    [SerializeField] private float dawnStart, dawnEnd;
    [SerializeField] private float duskStart, duskEnd;

    private void Start()
    {
        //_skybox.SetFloat("_BlendCubeMaps", 0f);
    }

    private void Update()
    {
        if (_preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            _timeOfDay += Time.deltaTime * 24 / _secondsInDay;
            _timeOfDay %= 24; //Modulus to ensure always between 0-24
        }
        UpdateLighting();
        UpdateSkybox();
    }


    private void UpdateLighting()
    {
        float timePercent = _timeOfDay / 24;

        //Set ambient and fog
        RenderSettings.ambientLight = _preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (_sun != null)
        {
            _sun.color = _preset.DirectionalColor.Evaluate(timePercent);

            _sun.transform.localRotation = Quaternion.Euler(new Vector3(timePercent * 360f - 90, 180, 0));
        }
    }

    private void UpdateSkybox()
    {
        float blend;
        // dawn
        if (dawnStart <= _timeOfDay && _timeOfDay <= dawnEnd)
        {
            blend = dawnEnd - _timeOfDay;
            blend /= dawnEnd - dawnStart;
        }
        // dusk
        else if (duskStart <= _timeOfDay && _timeOfDay <= duskEnd)
        {
            blend = _timeOfDay - duskStart;
            blend /= duskEnd - duskStart;
        }
        // day
        else if (dawnEnd <= _timeOfDay && _timeOfDay <= duskStart)
        {
            blend = 0;
        }
        // night
        else
        {
            blend = 1;
        }
        _skybox.SetFloat("_BlendCubemaps", blend);
    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (_sun != null)
        {
            return;
        }

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            _sun = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    _sun = light;
                    return;
                }
            }
        }
    }

}
