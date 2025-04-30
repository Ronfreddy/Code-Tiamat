using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Sprite))]
public class SpritePreviewDrawer : PropertyDrawer
{
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the sprite from the property
        Sprite sprite = property.objectReferenceValue as Sprite;

        //// Draw the default property field
        EditorGUI.PropertyField(position, property, label);

        //EditorGUI.BeginProperty(position, label, property);

        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // If the sprite is not null, draw its preview
        if (sprite != null)
        {
            Rect previewRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, 64, 64);
            //EditorGUI.DrawPreviewTexture(previewRect, sprite.texture);
            GUI.DrawTexture(previewRect, sprite.texture, ScaleMode.ScaleToFit);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + 66; // Adjust height for preview
    }
}
#endif