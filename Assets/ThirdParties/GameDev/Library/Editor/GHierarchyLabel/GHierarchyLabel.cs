using UnityEditor;

using UnityEngine;

namespace GameDev.Library
{
    [InitializeOnLoad]
    public class GHierarchyLabel : MonoBehaviour
    {
        static GHierarchyLabel()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj != null && obj.name.StartsWith("---", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.gray);
                EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("-", "").ToString());
            }
        }
    }
}
