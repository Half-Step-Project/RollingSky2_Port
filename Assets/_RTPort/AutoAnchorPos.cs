using Sirenix.OdinInspector;
using UnityEngine;

namespace _RTPort
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class AutoAnchorPos : MonoBehaviour
    {
        [Title("Basic")]
        public Vector2 originPos;
        public Vector2 deltaLessRatio, deltaOverRatio;
        [SerializeField] private bool enableX, enableY;

        [Title("Custom")]
        [SerializeField] private bool usingCustomRatio = false;
        [SerializeField, ShowIf(nameof(usingCustomRatio))]
        private Vector2 customScreenRatio;

        [SerializeField] private bool usingCustomOrigin;
        [SerializeField, ShowIf(nameof(usingCustomOrigin))]
        public Vector2 customLessOrigin, customOverOrigin;

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
            
            var pos = usingCustomOrigin && !Mathf.Approximately(delta, 1)
                ? delta < 1 ? customLessOrigin : customOverOrigin
                : originPos;
            
            if (delta > 1)
            {
                if (enableX) pos = pos.SetX(originPos.x + deltaOverRatio.x * (delta - 1f) * screenX);
                if (enableY) pos = pos.SetY(originPos.y + deltaOverRatio.y * (delta - 1f) * screenY);
            }
            else if (delta < 1)
            {
                if (enableX) pos = pos.SetX(originPos.x + deltaLessRatio.x * (delta - 1f) * screenX);
                if (enableY) pos = pos.SetY(originPos.y + deltaLessRatio.y * (delta - 1f) * screenY);
            }
            
            _transform.anchoredPosition = pos;
        }
    }
}
