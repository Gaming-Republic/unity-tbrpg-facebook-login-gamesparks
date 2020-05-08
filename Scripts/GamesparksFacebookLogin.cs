﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using UnityEngine.Events;

[RequireComponent(typeof(GSGameService))]
public class GamesparksFacebookLogin : BaseFacebookLoginService
{
    private GSGameService service;

    private void Awake()
    {
        service = GetComponent<GSGameService>();
    }

    public override void LoginWithFacebook(string userId, string accessToken, UnityAction<PlayerResult> onFinish)
    {
        var result = new PlayerResult();
        result.player = new Player();
        var request = new FacebookConnectRequest();
        request.SetAccessToken(accessToken);
        request.Send((authResponse) =>
        {
            if (!authResponse.HasErrors)
                service.RequestAccountDetails(result, onFinish);
            else
            {
                Debug.LogError("GameSparks error while login: " + authResponse.Errors.JSON);
                result.error = GameServiceErrorCode.UNKNOW;
                onFinish(result);
            }
        });
    }
}
