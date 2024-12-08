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

## Firebase Realtime Database

NoSQL, JSON 기반 네트워크 데이터페이스

### 데이터 구조화

Realtime Database는 직렬화된 클래스와 유사한 형태의 JSON 트리를 통해 데이터를 접근/편집할 수 있다.

```C#
DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;
root.Child("users").Child(userId).Child("username").SetValueAsync(name);
```

위의 코드는 `"users"` 아래의 `userId` 아래의 `"username"`의 값을 `name`으로 설정한다.  

#### 주의사항: 비동기

[데이터를 트랜잭션으로 저장](https://firebase.google.com/docs/database/unity/save-data#save_data_as_transactions)
함수 명의 `Async`에서도 드러나듯 네트워크상의 데이터를 사용하는 것이기 때문에 비동기 방식으로 진행된다. 이때 값을 변경하는 경우 변경한 값을 대입하는 방식을 사용하면 동시에 발생한 변경사항중 하나가 덮어씌워지는 상황이 발생하기 쉽다.  
따라서 `RunTransaction()`을 사용해 `새로운 값(예: 10 -> 15)`이 아닌 `값에 대한 변경 내용(예: +5)`을 전달하는 방식으로 안정적으로 작동시킬 수 있다.

### 보안 규칙

[Firebase 실시간 데이터베이스 보안 규칙 이해](https://firebase.google.com/docs/database/security)  
데이터베이스의 각 항목마다 읽기`.read`, 쓰기`.write`, 유효성`.validate` 규칙을 설정 가능하며 추가로 정렬 기준`.indexOn`을 미리 지정해 자주 사용하는 정렬에 대한 색인을 생성할 수도 있다.
