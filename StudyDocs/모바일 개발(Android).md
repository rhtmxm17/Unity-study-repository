# 모바일 개발(Android)

## IL2CPP

`Project Settings`-`Player`-`Other Settings`-`Scripting Backend`: 중간 언어로 CIL을 사용할지 C++까지 사용할지를 결정한다. IL2CPP를 선택해서 특히 모바일 환경에서의 성능 향상을 기대할 수 있다. 하지만 빌드 시간이 더 길어지는 점에 유의하자.

## 디바이스 센서 사용하기

모바일에서는 GPS, 자이로와 같은 추가로 활용할 수 있는 센서가 있다. 앱이 디바이스 센서를 사용하기 위해서는  

1. 매니페스트를 준비해서 이러한 권한을 요청할 수 있음을 알려야 하며
1. 실행중 실제로 권한을 요청하는 과정을 거쳐야 한다.

[Unity에서 Android 매니페스트 생성](https://docs.unity3d.com/kr/2021.2/Manual/overriding-android-manifest.html)  
[UnityEngine.Android.Permission 매뉴얼(권한 요청하기)](https://docs.unity3d.com/2021.3/Documentation/Manual/android-permissions-in-unity.html)
[Android 앱 매니페스트 개요](https://developer.android.com/guide/topics/manifest/manifest-intro?hl=ko)  
[Android 권한 일람](https://developer.android.com/reference/android/Manifest.permission)

## 디버깅

### Android Logcat

연결된 디바이스에서의 콘솔 출력, 메모리 확인, 입력 테스트 등의 디버깅 기능을 사용할 수 있다.
