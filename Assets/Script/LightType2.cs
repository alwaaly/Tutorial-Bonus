using UnityEngine;

public class LightType2 : LightBase {

    private void Update() {
        if (!isOn) return;
        progress += Time.deltaTime;
        objectRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(intensityID, lightIntensityCurve.Evaluate(progress));
        propertyBlock.SetColor(ColorID, new Color(.25f * lightIntensityCurve.Evaluate(progress), lightIntensityCurve.Evaluate(progress), .5f * lightIntensityCurve.Evaluate(progress)));
        objectRenderer.SetPropertyBlock(propertyBlock);
        progress %= 1;
    }
}
