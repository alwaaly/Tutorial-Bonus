using UnityEngine;

public class LightType3 : LightBase {
    private void Update() {
        if (!isOn) return;
        progress += Time.deltaTime * 10;
        objectRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(intensityID, lightIntensityCurve.Evaluate(progress));
        propertyBlock.SetColor(ColorID, new Color(.5f * lightIntensityCurve.Evaluate(progress), .1f * lightIntensityCurve.Evaluate(progress), .5f * lightIntensityCurve.Evaluate(progress)));
        objectRenderer.SetPropertyBlock(propertyBlock);
        progress %= 1;
    }
}
