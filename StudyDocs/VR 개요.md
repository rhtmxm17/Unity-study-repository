# VR 개요

가상의 공간을 모니터로 출력하는 대신, Head Mount Display를 통해 사용자의 시야를 완전히 사용하는 것으로 가상의 공간에 위치해 있는 것 과 같은 체감을 주는 것

* VR 멀미  
  균형 기관의 감각과 눈에 보이는 감각의 위치 정보가 달라서 감각 충돌이 일어나서 발생한다.  
  이를 완화하기 위해서는 시각적 감각과 현실의 신체적 감각의 차이를 최대한 줄여야 한다.

## 개발 환경 구성

* VR 기기 연결(메타 퀘스트)  
  PC에서 Meta Quest Link를 설치 및 준비, VR 기기에서 설정을 통해 Quest Link를 실행해 연동 상태로 만든다.
* Unity  
  Oculus XR Plugin, OpenXR Plugin 패키지를 설치  
  XR Interaction Toolkit 패키지는 다양한 VR 기기의 범용 입력을 지원한다  
  XR Plug-in Management 패키지 설정에서 OpenXR을 활성화 한다.

### XR 디바이스 시뮬레이터

[매뉴얼](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.6/manual/xr-device-simulator-overview.html)  
XR Interaction Toolkit에 포함된, VR 기기를 사용하기 어려운 상황에서의 테스트 기능이다. 키보드, 마우스 입력을 VR 디바이스에서의 동작으로 매핑해서 테스트할 수 있다.

## VR 핵심 구성요소

### XR Origin

가상 환경 상에서 플레이어이다. 카메라, 좌우 컨트롤러 등을 포함한다.

### Locomotion System

플레이어의 이동(기본 이동, 순간이동, 사다리)을 관리한다. VR에서 아무리 비어있는 공간이어도 현실에서의 위치에는 제약이 있기 때문에 컨트롤러를 통한 회전, 이동, 순간이동을 지원한다.

### XR Interaction Manager

플레이어와 상호작용 대상의 상호작용(쥐기, 버튼, 누르기Poke 등) 발생을 관리한다. 상호작용 대상(Interactable)은 상호작용 방법에 따른 동작을 구현하고, 플레이어는 그 상호작용을 트리거하는 방식이다.  
상호 작용 방식에 따라 다른 레이어를 사용하는 것으로 가능한 상호작용 방식을 나눌 수 있다. 이때, 레이어는 Physics Layermask와 방식은 유사하지만 실제로는 별도의 레이어 그룹을 사용한다. ([해당 내용 Manual](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.6/manual/interaction-layers.html))

* XR Socket Interactor: 플레이어 대신 Direct Interactor와 같이 Interactable을 들고있을 수 있게 하는 컴포넌트. 열쇠 구멍, 아이템 거치대 등으로 활용할 수 있다.

## VR에서의 UI

VR 환경에서 기존과 같이 카메라 오버레이로 UI를 출력하면 눈 바로 앞에 UI가 위치하고, 월드상에 존재하는 컨트롤러로 선택하기 곤란해진다. 따라서 UI를 월드상에 배치하고 컨트롤러로 가리켜서 사용하는 방식을 사용한다.

1. EventSystem에 XR UI Input Modul 추가
1. Canvas에 Tracked device Graphic Raycaster 추가

## 핸드 트래킹(XR Hands)

[매뉴얼](https://docs.unity3d.com/Packages/com.unity.xr.hands@1.4/manual/index.html)  
VR 디바이스에서 패스스루 카메라 등을 이용해 사용자의 손 동작을 인식하는 기능이다. 컨트롤러를 필요로 하지 않는 입력으로 사용할 수 있다.

* Hand Shape 애셋: 손으로 취할 수 있는 각종 제스처를 의미한다.
* XR Hand Tracking Event 컴포넌트: 지속적으로 손 동작을 방향, 구부러진 정도 값의 형태로 감지한다.
* Static Hand Geasture 컴포넌트: 담당한 Hand Shape가 취해졌는지, 해제되었는지에 따라 이벤트를 발생시킨다.
