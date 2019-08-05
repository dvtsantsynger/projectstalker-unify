using TMPro;
using UniLayouts.Views;
using UnityEngine;

namespace UniLayouts.MVP { 
    public  class Activity : MonoBehaviour {
        public virtual TMP_FontAsset[] Fonts    { get; }
        public virtual UIViewGroup     RootView { get; }
        public virtual GameObject      UIObject { get; }
    }
}