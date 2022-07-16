using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class DependsOnAttribute : PropertyAttribute {

    public enum ComparisonType { Equality, GreaterThan, SmallerThan, NonEquality };

    public string propertyToCompare;

    public object valueToCompare;

    public ComparisonType comparisonType;

    public DependsOnAttribute(string propertyToCompare, object valueToCompare = null, ComparisonType comparisonType = ComparisonType.Equality) {
        this.propertyToCompare = propertyToCompare;
        this.valueToCompare = valueToCompare;
        this.comparisonType = comparisonType;
    }
}