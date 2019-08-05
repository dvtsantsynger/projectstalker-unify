using UniLayouts.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UniLayouts.Views { 
    public class UIView {

        class UIEventHandler :MonoBehaviour {
            public UIView view;
        }

        public const int MATCH_PARENT = -1;
        public const int WRAP_CONTENT =  0;
        public enum ViewVisible { Visible, Invisible, Gone }
        public enum ViewEdge { Left, Right, Top, Bottom }

        internal RectTransform rectTransform;

        public UIViewGroup Parent {get; internal set; }

        public Image Background { get; private set; }
        public Image Foreground { get; private set; }
        public float MeasureWidth { get { return rectTransform.rect.width; } }
        public float MeasureHeight { get { return rectTransform.rect.height; } }

        public float Left { get { return rectTransform.anchoredPosition.x; } }
        public float Top { get { return -rectTransform.anchoredPosition.y; } }

        RectOffset paddings = new RectOffset();
        public RectOffset Paddings { get { return paddings; } }

        RectOffset margins = new RectOffset();
        public RectOffset Margins { get { return margins;} }

        internal UIViewGroup.LayoutParams layoutParams = new UIViewGroup.LayoutParams();

        Vector2 minSize = Vector2.zero;
        public float MinWidth {
            get { return minSize.x; }
            set {
                if (value != minSize.x) {
                    minSize.x = Mathf.Abs(value);
                    RequestLayout();
                }
            }
        }
        public float MinHeight {
            get { return minSize.y; }
            set {
                if (value != minSize.y) {
                    minSize.y = Mathf.Abs(value);
                    RequestLayout();
                }
            }
        }

        public float Width {
            get { return layoutParams.Width; } 
            internal set { 
                if (value != layoutParams.Width) {
                    layoutParams.Width = value;
                }
            } }

        public float Height {
            get { return layoutParams.Height; } 
            internal set { 
                if (value != layoutParams.Height) {
                    layoutParams.Height = value;
                }
            } }

        ViewVisible visible;
        public ViewVisible Visible { 
            get { return visible; }
            set { 
                    if (value != visible) {
                        Graphic[] all;
                        switch(value) {
                            case ViewVisible.Gone: { 
                                rectTransform.sizeDelta = Vector3.zero;
                                rectTransform.gameObject.SetActive(false);
                                break;
                            }
                            case ViewVisible.Invisible: {
                                rectTransform.gameObject.SetActive(true);
                                all = rectTransform.GetComponentsInChildren<Graphic>(true);
                                foreach(Graphic g in all) g.enabled = false;
                                break;
                            }
                            case ViewVisible.Visible: {
                                rectTransform.gameObject.SetActive(true);
                                all = rectTransform.GetComponentsInChildren<Graphic>(true);
                                foreach(Graphic g in all) g.enabled = true;
                                break;
                            }
                        }
                        visible = value;
                        RequestLayout();
                    }
                }
        }

        internal GameObject Object { get { return rectTransform.gameObject; } }

        public Activity Context { get; private set; }

        public UIView(Activity context) {
            this.Context = context;
            GameObject gameObject = new GameObject(GetType().Name);
            gameObject.AddComponent<UIEventHandler>().view = this;

            rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);

            Background = gameObject.AddComponent<Image>();
            Background.raycastTarget = false;
            CanvasRenderer bcr = Background.GetComponent<CanvasRenderer>();
            bcr.cullTransparentMesh = true;

            gameObject.AddComponent<Mask>().showMaskGraphic = true;

            GameObject fgo = new GameObject("_FG");
            fgo.transform.SetParent(rectTransform);
            RectTransform fgt = fgo.AddComponent<RectTransform>();
            fgt.anchorMin = Vector2.zero;
            fgt.anchorMax = Vector2.one;
            fgt.offsetMin = Vector2.zero;
            fgt.offsetMax = Vector2.zero;
            fgt.pivot = new Vector2(0, 1);
            Foreground = fgo.AddComponent<Image>();
            Foreground.raycastTarget = false;
            Foreground.color = new Color(0, 0, 0, 0);
            CanvasRenderer fcr = Foreground.GetComponent<CanvasRenderer>();
            fcr.cullTransparentMesh = true;
        }

        public UIViewGroup.LayoutParams GetLayoutParams() {
            return layoutParams;
        }

        public void SetLayoutParams(UIViewGroup.LayoutParams layoutParams) {
            this.layoutParams = layoutParams;
            RequestLayout();
        }

        public void SetPaddings(int left, int right, int top, int bottom) {
            if (Paddings.left != left || Paddings.right != right || Paddings.top != top || Paddings.bottom != bottom) {
                Paddings.left = left;
                Paddings.right = right;
                Paddings.top = top;
                Paddings.bottom = bottom;
                RequestLayout();
            }
        }

        public void SetPadding(int offset, ViewEdge edge) {
            switch(edge) {
                case ViewEdge.Bottom:
                    if (Paddings.bottom != offset) {
                        Paddings.bottom = offset;
                        RequestLayout();
                    }
                    break;
                case ViewEdge.Left:
                    if (Paddings.left != offset) {
                        Paddings.left = offset;
                        RequestLayout();
                    }
                    break;
                case ViewEdge.Right:
                    if (Paddings.right != offset) {
                        Paddings.right = offset;
                        RequestLayout();
                    }
                    break;
                case ViewEdge.Top:
                    if (Paddings.top != offset) {
                        Paddings.top = offset;
                        RequestLayout();
                    }
                    break;
            }
        }

        public void SetMargins(int left, int right, int top, int bottom) {
            if (Margins.left != left || Margins.right != right || Margins.top != top || Margins.bottom != bottom) {
                Margins.left = left;
                Margins.right = right;
                Margins.top = top;
                Margins.bottom = bottom;
                RequestLayout();
            }
        }

        public void SetMargin(int offset, ViewEdge edge) {
            switch(edge) {
                case ViewEdge.Bottom:
                    if (Margins.bottom != offset) {
                        Margins.bottom = offset;
                        RequestLayout();
                    }
                    break;
                case ViewEdge.Left:
                    if (Margins.left != offset) {
                        Margins.left = offset;
                        RequestLayout();
                    }
                    break;
                case ViewEdge.Right:
                    if (Margins.right != offset) {
                        Margins.right = offset;
                        RequestLayout();
                    }
                    break;
                case ViewEdge.Top:
                    if (Margins.top != offset) {
                        Margins.top = offset;
                        RequestLayout();
                    }
                    break;
            }
        }

        internal virtual Vector2 CalcWrappedSize() {
            return minSize;
        }

        internal void setMeasuredDimension(float measuredWidth, float measuredHeight) {
            rectTransform.sizeDelta = new Vector2(measuredWidth, measuredHeight);
        }

        internal virtual void OnMeasure(float widthMeasureSpec, float heightMeasureSpec) {
            float measuredWidth = Width;
            float measuredHeight = Height;

            Vector2 wrap = CalcWrappedSize();

            if (Width == UIView.WRAP_CONTENT) measuredWidth = Mathf.Max(wrap.x, minSize.x);
            if (Height == UIView.WRAP_CONTENT) measuredHeight = Mathf.Max(wrap.y, minSize.y);

            if (Width < 0) measuredWidth = widthMeasureSpec * Mathf.Abs(Width);
            if (Height < 0) measuredHeight = heightMeasureSpec * Mathf.Abs(Height);

            setMeasuredDimension(measuredWidth - Paddings.left - Paddings.right, measuredHeight - Paddings.top - Paddings.bottom);
        }

        internal virtual void OnLayout(float left, float top, float right, float bottom) { }

        internal void SetPosition(float left, float top) {
            rectTransform.anchoredPosition = new Vector2(left, -top);
        }

        public void RequestLayout() {
            if (Parent == null) {
                OnLayout(0, 0, MeasureWidth, MeasureHeight);
            } else {
                Parent.OnLayout(0, 0, Parent.MeasureWidth, Parent.MeasureHeight);
            }
        }

        public void Destroy() {

        }
    }
}