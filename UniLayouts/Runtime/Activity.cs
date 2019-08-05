using TMPro;
using UnityEngine;

namespace UniLayouts.MVP { 
    public interface Activity {
        TMP_FontAsset[] Fonts    { get; }
        UIViewGroup     RootView { get; }
        GameObject      UIObject { get; }
    }
}