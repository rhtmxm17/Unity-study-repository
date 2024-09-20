# Unity study repository

유니티 공부에 사용한 저장소입니다.

## 헬리콥터([씬 파일](./Unity-study/Assets/Scenes/Helicopter.unity))

주요 주제: Transform 컴포넌트 스터디

* 전후 입력: 전진, 후진
* 좌우 입력: 회전
* 점프 입력: 프로펠러 가속, 임계값보다 빠르면 상승 느리면 하강
* Fire3 입력: 입력중 미사일 발사

## 행성계([씬 파일](./Unity-study/Assets/Scenes/Satellite.unity))

주요 주제: Transform 컴포넌트 스터디(특히 회전)

씬에 배치한 큐브들이 다른 큐브를 대상으로 공전 및 스스로 자전한다.

## 탱크([씬 파일](./Unity-study/Assets/Scenes/Tank.unity))

주요 주제: 프리펩, 오브젝트 풀, RigidBody

* WASD 입력: 전진, 후진 및 회전 이동
* 상하좌우 방향키: 포신 각도 조정
* 스페이스바: 발사
* 1, 2, 3: 포탄 선택

포신을 몸체와 별도로 방향 조정하게 할 수 있다. 포탄은 종류별로 소소한 차이를 두었으며 오브젝트 풀 패턴을 사용.

## Dodge 미니게임([프로젝트 폴더](./Dodge/))

주요 주제: Transform, 물리 및 충돌 기초, Raycast 기초 정리

* 방향 입력: 플레이어 이동

주위에 배치된 기둥이 발사하는 총알을 피하는 미니게임.

## FPS 미니게임([씬 파일](./Unity-study/Assets/Scenes/FPS.unity))

주요 주제: 카메라, Raycast, Canvas(UGUI)

* WASD 입력: 전후좌우 이동
* 마우스 이동: 카메라 회전
* 좌클릭: 발사
* 우클릭: 줌인
* R키: 재장전

## Imports

커밋에 포함시키지 않은, AssetStore 등에서 적용시킨 에셋

* TextMesh Pro(Unity Package)
* Cinemachine(Unity Package)
* Unity-Chan! Model([AssetStore](https://assetstore.unity.com/packages/3d/characters/unity-chan-model-18705))
* Legacy Particle Pack([AssetStore](https://assetstore.unity.com/packages/vfx/particles/legacy-particle-pack-73777))
* AllSky Free([AssetStore](https://assetstore.unity.com/packages/2d/textures-materials/sky/allsky-free-10-sky-skybox-set-146014))
* Free UI Click Sound Pack([AssetStore](https://assetstore.unity.com/packages/audio/sound-fx/free-ui-click-sound-pack-244644))
* Sunny Land([AssetStore](https://assetstore.unity.com/packages/2d/characters/sunny-land-103349))
* Mixamo의 애니메이션([Site](https://www.mixamo.com/))
