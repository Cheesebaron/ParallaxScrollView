ParallaxScrollView
==================

A Parallax ScrollView for Xamarin.Android, which is ported from [this Android project](https://github.com/chrisjenx/ParallaxScrollView) originally made by [Christopher Jenkins](http://chrisjenx.co.uk/).

##Usage

Take a look at the sample for how to use the component.

Two Views, a Background and a Foreground View, are needed to have the ParallaxScrollView working. The Foreground View gets wrapped in ObservableScrollView, which itself exposes an Event telling the ScrollView was scrolled, regardless of what you put in there.

###Attributes

* From `Resource.Stylables`, `parallexOffset`: This number has to be between 0.1 and 1.0, it gets defaulted to 0.3 if values exceeds those boundaries.
* An equivalent programatical Property is also present `ParallaxScrollView.ParallaxOffset`, which does the same as above.

###Background

The Background View will stretch, if smaller, to the same size as of the parent. The background is moved based on the ScrollView content size and i.e. setting a `parallaxFactor` of 0.5f, will approximatly move the background at a half rate of the foreground scroll.

###Foreground

Make sure it fills the parent.

##Example layout

```xml
<?xml version="1.0" encoding="utf-8"?>
<cheesebaron.parallaxscrollview.ParallaxScrollView
  xmlns:android="http://schemas.android.com/apk/res/android"
  xmlns:app="http://schemas.android.com/apk/res-auto"
  android:id="@+id/scroll_view"
  android:layout_width="match_parent"
  android:layout_height="match_parent"
  app:parallaxOffset=".3">
  
    <!-- Background -->

  <ImageView
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:gravity="center"
    android:scaleType="fitXY"
    android:src="@drawable/bg" />
    
  <!-- Foreground -->

  <cheesebaron.parallaxscrollview.ObservableScrollView
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:layout_gravity="center">
    
    <!-- Just like ScrollView, we can only have one direct child here, 
         so wrap in a Layout if you need more views inside the scroll view -->
    <TextView
      android:layout_width="match_parent"
      android:layout_height="wrap_content"
      android:background="@android:color/white"
      android:padding="@dimen/spacing"
      android:text="Hello ParallaxScrollView!" />
    
  </cheesebaron.parallaxscrollview.ObservableScrollView>
</cheesebaron.parallaxscrollview.ParallaxScrollView>
```

##License

Copyright 2013 Tomasz Cielecki

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
