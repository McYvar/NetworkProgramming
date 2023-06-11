﻿using System.Collections.Generic;

[System.Serializable]
public class Results
{
    public List<Result> result;

    public override string ToString()
    {
        string message = "";
        foreach (var item in result)
        {
            message += item.ToString() + "\n";
        }
        return message;
    }
}

[System.Serializable]
public class Result
{
    public int code;
    public string session_id;
    public int server_id;
    public int user_id;
    public string email;
    public string username;

    public Result()
    {
        code = -1;
        session_id = null;
        server_id = -1;
        user_id = -1;
        email = null;
        username = null;
    }

    public override string ToString()
    {
        return $"code: {code}, session_id: {session_id}, server_id: {server_id}, user_id: {user_id}, email: {email}, username: {username}";
    }
}