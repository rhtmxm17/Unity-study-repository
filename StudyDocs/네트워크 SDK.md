# 네트워크 SDK

## Photon Fusion

[기술 문서](https://doc.photonengine.com/fusion/)  
클라우드 상에 서버 앱을 생성하고 접속이 가능하다

### 프로젝트 설정

[기술 문서 > 시작하기](https://doc.photonengine.com/ko-kr/fusion/current/tutorials/host-mode-basics/1-getting-started)  
SDK는 [유니티 AssetStore](https://assetstore.unity.com/packages/tools/network/photon-fusion-267958)에서 추가해도 된다  
프로젝트에 추가 후 상단 Tools 메뉴에서 메인 Hub창을 찾을 수 있다

* 테스트 환경 구성 팁: [ParrelSync 패키지](https://github.com/VeriorPies/ParrelSync)  
  Assets와 ProjectSettings를 바로가기 형태로 공유하는 복제 프로젝트를 만들어준다. 에디터 플레이모드로 네트워크 테스트하기에 유용

### 주요 기능

* Network Runner 컴포넌트: 네트워크 연결을 실제로 유지 관리하는 관리자 컴포넌트

* Network Object 컴포넌트: 네트워크 상에서 공유될 정보를 포함한다는 것을 지정
* Network Transform 컴포넌트: 네트워크 상에서 Transform을 공유하는 기능

* SimulationBehaviour 클래스: 네트워크 기능을 추가로 구현하기 위한 베이스 클래스. Behaviour를 상속하고 있다. (NetworkBehaviour가 주로 사용)

* IPlayerJoined, IPlayerLeft 등의 인터페이스:
  레이어 접속, 종료 등 특정 상황이 발생했을 경우 Network Runner가 같은 GameObject가 갖는 해당 인터페이스를 호출한다

* Networked 어트리뷰트: [API Reference](https://doc-api.photonengine.com/en/fusion/v2/class_fusion_1_1_networked_attribute.html)  
  네트워크 상에서 동기화 되어야 하는 프로퍼티
* OnChangedRender 어트리뷰트:  
  Networked로 지정된 프로퍼티의 값이 변경되었을 때 호출될 메서드를 지정한다.  
  set에서 이벤트를 트리거해도 다른 클라이언트에서 값을 변경할 경우 호출되지 않기 때문에 필요하다.

### Network Object의 소유권

Network Object는 생성한 클라이언트가 소유권을 갖거나 따로 소유권을 지정해줄 수 있다. 소유권을 갖는 클라이언트에서만 Network Object가 관리하는 정보를 수정할 수 있다.  
또한 클라이언트의 접속이 종료될 경유 소유중인 Network Object가 파괴된다.

NetworkBehaviour라면 HasStateAuthority 필드로 소유권을 확인할 수 있다.

### RPC(원격 프로시저 호출)

내가 상대 캐릭터를 공격하는 상황을 가정하자. 공격 판정은 스스로 수행해도 문제가 없지만 이후 데미지 처리는 소유권을 갖는 상대 클라이언트에서 처리해야 한다.  
메서드에 `[RPC(호출 가능 클라이언트, 호출 대상 클라이언트)]` 어트리뷰트를 지정해 사용한다.

### Network에서의 Time

NetworkBehaviour의 FixedUpdateNetwork() 시간 간격을 사용할 때는 Time.deltaTime 대신 Runner.DeltaTime을 사용한다.

## Photon PUN

Photon에서 Fusion 이전에 개발된 유니티 네트워크 SDK  
Fusion에 비해 오래된 버전이기 때문에 기능과 성능이 떨어질 수 있으나 검증된 버전이며 여러 곳에서 사용되고있다.

### 요청 + 반응 컨셉

PhotonNetwork 클래스를 사용해 클라이언트가 서버에게 요청을 보내고, MonobehaviourPunCallbacks 클래스의 콜백을 재정의해 서버의 반응을 받는 구조로 이루어져있다.

### 커스텀 프로퍼티

방 또는 플레이어에 추가로 **동기화 가능한** 정보를 부여하는 방법. `ExitGames.Client.Photon.HashTable`을 사용해 프로퍼티 이름과 값을 페어로 할당해 Dictionary처럼 사용한다.  
프로퍼티를 부여 또는 변경할 때에는 SetCustomProperties()를 사용해야 동기화된다.  
