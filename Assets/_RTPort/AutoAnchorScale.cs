using Sirenix.OdinInspector;
using UnityEngine;

namespace _RTPort
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class AutoAnchorScale : MonoBehaviour
    {
        private enum Type { SizeDelta = 0, LocalScale = 1 }

        [Title("Basic")]
        [SerializeField] private Type usingType = Type.SizeDelta;
        public Vector2 originScale;
        public Vector2 deltaLessRatio, deltaOverRatio;
        [SerializeField] private bool enableX, enableY;

        [Title("Custom")] 
        [SerializeField] private bool usingCustomRatio = false;
        [SerializeField, ShowIf(nameof(usingCustomRatio))]
        private Vector2 customScreenRatio;

        private RectTransform _transform;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            Match();
        }

        private void Update() => Match();
        
        private void Match()
        {
            const float defaultRatio = 16f / 9;
            var screenRatio = usingCustomRatio ? customScreenRatio.x / customScreenRatio.y : defaultRatio;
            var delta = (float)Screen.width / Screen.height / screenRatio;
            
            var screenX = usingCustomRatio ? 1920 / defaultRatio * screenRatio : 1920;
            const float screenY = 1080f;
            var scl = originScale;
            
            if (usingType is Type.SizeDelta)
            {
                if (delta > 1)
                {
                    if (enableX) scl = scl.SetX(originScale.x + deltaOverRatio.x * (delta - 1f) * screenX);
                    if (enableY) scl = scl.SetY(originScale.y + deltaOverRatio.y * (delta - 1f) * screenY);
                }
                else if (delta < 1)
                {
                    if (enableX) scl = scl.SetX(originScale.x + deltaLessRatio.x * (delta - 1f) * screenX);
                    if (enableY) scl = scl.SetY(originScale.y + deltaLessRatio.y * (delta - 1f) * screenY);
                }
                
                _transform.sizeDelta = scl;
            }
            else if (usingType is Type.LocalScale)
            {
                if (delta > 1)
                {
                    if (enableX) scl = scl.SetX(originScale.x + deltaOverRatio.x * (delta - 1f));
                    if (enableY) scl = scl.SetY(originScale.y + deltaOverRatio.y * (delta - 1f));
                }
                else if (delta < 1)
                {
                    if (enableX) scl = scl.SetX(originScale.x + deltaLessRatio.x * (delta - 1f));
                    if (enableY) scl = scl.SetY(originScale.y + deltaLessRatio.y * (delta - 1f));
                }
                
                _transform.localScale = new Vector3(scl.x, scl.y, 1f);
            }
        }
    }
}