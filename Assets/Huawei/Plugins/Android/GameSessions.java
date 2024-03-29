/*
 * Copyright (c) Huawei Technologies Co., Ltd. 2019-2020. All rights reserved.
 * Generated by the CloudDB ObjectType compiler.  DO NOT EDIT!
 */
package com.refapp.stackpro.huawei;

import com.huawei.agconnect.cloud.database.annotations.PrimaryKeys;
import com.huawei.agconnect.cloud.database.CloudDBZoneObject;
import com.huawei.agconnect.cloud.database.Text;

import java.util.Date;

/**
 * Definition of ObjectType GameSessions.
 *
 * @since 2023-02-09
 */
@PrimaryKeys({"id"})
public final class GameSessions extends CloudDBZoneObject {
    private String id;

    private String huaweiIdMail;

    private Integer sessionNumber;

    private Integer score;

    public GameSessions() {
        super(GameSessions.class);
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getId() {
        return id;
    }

    public void setHuaweiIdMail(String huaweiIdMail) {
        this.huaweiIdMail = huaweiIdMail;
    }

    public String getHuaweiIdMail() {
        return huaweiIdMail;
    }

    public void setSessionNumber(Integer sessionNumber) {
        this.sessionNumber = sessionNumber;
    }

    public Integer getSessionNumber() {
        return sessionNumber;
    }

    public void setScore(Integer score) {
        this.score = score;
    }

    public Integer getScore() {
        return score;
    }

}
