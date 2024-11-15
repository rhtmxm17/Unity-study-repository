# Firebase(네트워크 데이터 베이스)

[Unity에 추가하기 및 FirebaseApp 초기화](https://firebase.google.com/docs/unity/setup)
사용자 인증 등에 사용할 수 있는 네트워크 데이터베이스 관리 시스템

## Firebase Authentication(파이어베이스 사용자 인증)

[문서](https://firebase.google.com/docs/auth/unity/start)
대부분의 앱에는 사용자를 구분하기 위한 정보를 필요로 한다. 게임이라면 로그인으로 어느 클라이언트에서든 자신의 계정을 사용하는 것이 대표적이다.  
Authentication은 이러한 사용자 계정 인증을 위한 기능을 제공한다.

### 프로젝트에서 Authentication 설정

[Firebase Console 페이지](https://console.firebase.google.com/)에서 Authentication에 사용할 로그인 방법을 선택할 수 있다.

### 주요 기능 예시

`FirebaseAuth`의 사용에는 `FirebaseApp` 초기화가 성행되어야 한다.
네트워크를 통해 기능을 사용하기 때문에 대부분의 기능들은 비동기로 실행된다.  
반환 타입인 `Task`에 `ContinueWithOnMainThread`(Firebase 확장 메서드)를 실행해서 완료시 이어서 실행할 델리게이트를 추가하는 식으로 사용한다.

* CreateUserWithEmailAndPasswordAsync: 이메일과 패스워드로 계정을 생성한다
* SignInWithEmailAndPasswordAsync: 이메일과 패스워드로 로그인 한다
* SendEmailVerificationAsync: 인증용 메일을 보낸다

#### ContinueWithOnMainThread

System의 ContinueWith은 메인 스레드와는 다른 스레드에서 델리게이트가 실행되기 때문에, 델리게이트 내에서 메인 스레드와 같은 메모리를 수정하는 등 동기화 오류의 위험이 매우 크다. Firebase.Extensions 네임스페이스의 ContinueWithOnMainThread는 해당 델리게이트를 메인스레드에서 수행하게 해준다.
