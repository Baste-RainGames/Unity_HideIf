using UnityEngine;
using UnityEditor;

public abstract class HidingAttributeDrawer : PropertyDrawer {

    /// <summary>
    /// Checks if a property is set to be hidden by a HideIfAttribute.
    /// 
    /// Usefull for other property drawers that should respect the HideIfAttribute
    /// </summary>
    public static bool CheckShouldHide(SerializedProperty property) {
        try {
            bool shouldHide = false;

            HidingAttribute[] attachedAttributes =
                (HidingAttribute[])
                    property.serializedObject.targetObject.GetType().GetField(property.name).GetCustomAttributes(typeof (HidingAttribute), false);

            foreach (var hider in attachedAttributes) {
                if (!ShouldDraw(property.serializedObject, hider)) {
                    shouldHide = true;
                }
            }

            return shouldHide;
        }
        catch {
            return false;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (!CheckShouldHide(property)) {
            EditorGUI.PropertyField(position, property, true);
        }
    }

    public abstract bool ShouldDraw(SerializedObject obj);

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        //Even if the property height is 0, the property gets margins of 1 both up and down.
        //So to truly hide it, we have to hack a height of -2 to counteract that! 
        return !CheckShouldHide(property) ? base.GetPropertyHeight(property, label) : -2f;
    }

    public static bool ShouldDraw(SerializedObject obj, HidingAttribute hider) {
        var hideIf = hider as HideIfAttribute;
        if (hideIf != null) {
            return HideIfAttributeDrawer.ShouldDraw(obj, hideIf);
        }

        var hideIfNull = hider as HideIfNullAttribute;
        if (hideIfNull != null) {
            return HideIfNullAttributeDrawer.ShouldDraw(obj, hideIfNull);
        }

        var hideIfNotNull = hider as HideIfNotNullAttribute;
        if (hideIfNotNull != null) {
            return HideIfNotNullAttributeDrawer.ShouldDraw(obj, hideIfNotNull);
        }

        var hideIfEnum = hider as HideIfEnumValueAttribute;
        if (hideIfEnum != null) {
            return HideIfEnumValueAttributeDrawer.ShouldDraw(obj, hideIfEnum);
        }

        Debug.LogWarning("Trying to check unknown hider type: " + hider.GetType().Name);
        return false;
    }
}

[CustomPropertyDrawer(typeof (HideIfAttribute))]
public class HideIfAttributeDrawer : HidingAttributeDrawer {

    public override bool ShouldDraw(SerializedObject obj) {
        return ShouldDraw(obj, attribute as HideIfAttribute);
    }

    public static bool ShouldDraw(SerializedObject obj, HideIfAttribute attribute) {
        var prop = obj.FindProperty(attribute.variable);
        if (prop == null) {
            return true;
        }
        return prop.boolValue != attribute.state;
    }
}

[CustomPropertyDrawer(typeof (HideIfNullAttribute))]
public class HideIfNullAttributeDrawer : HidingAttributeDrawer {

    public override bool ShouldDraw(SerializedObject obj) {
        return ShouldDraw(obj, attribute as HideIfNullAttribute);
    }

    public static bool ShouldDraw(SerializedObject obj, HideIfNullAttribute hideIfNullAttribute) {
        var prop = obj.FindProperty(hideIfNullAttribute.variable);
        if (prop == null) {
            return true;
        }

        return prop.objectReferenceValue != null;
    }
}

[CustomPropertyDrawer(typeof (HideIfNotNullAttribute))]
public class HideIfNotNullAttributeDrawer : HidingAttributeDrawer {

    public override bool ShouldDraw(SerializedObject obj) {
        return ShouldDraw(obj, attribute as HideIfNotNullAttribute);
    }

    public static bool ShouldDraw(SerializedObject obj, HideIfNotNullAttribute hideIfNotNullAttribute) {
        var prop = obj.FindProperty(hideIfNotNullAttribute.variable);
        if (prop == null) {
            return true;
        }

        return prop.objectReferenceValue == null;
    }
}

[CustomPropertyDrawer(typeof (HideIfEnumValueAttribute))]
public class HideIfEnumValueAttributeDrawer : HidingAttributeDrawer {

    public override bool ShouldDraw(SerializedObject obj) {
        return ShouldDraw(obj, attribute as HideIfEnumValueAttribute);
    }

    public static bool ShouldDraw(SerializedObject obj, HideIfEnumValueAttribute hideIfEnumValueAttribute) {
        var enumProp = obj.FindProperty(hideIfEnumValueAttribute.variable);
        var states = hideIfEnumValueAttribute.states;
        
        bool equal = false;
        for(int i = 0; i < states.Length; i++) {
            if(states[i] == enumProp.intValue) {
                equal = true; //enumProp.enumValueIndex gives the order in the enum list, not the actual enum value
                break; 
            }
        }

        return equal != hideIfEnumValueAttribute.hideIfEqual;
    }
}
