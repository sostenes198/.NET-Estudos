db = db.getSiblingDB('admin');
db.auth('admin', 'Password');

db = db.getSiblingDB('MDB_ESTUDOS_DOCKER_MONGO');
db.createUser({
    user: 'sa',
    pwd: 'Password',
    roles: [
        {
            role: "readWrite",
            db: "MDB_ESTUDOS_DOCKER_MONGO"
        },
    ]
})

db.createCollection("Estudos",
    {
        "validator":{
            "$jsonSchema":{
                "bsonType":"object",
                "description":"Descricao",
                "required":[
                    "document"
                ],
                "properties":{
                    "document":{
                        "long":"string",
                        "description":"Este campo representa o CPF."
                    },
                    "transaction":{
                        "bsonType":"string"
                    },                
                    "date":{
                        "bsonType":"date"
                    },
                    "product":{
                        "bsonType":"int"
                    }                    
                }
            }
        }
    })

db.Estudos.insertOne(
    {
        _id: ObjectId("6384899599939350bbedaf1a"),
        document: 45678910,
        transaction: "bb72c583-b8c5-4fcf-a527-b7a34ecb9577",
        date: ISODate("2021-01-01T00:00:00.000Z"),
        productId: 1
    }
)