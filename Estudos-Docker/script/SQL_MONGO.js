db.auth('admin', 'Pass@word');

db.createUser({
    user: 'sa',
    pwd: 'Pass@word',
    roles: [
        {
            role: "userAdminAnyDatabase",
            db: "admin"
        },
        {
            role: "dbAdminAnyDatabase",
            db: "admin"
        },
        {
            role: "readWriteAnyDatabase",
            db: "admin"
        }
    ]
});

db = db.getSiblingDB('Collection_Test');
db.createCollection('Session')
db.Session.insertOne(
    {
        _id: ObjectId("615f9d2ae3d27c5ce96fe21e"),
        externalDatabaseRefID: "09296604044",
        additionalSessionData: {
            isAdditionalDataPartiallyIncomplete: true
        },
        ageEstimateGroupEnumInt: 4,
        callData: {
            tid: "230fb884-8d34-4e38-a9f5-edea4b257758",
            path: "/enrollment-3d",
            date: ISODate("2021-01-01T00:00:00.000Z"),
            epochSecond: 1633655305,
            requestMethod: "POST"
        },
        error: false,
        faceScanSecurityChecks: {
            replayCheckSucceeded: true,
            sessionTokenCheckSucceeded: true,
            auditTrailVerificationCheckSucceeded: true,
            faceScanLivenessCheckSucceeded: true
        },
        retryScreenEnumInt: 0,
        scanResultBlob: "AAEAAABCAAAAAAAAAACOS7ujWlV4GCVkjnz3s/Eop8ROtnS9je/rNpeDla5vk1iwiN6Ksg6zlsfA+ptrFiqAyDWQXau76z2zsY7GjJ5ouA==",
        serverInfo: {
            version: "9.4.6",
            type: "Standard",
            mode: "Production"
        },
        success: true,
        wasProcessed: true,
        data: {
            faceMap: "YXNkYXNk",
            auditTrailImage: "YXNkYXNk",
            lowQualityAuditTrailImage: "YXNkYXNk"
        }
    }
)
