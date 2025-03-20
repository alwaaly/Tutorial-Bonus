using UnityEngine;

public class LightBase : MonoBehaviour, IInteractable {
    [SerializeField] protected AnimationCurve lightIntensityCurve;
    [SerializeField] protected Renderer objectRenderer;
    protected MaterialPropertyBlock propertyBlock;
    protected int intensityID;
    protected int ColorID;
    protected float progress;
    public bool isOn;
    private void Awake() {
        intensityID = Shader.PropertyToID("_Intensity");
        ColorID = Shader.PropertyToID("_Color");
        propertyBlock = new();
        isOn = true;
    }
    public void OnInteract() {
        isOn = !isOn;
        if (!isOn) {
            propertyBlock.SetFloat(intensityID, 0);
            propertyBlock.SetColor(ColorID, Color.white);
            objectRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
