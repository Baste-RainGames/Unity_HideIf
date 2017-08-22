# Unity_HideIf

This project adds a small set of attributes with attached drawers for use in Unity. They allow you to conditionally hide fields in your MonoBehaviours based on the value of other fields.

This is usefull when you have scripts with small variations, where only some variations need certain fields.

## Installation

Drop everything in your Assets folder. The code is placed in Plugins/Hideif, and contains one script containing the attribute definitions, one script for the propertyDrawers for those attributes, and an example script.

## Example

There's an example file in Plugins/HideIf/HideIfExampleScript.cs. You can safely delete it. Throw it on a GameObject somewhere to see it in action.

## Contributions

Please. This is not meant to be a generalized editor utility package, but if you have fixes for corner cases or useful extensions you want to add (like HideIfInRange/HideIfOutsideRange for ints), go for it!
