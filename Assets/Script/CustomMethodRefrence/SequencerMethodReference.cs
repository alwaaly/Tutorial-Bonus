using UnityEngine;
using System.Reflection;

[System.Serializable]
public class SequencerMethodReference {
    public GameObject targetObject;
    public int componentIndex;
    public string methodName;
    public int methodIndex;
    public int SelectedOrder;

    // use thes to select the right method if the script that selected are change (ex. add or remove method)
    public string OldComponentName;
    public string[] OldMethodsName;
    public string OldSelectedMethodName;
    public AcionInvokeOrder Order {
        get {
            return (AcionInvokeOrder)SelectedOrder;
        }
    }
    public enum AcionInvokeOrder { Start, End }

    // Cached data
    private Component _cachedComponent;
    private MethodInfo _cachedMethod;
    private int _lastComponentIndex = -1;
    private string _lastMethodName;

    public void Invoke() {
        // Early exit if target is invalid
        if (targetObject == null ||
            componentIndex < 0 ||
            string.IsNullOrEmpty(methodName)) return;

        // Check if component/method has changed
        bool needsRefresh =
            _lastComponentIndex != componentIndex ||
            _lastMethodName != methodName ||
            _cachedComponent == null;

        if (needsRefresh) {
            RefreshCache();
        }

        // Invoke cached method
        _cachedMethod?.Invoke(_cachedComponent, null);
    }

    private void RefreshCache() {
        // Get component from target object
        Component[] components = targetObject.GetComponents<Component>();
        if (componentIndex < 0 || componentIndex >= components.Length) return;

        _cachedComponent = components[componentIndex];
        _lastComponentIndex = componentIndex;

        // Get method from component
        if (_cachedComponent != null) {
            _cachedMethod = _cachedComponent.GetType().GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.Instance
            );
            _lastMethodName = methodName;
        }
    }
}