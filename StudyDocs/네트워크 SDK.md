# 네트워크 SDK

## Photon Fusion

[기술 문서](https://doc.photonengine.com/fusion/)  
[API Reference](https://doc-api.photonengine.com/en/fusion/v2/index.html)  
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

[기술문서](https://doc.photonengine.com/pun/current/getting-started/pun-intro)  
[API Reference](https://doc-api.photonengine.com/en/pun/current/index.html)  
Photon에서 Fusion 이전에 개발된 유니티 네트워크 SDK  
Fusion에 비해 오래된 버전이기 때문에 기능과 성능이 떨어질 수 있으나 검증된 버전이며 여러 곳에서 사용되고있다.

### 요청 + 반응 컨셉

PhotonNetwork 클래스를 사용해 클라이언트가 서버에게 요청을 보내고, MonobehaviourPunCallbacks 클래스의 콜백을 재정의해 서버의 반응을 받는 구조로 이루어져있다.

### 커스텀 프로퍼티

방 또는 플레이어에 추가로 **동기화 가능한** 정보를 부여하는 방법. `ExitGames.Client.Photon.HashTable`을 사용해 프로퍼티 이름과 값을 페어로 할당해 Dictionary처럼 사용한다.  
프로퍼티를 부여 또는 변경할 때에는 SetCustomProperties()를 사용해야 동기화된다.  

### 매치메이킹으로부터 게임 시작하기

* PhotonNetwork.LoadLevel(): 서버에 통지하면서 씬을 전환한다. 항상 LoadSceneMode.Single이다.
* PhotonNetwork.AutomaticallySyncScene: 마스터 클라이언트가 PhotonNetwork를 통해 장면 전환시 방에 있는 다른 클라이언트도 전환하는 설정

### 네트워크 게임 플레이 주요 기능

* Photon View 컴포넌트:  
  해당 객체를 네트워크 객체로 사용할 것을 지정한다. PhotonNetwork.Instanciate()를 통해 생성해야 네트워크 객체로 작동한다. 이때, **이름으로부터 생성하기 때문에** Resources 폴더에서 찾아서 생성한다.
* MonoBehaviourPun 클래스: MonoBehaviour에 기준이 되는 Photon View에 대한 참조를 추가한 베이스 클래스
* IPunObservable 인터페이스:  
  변수 동기화를 위한 인터페이스. PhotonStream을 통해 일관된 순서로 값 데이터를 object 타입으로 보내고 읽어오는 구현을 해야한다.  
  Photon View에 대한 참조는 id가 모든 클라이언트에서 같은 값을 갖는 점을 이용해 간접적으로 전달 가능한다.

### PunRPC

PUN에서는 \[PunRPC\] 어트리뷰트를 필요로 하고, `photonView.RPC(메서드 이름)` 을 통해 호출한다.

#### RpcTarget

RPC 함수를 수신할 대상을 지정한다. 특수한 설정으로 `Buffered`와 `ViaServer`가 있다.

* Buffered: 서버에서 해당 RPC를 기록해두었다가 이후 참여한 플레이어에서도 해당 RPC가 실행되도록 한다.
* ViaServer: 본래 RPC는 자기 자신은 즉시 실행되고 다른 클라이언트는 서버를 거쳐서 실행되는데, 이러한 방식은 판정의 공정성이 훼손될 수 있다. ViaServer방식은 스스로의 RPC도 다른 클라이언트와 동일한 타이밍에 실행시켜준다.

### 데이터 동기화 방법

[기술 문서 해당 내용](https://doc.photonengine.com/ko-kr/pun/current/gameplay/synchronization-and-state#rpc)  
갱신 빈도에 따라 권장하는 동기화 방법이 다르다. OnPhotonSerializeView는 프레임 단위로 갱신 되는 상황에, RPC는 빈번히 발생하는 상황에, 커스텀 프로퍼티는 가끔씩만 발생하는 상태 변경에 유리하다.  
초기화 데이터의 동기화가 필요한 경우에는 PhotonNetwork.Instanciate()의 data 매개변수를 입력하고 생성된 오브젝트에서 `photonView.InstantiationData`로 가져오는 방법이 있다.

### 동기화 기법

#### 지연 보상

물리적으로 데이터 전송에는 미세한 지연 시간이 걸릴 수 밖에 없다. 적중 판정과 같이 약간의 시간 차이로 결과가 달라질 수 있는 요소에는 지연 시간으로 인한 오차를 보정해주는 기법을 사용하는 것이 좋다.
이동하는 플레이어의 위치를 동기화하는 경우, 현재 속도 정보를 같이 보내는 것으로 수신측에서 지연 시간으로부터 예상 현재 위치를 역산할 수 있다.  
Rpc로 투사체를 생성할 경우, 수신측에서 투사체의 속도와 지연 시간으로부터 생성 위치를 조정하는 것으로 같은 위치에 놓여있도록 할 수 있다.  
이렇게 지연 시간에 따라 송신측에서의 데이터를 계산 또는 예측해 보정하는 것이 지연 보상이다.

* Photon Rigidbody View, Photon Transform View:  
  지연 보상 개념이 적용되어있는 위치 동기화 컴포넌트. 둘 모두 이미 지연 보상이 적용되어있기 때문에 한 객체에 둘을 동시에 사용하지 않는다.

#### 마이그레이션

마스터 클라이언트가 연결 해제되는 경우와 같이 마스터 클라이언트가 변경될 경우에 대비하면 멀티플레이의 선택지도 넓어지고 사용자 경험에도 좋다.  
PUN에서는 마스터 클라이언트가 변경되었을 경우 OnMasterClientSwitched() 콜백이 호춛되어 이때 스테이지 관리 코루틴 개시 등 필요한 처리를 할 수 있고,
Photon View 오브젝트를 룸 오브젝트로 설정하면 소유권이 특정 클라이언트가 아닌 마스터 클라이언트 권한에 묶이는 방식으로 작동해서
마스터 클라이언트가 접속 해제하더라도 소멸되지 않고 다음 마스터 클라이언트로 소유권이 이전된다.
