using UniLayouts.MVP;
using UnityEngine;


namespace UniLayouts.Views { 
    public enum HGravity { Left, Center, Right }
    public enum VGravity { Top, Center, Bottom }

    public class UIFrameLayout : UIViewGroup {

        [System.Serializable]
        public class LayoutParams : UIViewGroup.LayoutParams {
            [SerializeField]
            public HGravity GravityHorizontal = HGravity.Left;

            [SerializeField]
            public VGravity GravityVertical = VGravity.Top;
        }

        public UIFrameLayout(Activity context) : base(context) { }

        internal override void OnLayout(float left, float top, float right, float bottom) {
            foreach(UIView v in children) {
                if (!(v.GetLayoutParams() is LayoutParams)) {
                    LayoutParams newlp = new LayoutParams();
                    newlp.Width = v.GetLayoutParams().Width;
                    newlp.Height = v.GetLayoutParams().Height;
                    v.layoutParams = newlp;
                }

                LayoutParams lp = (LayoutParams)v.GetLayoutParams();
                v.OnMeasure(MeasureWidth - Paddings.left - Paddings.right, MeasureHeight - Paddings.top - Paddings.bottom);

                float l = Paddings.left;
                switch(lp.GravityHorizontal) {
                    case HGravity.Left:
                        l = left + Paddings.left;
                        break;
                    case HGravity.Center:
                        l = (right - left - v.MeasureWidth) / 2;
                        break;
                    case HGravity.Right:
                        l = right - Paddings.right - v.MeasureWidth;
                        break;
                }

                float t = 0;
                switch(lp.GravityVertical) {
                    case VGravity.Top:
                        t = top + Paddings.top;
                        break;
                    case VGravity.Center:
                        t = (bottom - top - v.MeasureHeight) / 2;
                        break;
                    case VGravity.Bottom:
                        t = bottom - Paddings.bottom - v.MeasureHeight;
                        break;
                }

                v.SetPosition(l, t);
            }
        }
    }
}