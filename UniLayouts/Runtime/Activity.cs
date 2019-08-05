using TMPro;
using UniLayouts.Views;
using UnityEngine;

namespace UniLayouts.MVP { 
    public abstract class Activity : MonoBehaviour {
        public TMP_FontAsset[] Fonts    { get; }
        public UIViewGroup     RootView { get; }
        public GameObject      UIObject { get; }
    }
}