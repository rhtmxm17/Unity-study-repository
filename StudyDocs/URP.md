# URP

기본 설정의 Built-In Render Pipeline은 사용이 간단하지만 기능이 한정적이고 동시에 안정성을 위해 성능이 희생된 면이 있다.  
URP를 사용해서 새로운 그래픽 기능등을 머티리얼, 카메라 등에서 사용할 수 있다.

드로우 콜 단위 디버깅: <https://docs.unity3d.com/2021.3/Documentation/Manual/FrameDebugger.html>

* Built-In Render Pipeline: URP를 사용하지 않으면 기본적으로 사용하는 파이프라인. 간단한 사용을 기본으로 하고있기 때문에 커스텀의 여지가 적다.

## 설정하기

Project Setting - Graphics - Scriptable Render Pipeline Settings  
이곳에 작성하거나 가져온 URP Asset을 등록하면 된다.

***
***

## URP Asset

Universal Renderer Data: 단일 렌더링 설정. 카메라마다 설정을 선택 및 동적으로 변경할 수도 있다.

## Volume 컴포넌트

영역과 그 영역 안에서 사용할 Post-processing의 Volume Profile을 결정한다.

***
***

## Shader

출력 과정에서의 정점 / 픽셀의 각종 계산 과정을 직접 제작해서 렌더링 방식을 생성할 수 있다.  
텍스쳐, 색상, float 등의 프로퍼티를 받아서 Shader에서 처리한 결과로 출력 색상이 나온다.

### Shader Graph

Shader를 그래프 UI로 정보 입출력을 연결해서 제작하는 방식.  
입력된 프로퍼티는 노드 형태로 끌어와서 계산에 사용한다.

***
***

## Convert

Window - Render - Rendering Pipeline Converter 탭에서 Built-in으로 준비된 에셋(또는 프로젝트 전체)를 URP로 맞춰줄 수 있다.  
변경할 카테고리를 선택하고 Initialize Converters를 선택해 변경할 에셋을 확인한 뒤 Convert 한다.
