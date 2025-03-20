#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
[CustomPropertyDrawer(typeof(SequencerMethodReference))]
public class MethodReferenceDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty targetObjectProp = property.FindPropertyRelative("targetObject");
        SerializedProperty componentIndexProp = property.FindPropertyRelative("componentIndex");
        SerializedProperty methodNameProp = property.FindPropertyRelative("methodName");
        SerializedProperty methodIndexProp = property.FindPropertyRelative("methodIndex");
        SerializedProperty selectedOrderProp = property.FindPropertyRelative("SelectedOrder");
        SerializedProperty OldMethodsNameProp = property.FindPropertyRelative("OldMethodsName");
        SerializedProperty OldComponentNameProp = property.FindPropertyRelative("OldComponentName");
        SerializedProperty OldSelectedMethodNameProp = property.FindPropertyRelative("OldSelectedMethodName");

        Rect objectRect = new Rect(position.x,position.y, position.width / 2, EditorGUIUtility.singleLineHeight * 3);
        EditorGUI.PropertyField(objectRect, targetObjectProp,new GUIContent());
        //position.y += EditorGUIUtility.singleLineHeight;
        position.x += position.width / 2;

        if (targetObjectProp.objectReferenceValue != null) {
            GameObject targetObject = targetObjectProp.objectReferenceValue as GameObject;
            Component[] components = targetObject.GetComponents<Component>();
            if (components.Length > 0) {
                string[] componentNames = new string[components.Length];
                for (int i = 0; i < components.Length; i++) {
                    componentNames[i] = components[i].GetType().Name;
                }
                Rect componentRect = new Rect(position.x, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
                int selectedComponentIndex = componentIndexProp.intValue;
                selectedComponentIndex = Mathf.Clamp(selectedComponentIndex, 0, components.Length - 1);
                selectedComponentIndex = EditorGUI.Popup(componentRect, selectedComponentIndex, componentNames);
                componentIndexProp.intValue = selectedComponentIndex;

                position.y += EditorGUIUtility.singleLineHeight;
                //position.x += position.width / 2;

                Type componentType = components[selectedComponentIndex].GetType();
                MethodInfo[] methods = componentType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).Where(m =>
                            m.ReturnType == typeof(void) && m.GetParameters().Length == 0 && !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_")).ToArray();
                if (methods.Length > 0) {
                    string[] methodNames = new string[methods.Length];
                    for (int i = 0; i < methods.Length; i++) {
                        methodNames[i] = methods[i].Name;
                    }

                    // fix the problem of selecting random methedo after editing the selected script ex. adding removing method
                    bool thereIsChangeINTheScript = false;
                    if (OldMethodsNameProp.arraySize != 0) {
                        if (!string.IsNullOrEmpty(OldComponentNameProp.stringValue)) {
                            if (OldComponentNameProp.stringValue == components[componentIndexProp.intValue].GetType().ToString()) {
                                for (int i = 0; i < methods.Length; i++) {
                                    for (int j = 0; j < OldMethodsNameProp.arraySize; j++) {
                                        if (methodNames[i] == OldMethodsNameProp.GetArrayElementAtIndex(i).stringValue) {
                                            thereIsChangeINTheScript = false;
                                            break;
                                        }
                                        thereIsChangeINTheScript = true;
                                    }
                                    if (thereIsChangeINTheScript) break;
                                }
                            }
                        }
                    }

                    if (thereIsChangeINTheScript) {
                        int OldMethodIndex = 0;
                        for (int i = 0; i < methodNames.Length; i++) {
                            if(OldSelectedMethodNameProp.stringValue == methodNames[i])
                                OldMethodIndex = i;
                        }
                        methodIndexProp.intValue = OldMethodIndex;
                    }

                    SetNewStringArray(OldMethodsNameProp, methodNames);
                    OldComponentNameProp.stringValue = componentNames[selectedComponentIndex];


                    Rect methodRect = new Rect(position.x, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
                    int selectedMethodIndex = methodIndexProp.intValue;
                    selectedMethodIndex = Mathf.Clamp(selectedMethodIndex, 0, methodNames.Length - 1);
                    selectedMethodIndex = EditorGUI.Popup(methodRect, selectedMethodIndex, methodNames);
                    methodIndexProp.intValue = selectedMethodIndex;
                    OldSelectedMethodNameProp.stringValue = methodNames[selectedMethodIndex];
                    methodNameProp.stringValue = methodNames[selectedMethodIndex];

                    position.y += EditorGUIUtility.singleLineHeight;

                    Rect orderRect = new Rect(position.x, position.y, position.width / 2, EditorGUIUtility.singleLineHeight);
                    int selecetedOrder = selectedOrderProp.intValue;
                    string[] orderNames = new string[Enum.GetValues(typeof(SequencerMethodReference.AcionInvokeOrder)).Length];
                    for (int i = 0; i < orderNames.Length; i++) {
                        orderNames[i] = ((SequencerMethodReference.AcionInvokeOrder)i).ToString();
                    }
                    selecetedOrder = Mathf.Clamp(selecetedOrder, 0, orderNames.Length - 1);
                    selecetedOrder = EditorGUI.Popup(orderRect, selecetedOrder, orderNames);
                    selectedOrderProp.intValue = selecetedOrder;
                }
            }
        }

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // Height depends on whether a GameObject is assigned
        return EditorGUIUtility.singleLineHeight * 3;
    }
    void SetNewStringArray(SerializedProperty stringArrayProp, string[] newArray) {
        // Resize the array to match the new array
        stringArrayProp.arraySize = newArray.Length;

        // Copy elements from the new array
        for (int i = 0; i < newArray.Length; i++) {
            SerializedProperty elementProp = stringArrayProp.GetArrayElementAtIndex(i);
            elementProp.stringValue = newArray[i];
        }

        // Apply changes
        stringArrayProp.serializedObject.ApplyModifiedProperties();
    }
}
#endif