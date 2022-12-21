using UnityEngine;

namespace Utils
{
    public static class UtilsClass
    {
        public const int SortingOrderDefault = 5000;
        
        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = SortingOrderDefault)
        {
            color ??= Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
        
        // Create Text in the World
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        
        // 
        /// <summary>
        /// Get Mouse Position in World with Z = 0f.
        /// Only works with camera in orthographic.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouse2DWorldPosition()
        {
            Vector3 vec = GetMouse2DWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouse2DWorldPosition(Camera worldCamera)
        {
            Vector3 vec = GetMouse2DWorldPositionWithZ(Input.mousePosition, worldCamera);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouse2DWorldPositionWithZ() {
            return GetMouse2DWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouse2DWorldPositionWithZ(Camera worldCamera) {
            return GetMouse2DWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouse2DWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
        public static Vector3 GetDirToMouse(Vector3 fromPosition)
        {
            Vector3 mouseWorldPosition = GetMouse2DWorldPosition();
            return (mouseWorldPosition - fromPosition).normalized;
        }

        /// <summary>
        /// Get World Position from UI Position.
        /// 3D world version.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMouse3DWorldPosition() {
            return GetMouse3DWorldPosition(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouse3DWorldPosition(Camera worldCamera) {
            return GetMouse3DWorldPosition(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouse3DWorldPosition(Vector3 screenPosition, Camera worldCamera) {
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0f));
            xy.Raycast(ray, out float distance);
            return ray.GetPoint(distance);
        }
    }
}