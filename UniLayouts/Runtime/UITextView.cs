using UniLayouts.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UniLayouts.Views { 
    public class UITextView : UIView {

        TMPro.TextMeshProUGUI text;

        public UITextView(Activity context) : base(context) {
            text = new GameObject("_Text").AddComponent<TMPro.TextMeshProUGUI>();
            text.font = context.Fonts[0];
            text.color = Color.black;
            text.transform.SetParent(rectTransform);
            text.transform.SetAsFirstSibling();
            text.text = "dfdfdsfsdfds";
            text.fontSize = 16;
            text.ForceMeshUpdate(true);
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = Vector2.zero;
            text.rectTransform.offsetMax = Vector2.zero;
            text.raycastTarget = false;
            text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        internal override Vector2 CalcWrappedSize() {
            return text.GetPreferredValues();
        }
    }
}