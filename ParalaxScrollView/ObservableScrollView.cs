using System;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace Cheesebaron.ParallaxScrollView
{
    public class ObservableScrollViewEventArgs : EventArgs
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int OldLeft { get; set; }
        public int OldTop { get; set; }
    }

    public delegate void ObservableScrollViewEventHandler(object sender, ObservableScrollViewEventArgs args);

    public class ObservableScrollView : ScrollView
    {
        public event ObservableScrollViewEventHandler ScrollChanged;

        public ObservableScrollView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        { }

        protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
        {
            base.OnScrollChanged(l, t, oldl, oldt);

            if (null != ScrollChanged)
                ScrollChanged(this, new ObservableScrollViewEventArgs
                    {
                        Left = l, Top = t, OldLeft = oldl, OldTop = oldt
                    });
        }
    }
}