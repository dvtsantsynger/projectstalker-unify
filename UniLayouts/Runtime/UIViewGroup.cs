using System.Collections.Generic;
using UniLayouts.MVP;
using UnityEngine;

namespace UniLayouts.Views { 
    public abstract class UIViewGroup : UIView {

        [System.Serializable]
        public class LayoutParams {
            [SerializeField]
            public float Width;

            [SerializeField]
            public float Height;
        }

        internal List<UIView> children 
            = new List<UIView>();

        public UIViewGroup(Activity context) : base(context) { }

        public void AddView(UIView child, int width, int height, RectOffset margins, RectOffset paddings) {
            child.rectTransform.SetParent(this.rectTransform);
            child.Width = width;
            child.Height = height;
            child.Parent = this;
            children.Add(child);
            child.SetMargins(margins.left, margins.right, margins.top, margins.bottom);
            child.SetPaddings(paddings.left, paddings.right, paddings.top, paddings.bottom);
            child.RequestLayout();
            Foreground.transform.SetAsLastSibling();
        }

        public void AddView(UIView child, int width, int height, RectOffset margins) {
            AddView(child, width, height, margins, new RectOffset());
        }

        public void AddView(UIView child, int width, int height) {
            AddView(child, width, height, new RectOffset(), new RectOffset());
        }

        public void RemoveAllViews() {
            foreach (UIView v in children) v.Destroy();
            children.Clear();
            RequestLayout();
        }

        public int ViewCount {
            get { return children.Count; }
        }

        public UIView GetView(int idx) {
            return (idx < 0 || idx >= ViewCount) ? null : children[idx];
        }

        public void RemoveView(UIView view) {
            if (children.Remove(view)) { 
                RequestLayout();
            }
        }

        public void RemoveView(int idx) {
            if (idx >= 0 && idx < ViewCount) {
                children.RemoveAt(idx);
                RequestLayout();
            }
        }
    }
}