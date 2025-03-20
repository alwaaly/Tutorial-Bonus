using UnityEngine;

public class LightType1 : LightBase {
    private void Update() {
        if (!isOn) return;
        progress += Time.deltaTime;
        objectRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(intensityID, progress);
        objectRenderer.SetPropertyBlock(propertyBlock);
        progress %= 1;
    }
}
