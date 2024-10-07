using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;

public class PermissionManager
{
    public const string FineLocation = Permission.FineLocation;

    public static void Request(string targetPermission, UnityAction grantedAction, UnityAction deniedAction = null)
    {
        // 입력받은 종류의 권한이 이미 승인되어 있다면
        if (Permission.HasUserAuthorizedPermission(targetPermission))
        {
            //  grantedAction을 호출한다.
            grantedAction();
        }
        // 승인되어 있지 않다면
        else
        {
            // 권한 처리(승인/거절/강한 거절)시 반응(콜백)에 대한 정보를 담는 변수 선언
            PermissionCallbacks pCallback = new();

            // 승인 시, grantedAction()액션을 미리 등록하고 호출될 수 있도록 한다.
            pCallback.PermissionGranted += _ => grantedAction();
            if (deniedAction != null)
                pCallback.PermissionDenied += _ => deniedAction();

            // Permission 구조체에서 권한을 요청한다.
            Permission.RequestUserPermission(targetPermission, pCallback);
        }
    }
}
