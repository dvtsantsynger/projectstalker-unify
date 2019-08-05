using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniLayouts.MVP { 
    public abstract class MvpPresenter<V> where V : MvpView {

        public V View { get; private set; }

        protected GameObject ViewObject;

        public virtual void AttachView(V view) {
            this.View = view;
            this.ViewObject = ((MonoBehaviour)((object)view)).gameObject;
        }

        public void DetachView() { }
    }
}