using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Cheesebaron.ParallaxScrollView
{
    public class ParallaxScrollView : ViewGroup
    {
        private const GravityFlags DefaultChildGravity = GravityFlags.CenterHorizontal;
        private new const string Tag = "ParallaxScrollView";
        private const float DefaultParallaxOffset = 0.2f;

        private float _parallaxOffset = DefaultParallaxOffset;
        private View _background;
        private ObservableScrollView _scrollView;

        private int _backgroundRight;
        private int _backgroundBottom;

        private int _scrollContentHeight;
        private int _scrollViewHeight;

        private float _scrollDiff;

        public float ParallaxOffset
        {
            get { return _parallaxOffset; }
            set
            {
                var offset = (float)Math.Round(value * 100) / 100;
                _parallaxOffset = offset > 0.0 ? offset : DefaultParallaxOffset;

                RequestLayout();
            }
        }

        public ParallaxScrollView(Context context) 
            : this(context, null)
        { }

        public ParallaxScrollView(Context context, IAttributeSet attrs) 
            : this(context, attrs, 0)
        { }

        public ParallaxScrollView(Context context, IAttributeSet attrs, int defStyle) 
            : base(context, attrs, defStyle)
        {
            var a = Context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.parallaxscrollview, 0, 0);

            try
            {
                ParallaxOffset = a.GetFloat(Resource.Styleable.parallaxscrollview_parallaxOffset,
                                            DefaultParallaxOffset);
            }
            finally
            {
                a.Recycle();
            }
            
        }

        public override void AddView(View child)
        {
            if (ChildCount > 1)
                throw new ArgumentException("ParallaxScrollView can only host two direct children");
            base.AddView(child);
        }

        public override void AddView(View child, int index)
        {
            if (ChildCount > 1)
                throw new ArgumentException("ParallaxScrollView can only host two direct children");
            base.AddView(child, index);
        }

        public override void AddView(View child, int index, LayoutParams @params)
        {
            if (ChildCount > 1)
                throw new ArgumentException("ParallaxScrollView can only host two direct children");
            base.AddView(child, index, @params);
        }

        public override void AddView(View child, int width, int height)
        {
            if (ChildCount > 1)
                throw new ArgumentException("ParallaxScrollView can only host two direct children");
            base.AddView(child, width, height);
        }

        public override void AddView(View child, LayoutParams @params)
        {
            if (ChildCount > 1)
                throw new ArgumentException("ParallaxScrollView can only host two direct children");
            base.AddView(child, @params);
        }

        protected override void OnFinishInflate()
        {
            base.OnFinishInflate();

            if (ChildCount > 2)
                throw new InvalidOperationException("ParallaxScrollView can only host two direct children");

            OrganiseViews();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            if (_scrollView != null)
            {
                MeasureChild(_scrollView, MeasureSpec.MakeMeasureSpec(
                    MeasureSpec.GetSize(widthMeasureSpec), MeasureSpecMode.AtMost),
                    MeasureSpec.MakeMeasureSpec(MeasureSpec.GetSize(heightMeasureSpec), 
                    MeasureSpecMode.AtMost));

                _scrollContentHeight = _scrollView.GetChildAt(0).MeasuredHeight;
                _scrollViewHeight = _scrollView.MeasuredHeight;
            }
            if (_background != null)
            {
                var minHeight = (int) (_scrollViewHeight + _parallaxOffset
                                   * (_scrollContentHeight - _scrollViewHeight));
                minHeight = Math.Max(minHeight, MeasureSpec.GetSize(heightMeasureSpec));

                MeasureChild(_background, MeasureSpec.MakeMeasureSpec(
                    MeasureSpec.GetSize(widthMeasureSpec), MeasureSpecMode.Exactly),
                    MeasureSpec.MakeMeasureSpec(minHeight, MeasureSpecMode.Exactly));

                _backgroundRight = Left + _background.MeasuredWidth;
                _backgroundBottom = Top + _background.MeasuredHeight;

                _scrollDiff = (_background.MeasuredHeight - _scrollViewHeight)
                              / (float) (_scrollContentHeight - _scrollViewHeight);
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var parentLeft = PaddingLeft;
            var parentRight = r - l - PaddingRight;
            var parentTop = PaddingTop;
            var parentBottom = b - t - PaddingBottom;

            if (_scrollView != null && _scrollView.Visibility != ViewStates.Gone)
            {
                var lp = (FrameLayout.LayoutParams) _scrollView.LayoutParameters;
                var width = _scrollView.MeasuredWidth;
                var height = _scrollView.MeasuredHeight;

                int childLeft;
                int childTop;

                var gravity = lp.Gravity;
                if (gravity == GravityFlags.NoGravity)
                    gravity = DefaultChildGravity;

                var horizontalGravity = gravity & GravityFlags.HorizontalGravityMask;
                var verticalGravity = gravity & GravityFlags.VerticalGravityMask;

                switch (horizontalGravity)
                {
                    case GravityFlags.Left:
                        childLeft = parentLeft - lp.LeftMargin;
                        break;
                    case GravityFlags.CenterHorizontal:
                        childLeft = parentLeft + (parentRight - parentLeft - width) / 2 + lp.LeftMargin - lp.RightMargin;
                        break;
                    case GravityFlags.Right:
                        childLeft = parentRight - width - lp.RightMargin;
                        break;
                    default:
                        childLeft = parentLeft + lp.LeftMargin;
                        break;
                }

                switch (verticalGravity)
                {
                    case GravityFlags.Top:
                        childTop = parentTop + lp.TopMargin;
                        break;
                    case GravityFlags.CenterVertical:
                        childTop = parentTop + (parentBottom - parentTop - height) / 2 + lp.TopMargin - lp.BottomMargin;
                        break;
                    case GravityFlags.Bottom:
                        childTop = parentBottom - height - lp.BottomMargin;
                        break;
                    default:
                        childTop = parentTop + lp.TopMargin;
                        break;
                }

                _scrollView.Layout(childLeft, childTop, childLeft + width, childTop + height);
            }

            if (_background != null && _scrollView != null)
            {
                var scrollYCenterOffset = -_scrollView.ScrollY;
                var offset = (int) (scrollYCenterOffset * _scrollDiff);
                _background.Layout(Left, offset, _backgroundRight, offset + _backgroundBottom);
            }
        }

        public override LayoutParams GenerateLayoutParams(IAttributeSet attrs)
        {
            return new FrameLayout.LayoutParams(Context, attrs);
        }

        protected override LayoutParams GenerateLayoutParams(LayoutParams p)
        {
            return new FrameLayout.LayoutParams(p);
        }

        protected override LayoutParams GenerateDefaultLayoutParams()
        {
            return new FrameLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent,
                                                GravityFlags.CenterHorizontal);
        }

        protected override bool CheckLayoutParams(LayoutParams p)
        {
            return p is FrameLayout.LayoutParams;
        }

        private void OrganiseViews()
        {
            if (ChildCount <= 0) return;

            if (ChildCount == 1)
            {
                var foreground = GetChildAt(0);
                OrganiseBackgroundView(null);
                OrganiseForegroundView(foreground);
            }
            else if (ChildCount == 2)
            {
                var background = GetChildAt(0);
                var foreground = GetChildAt(1);

                OrganiseBackgroundView(background);
                OrganiseForegroundView(foreground);
            }
            else
                throw new InvalidOperationException("ParallaxScrollView can host only two direct children");
        }

        private void OrganiseBackgroundView(View background)
        {
            _background = background;
        }

        private void OrganiseForegroundView(View foreground)
        {
            var insertPos = ChildCount - 1;

            if (foreground is ObservableScrollView)
            {
                _scrollView = (ObservableScrollView)foreground;
                _scrollView.ScrollChanged -= ScrollViewOnScrollChanged;
            }
            else if (foreground is ViewGroup && !(foreground is ScrollView))
            {
                _scrollView = new ObservableScrollView(Context, null);
                RemoveView(foreground);
                _scrollView.AddView(foreground);
                AddView(_scrollView, insertPos);
            }
            else if (foreground is ScrollView)
            {
                var child = ((ScrollView) foreground).ChildCount > 0 ? 
                    ((ScrollView) foreground).GetChildAt(0) : null;

                _scrollView = new ObservableScrollView(Context, null);
                RemoveView(foreground);
                if (child != null)
                    _scrollView.AddView(child);
                AddView(_scrollView, insertPos);
            }
            else
            {
                _scrollView = new ObservableScrollView(Context, null);
                RemoveView(foreground);
                _scrollView.AddView(foreground);
                AddView(_scrollView, insertPos);
            }

            if (_scrollView != null)
            {
                _scrollView.LayoutParameters = foreground.LayoutParameters;
                _scrollView.ScrollChanged += ScrollViewOnScrollChanged;
                _scrollView.FillViewport = true;
            }
        }

        private void ScrollViewOnScrollChanged(object sender, ObservableScrollViewEventArgs args)
        {
            RequestLayout();
        }
    }
}