using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UniLayouts.MVP { 
    public enum Orientation { Horizontal, Vertical }

    public abstract class MvpActivity<V, P> : MonoBehaviour, Activity where V : MvpView where P : MvpPresenter<V>, new() {

        [SerializeField]
        private TMP_FontAsset[] fonts;

        private Canvas canvas;

        private CanvasScaler scaler;

        private GraphicRaycaster raycaster;

        private EventSystem eventSystem;

        private StandaloneInputModule inputModule;

        private RectTransform rectTransform;

        private UIViewGroup rootView;

        private GameObject activityUI;

        public abstract void OnCreate(UIViewGroup rootView);

        private P presenter;

        private float canvasWidth;
        private float canvasHeight;

        protected P Presenter { get { return presenter; } }

        protected virtual void Awake() {
             presenter = new P();
             presenter.AttachView((V)((object)this));
        }

        private void Start() {
            activityUI = new GameObject("Editor UI");
            activityUI.transform.SetParent(transform);

            // Canvas
            canvas = activityUI.AddComponent<Canvas>();
            canvas.pixelPerfect = true;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            rectTransform = canvas.GetComponent<RectTransform>();
            canvasWidth = rectTransform.rect.width;
            canvasHeight = rectTransform.rect.height;

            // Canvas Scaler
            scaler = activityUI.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            // GraphicRaycaster
            raycaster = activityUI.AddComponent<GraphicRaycaster>();

            // EventSystem
            eventSystem = activityUI.AddComponent<EventSystem>();
            eventSystem.pixelDragThreshold = 1;
            inputModule = activityUI.AddComponent<StandaloneInputModule>();

            // Layout
            rootView = new UIFrameLayout(this);
            rootView.Object.transform.SetParent(activityUI.transform);
            rootView.Object.name = "Root ViewGroup";
            RectTransform rt = rootView.Object.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        

            OnCreate(rootView);
        }

        void Update() {
            if (canvasWidth != rectTransform.rect.width || canvasHeight != rectTransform.rect.height) {
                canvasWidth = rectTransform.rect.width;
                canvasHeight = rectTransform.rect.height;
                rootView.RequestLayout();
            }
        }

        public TMP_FontAsset[] Fonts {
            get { return fonts; }
        }

        public UIViewGroup RootView {
            get { return rootView; }
        }

        public GameObject UIObject {
            get { return activityUI; }
        }
    }
}