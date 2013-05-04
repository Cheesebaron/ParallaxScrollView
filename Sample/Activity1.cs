using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cheesebaron.ParallaxScrollView;

namespace Sample
{
    [Activity(Label = "ParallaxScrollView Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity, View.IOnClickListener
    {
        private ParallaxScrollView _scrollView;
        private TextView _factorText;
        private Button _minus, _plus;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.acitivity_demo);

            _scrollView = FindViewById<ParallaxScrollView>(Resource.Id.scroll_view);
            _factorText = FindViewById<TextView>(Resource.Id.factor_text);
            _minus = FindViewById<Button>(Resource.Id.minus);
            _minus.SetOnClickListener(this);
            _plus = FindViewById<Button>(Resource.Id.plus);
            _plus.SetOnClickListener(this);
        }

        public void OnClick(View v)
        {
            var offset = _scrollView.ParallaxOffset;
            switch (v.Id)
            {
                case Resource.Id.minus:
                    offset = _scrollView.ParallaxOffset;
                    offset = offset - 0.05f;
                    _scrollView.ParallaxOffset = offset;
                    offset = _scrollView.ParallaxOffset;
                    _factorText.Text = string.Format("{0}", offset);
                    break;
                case Resource.Id.plus:
                    offset = _scrollView.ParallaxOffset;
                    offset = offset + 0.05f;
                    _scrollView.ParallaxOffset = offset;
                    offset = _scrollView.ParallaxOffset;
                    _factorText.Text = string.Format("{0}", offset);
                    break;
            }
            if (offset * 100 <= 10)
            {
                _minus.Enabled = false;
                _plus.Enabled = true;
            }
            else
            {
                _minus.Enabled = true;
                _plus.Enabled = true;
            }
        }
    }
}

