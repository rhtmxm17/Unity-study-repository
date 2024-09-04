
# \[System.Flags] 어트리뷰트: 비트단위 연산 활용

enum을 LayerMask와 같이 여러가지 값을 bit단위 on/off flag 형식으로 사용할 것을 지정하는 어트리뷰트. 인스펙터 창에서도 체크박스 형태로 나온다. 중복 가능한 상태 등에 쓰일 수 있다.

```C#
// 판정을 위한 원소 속성 플래그를 가정
[System.Flags]
enum State
{
    Pyro        = 0b0001,
    Hydro       = 0b0010,
    Aero        = 0b0100,
}
```

```C#
State pureAeroMask = ~State.Aero;
State pyroOrHydro = State.Pyro | State.Hydro;

if(State.Aero & flag); // 비행 타입을 갖고있다면 true인 조건
if(pureAeroMask & flag); // 비행 이외의 타입을 갖고있다면 true인 조건
if(pyroOrHydro & flag); // 불, 물 타입중 하나라도 갖고있다면 true인 조건
```
