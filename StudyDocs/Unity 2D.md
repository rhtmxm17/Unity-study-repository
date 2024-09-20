# Unity 2D

유니티에서 2D는 근본적인 구조가 3D와 다르지 않다. 실제로 xyz좌표를 모두 사용하고 카메라를 직교투영으로 둔다.  
단, 물리엔진은 2D 전용을 사용한다.

## Physics 2D

게임 로직이 2D로 돌아간다면 Rigid Body, Collider, **메시지 함수(실수하기 쉬움!)** 모두 2D 전용으로 사용하는 것으로 2D 물리엔진을 사용할 수 있다.  
3D 에서의 Physics와 Physics 2D는 상호작용 하지 않은다. 클래스부터가 Physics와 Physics2D로 다름.

### Effector 컴포넌트

2D의 특수한 표현을 위한 물리 확장 기능들. Collider2D에서 Used By Effector 옵션을 활성화 해야 한다.

* Platform Effector: 플랫포머 게임의 점프로 오를 수 있는 발판과 같이, 충돌이 가능한 방향을 제한하는 기능
* Buoyancy Effector: 부력을 적용
* Area Effector: 영역 내의 물체에 힘을 적용. 바람과 같은 효과에 사용된다.
* Point Effector: 한 지점을 향한 힘을 적용. 밀어내거나 블랙홀처럼 당기는 힘
* Surface Effector: 컨베이어 벨트처럼 표면에 힘을 적용

## Sprite Texture(Texture2D 에셋에서)

* Sprite Mode: Multiple일 경우 스프라이트의 일부만 선택해서 출력할 수 있다. 이미지 하나를 올려놓고 uv만 바꿔서 출력하는 것이 성능적 이점을 얻을 수 있고, Multiple로 구성된 스프라이트([스프라이트 아틀라스](https://docs.unity3d.com/2021.3/Documentation/Manual/class-SpriteAtlas.html)) 자체가 같이 쓰는 스프라이트 묶음으로 보고 관리하기도 편리하다. 연속된 동작이나 타일맵에서 많이 사용한다.  
유니티 밖에서 가져온 에셋이라 잘려있지 않다면 Sprite Editor에서 자동으로 잘라주는 설정들이 있다.
* Pixels Per Unit: 크기 '1'에 몇픽셀이 들어가는지를 결정하는 옵션. 2<sup>n</sup> 에 해당하는 값(16, 32 등)으로 설정하는 것이 일반적이고 또 유리하다.
* Pivot: 스프라이트의 기준 위치. UI에서의 Pivot과 같다. 캐릭터라면 일반적으로 발밑이 되는 Bottom을 Pivot으로 둔다.

### Sprite Atlas 에셋

텍스쳐 또는 텍스쳐가 포함된 폴더들을 묶어서 빌드시 하나의 텍스쳐로서 사용하는 것으로 그래픽 최적화를 꾀하는 에셋.

### 9-slicing Sprites

테두리가 있는 UI 스프라이트와 같이, 크기를 늘려도 외곽의 형태를 유지시킬 때 사용한다.

## Sprite Renderer

* Flip: 스프라이트의 좌우 반전 설정. 탑다운 형식의 2D게임(엔터 더 건전 등)에서 오브젝트의 실제 방향과 스프라이트 출력 방향을 구분해서 사용하고자 하면 필요하다.

## 2D 에서의 Cinemachine

Cinemachine Confiner 2D와 같이 Cinemachine 패키지 또한 2D용 기능을 갖고있다.

## 쿼터뷰

Project Settings - Graphics 에서 정렬 기준을 Y축으로 바꾸는 것으로 정렬 방식을 쿼터뷰 스타일로 할 수도 있다.

## 2D Animation

2D Animation 또한 Animatior 컴포넌트와 Animation 에셋으로 작동한다.

### Sprite Animation

만화 영화를 출력하듯, 스프라이트 자체를 바꿔가면서 움직임을 보여주는 애니메이션. Animation 에셋에 여러 스프라이트를 등록해서 시간에 따라 사용할 스프라이트를 결정한다.

스프라이트 애니메이션에서는 각종 동작 하나하나를 모두 별개의 애니메이션으로 구현해야하기 때문에, 조건값의 변경으로 애니메이션을 전환하기 보다는 Animator.Play로 특정 애니메이션을 실행시켜주는 것이 편리한 경우가 많다.

### Spine Animation

3D에서 사용했던 Animation과 같이, 텍스쳐를 관절 구조로 나누어서 관절마다 위치를 조정해 움직이는 애니메이션
