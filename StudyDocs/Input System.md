# Input System

[패키지 메뉴얼](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/index.html)
기본 Input 클래스의 플랫폼 한계(터치스크린, VR기기)와 같은 문제를 개선한 패키지

## 준비

Project Seettings - Player - Other Settings - Configuration - Active Input Handling 이 Both 또는 Input System Package로 되어있는지 확인한다.

## Player Input 컴포넌트

사용할 Input Action 에셋을 결정한다.

## Input Action 에셋

**저장에 유의하자!** Auto-Save 체크박스로 설정 가능

스크립트상에서 받는 Action 명칭 및 기능과 유저의 입력 방법을 매칭해준다.

## 이벤트 형식으로 작동시키기

<https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/RespondingToActions.html#responding-to-actions-using-callbacks>

## 터치 입력 보조

터치 스크린 상에서 키보드나 컨트롤러와 같은 입력 방식을 사용하기 위한 컴포넌트들이다.

### On-Screen Stick

가상 조이스틱을 만들어서 방향 입력을 하는 컴포넌트

### On-Screen Button

버튼 UI를 물리 버튼처럼 사용하게 하는 컴포넌트
