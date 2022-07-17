using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DependsOnAttribute))]
public class DependsOnAttributeDrawer : PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

        return NeedsToBeShown(property) ? EditorGUI.GetPropertyHeight(property) : 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        DependsOnAttribute atr = (DependsOnAttribute) attribute;

        if (NeedsToBeShown(property)) {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    private bool NeedsToBeShown(SerializedProperty property) {

        DependsOnAttribute thisAttribute = attribute as DependsOnAttribute;

        if (thisAttribute.propertyToCompare == null || thisAttribute.valueToCompare == null) {
            Debug.LogWarning("[DependsOn]: " + property.name + ": given referenced property or value is missing.");
            return true;
        }

        string pathProp = property.propertyPath;
        string conditionPath = pathProp.Replace(property.name, thisAttribute.propertyToCompare); // required to solve "nested" elements

        SerializedProperty baseAttribute = property.serializedObject.FindProperty(conditionPath);

        if (baseAttribute == null) {
            Debug.LogWarning("[DependsOn]: " + property.name + ": Base attribute with name '" + thisAttribute.propertyToCompare + "' could not be found.");
            return true;
        }

        switch (baseAttribute.propertyType) {

            default:
                Debug.LogWarning("[DependsOn]: " + property.name + ": Unknown property type '" + baseAttribute.propertyType.ToString() + "'.");
                return true;

            case SerializedPropertyType.Enum:
                string ourEnum = thisAttribute.valueToCompare.ToString().ToUpper();
                string otherEnum = ""; // might have been removed in an update
                if (baseAttribute.enumValueIndex == -1 || baseAttribute.enumValueIndex >= baseAttribute.enumDisplayNames.Length) {
                    otherEnum = "Unknown";
                } else {
                    otherEnum = baseAttribute.enumDisplayNames[baseAttribute.enumValueIndex].ToUpper();
                }

                switch (thisAttribute.comparisonType) {

                    case DependsOnAttribute.ComparisonType.Equality:
                        return ourEnum.Equals(otherEnum);

                    case DependsOnAttribute.ComparisonType.NonEquality:
                        return !ourEnum.Equals(otherEnum);

                    default:
                        Debug.LogWarning("[DependsOn]: " + property.name + ": Comparison type type '" + thisAttribute.comparisonType + "' is not supported for Enum.");
                        return true;
                }

            case SerializedPropertyType.Float:
                return Compare(thisAttribute.comparisonType, new Decimal((float) thisAttribute.valueToCompare), new Decimal(baseAttribute.floatValue));

            case SerializedPropertyType.Integer:
                return Compare(thisAttribute.comparisonType, new Decimal((int) thisAttribute.valueToCompare), new Decimal(baseAttribute.intValue));
        }
    }

    private bool Compare(DependsOnAttribute.ComparisonType comparisonType, Decimal thisDecimal, Decimal otherDecimal) {
        switch (comparisonType) {
            case DependsOnAttribute.ComparisonType.Equality:
                return thisDecimal == otherDecimal;
            case DependsOnAttribute.ComparisonType.NonEquality:
                return thisDecimal != otherDecimal;
            case DependsOnAttribute.ComparisonType.GreaterThan:
                return otherDecimal > thisDecimal;
            case DependsOnAttribute.ComparisonType.SmallerThan:
                return otherDecimal < thisDecimal;

            default:
                return false;
        }
    }
}